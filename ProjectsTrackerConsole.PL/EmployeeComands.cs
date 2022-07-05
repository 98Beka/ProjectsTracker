using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTrackerConsole.PL.Interfaces;

namespace ProjectsTrackerConsole.PL {
    //Get Employees list
    internal class GetAllEmployeeCommand : IPageCommand {
        public ConsoleKey Key { get; } = ConsoleKey.D1;

        public async Task ExecCommandAsync(IService service) {
            var entitys =  service.GetEmployees();
            foreach (var e in entitys)
                Console.WriteLine(Tools.MakeCell(e.Name) + Tools.MakeCell(e.Id));
            Console.ReadKey(true);
        }
    }

    //Add Employee
    internal class EmployeeAddCommand : IPageCommand {
        public ConsoleKey Key { get; } = ConsoleKey.D3;

        public async Task ExecCommandAsync(IService service) {
            var emp = new EmployeeDTO();
            new Editor().ChangePropValue(emp, new List<string> { "Projects", "ProjectsAsLead", "Id" });
            try {
                await service.AddOrEditEmployeeAsync(emp);
            } catch {
                Tools.WriteColor("Incorect employee format", ConsoleColor.Red);
                Console.ReadKey(true);
                return;
            }
        }
    }

    //Delete Employee 
    internal class DeleteEmployeeCommand : IPageCommand {
        public ConsoleKey Key { get; } = ConsoleKey.D4;

        public async Task ExecCommandAsync(IService service) {
            try {
                await service.DeleteEmployeeAsync(Tools.GetId());
            } catch (Exception ex) {
                Tools.WriteColor(ex.Message, ConsoleColor.Red);
                Console.ReadKey(true);
                return;
            }
        }
    }

    //GetById
    internal class GetEmployeeByIdCommand : IPageCommand {
        public ConsoleKey Key { get; } = ConsoleKey.D2;

        private List<string> ignorProps = new List<string> { "Projects", "ProjectsAsLead" };

        public async Task ExecCommandAsync(IService service) {
            var cmd = new ConsoleKeyInfo();
            EmployeeDTO entity = null;
            try {
                entity = await service.GetEmployeeAsync(Tools.GetId());
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
                       new  Editor().ChangePropValue(entity, ignorProps);
                        try {
                            await service.AddOrEditEmployeeAsync(entity);
                        } catch {
                            Tools.WriteColor("Incorect employee format", ConsoleColor.Red);
                            Console.ReadKey(true);
                            return;
                        }
                        entity = await service.GetEmployeeAsync(entity.Id);
                        break;
                    case ConsoleKey.D2:
                        await service.TieEmployeeProjectAsync(entity.Id, Tools.GetId("ProjectId: "));
                        entity = await service.GetEmployeeAsync(entity.Id);
                        break;

                    case ConsoleKey.D3:
                        await service.SeparateEmployeeProjectAsync(entity.Id, Tools.GetId("ProjectId: "));
                        entity = await service.GetEmployeeAsync(entity.Id);
                        break;
                }
            }
            HeaderWriter.WriteHeader(Pages.Employees);
        }

        public void TypeInterface(EmployeeDTO entity) {
            HeaderWriter.WriteHeader(Pages.Employee);
            Tools.ShowPropsValue(entity, ignorProps);
            Tools.WriteColor("Projects:", ConsoleColor.Blue);
            Tools.WriteColor(Tools.MakeCell("Project Name") + "[  id  ]", ConsoleColor.Blue);
            if (entity.Projects != null)
                foreach (var p in entity.Projects)
                    Console.WriteLine(Tools.MakeCell(p.Name) + Tools.MakeCell(p.Id));
        }
    }
}