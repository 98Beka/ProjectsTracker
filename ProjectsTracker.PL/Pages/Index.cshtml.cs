using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectsTracker.PL.ViewModels;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.BusinessObjects;
using AutoMapper;

namespace ProjectsTracker.PL.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapperToView;
        private readonly IMapper _mapperToDTO;
        public  List<ProjectView>? Projects { get; set; }

        public ProjectView? Project { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger, IProjectService projectService) {
            _logger = logger;
            _projectService = projectService;
            _mapperToView = new MapperConfiguration(cnf => cnf.CreateMap<ProjectDTO, ProjectView>()).CreateMapper();
            _mapperToDTO = new MapperConfiguration(cnf => cnf.CreateMap<ProjectView, ProjectDTO>()).CreateMapper();
            Projects = _mapperToView.Map<List<ProjectView>>(_projectService.GetProjects(new ProjectFilterNone()));
            Project = Projects?.FirstOrDefault();
        }

        

        public void OnGet(int id) {
            Project = Projects[id];
        }

        public  void OnPostShow(int id) {
            if(Projects != null)
                Project = Projects[id];
        }

        public async Task<IActionResult> OnPostSaveChangesAsync() {
            if (!ModelState.IsValid)
                return Page();
            Project.Name = "ivan";
           
            await _projectService.AddOrEditProjectAsync(_mapperToDTO.Map<ProjectDTO>(Project));
            return Page();
        }
    }
}