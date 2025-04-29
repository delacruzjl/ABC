Feature: AntecedentRepository

As an engineer 
I want to retrieve antecedents from the database following repository pattern
So that I can ensure the repository is working as expected

@antecedents
Scenario: Should retrieve all antecedents from the database
	Given I have a unit of work 
	And 5 rows in the antecedent table
	When I request all antecedents from the database
	Then I should receive all the antecedents from the database

@antecedents
Scenario: Should retrieve antecedents by id from the database
	Given I have a unit of work 
	And 5 rows in the antecedent table
	When I request antecedents by an existing id from the database
	Then I should receive the antecedents by id from the database

@antecedents
Scenario: Should throw exception if antecedents not found
	Given I have a unit of work 
	And 5 rows in the antecedent table
	When I request antecedents by a non-existing id from the database
	Then I should receive an exception indicating antecedents not found

@antecedents
Scenario: Should delete antecedents from the database
	Given I have a unit of work 
	And 5 rows in the antecedent table
	When I request antecedents by an existing id from the database
	And I delete an antecedent from the database
	And Save changes in the unit of work
	Then I should not find the antecedent in the database

@antecedents
Scenario: Should throw exception if antecedents not found when deleting
	Given I have a unit of work 
	And 2 rows in the antecedent table
	When I request antecedents by a non-existing id from the database
	Then I should receive an exception indicating antecedents not found When deleting

@antecedents
Scenario: Should retrieve antecedent by name
	Given I have a unit of work 
	And 5 rows in the antecedent table
	When I request antecedents by an existing name from the database
	Then I should receive the antecedents by name from the database