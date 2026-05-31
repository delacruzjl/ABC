Feature: EntityTranslationValidator

As a user
I want to be notified when I try to enter invalid translation data
so that multilanguage content stays valid

@translations
Scenario: Entity translation with valid values should pass validation
	Given A valid entity translation
	When validating the entity translation
	Then entity translation validation should succeed

@translations
Scenario: Entity translation with empty language should fail validation
	Given An entity translation with an empty language
	When validating the entity translation
	Then entity translation validation should fail for "Language"

@translations
Scenario: Entity translation with empty name should fail validation
	Given An entity translation with an empty name
	When validating the entity translation
	Then entity translation validation should fail for "Name"

@translations
Scenario: Entity translation with empty description should fail validation
	Given An entity translation with an empty description
	When validating the entity translation
	Then entity translation validation should fail for "Description"

@translations
Scenario: Entity translation with empty entity id should fail validation
	Given An entity translation with an empty entity id
	When validating the entity translation
	Then entity translation validation should fail for "EntityId"
