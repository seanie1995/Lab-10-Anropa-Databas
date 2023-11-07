select CompanyName, ShippedDate from Orders
join Customers on Customers.CustomerId = Orders.CustomerId
order by CompanyName
	