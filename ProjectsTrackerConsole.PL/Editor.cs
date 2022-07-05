using ProjectsTracker.BLL.DataTransferObjects;
using System.Reflection;


namespace ProjectsTrackerConsole.PL {
    internal class Editor {

        public  void ChangePropValue(ProjectDTO input, List<string> ignorProps) {
            HeaderWriter.WriteHeader(Pages.Editor);
            Tools.ShowPropsValue(input, ignorProps);
            while (true) {
                Console.Clear();
                Tools.ShowPropsValue(input, ignorProps);

                Console.WriteLine("\nName is necessery");
                Console.WriteLine("\nto chage value write:");
                Tools.WriteColor("propertyName=Value\n", ConsoleColor.Blue);
                string? answer = Tools.ReadConsole("edditing...\n")?.ToLower();


                if (answer != "" && answer != null) {

                    var answerArr = answer.Split('=', StringSplitOptions.RemoveEmptyEntries);

                    if (answer.Contains("priority")) {
                        try {
                            input.Priority = Int32.Parse(answerArr[1]);
                        } catch { Console.WriteLine("Incorrect format of value"); }
                        continue;
                    }

                    if (answerArr.Contains("start") || answerArr.Contains("finish")) {

                        try {
                        if (answerArr.Contains("start"))
                            input.Start = DateTime.Parse(answerArr[1]);
                        else
                            input.Finish = DateTime.Parse(answerArr[1]);
                        continue;

                        } catch {
                            Console.WriteLine("Incorrect format of date");
                            Console.WriteLine("example:01/01/2020");
                            Console.ReadKey();
                        }
                    }  
 
                        //change properties
                    foreach (PropertyInfo prop in input.GetType().GetProperties()) {
                        if (ignorProps.Contains(prop.Name))
                            continue;
                        else if (prop.Name.ToLower() == answerArr[0].ToLower())
                            prop.SetValue(input, answerArr[1]);
                    }
                }
                HeaderWriter.WriteHeader(Pages.Editor);
                Tools.ShowPropsValue(input, ignorProps);
                ConsoleKeyInfo cmd = Console.ReadKey();
                if (cmd.Key == ConsoleKey.Backspace)
                    break;
            }
        }

        public EmployeeDTO ChangePropValue(EmployeeDTO input, List<string> ignorProps) {
            HeaderWriter.WriteHeader(Pages.Editor);
            Tools.ShowPropsValue(input, ignorProps);
            while (true) {
                Console.Clear();
                Tools.ShowPropsValue(input, ignorProps);
                Console.WriteLine("\nName is necessery");
                Console.WriteLine("\nto chage value write:");
                Tools.WriteColor("propertyName=Value\n", ConsoleColor.Blue);
                string? answer = Tools.ReadConsole("edditing...\n");


                if (answer != "" && answer != null) {
                    string[] answerArr = answer.Split('=', StringSplitOptions.RemoveEmptyEntries);

                    if (answerArr.Length == 2) {
                        //change properties
                        foreach (PropertyInfo prop in input.GetType().GetProperties()) {
                            if (ignorProps.Contains(prop.Name))
                                continue;
                            else if (prop.Name.ToLower() == answerArr[0].ToLower())
                                prop.SetValue(input, answerArr[1]);
                        }
                    }

                }
                HeaderWriter.WriteHeader(Pages.Editor);
                Tools.ShowPropsValue(input, ignorProps);
                ConsoleKeyInfo cmd = Console.ReadKey();
                if (cmd.Key == ConsoleKey.Backspace)
                    break;
            }
            return input;
        }
    }
}
