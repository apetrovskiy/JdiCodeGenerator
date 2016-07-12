Feature: ParsePage
	In order to convert a web page into a collection of code entries
	As a user
	I want to run code conversion
	
# @ignore @parsePage
Scenario: Process a Bootstrap3 page
	Given I have a Bootstrap web page "..\Data\WashingtonEdu.txt"
	When I start the parser app
	Then the result should be a collection of elements

# @ignore
Scenario: Process a HTML5 page
	Given I have a Bootstrap web page "..\Data\LentaRu.txt"
	When I start the parser app
	Then the result should be a collection of elements