using Microsoft.AspNetCore.Mvc.Filters;
using ProjectsTracker.BLL.BusinessObjects;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.PL.Abstracs;

namespace ProjectsTracker.PL {
    public class ProjectFilterContainer : FilterContainer<IProjectFilter> {
        public override IProjectFilter Filter { get; set; }
    }
}
