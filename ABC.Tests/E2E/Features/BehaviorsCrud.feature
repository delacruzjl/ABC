@e2e
Feature: Behaviors CRUD

As an admin
I want to manage behaviors through the UI
So that I can maintain the list of behavior options for observations

Background:
	Given I am logged in as an admin

Scenario: Navigate to behaviors page
	When I click the "Behaviors" nav button
	Then I should be on the "/behaviors" page

Scenario: Create a new behavior
	When I click the "Behaviors" nav button
	And I click the "Add Behavior" button
	And I fill in the behavior form with name "Test Behavior E2E" and description "E2E test description"
	And I click the "Add" button
	Then I should be on the "/behaviors" page
	And I should see "Test Behavior E2E" on the page

Scenario: Delete a behavior
	When I click the "Behaviors" nav button
	And I create a behavior with name "Delete Me E2E" and description "To be deleted"
	Then I should see "Delete Me E2E" on the page
	When I delete the behavior "Delete Me E2E"
	Then I should not see "Delete Me E2E" on the page
