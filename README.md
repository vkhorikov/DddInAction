Welcome to DDD in Action
=====================

This is a sample project aimed to show how to apply DDD principles. The domain is rather simple: the software models behavior of a snack machine and ATM leaving behind such technical details as precessing credit card data and focusing on the core domain logic.

Although DDD is the main focus of the project, I also used TDD practices, MVVM pattern and [functional design principles][L1] to build it. In terms of technologies, the project relies on C#, WPF, NHibernate and SQL Server.

How to Get Started
--------------

In order to run the application, you need to [create database][L2] and change the connection string in the [composition root][L3]

[L1]: http://enterprisecraftsmanship.com/2015/03/02/functional-c-immutability/
[L2]: blob/master/DddInAction.DB/DBSchema.txt
[L3]: blob/master/DddInAction.Logic/Utils/Initer.cs