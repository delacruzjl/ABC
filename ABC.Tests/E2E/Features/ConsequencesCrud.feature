@e2e
Feature: Consequences CRUD

As an admin
I want to manage consequences through the UI
So that I can maintain the list of consequence options for observations

Background:
	Given I am logged in as an admin

Scenario: Navigate to consequences page
	When I click the "Consequences" nav button
	Then I should be on the "/consequences" page

Scenario: Create a new consequence
	When I click the "Consequences" nav button
	And I click the "Add Consequence" button
	And I fill in the consequence form with name "Test Consequence E2E" and description "E2E test description"
	And I click the "Add" button
	Then I should be on the "/consequences" page
	And I should see "Test Consequence E2E" on the page

Scenario: Delete a consequence
	When I click the "Consequences" nav button
	And I create a consequence with name "Delete Me E2E" and description "To be deleted"
	Then I should see "Delete Me E2E" on the page
	When I delete the consequence "Delete Me E2E"
	Then I should not see "Delete Me E2E" on the page
