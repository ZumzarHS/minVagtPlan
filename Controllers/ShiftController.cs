using Microsoft.AspNetCore.Mvc;
using minVagtPlan.Data;
using Microsoft.EntityFrameworkCore;
using minVagtPlan.Models.ViewModels;
using minVagtPlan.Models.Entities;
using Microsoft.AspNetCore.Authorization;


namespace minVagtPlan.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShiftController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ShiftController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult AddShift()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddShift(AddShiftViewModel viewModel)
        {
            try
            {
                var shift = new Shift
                {
                    ShiftId = Guid.NewGuid(),
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime,
                };
                await dbContext.Shifts.AddAsync(shift);
                await dbContext.SaveChangesAsync();
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while saving the shift.");
                return StatusCode(500, e);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditShift(Guid id)
        {
            var shift = await dbContext.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            var viewModel = new EditShiftViewModel
            {
                ShiftId = shift.ShiftId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditShift(EditShiftViewModel viewModel)
        {
            var shift = await dbContext.Shifts.FindAsync(viewModel.ShiftId);
            if (shift == null)
            {
                return NotFound();
            }
            shift.StartTime = viewModel.StartTime;
            shift.EndTime = viewModel.EndTime;
            await dbContext.SaveChangesAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteShift(Shift viewModel)
        {
            var shift = await dbContext.Shifts.AsNoTracking().FirstOrDefaultAsync(s => s.ShiftId == viewModel.ShiftId);
            if (shift == null)
            {
                return NotFound();
            }
            else
            {
            dbContext.Shifts.Remove(shift);
            await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("ListShifts", "Shift");
        }

        [HttpGet]
        public async Task<IActionResult> ListShifts()
        {
            var shifts = await dbContext.Shifts.ToListAsync();
            return View(shifts);
        }
    }
}
