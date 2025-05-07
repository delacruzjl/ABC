Feature: CreateChildCondition

As an engineer
I want to be able to send ChildCondition objects to the database 
so that I can persist the data

@childconditions
Scenario: Should return ChildCondition response with errors in the response when invalid
	Given An ChildCondition object with an empty name and description
	And ChildCondition service returns null
	When executing the CreateChildConditionResponse handler
	Then child condition response should contain 1 error objects in array

@ChildConditions
Scenario: Should return ChildCondition response with error if the database is down
	Given an ChildCondition object with name: Test
	And the child condition database is down
	When executing the CreateChildConditionResponse handler
	Then child condition response should contain 1 error objects in array

@ChildConditions
Scenario: Should return ChildCondition response with error if no rows are affected
	Given an ChildCondition object with name: test
	And SaveChanges returns 0 child condition rows affected
	When executing the CreateChildConditionResponse handler
	Then child condition response should contain 1 error objects in array

@ChildConditions
Scenario: Should save ChildCondition and return true when valid
	Given an ChildCondition object with name: test
	And ChildCondition service returns null
	And SaveChanges returns 1 child condition rows affected
	When executing the CreateChildConditionResponse handler
	Then child condition response should contain 0 error objects in array
	And child condition response should be true
