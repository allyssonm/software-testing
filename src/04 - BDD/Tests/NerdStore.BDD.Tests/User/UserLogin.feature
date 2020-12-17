Feature: User - Login
	As an user
	I want to login
	So that I can access all features

Scenario: Perform login successfully
	Given That the visitor is accessing the store website
	When He click on login
	And Fill in the login form data
			| Data             |
			| E-mail           |
			| Password         |
	And Click the login button
	Then He will be redirected to the showcase
	And A greeting with your email will be displayed in the top menu