Feature: AntecedentRepository

As an engineer I want to retrieve antecedents from the database following repository pattern
So that I can ensure the repository is working as expected

@tag1
Scenario: Should retrieve all antecedents from the database
	Given I have a unit of work 
	And 5 rows in the antecedent table
	When I request all antecedents from the database
	Then I should receive all the antecedents from the database
