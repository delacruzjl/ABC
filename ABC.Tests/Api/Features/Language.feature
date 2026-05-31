Feature: Language

As an authenticated user
I want to manage my preferred language and view translated antecedents
so that multilanguage content matches my settings

@language
Scenario: Update preferred language with supported value should succeed
	Given the current user has preferred language "en"
	And the requested preferred language is "es"
	When I update the preferred language
	Then the preferred language update should succeed
	And the persisted preferred language should be "es"

@language
Scenario: Update preferred language with unsupported value should fail
	Given the current user has preferred language "en"
	And the requested preferred language is "fr"
	When I update the preferred language
	Then the preferred language update should fail with unsupported language

@translatedAntecedents
Scenario: Get translated antecedents returns Spanish translations when user language is es
	Given the current user has preferred language "es"
	And an antecedent exists with English values
	And a Spanish translation exists for the antecedent
	When I request translated antecedents
	Then the antecedent should be returned in Spanish

@translatedAntecedents
Scenario: Get translated antecedents returns English values when user language is en
	Given the current user has preferred language "en"
	And an antecedent exists with English values
	When I request translated antecedents
	Then the antecedent should be returned in English
