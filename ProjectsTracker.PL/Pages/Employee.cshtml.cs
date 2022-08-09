using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectsTracker.BLL.DataTransferObjects;
using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.BLL.Profiles;
using ProjectsTracker.PL.ViewModels;

namespace ProjectsTracker.PL.Pages
{
    public class EmployeeModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        public List<EmployeeView>? Employees { get; set; }

        [BindProperty]
        public EmployeeView? Employee { get; set; } = default!;

        public EmployeeModel(IEmployeeService employeeService, IMapper mapper) {
            _employeeService = employeeService;
            _mapper = mapper;
            Employees = _mapper.Map<List<EmployeeView>>(_employeeService.GetEmployees());
        }

        public void OnGet() {
            Employee = Employees?.FirstOrDefault();

        }

        public void OnPostShow(int i) {
            if (Employees != null)
                Employee = Employees[i];
        }

        public async Task<IActionResult> OnPostSaveChangesAsync(int id) {
            if (!ModelState.IsValid)
                return Page();
            var oldEmployee = await _employeeService.GetEmployeeAsync(id);
            Employee.Id = id;
            _mapper.Map(Employee, oldEmployee);
            await _employeeService.AddOrEditEmployeeAsync(oldEmployee);
            _mapper.Map<EmployeeDTO, EmployeeView>(oldEmployee, Employees.FirstOrDefault(p => p.Id == id));
            return Page();
        }

        public async Task<IActionResult> OnPostAddEmployee() {
            var employee = new EmployeeDTO();
            var res = await _employeeService.AddOrEditEmployeeAsync(employee);
            Employee = _mapper.Map<EmployeeView>(res);
            Console.WriteLine(Employee.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveEmployee(int id) {
            await _employeeService.DeleteEmployeeAsync(id);
            Employees = _mapper.Map<List<EmployeeView>>(_employeeService.GetEmployees());
            return Page();
        }
    }
}
