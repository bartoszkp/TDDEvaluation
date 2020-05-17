calculate_conformance.py - Command line: `python calculate_conformance.py [REPO
NAME]` (e.g. `python calculate_conformance.py tdd1`). Calculate process
conformance for the given repository. Uses different classification algorithm
depending on type of the respository - tdd or tld. For nut always returns
100. Uses data from `classified_commits` directory.

calculate_participant_conformance.py - Command line: `python
calculate_participant_conformance.py [PARTICIPANT ID]` (e.g. `python
calculate_participant_conformance P1`). Calculates average process conformance
for a participant, averaging over TDD and TLD. Also exports functions for
calculating conformance for TDD and TLD separately. Uses data from
`classified_commits` directory.

conformance_report.py - Command line: `python conformance_report.py`. Generates
a table with process conformance for each repository for each iteration. Each
cell contains process conformance for commits only for the appropriate iteration
for TDD or TLD according to the technique that should have been used in each
repository. Uses data from `classified_commits` directory.

bugcount_report.py - Command line: `python bugcount_report.py`. Generates a
table with number of bugs in each repository in each iteration. Requires
unzipped `test_results` directory with all test results.

calculate_coverage.py - Command line: `python calculate_coverage.py [XML TEST
RESULTS FILE NAME]` (e.g. `python calculate_coverage.py
test_results\2016-08-07_unit_cover.xml`). Extracts the code coverage data from
NCover test results XML report. Requires unzipped `test_results` directory with
test results.

coverage_report.py - Command line: `python coverage_report.py`. Generates a
table with code coverage in each repository in each iteration. Requires unzipped
`test_results` directory with all test results.

assignment_plan.py - Plan of assignment of participants to repositories for each
iteration in the experiment. Also contains assignment of participants to
competence groups.

date_str.py - Utility for managing custom tuple date format.

get_commit_author.py - Exposes a function that returns ID of a participant who
had been working in a given repository at a given date.

get_failed_categories.py - Utility exposing a function to parse NUnit test
result TRX reports. Exposes function to calculate the number of failures for
each test category. Each test category is mapped to each task the participants
had to implement.

get_iteration_idx_from_date.py - Utility exposing a function to find an index of
an iteration for a given date.

issue_dates.py - Utility exposing a dictionary mapping each task (issue) to a
start date of an iteration in which the task had been introduced to the
participants.

iteration_finish_dates.py - Utility exposing a list mapping indexes of
iterations to their end dates.

repo_list.py - Utility exposing lists of repository names in different orders.

classified_commits/categorize_commits.py - Command line: `python
categorize_commits.py [REPO NAME]` (e.g. `python categorize_commits.py
tdd1`). Classifies commits into "good", "bad" and "ignored", according to the
technique assigned to the given repository and produces a report text file
containins a section for each category, with short explanation for each
commit. Requires the repository to be cloned locally. Depends on `git` and
`msbuilder` Python modules and the `NUGet.exe` tool with its configuration, to
be able to recompile and retest the code in the repository after each commit.
