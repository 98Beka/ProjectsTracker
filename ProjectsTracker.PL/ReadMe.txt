to add migration by Package Manager Console, type this:
	1) Add-migration InitialCreate -Context ApplicationContext -Project ProjectsTracker.DAL -Verbose
	2) Update-database

don't forget to add ConnectionString in appsettings.json