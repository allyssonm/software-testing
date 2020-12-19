Feature: Order - Add Item to Order
	As an user
	I want to add an item to order
	So that I can buy it later

Scenario: Add item to order sucessfully
	Given the user is already logged in
	And a product is in showcase
	And it is available on stock
	When the user adds an unit to order
	Then the user will be redirected to the purchase summary
	And the order's total price will be exactly the same as the added item
