Feature: Mutation

As a user
I want to be able to apply mutations to objects
so that I can perform CRUD operations on entities

@antecedents
Scenario: Should request a handler for CreateAntecedentCommand
	Given I have a valid CreateAntecedentCommand request
	When I send the request to the antecedents API
	Then I should send a request to the CreateAntecedentCommand handler
	
@behaviors
Scenario: Should request a handler for CreateBehaviorCommand
	Given I have a valid CreateBehaviorCommand request
	When I send the request to the behaviors API
	Then I should send a request to the CreateBehaviorCommand handler
	
@consequences
Scenario: Should request a handler for CreateConsequenceCommand
	Given I have a valid CreateConsequenceCommand request
	When I send the request to the consequences API
	Then I should send a request to the CreateConsequenceCommand handler
	
@children
Scenario: Should request a handler for CreateChildCommand
	Given I have a valid CreateChildCommand request
	When I send the request to the children API
	Then I should send a request to the CreateChildCommand handler