Feature: CreateChild

In order to create a new Child
As a user
I want to be able to create a Child

@Children
Scenario: Should create a new Child
	Given the following Child data:
		| LastName     | FirstName          | BirthYear | Conditions |
		| Test Child   | Test Description   | 2020      | condition1,condition2 |
		| Test 2 Child | Test 2 Description | 1980      | condition1 |
	And calls to the Child service by name returns null
	And the Child should be saved in the database
	And child Conditions exist in the database
	When I send a request to Child mutation
	Then the Child should call the handler the same amount of times as the data I sent
	Then Child response should contain 0 error objects in array

@Children
Scenario: Should error saving Child if the database is down
	Given a Child object with name: Jose and description: test
	And the SaveChanges method does not affect any Child rows
	When I send a request to Child mutation
	Then Child response should contain 1 error objects in array