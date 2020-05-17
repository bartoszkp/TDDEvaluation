import os
from calculate_coverage import calculate_coverage
from iteration_finish_dates import iteration_finish_dates
from date_str import date_str
from repo_list import tdd_tld_nut_repo_list

print("\t"+"\t\t\t\t".join((date_str(date) for date in iteration_finish_dates)))
for date in iteration_finish_dates:
    print("\tUnit coverage\tIntegration coverage\tUnit complex coverage\tIntegration complex coverage", end="")
print()
for repo in tdd_tld_nut_repo_list:
    print(repo,end="")
    for date in iteration_finish_dates:
        unit = calculate_coverage(os.path.join('test_results', repo, date_str(date)+'_unit_cover.xml'))
        integration = calculate_coverage(os.path.join('test_results', repo, date_str(date)+'_integration_cover.xml'))
        unit = tuple((u if u else 0 for u in unit))
        integration = tuple((i if i else 0 for i in integration))
        print("\t%.2f\t%.2f\t%.2f\t%.2f" % (unit+integration), end="")
    print()
