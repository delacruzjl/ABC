Feature: Antecedent

As an Engineer 
I want to compare two antecedents 
so that I can successfully match objects with the same Id.

@antecedents
Scenario: Antecedents with same Id should match with Equals method
	Given An antecedent with Id "20f83509-d656-497c-8085-6ec30f743715"
	And another antecedent with Id "20f83509-d656-497c-8085-6ec30f743715"
	When Comparing objects by id
	Then Equals should be True

@antecedents
Scenario: Antecedents with same Id should match with operator
	Given An antecedent with Id "20f83509-d656-497c-8085-6ec30f743715"
	And another antecedent with Id "20f83509-d656-497c-8085-6ec30f743715"
	When Comparing objects with equals operator
	Then Equals should be True

@antecedents
Scenario: Antecedents with different Ids should NOT match using operator
	Given An antecedent with Id "20f83509-d656-497c-8085-6ec30f743715"
	And another antecedent with Id "a023b8f1-1354-4cd6-a4a3-49ecd2c1cded"
	When Comparing objects with different operator
	Then Different should be True