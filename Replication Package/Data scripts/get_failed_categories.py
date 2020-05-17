import xml.etree.ElementTree

mstest_namespace = 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'

def get_test_definitions_from_node(test_run_node):
    category_to_test = {}
    test_to_category = {}
    test_id_to_name = {}
    definitions = test_run_node.find('{%s}TestDefinitions' % mstest_namespace)
    definitions = definitions.findall('{%s}UnitTest' % mstest_namespace)
    for definition in definitions:
        category = definition.find('{%s}TestCategory' % mstest_namespace)
        categories = []
        if category is not None:
            for c in category.findall('{%s}TestCategoryItem' % mstest_namespace):
                categories.append(c.attrib['TestCategory'])
        else:
            categories = ['Unknown']
        test_name = definition.attrib['name']
        test_id = definition.attrib['id']
        properties = definition.find('{%s}Properties' % mstest_namespace)
        if properties is not None:
            property = properties.find('{%s}Property' % mstest_namespace)
            if property is not None:
                key = property.find('{%s}Key' % mstest_namespace)
                if key is not None and key.text == '__Internal_DeclaringClassName__':
                    value = property.find('{%s}Value' % mstest_namespace)
                    if value is not None:
                        value = value.text.split(',')[0]
                        test_name = value + ']].' + test_name
        for c in categories:
            category_to_test.setdefault(c, []).append(test_id)
            test_to_category.setdefault(test_id, set()).add(c)
        test_id_to_name[test_id] = test_name
    return category_to_test, test_to_category, test_id_to_name

def get_failures(test_run_node, test_definitions):
    results = test_run_node.find('{%s}Results' % mstest_namespace)
    if results is None:
        return {'Unknown':[]}

    results = results.findall('{%s}UnitTestResult' % mstest_namespace)

    category_to_test, test_to_category, test_id_to_name = test_definitions
    failures = {}
    for result in results:
        if result.attrib.get('outcome') != 'Passed':
            message = result.find('{%s}Output/{%s}ErrorInfo/{%s}Message' \
                                  % (mstest_namespace, mstest_namespace, mstest_namespace))
            message = message.text if message is not None else '?'
            test_id = result.attrib['testId']
            categories = test_to_category[test_id]
            test_name = test_id_to_name[test_id]
            for category in categories:
                failures.setdefault(category, []).append((test_name, message))
    return failures

def get_test_definitions(filename):
    tree = xml.etree.ElementTree.parse(filename)
    root = tree.getroot()
    return get_test_definitions_from_node(root)

def get_failed_categories(filename):    
    tree = xml.etree.ElementTree.parse(filename)
    root = tree.getroot()
    return get_failures(root, get_test_definitions_from_node(root))
