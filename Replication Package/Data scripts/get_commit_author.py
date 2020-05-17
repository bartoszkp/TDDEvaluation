from get_iteration_idx_from_date import get_iteration_idx_from_date
from assignment_plan import assignment_plan

def get_commit_author(repo, date):
    idx = get_iteration_idx_from_date(date)
    for author, repos in assignment_plan.items():
        if repos[idx] == repo:
            return author
    return None
