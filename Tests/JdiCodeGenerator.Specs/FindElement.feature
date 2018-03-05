Feature:    FindElement
                In    order    to    convert    a    web    page    into    a    collection    of    code    entries
                As    a    user
                I    want    to    run    code    conversion    and    get    the    element    expected
                
#    @ignore    @parsePage
Scenario:    Get    one    simple    element
                Given    I    have    a    Bootstrap    web    page    "..\Data\Bootstrap3\Simple\ButtonDefault.txt"
                When    I    start    the    parser    app
                Then    the    result    should    be    an    element    of    type    "IButton"

#    @ignore
Scenario:    Get    another    element
                Given    I    have    a    Bootstrap    web    page    "..\Data\Bootstrap3\Simple\InputGroupAddon0.txt"
                When    I    start    the    parser    app
                Then    the    result    should    be    an    element    of    type    "ITextField"
