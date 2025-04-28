Feature: BehaviorValidator

As a user
I want to be notified when I try to enter invalid behavior data
so that I can keep data collection clean

@behaviors
Scenario: Behavior contains a name that already exists should fail
	Given A Behavior with Id "20f83509-d656-497c-8085-6ec30f743715", and attributes  "Fake Name" and "Fake Description"
	And behavior name already exists
	When validating the behavior
	Then Should fail behavior validation
