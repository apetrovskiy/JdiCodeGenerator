﻿Feature: ParsePage
	In order to convert a web page into a collection of code entries
	As a user
	I want to run code conversion

@parsePage
Scenario: Get one simple element
	Given I have a web page with a button
	When I start the parser app
	Then the result should be an element of type "IButton"
