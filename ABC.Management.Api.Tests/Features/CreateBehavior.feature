Feature: CreateBehavior

In order to create a new behavior
As a user
I want to be able to create a behavior

@behaviors
Scenario: Should create a new behavior
	Given the following behavior data:
		| Name         | Description       |
		| Test Behavior | Test Description  |
		| Test 2 Behavior | Test 2 Description  |
	And calls to the behavior service by name returns null
	And the behavior should be saved in the database
	When I send a request to behavior mutation
	Then the response should call the handler the same amount of times as the data I sent
	Then behavior response should contain 0 error objects in array

@behaviors
Scenario: Should error saving behavior if the database is down
	Given a behavior object with name: Jose and description: test
	And the database is down
	When I send a request to behavior mutation
	Then behavior response should contain 1 error objects in array