Feature: User - Register
	As a store visitor
	I want to register an account for myself
	So that I can make purchases in the store

Scenario: Perform user register successfully
	Given That the visitor is accessing the store website
	When He click on register
	And Fill in the form data
			| Data             |
			| E-mail           |
			| Password         |
			| Password confirm |
	And Click the register button
	Then He will be redirected to the showcase
	And A greeting with your email will be displayed in the top menu

Scenario: Registration's password without capital letters
	Given That the visitor is accessing the store website
	When He click on register
	And Fill in the form data with a password without capital letters
			| Data             |
			| E-mail           |
			| Password         |
			| Password confirm |
	And Click the register button
	Then He will receive an error message that the password must contain a capital letter

Scenario: Registration's password without special characters
	Given That the visitor is accessing the store website
	When He click on register
	And Fill in the form data with a password without a special character
			| Data             |
			| E-mail           |
			| Password         |
			| Password confirm |
	And Click the register button
	Then He will receive an error message that the password must contain a special character
