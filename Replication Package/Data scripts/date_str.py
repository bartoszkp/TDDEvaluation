import datetime

def date_str(date):
    return "%d-%02d-%02d" % date

def str_date(date_str):
    parts = date_str.split('-')
    return datetime.datetime(int(parts[0]), int(parts[1]), int(parts[2]))
