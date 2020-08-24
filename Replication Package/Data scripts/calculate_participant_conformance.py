import sys
from get_commit_author import get_commit_author
from repo_list import group_repo_list
from calculate_conformance import calculate_conformance_from_commits
from calculate_conformance import count_commits

def filter_commits(commits, login, repo):
    return [c for c in commits if get_commit_author(repo, c[2]) == login]

def get_all_participants_commits(login, repo):
    good, bad, unknown = count_commits(repo)
    return tuple((filter_commits(c, login, repo) for c in [good, bad, unknown]))

def calculate_participant_conformance(login):
    good, bad, unknown = [], [], []
    for repo in group_repo_list:
        if repo.lower().startswith('nut'):
            continue
        g, b, i = get_all_participants_commits(login, repo)
        good += g
        bad += b
        unknown += i

    print('unknown:',len(unknown),'which is',(float(len(unknown))/(float(len(good)+len(bad)+len(unknown)))))
    return calculate_conformance_from_commits(good, bad, unknown)

def calculate_participant_conformance_tdd(login):
    good, bad, unknown = [], [], []
    for repo in group_repo_list:
        if repo.lower().startswith('nut'):
            continue
        if repo.lower().startswith('tld'):
            continue
        g, b, i = get_all_participants_commits(login, repo)
        good += g
        bad += b
        unknown += i

    return calculate_conformance_from_commits(good, bad, unknown)
    
def calculate_participant_conformance_tld(login):
    good, bad, unknown = [], [], []
    for repo in group_repo_list:
        if repo.lower().startswith('nut'):
            continue
        if repo.lower().startswith('tdd'):
            continue
        g, b, i = get_all_participants_commits(login, repo)
        good += g
        bad += b
        unknown += i

    return calculate_conformance_from_commits(good, bad, unknown)

if __name__ == "__main__":
    login = sys.argv[1]
    print(100.0 * calculate_participant_conformance(login))
