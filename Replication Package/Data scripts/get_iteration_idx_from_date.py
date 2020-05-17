from datetime import datetime
from iteration_finish_dates import iteration_finish_dates

def get_iteration_idx_from_date(date):
    finish_dates = [datetime(y, m, d, 23, 59, 59) for y, m, d in iteration_finish_dates]
    return len([i for i, d in enumerate(finish_dates) if d < date])
