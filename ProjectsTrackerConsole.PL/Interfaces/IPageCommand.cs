using ProjectsTracker.BLL.Interfaces;

namespace ProjectsTrackerConsole.PL.Interfaces {
    internal interface IPageCommand {
        public ConsoleKey Key { get; }
        public Task ExecCommandAsync(IService service);
    }
}
