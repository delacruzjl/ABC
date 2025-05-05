Feature: RemoveAntecedent 

As a user
I want to be able to remove antecedent
so that I can delete invalid rows from the database

@antecedents
Scenario: Should delete antecedent with Id
	Given an antecedent Id of 7f083ddd-d743-4ff3-a572-fb44229c39ab
	And the antecedent with that Id exists in the database
	When I send a request to Antecedent mutation for removal
	Then handler should return true
	And Antecedent response should contain 0 error objects in array

@antecedents
Scenario: Should error when antecedent Id is not found
	Given an antecedent Id of 7f083ddd-d743-4ff3-a572-fb44229c39ab
	And the antecedent with that Id does not exist in the database
	When I send a request to Antecedent mutation for removal
	Then handler should return false
	And Antecedent response should contain 1 error objects in array