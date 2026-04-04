Feature: RemoveChildCondition

As a user
I want to be able to remove ChildCondition
so that I can delete invalid rows from the database

@ChildConditionren
Scenario: Should delete ChildCondition with Id
	Given a ChildCondition Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the ChildCondition with that Id exists in the database
	When I send a request to ChildCondition mutation for removal
	Then ChildCondition handler should return true
	And ChildCondition response should contain 0 error objects in array

@ChildConditionren
Scenario: Should error when ChildCondition Id is not found
	Given a ChildCondition Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the ChildCondition with that Id does not exist in the database
	When I send a request to ChildCondition mutation for removal
	Then ChildCondition handler should return false
	And ChildCondition response should contain 1 error objects in array