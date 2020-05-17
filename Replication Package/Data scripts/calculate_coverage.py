import sys
import xml.etree.ElementTree

opencover_namespace = 'http://www.w3.org/2001/XMLSchema'

def calculate_coverage(file_name):
    tree = xml.etree.ElementTree.parse(file_name)
    root = tree.getroot()

    modules = root.find('Modules').findall('Module')

    non_skipped = [m for m in modules if not m.attrib.get('skippedDueTo')]

    class_coverage = {}
    class_coverage_comp = {}
    for module in non_skipped:
        classes = module.find('Classes').findall('Class')
        for class_ in classes:
            summary = class_.find('Summary')
            fullname = class_.find('FullName').text
            if fullname not in class_coverage:
                class_coverage[fullname] = {}
            if fullname not in class_coverage_comp:
                class_coverage_comp[fullname] = {}
            for method in class_.find('Methods').findall('Method'):
                if not method.attrib.get('skippedDueTo'):
                    method_summary = method.find('Summary')
                    total_sequence_points = int(method_summary.attrib['numSequencePoints'])
                    method_name = method.find('Name').text
                    if method_name not in class_coverage[fullname]:
                        class_coverage[fullname][method_name] = (0, total_sequence_points)
                    if method.attrib['cyclomaticComplexity'] != "1":
                        if method_name not in class_coverage_comp[fullname]:
                            class_coverage_comp[fullname][method_name] = (0, total_sequence_points)
                    
                    if method.attrib['visited'] == "true":
                        visited_sequence_points, total_sequence_points = class_coverage[fullname][method_name]
                        visited_sequence_points = max(visited_sequence_points, int(method_summary.attrib['visitedSequencePoints']))
                        class_coverage[fullname][method_name] = (visited_sequence_points, total_sequence_points)
                        if method.attrib['cyclomaticComplexity'] != "1":
                            visited_sequence_points, total_sequence_points = class_coverage_comp[fullname][method_name]
                            visited_sequence_points = max(visited_sequence_points, int(method_summary.attrib['visitedSequencePoints']))
                            class_coverage_comp[fullname][method_name] = (visited_sequence_points, total_sequence_points)

    total_sequence_points = 0
    visited_sequence_points = 0

    for class_,method_coverage in class_coverage.items():
        total_method_sequence_points = sum(m[1] for m in method_coverage.values() if m[1] > 0)
        visited_method_sequence_points = sum(m[0] for m in method_coverage.values() if m[1] > 0)
        if total_method_sequence_points == 0:
            continue
        total_sequence_points += total_method_sequence_points
        visited_sequence_points += visited_method_sequence_points

    coverage = 100.0*visited_sequence_points/total_sequence_points if total_sequence_points > 0 else None

    total_sequence_points = 0
    visited_sequence_points = 0

    for class_,method_coverage in class_coverage_comp.items():
        total_method_sequence_points = sum(m[1] for m in method_coverage.values() if m[1] > 0)
        visited_method_sequence_points = sum(m[0] for m in method_coverage.values() if m[1] > 0)
        if total_method_sequence_points == 0:
            continue
        total_sequence_points += total_method_sequence_points
        visited_sequence_points += visited_method_sequence_points

    complex_coverage = 100.0*visited_sequence_points/total_sequence_points if total_sequence_points > 0 else None

    return coverage, complex_coverage

if __name__ == "__main__":
    coverage, complex_coverage = calculate_coverage(sys.argv[1])
    print("Coverage:", coverage)
    print("Complex coverage:", complex_coverage)
