Feature: Query

as a user 
I want to be able to query data from the database
so that I can manage content

@antecedents
Scenario: should call antecedents from unit of work
	Given a query request for antecedents
	Then the antecedents from the unit of work should be executed

@behaviors
Scenario: should call behaviors from unit of work
	Given a query request for behaviors
	Then the hehaviors from the unit of work should be execute

@consequences
Scenario: should call consequences from unit of work
	Given a query request for consequences
	Then the consequences from the unit of work should be execute

@children
Scenario: should call children from unit of work
	Given a query request for children
	Then the children from the unit of work should be execute