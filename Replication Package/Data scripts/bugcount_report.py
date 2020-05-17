import os
import glob
import json
import datetime
from get_failed_categories import get_failed_categories
from iteration_finish_dates import iteration_finish_dates
from date_str import date_str
from repo_list import tdd_tld_nut_repo_list
from issue_dates import issue_dates
print("\t"+"\t".join((date_str(date) for date in iteration_finish_dates)))

def get_issue_date(i):
    return datetime.datetime.strptime(issue_dates[i], '%Y-%m-%d')

for repo in tdd_tld_nut_repo_list:
    print(repo,end="")
    for date in iteration_finish_dates:
        iteration_date = datetime.datetime(date[0], date[1], date[2])
        test_results_file = os.path.join('test_results', repo, date_str(date) + "_integration.trx")
        failures = get_failed_categories(test_results_file)
        failures = [f for f in failures.keys() if f in issue_dates and get_issue_date(f) < iteration_date]
        failures = len(failures)
        print("\t%8u" % failures, end="")
    print()
