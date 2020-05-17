import sys
import os
import git
import datetime
import shutil
import subprocess
import git
import time
from itertools import chain
from msbuilder import MSBuilder
from calculate_coverage import calculate_coverage
from get_failed_categories import get_failed_categories
from repo_list import tdd_tld_nut_repo_list

def get_failing_tests_and_cover(commit, git_repo, repo):
    git_repo.head.reference = commit
    git_repo.head.reset(index=True, working_tree=True)
    shutil.copy('NuGet.config', repo + r'\Signals')
    os.chdir(repo + r'\Signals')
    p = subprocess.Popen([r'..\..\NuGet.exe', 'restore'], stdout=subprocess.PIPE)
    p.communicate()
    if p.returncode != 0:
        print("NuGet restore error")
        sys.exit(-1)
    os.chdir(r'..\..')

    msbuilder = MSBuilder()
    msbuilder.logging = False
    projPath = os.path.join(os.path.abspath(repo), 'Signals', 'Signals.sln')
    if not msbuilder.build(projPath):
        return None, None
    if os.path.exists('testResultsUnit.trx'):
        os.remove('testResultsUnit.trx')
    current_path = os.path.abspath(os.getcwd())
    if not msbuilder.unit_test_and_cover(projPath, current_path):
        print("failed unit_test_and_cover")
        sys.exit(-1)
    coverage = calculate_coverage('unit_opencover.xml')[0]
    result = get_failed_categories('testResultsUnit.trx')
    os.remove('unit_opencover.xml')
    os.remove('testResultsUnit.trx')
    return ([r[0] for r in result.get('Unknown', [])], coverage)

def get_commits_categorized_by_content(git_repo):
    impl = []
    test = []
    mixed = []

    for c in git_repo.iter_commits('master'):
        parents = c.parents
        if len(parents) != 1:
            print("Ignoring",c,":",c.message," (multiple parents)")
            continue
        if '[INFRASTRUCTURE]' in c.message:
            print("Ignoring",c,":",c.message)
            continue
        diff = c.diff(c.parents[0])
        paths = set(chain((b.a_path for b in diff), (b.b_path for b in diff)))
        if all((p.startswith('Signals/WebService.Tests') or p.startswith('ExampleSignalClient') for p in paths)):
            test.append(c)
        elif all((not p.startswith('Signals/WebService.Tests') for p in paths)):
            impl.append(c)
        else:
            mixed.append(c)

    return (impl, test, mixed)

def categorize_mixed_commit_by_semantics(result_lists, failing_tests_parent, coverage_parent, failing_tests, coverage):
    if failing_tests is None:
        return (result_lists[1], 'Break compilation', '')

    if failing_tests_parent is None:
        return (result_lists[0], 'Compilation fix', '')

    new_failing = [ft for ft in failing_tests if ft not in failing_tests_parent]
    if new_failing:
        return (result_lists[1], 'Break tests', '')

    if coverage < coverage_parent-0.1:
        return (result_lists[1], 'Reduced coverate', (coverage_parent, '->', coverage))

    return (result_lists[0], 'Mixed but no coverage loss', (coverage_parent, '->', coverage))

def get_commits_categorized_by_semantics_tdd(git_repo):
    git_repo = git.Repo(repo)
    
    impl, test, mix = get_commits_categorized_by_content(git_repo)

    good = []
    bad = []
    unknown = []
    for m in mix:
        parent = m.parents[0]
        failing_tests_parent, coverage_parent = get_failing_tests_and_cover(parent, git_repo, repo)
        failing_tests, coverage = get_failing_tests_and_cover(m, git_repo, repo)

        classification = categorize_mixed_commit_by_semantics((good, bad, unknown), failing_tests_parent, coverage_parent, failing_tests, coverage)
        classification[0].append((m, classification[1], classification[2]))

    for i in impl:
        parent = i.parents[0]
        failing_tests_parent, coverage_parent = get_failing_tests_and_cover(parent, git_repo, repo)
        failing_tests, coverage = get_failing_tests_and_cover(i, git_repo, repo)
        if failing_tests is None:
            if failing_tests_parent is None:
                unknown.append((i, 'Still does not compile', []))
            else:
                bad.append((i, 'Break compilation', []))
            continue
        if failing_tests_parent is None:
            good.append((i, 'Compilation fix', []))
            continue

        if failing_tests_parent:
            fixed_tests = [ft for ft in failing_tests_parent if ft not in failing_tests]
            new_failing = [ft for ft in failing_tests if ft not in failing_tests_parent]
            if fixed_tests and not new_failing:
                good.append((i, 'Fix tests', fixed_tests))
            elif fixed_tests and new_failing:
                unknown.append((i, 'Some fixed, some new failing', new_failing))
            elif not fixed_tests and not new_failing:
                unknown.append((i, 'Nothing fixed', failing_tests_parent))
            elif not fixed_tests and new_failing:
                bad.append((i, 'Break tests, nothing fixed', new_failing))
        elif failing_tests:
            bad.append((i, 'Break tests', []))
        else:
            if abs(float(coverage_parent) - float(coverage)) < 0.1:
                good.append((i, 'REF', (coverage_parent, '->', coverage)))
            elif float(coverage_parent) < float(coverage):
                good.append((i, 'REF, increased coverage', (coverage_parent, '->', coverage)))
            else:
                bad.append((i, 'REF, reduced coverage', (coverage_parent, '->', coverage)))

    for t in test:
        parent = i.parents[0]
        failing_tests_parent, coverage_parent = get_failing_tests_and_cover(parent, git_repo, repo)
        failing_tests, coverage = get_failing_tests_and_cover(i, git_repo, repo)
        if failing_tests is None:
            if failing_tests_parent is None:
                unknown.append((t, 'Still does not compile', []))
            else:
                good.append((t, 'New tests - not compiling', []))
            continue
        if failing_tests_parent is None:
            good.append((t, 'Compilation fix in tests', []))
            continue
        if failing_tests_parent:
            unknown.append((t, 'Still failing tests or fixed tests', failing_tests_parent + failing_tests))
        elif failing_tests:
            good.append((t, 'New failing tests', failing_tests))
        else:
            if abs(float(coverage_parent) - float(coverage)) < 0.1:
                good.append((t, 'TEST REF', (coverage_parent, '->', coverage)))
            elif float(coverage_parent) < float(coverage):
                good.append((t, 'TEST REF, increased coverage', (coverage_parent, '->', coverage)))
            else:
                bad.append((t, 'TEST REF, reduced coverage', (coverage_parent, '->', coverage)))

    return (good, bad, unknown)
    
