@e2e
Feature: Login

As a user
I want to be able to log in to the application
So that I can access the management features

Scenario: Admin can log in and see the dashboard
	Given I am logged in as an admin
	Then I should see "ABC Management" on the page
	And I should see "Dashboard" on the page

Scenario: Dev user can log in and see the dashboard
	Given I am logged in as a user with email "testuser@example.com"
	Then I should see "ABC Management" on the page
	And I should see "Dashboard" on the page

Scenario: Unauthenticated user is redirected to login
	When I navigate to "/"
	Then I should be on the "/login" page
