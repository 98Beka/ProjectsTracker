using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectsTracker.PL.ViewModels;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.BusinessObjects;
using AutoMapper;
using ProjectsTracker.PL.Abstracs;

namespace ProjectsTracker.PL.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private static FilterContainer<IProjectFilter> _filterContainer;
        public  List<ProjectView> Projects { get; set; }
        public  List<EmployeeView> Employees { get; set; } 

        [BindProperty]
        public ProjectView Project { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IProjectService projectService, IEmployeeService employeeService, IMapper mapper, FilterContainer<IProjectFilter> filterContainer) {
            _filterContainer = filterContainer;
            _logger = logger;
            _projectService = projectService;
            _employeeService = employeeService;
            _mapper = mapper;
            Projects = _mapper.Map<List<ProjectView>>(_projectService.GetProjects(_filterContainer.Filter));
            Employees = _mapper.Map<List<EmployeeView>>(_employeeService.GetEmployees());
        }

        

        public void OnGet() =>
            Project = Projects.FirstOrDefault();


        public void OnGetShow(int id) =>
            Project = Projects.FirstOrDefault(p => p.Id == id);


        public void OnPostShow(int id) =>
            Project = Projects.FirstOrDefault(p => p.Id == id);


        public async Task<IActionResult> OnPostSaveChangesAsync(int id) {
            if (!ModelState.IsValid)
                return Page();
            var olProj = await _projectService.GetProjectAsync(id);
            Project.Id = id;
            _mapper.Map(Project, olProj);
            await _projectService.AddOrEditProjectAsync(olProj);
            _mapper.Map<ProjectDTO, ProjectView>(olProj, Projects.FirstOrDefault(p => p.Id == id));
            Response.Redirect($"/?id={id}&handler=show");
            return Page();
        }

        public async Task<IActionResult> OnPostAddProjectAsync() {
            var proj = new ProjectDTO();
            var res = await _projectService.AddOrEditProjectAsync(proj);
            Project = _mapper.Map<ProjectView>(res);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveProjectAsync(int id) {
            await _projectService.DeleteProjectAsync(id);
            Projects = _mapper.Map<List<ProjectView>>(_projectService.GetProjects(_filterContainer.Filter));
            return Page();
        }

        public IActionResult OnPostFiltrate(int filter) {
            switch(filter){

                case 1:
                    _filterContainer.Filter = new ProjectFilterByPriority();
                    break;
                case 2:
                    _filterContainer.Filter = new ProjectFilterByStartDate();
                    break;
                case 3:
                    _filterContainer.Filter = new ProjectFilterNone();
                    break;
            }
            Projects = _mapper.Map<List<ProjectView>>(_projectService.GetProjects(_filterContainer.Filter));
            Project = Projects.FirstOrDefault();
            return Page();
        }

        public async Task<List<EmployeeView>> GetProjectsEmployyeesAsync(int id) {
            var res = (await _projectService.GetProjectAsync(id)).Employees;
            return _mapper.Map<List<EmployeeView>>(res);
        }

        public async Task<IActionResult> OnPostBindEmployeeAsync(int id, int projectId) {
            Project = Projects.FirstOrDefault(p => p.Id == projectId);
            if (Project.Employees?.FirstOrDefault(e => e.Id == id) == null)
                await _projectService.TieEmployeeProjectAsync(projectId, id);
            else
                await _projectService.SeparateEmployeeProjectAsync(Project.Id, id);
            Response.Redirect($"/?id={projectId}&handler=show");
            return Page();
        }
    }
}