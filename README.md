Running the application
STEP 1
The very first thing that should be done is adding the database if it is on a new machine, since it is local. 
Open the nuget package manager console(under tools) and type this command "Update-Database"
STEP 2
Then when that is done navigate to view and select "SQL Server Object explorer"
Click on it and on the left side navigate to the created database "ST10082744_PROG7311_POE_DB"
Go to tables and look for "dboAspNetUsers"
right click on it and view designer
in the middle "phonenumber" : under the column alllow nulls make sure this is tick
Do the same thing for Name and Surname
STEP 3 
once that is done click on update database and when it is done you can run the project
NB: if the default populated data does not work you can create your own :
=====
Employee:
admin@agri.co.za
Admin@123
=====
Farmer:
koosie@box.com
AppelKoos1!
=============================
What the prototype is supposed to do

The prototype is a demonstartion of our web page solution, There is a database that is connected that stores farmer and employee
 information and also product information farmers can register and logg in and add products to their profiles. Adding product with details like name, category and
production date.
Employees can add farmer profiles and register them to the database and can view each farmers product list and sort them by date and type 
Sample data is added to the database. There is a login feature where there are 2 different roles for both users and they cant view
spesific details if they do not have that spesific role.
Easy navigation and user friendly and can add new data in a few steps
=========================
References
1.Troelsen, A and Japikse, P. 2017. Pro C# 7. With .NET and .NET Core. 8th ed. Apress.
2.Freeman, A. (2017). Pro ASP.NET Core MVC 2. Apress.
3.Beghloyan, E. (n.d.). Pro Entity Framework Core 2 for ASP.NET Core MVC. www.academia.edu. [online] Available at: https://www.academia.edu/37022680/Pro_Entity_Framework_Core_2_for_ASP_NET_Core_MVC
4.Kurtz, J. (2013). ASP.NET MVC 4 and the Web API. www.academia.edu. [online] Available at: https://www.academia.edu/8044459/ASP_NET_MVC_4_and_the_Web_API
5.Peres, R. (2017). Mastering ASP.NET Core 2.0: MVC patterns, configuration, routing, deployment, and more. [online] Amazon. Packt Publishing. Available at: https://www.amazon.co.uk/Mastering-ASP-NET-Core-2-0-configuration/dp/1787283682
