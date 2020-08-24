import sys
import datetime

def count_commits(repo, datefrom = None, dateto = None):
    good, bad, unknown = [], [], []
    with open('classified_commits\\'+repo.lower()+'_commit_categories.txt', 'r') as f:
        current = good
        for line in f:
            line = line.strip()
            if line == 'Good commits:':
                current = good
            elif line == 'Bad commits:':
                current = bad
            elif line == 'Unknown commits:':
                current = unknown
            else:
                parts = line.split(', ')
                commit = parts[0].split('"')[1]
                type = parts[1]
                dates = parts[-1].split(' ')[-4:]
                date = datetime.datetime.strptime(dates[0]+' '+dates[1], "%Y-%m-%d %H:%M:%S")
                if datefrom is not None:
                    if date < datefrom:
                        continue
                if dateto is not None:
                    if date > dateto:
                        continue
                current.append((commit, type, date))
    return good, bad, unknown

def calculate_conformance_from_commits(good, bad, unknown):
    total = len(good)+len(bad)+len(unknown)
    toremove = []
#    return float(len(good)) / (len(good) + len(bad))
    return (float(len(good)) / float(len(good)+len(bad))) * (1 - float(len(unknown))/total)

def calculate(repo, datefrom, dateto):
    good, bad, unknown = count_commits(repo, datefrom, dateto)
    return calculate_conformance_from_commits(good, bad, unknown)

def calculate_tdd_conformance(repo, datefrom, dateto):
    return calculate(repo, datefrom, dateto)

def calculate_tld_conformance(repo, datefrom, dateto):
    return calculate(repo, datefrom, dateto)

def calculate_conformance(repo, datefrom = None, dateto = None):
    if repo.lower().startswith('tdd'):
        return calculate_tdd_conformance(repo, datefrom, dateto)
    elif repo.lower().startswith('tld'):
        return calculate_tld_conformance(repo, datefrom, dateto)
    else:
        return 1

def parse_date(arg, hour, minute, second):
    datestr = tuple((int(p) for p in arg.split('-')))
    return datetime.datetime(datestr[0], datestr[1], datestr[2], hour, minute, second)

if __name__ == "__main__":
    datefrom = None
    dateto = None
    if len(sys.argv) > 2:
        datefrom = parse_date(sys.argv[2], 0, 0, 0)
    if len(sys.argv) > 3:
        dateto = parse_date(sys.argv[3], 23, 59, 59)
    conformance = calculate_conformance(sys.argv[1], datefrom, dateto)
    print(100.0 * conformance)
