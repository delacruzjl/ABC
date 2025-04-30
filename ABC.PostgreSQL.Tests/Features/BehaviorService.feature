Feature: BehaviorService

As an engineer
I want to validate behaviors data
so that users are not able to save invalid objects to the database

@behaviors
Scenario: Should return behavior by name
	Given I have a name found in the behavior table
	And I make a call to the behavior service
	When Get behavior by name
	Then I should receive the behavior object from the database