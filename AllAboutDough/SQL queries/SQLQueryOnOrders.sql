USE AllAboutDough;

SELECT * FROM orderUpdated;

SELECT DISTINCT OrderDate FROM orderUpdated WHERE OrderDate between '01/07/2018' and '02/07/2018';

SELECT Topping AS NonVegetarianToppings FROM orderUpdated WHERE IsToppingVegetarian = 'false';
SELECT COUNT (*) AS ThisManyPizzaSoldInTotal FROM orderUpdated where OrderDate between '01/07/2018' and '01/07/2018 23:59:59';
SELECT COUNT (*) AS VegetarianPizzasSoldOnFirstOfJuly FROM orderUpdated WHERE IsToppingVegetarian = 'true' OR IsToppingVegetarian = 'false' AND OrderDate between '01/07/2018' and '01/07/2018 23:59:59';
SELECT
(
SELECT DISTINCT CONVERT(date,OrderDate) 
FROM orderUpdated
WHERE CONVERT(date, OrderDate) = '01/07/2018') AS DatePeriodOfReport,
(
SELECT COUNT (orderDate)
FROM orderUpdated
WHERE IsToppingVegetarian = 'false'
AND OrderDate between '01/07/2018 18:00:00' and '01/07/2018 23:59:59') AS NumberOfNonVegetarianPizzasSold,
(
SELECT COUNT (OrderDate)
FROM orderUpdated 
WHERE IsToppingVegetarian = 'true' 
AND OrderDate between '01/07/2018' and '02/07/2018 23:59:59') AS NumberOfVegetarianPizzasSold;