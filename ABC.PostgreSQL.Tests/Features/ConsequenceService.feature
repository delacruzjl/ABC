Feature: ConsequenceService

As an engineer 
I want to validate no duplicate consequences are saved in the db
so that I generate better reports

@consequences
Scenario: Should return consequences by name
	Given I have a name found in the consequence table
	And I make a call to the consequence service
	When getting consequence by name
	Then I should receive the consequence object from the database