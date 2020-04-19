Feature: Notify Property Changed
	In order to have reactive user interface
	As an app developer
	I want the framework to handle property notifications properly

Scenario Outline: Single property change raises property change notification for supported cases
	When The '<Name>' is created
	And The number is changed to 5
	Then The property notification result is '<Result>'

Examples:
| Name                  | Result |
| TestNameClass         | true   |
| TestPropertyInfoClass | true   |
| TestExpressionClass   | true   |
