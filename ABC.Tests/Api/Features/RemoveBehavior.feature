Feature: RemoveBehavior

As a user
I want to be able to remove Behavior
so that I can delete invalid rows from the database

@behaviors
Scenario: Should delete Behavior with Id
	Given a Behavior Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the Behavior with that Id exists in the database
	When I send a request to Behavior mutation for removal
	Then behavior handler should return true
	And Behavior response should contain 0 error objects in array

@behaviors
Scenario: Should error when Behavior Id is not found
	Given a Behavior Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the Behavior with that Id does not exist in the database
	When I send a request to Behavior mutation for removal
	Then behavior handler should return false
	And Behavior response should contain 1 error objects in array