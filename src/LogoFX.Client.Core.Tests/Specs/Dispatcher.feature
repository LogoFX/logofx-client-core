Feature: Dispatcher
	In order to support multi-threaded UI apps
	As an app developer
	I want the framework to synchronize the UI-bound operations

Scenario: Changing single property with defined dispatcher should raise notifications via dispatcher
	Given The dispatcher is set to test dispatcher
	When The 'TestRegularClass' is created
	And The number is changed to 5 via SetProperty API
	Then The property change notification is raised via the test dispatcher

Scenario: Invoking all properties change with defined dispatcher should raise notification via dispatcher
	Given The dispatcher is set to test dispatcher
	When The 'TestNameClass' is created and empty notification is listened to
	And The all properties change is invoked
	Then The property change notification is raised via the test dispatcher