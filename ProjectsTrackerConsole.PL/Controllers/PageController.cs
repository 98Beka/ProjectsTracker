using ProjectsTracker.BLL.Interfaces;
using ProjectsTrackerConsole.PL.Interfaces;

namespace ProjectsTrackerConsole.PL.Controllers {
    internal class PageController {
        private List<IPageCommand> pageCommands;
        private IService service;
        public PageController(IService service, List<IPageCommand> pageCommands) {
            this.pageCommands = pageCommands;
            this.service = service;
        }

        public async Task OpenPageAsync(Pages pagesEnum) {
            ConsoleKeyInfo cmd = new ConsoleKeyInfo();

            while (cmd.Key != ConsoleKey.Backspace) { 
                HeaderWriter.WriteHeader(pagesEnum);
                cmd = Console.ReadKey(true);

                try {
                    foreach (var pgCommand in pageCommands) {
                        if (pgCommand.Key == cmd.Key)
                            await pgCommand.ExecCommandAsync(service);
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            } 
        }

    }
}
