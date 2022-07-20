# albelli assessment project
This project is a response to albelli assessment task, 
it accepts an order, stores it, and responds with the minimum bin width.

# Prerequisites
- dotnet 6.0

# Compile and Run
- Check out this source to your machine
- Follow the below steps to build the project
- run `dotnet restore`
- run `dotnet build`
- run `dotnet run --project .\src\WebAPI\WebAPI.csproj`


# Note:

I deviated a little bit from the albelli assessment task in some places because I thought descriptions are not correct or accurate at least,
For example in the  **Order information submitted by customers** part getting `OrderId` from the customer didn't make sense.
So instead After saving the order I'll return the OrderId along with the minimum bin width.

Or It was much easier to accept product types as strings and have a simple validation rule to check if the product type is valid.
but to achieve a dynamic and scalable solution I stored product types in a separate table and used product Ids instead.


# Improvements (TODO)

- **Better error handling**
- CRUD for products
- Add FluentValidation rules
- Add user/customer information to the orders
- Add more tests for full coverage
- Add pagination to the orders and product list

*Unfortunately, I haven't found the time to finish this project the way I wanted and I spent only two days on this repo, but if I had more time I would've continued with the above improvements.*
