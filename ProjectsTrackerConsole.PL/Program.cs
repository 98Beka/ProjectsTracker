using ProjectsTrackerConsole.PL;
using ProjectsTrackerConsole.PL.Controllers;
using ProjectsTrackerConsole.PL.Interfaces;
using Microsoft.Extensions.Configuration;
using ProjectsTracker.BLL.Services;
using ProjectsTracker.BLL.BusinessObjects;

Service service;
try {
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    Console.WriteLine("Loading...");
    service = new Service(configuration["ConnectionString"]);
    service.GetProjects(new ProjctFilterNone());
} catch {
    Tools.WriteColor("incorrect appsettings.json or ConnectionString in the appsettings.json", ConsoleColor.Red);
    Console.ReadKey();
    return;
}

var projectPage = new PageController(service, new List<IPageCommand> {
    new GetAllProjectsCommand(),
    new ProjectAddCommand(),
    new DeleteProjectCommand(),
    new GetProjectByIdCommand()
});


var employeePage = new PageController(service, new List<IPageCommand> {
    new GetAllEmployeeCommand(),
    new EmployeeAddCommand(),
    new DeleteEmployeeCommand(),
    new GetEmployeeByIdCommand()
});

//MainMen
while (true) {
    HeaderWriter.WriteHeader(Pages.Menu);
   
    switch (Console.ReadKey().Key) {
        case ConsoleKey.D1:
            await projectPage.OpenPageAsync(Pages.Projects);
            break;
        
        case ConsoleKey.D2:
            await employeePage.OpenPageAsync(Pages.Employees);
            break;        

        case ConsoleKey.Escape:
            Environment.Exit(0);
            break;
    }
}


