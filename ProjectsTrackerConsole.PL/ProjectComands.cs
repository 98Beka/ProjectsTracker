using ProjectsTracker.BLL.BusinessObjects;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTrackerConsole.PL.Interfaces;

namespace ProjectsTrackerConsole.PL {

    //Get Projects list
    internal class GetAllProjectsCommand : IPageCommand {
        public ConsoleKey Key { get; } = ConsoleKey.D1;
        public async Task ExecCommandAsync(IService service) {
            var cmd = new ConsoleKeyInfo();
            HeaderWriter.WriteHeader(Pages.Filter);
            cmd = Console.ReadKey();
            while (cmd.Key != ConsoleKey.Backspace) {
                HeaderWriter.WriteHeader(Pages.Filter);
                string header = "[  id  ]" + Tools.MakeCell("Project name");
                IEnumerable<ProjectDTO> entitys;
                switch (cmd.Key) {

                    case ConsoleKey.D2:
                        Tools.WriteColor(header + Tools.MakeCell("Priority"), ConsoleColor.Blue);
                        entitys = service.GetProjects(new ProjectFilterByPriority());
                        foreach (var e in entitys)
                            Console.WriteLine(Tools.MakeCell(e.Id) + Tools.MakeCell(e.Name) + Tools.MakeCell(e.Priority.ToString()));
                        break;

                    case ConsoleKey.D3:
                        entitys = service.GetProjects(new ProjectFilterByStartDate());
                        Tools.WriteColor(header + Tools.MakeCell("Start Date"), ConsoleColor.Blue);
                        foreach (var e in entitys)
                            Console.WriteLine(Tools.MakeCell(e.Id) + Tools.MakeCell(e.Name) + Tools.MakeCell(e.Start.ToShortDateString()));
                        break;

                    case ConsoleKey.D4:
                        entitys = service.GetProjects(new ProjectFilterByStartDateRange(Tools.GetDate("Date from"), Tools.GetDate("Date until")));
                        Tools.WriteColor(header + Tools.MakeCell("Start Date"), ConsoleColor.Blue);
                        foreach (var e in entitys)
                            Console.WriteLine(Tools.MakeCell(e.Id) + Tools.MakeCell(e.Name) + Tools.MakeCell(e.Start.ToShortDateString()));
                        break;
                    default:
                        entitys = service.GetProjects(new ProjctFilterNone());
                        Tools.WriteColor(header, ConsoleColor.Blue);
                        foreach (var e in entitys)
                            Console.WriteLine(Tools.MakeCell(e.Id) + Tools.MakeCell(e.Name));
                        break;
                }
                cmd = Console.ReadKey();
            }
        }
    }
    //Add Project
    internal class ProjectAddCommand : IPageCommand {
        public ConsoleKey Key { get; } = ConsoleKey.D3;

        public async Task ExecCommandAsync(IService service) {
            ProjectDTO proj = new ProjectDTO();
            proj.Start = DateTime.Today;
            new Editor().ChangePropValue(proj, new List<string> { "Employees", "TeamLead", "Id" });
            try {
                await service.AddOrEditProjectAsync(proj);
            } catch {
                Tools.WriteColor("Incorect project format", ConsoleColor.Red);
            Console.ReadKey(true);
                return;
            }
            HeaderWriter.WriteHeader(Pages.Projects);
        }
    }
    //Delete Project 
    internal class DeleteProjectCommand : IPageCommand {
        public ConsoleKey Key { get; } = ConsoleKey.D4;

        public async Task ExecCommandAsync(IService service) {
            try {
               await service.DeleteProjectAsync(Tools.GetId());
            } catch (Exception ex) {
                Tools.WriteColor(ex.Message, ConsoleColor.Red);
                Console.ReadKey(true);
                return;
            }
        }
    }
    //GetById
    internal class GetProjectByIdCommand : IPageCommand {
        public ConsoleKey Key { get;} = ConsoleKey.D2;

        private List<string> ignorProps = new List<string> { "Employees", "TeamLead" };

        public async Task ExecCommandAsync(IService service) {
            var cmd = new ConsoleKeyInfo();
            ProjectDTO entity = null;
            try {
                entity = await service.GetProjectAsync(Tools.GetId());
            } catch (Exception ex) {
                Tools.WriteColor(ex.Message, ConsoleColor.Red);
                Console.ReadKey(true);
                return;
            }

            while (cmd.Key != ConsoleKey.Backspace) {
                TypeInterface(entity);
                cmd = Console.ReadKey(true);
                switch (cmd.Key) {

                    case ConsoleKey.D1:
                        new Editor().ChangePropValue(entity, ignorProps);
                        try {
                            service.AddOrEditProjectAsync(entity);
                        } catch {
                            Tools.WriteColor("Incorect project format", ConsoleColor.Red);
                            Console.ReadKey(true);
                            return;
                        }
                        break;

                    case ConsoleKey.D2:
                        await service.TieEmployeeProjectAsync(entity.Id, Tools.GetId("EmployeeId: "));
                        entity = await service.GetProjectAsync(entity.Id);
                        break;
                    case ConsoleKey.D3:
                        int EmployeeId = Tools.GetId("EmployeeId: ");
                        var employee = entity.Employees?.FirstOrDefault(e => e.Id == EmployeeId);
                        if (employee == null) {
                            Console.WriteLine($"There isn't an employee with this id in {entity.Name.ToUpper()}");
                            Console.ReadKey(true);
                        } else {
                            await service.AppointTeamleadAsync(entity.Id, EmployeeId);
                            entity = await service.GetProjectAsync(entity.Id);
                        }
                        break;
                    case ConsoleKey.D4:
                        await service.SeparateEmployeeProjectAsync(entity.Id, Tools.GetId("EmployeeId: "));
                        entity = await service.GetProjectAsync(entity.Id);
                        break;
                }
            }
            HeaderWriter.WriteHeader(Pages.Projects);
        }
        public void TypeInterface(ProjectDTO entity) {

            HeaderWriter.WriteHeader(Pages.Project);
            Tools.ShowPropsValue(entity, ignorProps);

            Tools.WriteColor("Employees:", ConsoleColor.Blue);
            Tools.WriteColor(Tools.MakeCell("Employee Name") + "[  id  ]", ConsoleColor.Blue);
            if (entity.Employees != null)
                foreach (var e in entity.Employees)
                    Console.WriteLine(Tools.MakeCell(e.Name) + Tools.MakeCell(e.Id));

            Console.WriteLine();
            Tools.WriteColor(Tools.MakeCell("Teamlead name") + "[  id  ]", ConsoleColor.Blue);
            if (entity.TeamLead == null)
                Console.WriteLine("not found");
            else
                Console.WriteLine(Tools.MakeCell(entity.TeamLead.Name) + Tools.MakeCell(entity.TeamLead.Id));
        }
    }
}