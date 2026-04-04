Feature: CreateConsequence

In order to create a new consequence
As a user
I want to be able to create a consequence

@consequences
Scenario: Should create a new consequence
	Given the following consequence data:
		| Name         | Description       |
		| Test Consequence | Test Description  |
		| Test 2 Consequence | Test 2 Description  |
	And calls to the Consequence service by name returns null
	And the Consequence should be saved in the database
	When I send a request to Consequence mutation
	Then the consequence should call the handler the same amount of times as the data I sent
	Then Consequence response should contain 0 error objects in array

@consequences
Scenario: Should error saving Consequence if the database is down
	Given a Consequence object with name: Jose and description: test
	And the SaveChanges method does not affect any consequence rows
	When I send a request to Consequence mutation
	Then Consequence response should contain 1 error objects in array