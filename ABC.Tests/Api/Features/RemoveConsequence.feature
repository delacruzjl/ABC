Feature: RemoveConsequence

As a user
I want to be able to remove Consequence
so that I can delete invalid rows from the database

@consequences
Scenario: Should delete Consequence with Id
	Given a Consequence Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the Consequence with that Id exists in the database
	When I send a request to Consequence mutation for removal
	Then Consequence handler should return true
	And Consequence response should contain 0 error objects in array

@consequences
Scenario: Should error when Consequence Id is not found
	Given a Consequence Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the Consequence with that Id does not exist in the database
	When I send a request to Consequence mutation for removal
	Then Consequence handler should return false
	And Consequence response should contain 1 error objects in array