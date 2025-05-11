Feature: AntecedentService

As an engineer
I want to validate antecedents data
so that users are not able to save invalid objects to the database

@antecedents
Scenario: Should return antecedents by name
	Given I have a name found in the antecedent table
	And I make a call to the antecedent service
	Then I should receive the antecedent object from the database

@antecedents
Scenario: Should return antecedents by name case insensitive
	Given I have a name found in the antecedent table
	And I search for the same name in upper case
	And I make a call to the antecedent service
	Then I should receive the antecedent object from the database
