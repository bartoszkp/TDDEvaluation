import os
import sys
import subprocess
import datetime
from get_failed_categories import get_failed_categories
from iteration_finish_dates import iteration_finish_dates
from date_str import date_str
from repo_list import tdd_tld_nut_repo_list

def get_monday_before(date):
    return date - datetime.timedelta(date.weekday())

print("\t"+"\t".join((date_str(date) for date in iteration_finish_dates)))

for repo in tdd_tld_nut_repo_list:
    print(repo,end="")
    for date in iteration_finish_dates:
        d = datetime.datetime(date[0], date[1], date[2], 23, 59, 59)
        start_date = get_monday_before(d)
        start_date = date_str((start_date.year, start_date.month, start_date.day))
        end_date = date_str((d.year, d.month, d.day))

        p = subprocess.Popen(['python', 'calculate_conformance.py', repo, start_date, end_date], stdout=subprocess.PIPE)
        out = p.communicate()[0].decode('utf-8', 'ignore')
        out = out.split(os.linesep)
        if len(out) < 2:
            print("inner script error: ", date, repo)
            print(out)
            sys.exit(-1)
        last = out[-2].strip()
        try:
            conformance = float(last)
        except:
            print(out)
            sys.exit(-1)
        if p.returncode != 0:
            print("inner script error: ", date, repo)
            print(out)
            sys.exit(-1)
        print("\t%.1f" % conformance, end="")
    print()
