namespace ProjectsTrackerConsole.PL {
    public enum Pages {
        Menu,
        Projects,
        Project,
        Editor,
        Employees,
        Employee,
        Filter
    }
    public static class HeaderWriter {
        public static void WriteHeader(Pages pg) {
            Console.Clear();
            switch (pg) {

                case Pages.Menu:
                    Tools.WriteColor("MainMenu:", ConsoleColor.Green);
                    Tools.WriteColor(
                       Tools.MakeComandCell("projects", "1") +
                       Tools.MakeComandCell("employees", "2") +
                       Tools.MakeComandCell("exit", "escape"),
                       ConsoleColor.Blue);
                    break;

                case Pages.Projects:
                    Tools.WriteColor("Projects:", ConsoleColor.Green);
                    Tools.WriteColor(
                       Tools.MakeComandCell("show all", "1") +
                       Tools.MakeComandCell("show one", "2") +
                       Tools.MakeComandCell("add", "3") + 
                       Tools.MakeComandCell("delete", "4") +
                       Tools.MakeComandCell("close projects", "backspace"),
                       ConsoleColor.Blue);
                    break;
                
                case Pages.Editor:
                    Tools.WriteColor("Editor:", ConsoleColor.Green);
                    Tools.WriteColor(
                      Tools.MakeComandCell("edit", "enter") +
                      Tools.MakeComandCell("close editor", "backspace"),
                      ConsoleColor.Blue);                    
                    break;

                case Pages.Project:
                    Tools.WriteColor("Project:", ConsoleColor.Green);
                    Tools.WriteColor(
                      Tools.MakeComandCell("edit project", "1") +
                      Tools.MakeComandCell("add employee", "2") +
                      Tools.MakeComandCell("appint a teamleader", "3") +
                      Tools.MakeComandCell("delete employee", "4") +
                      Tools.MakeComandCell("close project", "backspace"),
                      ConsoleColor.Blue);
                    break;

                case Pages.Employees:
                    Tools.WriteColor("Employees:", ConsoleColor.Green);
                    Tools.WriteColor(
                       Tools.MakeComandCell("show all", "1") +
                       Tools.MakeComandCell("show one", "2") +
                       Tools.MakeComandCell("add", "3") +
                       Tools.MakeComandCell("delete", "4") +
                       Tools.MakeComandCell("close projects", "backspace"),
                      ConsoleColor.Blue);
                    break;

                case Pages.Employee:
                    Tools.WriteColor("Project:", ConsoleColor.Green);
                    Tools.WriteColor(
                      Tools.MakeComandCell("edit employee", "1") +
                      Tools.MakeComandCell("add project", "2") +
                      Tools.MakeComandCell("delete project", "3") +
                      Tools.MakeComandCell("close employee", "backspace"),
                      ConsoleColor.Blue);
                    break;
                
                case Pages.Filter:
                    Tools.WriteColor(
                      Tools.MakeComandCell("without sorting", "1") +
                      Tools.MakeComandCell("Sort by priority", "2") +
                      Tools.MakeComandCell("Sort by date", "3") +
                      Tools.MakeComandCell("Sort by date range", "4") +
                      Tools.MakeComandCell("close sorting", "backspace"),
                      ConsoleColor.Blue);
                    break;
            }
        }
    }
}
