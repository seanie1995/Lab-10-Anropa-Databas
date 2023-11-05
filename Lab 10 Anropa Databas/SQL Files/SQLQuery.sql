SELECT CustomerID, Count(*) AS OrderCount FROM Orders
Group by CustomerID  
