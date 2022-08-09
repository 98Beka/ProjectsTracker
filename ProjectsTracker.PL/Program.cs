using ProjectsTracker.BLL.Interfaces;
using ProjectsTracker.BLL.Services;
using AutoMapper;
using ProjectsTracker.BLL.Profiles;
using ProjectsTracker.PL;
using ProjectsTracker.PL.Abstracs;
using ProjectsTracker.BLL.BusinessObjects;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddSingleton<IProjectService, ProjectService>(
    s => new ProjectService(connectionString));
builder.Services.AddSingleton<IEmployeeService, EmployeeService>(
    s => new EmployeeService(connectionString));
builder.Services.AddSingleton<FilterContainer<IProjectFilter>, ProjectFilterContainer>(f => new ProjectFilterContainer { Filter = new ProjectFilterNone()});
builder.Services.AddSingleton<IMapper, IMapper>(
    m => new MapperConfiguration(c => {
        c.AddProfile<ProjectMapperConfig>();
        c.AddProfile<EmployeeMapperConfig>();
    }).CreateMapper());

// Add services to the container.
builder.Services.AddRazorPages();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
