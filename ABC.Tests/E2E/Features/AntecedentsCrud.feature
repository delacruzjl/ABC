@e2e
Feature: Antecedents CRUD

As an admin
I want to manage antecedents through the UI
So that I can maintain the list of antecedent options for observations

Background:
	Given I am logged in as an admin

Scenario: Navigate to antecedents page
	When I click the "Antecedents" nav button
	Then I should be on the "/antecedents" page

Scenario: Create a new antecedent
	When I click the "Antecedents" nav button
	And I click the "Add Antecedent" button
	And I fill in the antecedent form with name "Test Antecedent E2E" and description "E2E test description"
	And I click the "Add" button
	Then I should be on the "/antecedents" page
	And I should see "Test Antecedent E2E" on the page

Scenario: Delete an antecedent
	When I click the "Antecedents" nav button
	And I create an antecedent with name "Delete Me E2E" and description "To be deleted"
	Then I should see "Delete Me E2E" on the page
	When I delete the antecedent "Delete Me E2E"
	Then I should not see "Delete Me E2E" on the page
