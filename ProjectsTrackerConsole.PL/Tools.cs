namespace ProjectsTrackerConsole.PL {
    public static class Tools {
        static int tableCellSize = 20;
        static int tableSmallCellSize = 5;
        static int comandCellsize = 20;
        public static void WriteColor(string input, ConsoleColor clr) {
            Console.ForegroundColor = clr;
            Console.WriteLine(input);
            Console.ResetColor();
        }

        //value -> [value...]
        public static string MakeCell(string input) {
            int spaceSize = tableCellSize > input.Length ? tableCellSize : input.Length;
 
            return $"[{input}{new('.', spaceSize - input.Length)}]";
        }

        //value -> [value...]
        public static string MakeCell(int input) {
            string tmp = input.ToString();
            int spaceSize = tableSmallCellSize > tmp.Length ? tableSmallCellSize/2 : tmp.Length;

            return $"[{new(' ', spaceSize + tmp.Length % 2)}{input}{new(' ', spaceSize)}]";
        }

        //cmd, key -> cmd [key]
        public static string MakeComandCell(string input, string keyCode) {
            int spaceSize = comandCellsize > input.Length ? comandCellsize : input.Length;
            return $" {input}{new(' ', spaceSize - input.Length)}[{keyCode}]\n";
        }

        public static string? ReadConsole(string str) {
            Console.Write(str);
            string? res = Console.ReadLine();
            if (res == String.Empty)
                return null;
            else
                return res;
        }

        public static void ShowPropsValue<T>(T input, List<string> ignorProps) {
            WriteColor("\n" + MakeCell("Field") + MakeCell("value"), ConsoleColor.Blue);
            foreach (var prop in input.GetType().GetProperties()) {
                if (ignorProps.Contains(prop.Name))
                    continue;
                Console.Write(MakeCell(prop.Name));
                string value = prop.GetValue(input, null)?.ToString() ?? "null";
                Console.WriteLine(MakeCell(value));
            }
        }

        public static int GetId(string str = "Id: ") {
            Console.Write(str);
            string id = Console.ReadLine();
            int numVal = 0;
            try {
                numVal = Int32.Parse(id);
            } catch (FormatException e) {
                Console.WriteLine(e.Message);
            }
            return numVal;
        }

        public static DateTime GetDate(string str = "Date") {
            string date = ReadConsole($"\nExample:20/12/2022\n{str}: ");
            DateTime res = new DateTime();
            try {
                res = DateTime.Parse(date);
                return res;
            } catch (FormatException e) {
                Tools.WriteColor("Incorrect date format", ConsoleColor.Red);
            }
            Console.WriteLine(res.ToString());
            return res;
        }

    }
}
