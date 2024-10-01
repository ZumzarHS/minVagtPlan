using Microsoft.AspNetCore.Mvc;
using minVagtPlan.Data;
using minVagtPlan.Models.Entities;
using minVagtPlan.Models.ViewModels;

namespace minVagtPlan.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel viewModel)
        {
            try
            {
                var employee = new Employee
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Role = viewModel.Role
                };
                await dbContext.Employees.AddAsync(employee);
                await dbContext.SaveChangesAsync();
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the employee.");
                return StatusCode(500, e);
            }
        }
    }
}