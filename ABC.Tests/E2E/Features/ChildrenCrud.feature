@e2e
Feature: Children CRUD

As an admin
I want to manage children through the UI
So that I can maintain the list of children for observations

Background:
	Given I am logged in as an admin

Scenario: Navigate to children page
	When I click the "Children" nav button
	Then I should be on the "/children" page

Scenario: Create a new child
	When I click the "Children" nav button
	And I click the "+ Add Child" button
	And I fill in the child form with first name "E2EFirst" and last name "E2ELast" and birth year "2020"
	And I click the "Save" button
	Then I should be on the "/children" page
	And I should see "E2EFirst" on the page

Scenario: Edit a child
	When I click the "Children" nav button
	And I create a child with first name "EditFirst" and last name "EditLast" and birth year "2019"
	Then I should see "EditFirst" on the page
	When I click the edit button for child "EditFirst"
	And I update the child first name to "UpdatedFirst"
	And I click the "Save" button
	Then I should be on the "/children" page
	And I should see "UpdatedFirst" on the page

Scenario: Delete a child
	When I click the "Children" nav button
	And I create a child with first name "DeleteFirst" and last name "DeleteLast" and birth year "2018"
	Then I should see "DeleteFirst" on the page
	When I delete the child "DeleteFirst"
	Then I should not see "DeleteFirst" on the page
