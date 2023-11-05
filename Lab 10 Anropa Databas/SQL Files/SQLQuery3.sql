SELECT Customers.CustomerID, CompanyName, Orders.OrderID, Quantity FROM Customers
JOIN Orders ON Customers.CustomerID = Orders.CustomerID
JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID

