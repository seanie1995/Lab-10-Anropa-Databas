SELECT ProductName, UnitPrice, Categories.CategoryName FROM Products
JOIN Categories ON Products.CategoryID = Categories.CategoryID
ORDER BY CategoryName

SELECT ProductName, UnitPrice, Categories.CategoryName FROM Products
JOIN Categories ON Products.CategoryID = Categories.CategoryID
ORDER BY ProductName

SELECT Customers.CompanyName, Count(*) AS OrderCount FROM Orders
JOIN Customers ON Orders.CustomerID = Customers.CustomerID
GROUP BY CompanyName
ORDER BY OrderCount DESC 

SELECT LastName + ' ' + FirstName AS FullName, Employees.EmployeeID, EmployeeTerritories.TerritoryID, TerritoryDescription FROM EmployeeTerritories
JOIN Territories ON EmployeeTerritories.TerritoryID = Territories.TerritoryID
JOIN Employees ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
ORDER BY FullName