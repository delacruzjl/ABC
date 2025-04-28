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
	And Last Name is "Fake Last"
	And First name is "Fake First"
	And Age is 13
	When Validating child
	Then validation should be true

@child
Scenario: Child with negative value age should be invalid
	Given I create a Child entity
	And Last Name is "Fake Last Name"
	And First name is "Fake First"
	And Age is -20
	When Validating child
	Then validation should be false