def get_commits_categorized_by_semantics_tld(repo):
    git_repo = git.Repo(repo)

    impl, test, mix = get_commits_categorized_by_content(git_repo)

    good = []
    bad = []
    unknown = []

    for m in mix:
        parent = m.parents[0]
        failing_tests_parent, coverage_parent = get_failing_tests_and_cover(parent, git_repo, repo)
        failing_tests, coverage = get_failing_tests_and_cover(m, git_repo, repo)

        classification = categorize_mixed_commit_by_semantics((good, bad, unknown), failing_tests_parent, coverage_parent, failing_tests, coverage)
        classification[0].append((m, classification[1], classification[2]))

    for i in impl:
        parent = i.parents[0]
        failing_tests_parent, coverage_parent = get_failing_tests_and_cover(parent, git_repo, repo)
        failing_tests, coverage = get_failing_tests_and_cover(i, git_repo, repo)
        if failing_tests is None:
            bad.append((i, 'Does not compile', []))
            continue
        if failing_tests_parent is None:
            unknown.append((i, 'Compilation fix', []))
            continue

        if failing_tests:
            l = [ft for ft in failing_tests_parent] + [ft for ft in failing_tests]
            bad.append((i, 'Failing tests', l))
        elif failing_tests_parent:
            good.append((i, 'Failing tests fix', []))
        else:
            if abs(float(coverage_parent) - float(coverage)) < 0.1:
                good.append((i, 'REF', (coverage_parent, '->', coverage)))
            elif float(coverage_parent) > float(coverage):
                good.append((i, 'New implementation', (coverage_parent, '->', coverage)))
            else:
                unknown.append((i, 'IMPL INCREASED COVERAGE?', (coverage_parent, '->', coverage)))
        
    for t in test:
        parent = t.parents[0]
        failing_tests_parent, coverage_parent = get_failing_tests_and_cover(parent, git_repo, repo)
        failing_tests, coverage = get_failing_tests_and_cover(t, git_repo, repo)
        if failing_tests is None:
            bad.append((t, 'Does not compile', []))
            continue
        if failing_tests_parent is None:
            unknown.append((t, 'Test compilation fix', []))
            continue
        if failing_tests_parent:
            if not failing_tests:
                good.append((t, 'Test fix', []))
            else:
                l = [ft for ft in failing_tests]
                bad.append((t, 'Failing tests', l))
        elif failing_tests:
            l = [ft for ft in failing_tests]
            bad.append((t, 'Break tests', l))
        else:
            if abs(float(coverage_parent) - float(coverage)) < 0.1:
                good.append((t, 'Tests REF or new tests, no coverage inc', (coverage_parent, '->', coverage)))
            elif float(coverage_parent) < float(coverage):
                good.append((t, 'New tests', (coverage_parent, '->', coverage)))
            else:
                bad.append((t, 'New tests, reduce coverage?', (coverage_parent, '->', coverage)))

    return (good, bad, unknown)

def get_commits_categorized_by_semantics(repo):
    if repo.lower().startswith('tdd'):
        return get_commits_categorized_by_semantics_tdd(repo)
    elif repo.lower().startswith('tld'):
        return get_commits_categorized_by_semantics_tld(repo)
    else:
        return ('ALL', 'REF', (0, '->', 0))

if __name__ == "__main__":
    repo = sys.argv[1]
    good, bad, unknown = get_commits_categorized_by_semantics(repo)
    with open(repo + '_commit_categories.txt', 'w') as f:
        f.write("Good commits:\n")
        for c in good:
            authored_date = time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime(c[0].authored_date))
            committed_date = time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime(c[0].committed_date))
            f.write(str(c)+' '+authored_date+' '+committed_date+"\n")
        f.write("Bad commits:\n")
        for c in bad:
            authored_date = time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime(c[0].authored_date))
            committed_date = time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime(c[0].committed_date))
            f.write(str(c)+' '+authored_date+' '+committed_date+"\n")
        f.write("Unknown commits:\n")
        for c in unknown:
            authored_date = time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime(c[0].authored_date))
            committed_date = time.strftime("%Y-%m-%d %H:%M:%S", time.gmtime(c[0].committed_date))
            f.write(str(c)+' '+authored_date+' '+committed_date+"\n")
