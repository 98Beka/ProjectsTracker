using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Interfaces;

namespace ProjectsTracker.BLL.BusinessObjects {

    public class ProjctFilterNone : IProjectFilter {
        public List<ProjectDTO> Filtrate(List<ProjectDTO> input) {
            return input;
        }
    }

    public class ProjectFilterByPriority : IProjectFilter {
        public List<ProjectDTO> Filtrate(List<ProjectDTO> input) {
            var tmp = new List<ProjectDTO>();
            foreach (var p in input)
                tmp.Add(p);
            tmp.Sort((x, y) => x.Priority.Value.CompareTo(y.Priority.Value));

            for (int i = 0; i < tmp.Count; i++) {
                if (tmp[0].Priority == 0) {
                    var proj = tmp[0];
                    for (int j = 0; j < tmp.Count - 1; j++)
                        tmp[j] = tmp[j + 1];
                    tmp[tmp.Count - 1] = proj;
                }
            }
            return tmp;
        }
    }

    public class ProjectFilterByStartDate : IProjectFilter {
        public List<ProjectDTO> Filtrate(List<ProjectDTO> input) {

            List<ProjectDTO> tmp = new List<ProjectDTO>();
            foreach (var p in input)
                tmp.Add(p);
            tmp.Sort((x, y) => x.Start.CompareTo(y.Start));
            return tmp;
        }
    }

    public class ProjectFilterByStartDateRange : IProjectFilter {
        DateTime from;
        DateTime until;
        public ProjectFilterByStartDateRange(DateTime from, DateTime until) {
            this.from = from;
            this.until = until;
        }
        public List<ProjectDTO> Filtrate(List<ProjectDTO> input) {

            return new ProjectFilterByStartDate()
                .Filtrate(input.FindAll(p => p.Start >= from && p.Start <= until));
        }
    }

}