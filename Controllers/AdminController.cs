using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minVagtPlan.Data;
using minVagtPlan.Models.Entities;
using minVagtPlan.Models.ViewModels;
using NuGet.Protocol;

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
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel viewModel)
        {
            try
            {
                var employee = new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Role = viewModel.Role,
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
        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            var viewModel = new EditEmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Role = employee.Role
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel viewModel)
        {
            var employee = await dbContext.Employees.FindAsync(viewModel.EmployeeId);
            if (!ModelState.IsValid)
            {
                return View(viewModel);  // Return to the view if there are validation errors
            }

            if (employee is not null)
            {
                employee.FirstName = viewModel.FirstName;
                employee.LastName = viewModel.LastName;
                employee.Role = viewModel.Role;
                await dbContext.SaveChangesAsync();
            }
            
            return RedirectToAction("ListEmployee", "Admin");
        }
        [HttpGet]
        public async Task<IActionResult> ListEmployee()
        {
            var employees = await dbContext.Employees.ToListAsync();
            return View(employees);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(Employee viewModel)
        {
          
                var employee = await dbContext.Employees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.EmployeeId == viewModel.EmployeeId);
            if (employee is not null)
                {
                    dbContext.Employees.Remove(employee);
                    await dbContext.SaveChangesAsync();
                }
            else
            {
                return NotFound();
            }
                return RedirectToAction("ListEmployee", "Admin");
        }
    }
}