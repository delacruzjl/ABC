Feature: AntecedentValidator

As a user
I want to be notified when I try to enter invalid antecedent data
so that I can keep data collection clean

@antecedents
Scenario: Antecedent contains no name should fail validation
	Given An antecedent with empty name
	When validating
	Then Should throw validation exception

@antecedents
Scenario: Antecedent contains no description should fail validation
	Given An antecedent with attributes "Fake Name" and empty description
	When validating
	Then Should throw validation exception

@antecedents
Scenario: Antecedent contains a name that already exists should fail
	Given An antecedent with Id "20f83509-d656-497c-8085-6ec30f743715", and attributes  "Fake Name" and "Fake Description"
	And Antecedent with the same name exists
	When validating
	Then Should throw validation exception
