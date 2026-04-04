Feature: ChildValidator

As a parent
I want to be able to create children
So that I can assign ABC data to it

@child
Scenario: Child with empty last name should be invalid
	Given I create a Child entity
	And Last Name is empty
	When Validating child
	Then validation should be false

@child
Scenario: Child with all required fields should be valid
	Given I create a Child entity
	And Last Name is "Fake Last 1"
	And First name is "Fake First 1"
	And birth year is 2000
	And all conditions are found
	When Validating child
	Then validation should be true

@child
Scenario: Child with negative value birth year should be invalid
	Given I create a Child entity
	And Last Name is "Fake Last Name 2"
	And First name is "Fake First 2"
	And birth year is 1915
	When Validating child
	Then validation should be false
