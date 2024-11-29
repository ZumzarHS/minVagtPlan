using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using minVagtPlan.Data;
using minVagtPlan.Models.Entities;
using minVagtPlan.Models.ViewModels;

namespace minVagtPlan.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AssignmentController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> AssignShifts(Guid id)
        {
            var employee = await dbContext.Employees
                .Include(e => e.ShiftEmployees)
                .ThenInclude(se => se.Shift)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            var viewModel = new AssignShiftsViewModel
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                AssignedShiftIds = employee.ShiftEmployees.Select(se => se.ShiftId).ToList(),
                AvailableShifts = await dbContext.Shifts
                    .Select(s => new SelectListItem
                    {
                        Value = s.ShiftId.ToString(),
                        Text = $"{s.StartTime} - {s.EndTime}"
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignShifts(AssignShiftsViewModel viewModel)
        {
            try
            {
                var employee = await dbContext.Employees
                    .Include(e => e.ShiftEmployees)
                    .FirstOrDefaultAsync(e => e.EmployeeId == viewModel.EmployeeId);

                if (employee == null)
                {
                    return NotFound();
                }

                // Update shifts
                employee.ShiftEmployees.Clear();
                foreach (var shiftId in viewModel.AssignedShiftIds)
                {
                    employee.ShiftEmployees.Add(new ShiftEmployee
                    {
                        EmployeeId = employee.EmployeeId,
                        ShiftId = shiftId
                    });
                }

                await dbContext.SaveChangesAsync();

                return RedirectToAction("ListEmployee", "Employee");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> AssignEmployees(Guid id)
        {
            var shift = await dbContext.Shifts
                .Include(s => s.ShiftEmployees)
                .ThenInclude(se => se.Employee)
                .FirstOrDefaultAsync(s => s.ShiftId == id);

            if (shift == null)
            {
                return NotFound();
            }

            var viewModel = new AssignEmployeesViewModel
            {
                ShiftId = shift.ShiftId,
                ShiftDetails = $"{shift.StartTime} - {shift.EndTime}",
                AssignedEmployeeIds = shift.ShiftEmployees.Select(se => se.EmployeeId).ToList(),
                AvailableEmployees = await dbContext.Employees
                    .Select(e => new SelectListItem
                    {
                        Value = e.EmployeeId.ToString(),
                        Text = $"{e.FirstName} {e.LastName}"
                    })
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignEmployees(AssignEmployeesViewModel viewModel)
        {
            try
            {
                var shift = await dbContext.Shifts
                    .Include(s => s.ShiftEmployees)
                    .FirstOrDefaultAsync(s => s.ShiftId == viewModel.ShiftId);

                if (shift == null)
                {
                    return NotFound();
                }

                // Update employees
                shift.ShiftEmployees.Clear();
                foreach (var employeeId in viewModel.AssignedEmployeeIds)
                {
                    shift.ShiftEmployees.Add(new ShiftEmployee
                    {
                        ShiftId = shift.ShiftId,
                        EmployeeId = employeeId
                    });
                }

                await dbContext.SaveChangesAsync();

                return RedirectToAction("ListShifts", "Shift");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public IActionResult GetShifts()
        {
            var events = dbContext.Shifts
                .Include(shift => shift.ShiftEmployees)
                .ThenInclude(se => se.Employee)
                .Select(shift => new
                {
                    id = shift.ShiftId,
                    title = string.Join(", ", shift.ShiftEmployees.Select(se => $"{se.Employee.FirstName} {se.Employee.LastName}")),
                    start = shift.StartTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = shift.EndTime.ToString("yyyy-MM-ddTHH:mm:ss")
                }).ToList();

            return Json(events);
        }

    }
}
