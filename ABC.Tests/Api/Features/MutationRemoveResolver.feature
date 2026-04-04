Feature: MutationRemoveResolver

As a user
I want to be able to remove rows from the database
So that I can maintain a clean list of data


@antecedents
Scenario: Should request a handler for RemoveAntecedentCommand
	Given I have a valid RemoveAntecedentCommand request
	When I send the request to delete the antecedent
	Then I should send a request to the RemoveAntecedentCommand handler
	
@behaviors
Scenario: Should request a handler for RemoveBehaviorCommand
	Given I have a valid RemoveBehaviorCommand request
	When I send the request to delete the behavior
	Then I should send a request to the RemoveBehaviorCommand handler
	
@consequences
Scenario: Should request a handler for RemoveConsequenceCommand
	Given I have a valid RemoveConsequenceCommand request
	When I send the request to delete the consequence
	Then I should send a request to the RemoveConsequenceCommand handler
	
@children
Scenario: Should request a handler for RemoveChildCommand
	Given I have a valid RemoveChildCommand request
	When I send the request to delete the child
	Then I should send a request to the RemoveChildCommand handler