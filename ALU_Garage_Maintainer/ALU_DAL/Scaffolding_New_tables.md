### ***Example that scaffolds only selected tables 
### ***and creates the context in a separate folder 
### ***with a specified name and namespace:***



Scaffold-DbContext "Server=(localdb)\\mssqllocaldb;
					Database=Blogging;Trusted\_Connection=True;" 
					-Provider Microsoft.EntityFrameworkCore.SqlServer 
					-OutputDir Models -Tables "Blog","Post" 
					-ContextDir Context 
					-Context BlogContext 
					-ContextNamespace New.Namespace



Modify the above and use the following in the PM console and you can create the context for the rest of the new tables that you would be creating along the way.

Scaffold-DbContext "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ALU;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models

Scaffold-DbContext "Data source=(localdb)\MSSQLLocalDB;Initial catalog = ALU;" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -F
