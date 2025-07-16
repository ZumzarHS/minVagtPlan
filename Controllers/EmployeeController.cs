using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using minVagtPlan.Data;
using minVagtPlan.Models.Entities;
using minVagtPlan.Models.ViewModels;
using NuGet.Protocol;

namespace minVagtPlan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public EmployeeController(ApplicationDbContext dbContext)
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
            var employee = await dbContext.Employees
                .Include(e => e.ShiftEmployees)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }
            var viewModel = new EditEmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Role = employee.Role,
                ShiftIds = employee.ShiftEmployees.Select(se => se.ShiftId).ToList()
            };
        //    var assignedShifts = await dbContext.Shifts
        //.Where(s => s.ShiftEmployees.Any(se => se.EmployeeId == id))
        //.ToListAsync();

            ViewBag.Shifts = new MultiSelectList(await dbContext.Shifts.ToListAsync(), "ShiftId", "ShiftId");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel viewModel)
        {
            try
            {
                var employee = await dbContext.Employees
                 .Include(e => e.ShiftEmployees)
                 .FirstOrDefaultAsync(e => e.EmployeeId == viewModel.EmployeeId);

                if (!ModelState.IsValid)
                {
                    ViewBag.Shifts = new MultiSelectList(await dbContext.Shifts.ToListAsync(), "ShiftId", "ShiftId");
                    return View(viewModel); 
                }

                if (employee is not null)
                {
                    employee.FirstName = viewModel.FirstName;
                    employee.LastName = viewModel.LastName;
                    employee.Role = viewModel.Role;
                    // Update shifts
                    employee.ShiftEmployees.Clear();
                    foreach (var shiftId in viewModel.ShiftIds)
                    {
                        employee.ShiftEmployees.Add(new ShiftEmployee
                        {
                            EmployeeId = employee.EmployeeId,
                            ShiftId = shiftId
                        });
                    }
                    await dbContext.SaveChangesAsync();
                }

                return RedirectToAction("ListEmployee", "Employee");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                    return StatusCode(500, "Internal server error");
            }
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
                return RedirectToAction("ListEmployee", "Employee");
        }
    }
}