Feature: CreateAntecedent

As an engineer
I want to be able to send antecedent objects to the database 
so that I can persist the data

@antecedents
Scenario: Should return antecedent response with errors in the response when invalid
	Given An antecedent object with an empty name and description
	And antecedent service returns null
	When executing the CreateAntecedentResponse handler
	Then response should contain 2 error objects in array

@antecedents
Scenario: Should return antecedent response with error if the database is down
	Given an antecedent object with name: Jose and description: test
	And the database is down
	When executing the CreateAntecedentResponse handler
	Then response should contain 1 error objects in array

@antecedents
Scenario: Should return antecedent response with error if no rows are affected
	Given an antecedent object with name: Jose and description: test
	And SaveChanges returns 0 row affected
	When executing the CreateAntecedentResponse handler
	Then response should contain 1 error objects in array

@antecedents
Scenario: Should save antecedent and return true when valid
	Given an antecedent object with name: Jose and description: test
	And antecedent service returns null
	And SaveChanges returns 1 row affected
	When executing the CreateAntecedentResponse handler
	Then response should contain 0 error objects in array
	And response should be true
