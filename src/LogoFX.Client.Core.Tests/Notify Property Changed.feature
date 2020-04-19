Feature: Notify Property Changed
	In order to have reactive user interface
	As an app developer
	I want the framework to handle property notifications properly

Scenario Outline: Single property change in regular mode should raise property change notification
	When The '<Name>' is created
	And The number is changed to 5  in regular mode
	Then The property change notification result is '<Result>'

Examples:
| Name                  | Result |
| TestNameClass         | true   |
| TestPropertyInfoClass | true   |
| TestExpressionClass   | true   |

Scenario Outline: Single property change in silent mode should not raise property change notification
	When The '<Name>' is created
	And The number is changed to 5 in silent mode
	Then The property change notification result is '<Result>'

Examples:
| Name                  | Result |
| TestNameClass         | false  |
| TestPropertyInfoClass | false  |
| TestExpressionClass   | false  |

Scenario: Invoking all properties change should raise empty property change notification
	When The 'TestNameClass' is created and empty notification is listened to
	And The all properties change is invoked
	Then The property change notification result is 'true'

Scenario: Changing single property via SetProperty API should raise property change notification
	When The 'TestRegularClass' is created
	And The number is changed to 5 via SetProperty API
	Then The property change notification result is 'true'

