Feature: RemoveChild

As a user
I want to be able to remove Child
so that I can delete invalid rows from the database

@children
Scenario: Should delete Child with Id
	Given a Child Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the Child with that Id exists in the database
	When I send a request to Child mutation for removal
	Then child handler should return true
	And Child response should contain 0 error objects in array

@children
Scenario: Should error when Child Id is not found
	Given a Child Id of "7f083ddd-d743-4ff3-a572-fb44229c39ab"
	And the Child with that Id does not exist in the database
	When I send a request to Child mutation for removal
	Then child handler should return false
	And Child response should contain 1 error objects in array