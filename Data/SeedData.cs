    using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using minVagtPlan.Areas.Identity.Data;
using minVagtPlan.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace minVagtPlan.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<VagtPlanUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            string[] roleNames = { "Admin", "Employee" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admins
            var adminUsers = new[]
            {
                new { Email = "admin@admin.com", Password = "Admin@123", FirstName = "System", LastName = "Admin" },
                new { Email = "admin2@admin.com", Password = "Admin@123", FirstName = "Second", LastName = "Admin" }
            };

            foreach (var admin in adminUsers)
            {
                var user = await userManager.FindByEmailAsync(admin.Email);
                if (user == null)
                {
                    var newUser = new VagtPlanUser { UserName = admin.Email, Email = admin.Email, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(newUser, admin.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, "Admin");
                        dbContext.Employees.Add(new Employee
                        {
                            EmployeeId = Guid.NewGuid(),
                            FirstName = admin.FirstName,
                            LastName = admin.LastName,
                            Role = "Admin",
                            UserId = newUser.Id
                        });
                    }
                }
            }

            // Seed Employees
            var employeeUsers = new[]
            {
                new { Email = "employee1@test.com", Password = "Employee@123", FirstName = "Alice", LastName = "Andersen" },
                new { Email = "employee2@test.com", Password = "Employee@123", FirstName = "Bob", LastName = "Berg" },
                new { Email = "employee3@test.com", Password = "Employee@123", FirstName = "Charlie", LastName = "Christensen" }
            };

            foreach (var emp in employeeUsers)
            {
                var user = await userManager.FindByEmailAsync(emp.Email);
                if (user == null)
                {
                    var newUser = new VagtPlanUser { UserName = emp.Email, Email = emp.Email, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(newUser, emp.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, "Employee");
                        dbContext.Employees.Add(new Employee
                        {
                            EmployeeId = Guid.NewGuid(),
                            FirstName = emp.FirstName,
                            LastName = emp.LastName,
                            Role = "Employee",
                            UserId = newUser.Id
                        });
                    }
                }
            }

            // Seed Shifts
            if (!await dbContext.Shifts.AnyAsync())
            {
                var shifts = new[]
                {
                    new Shift { ShiftId = Guid.NewGuid(), StartTime = DateTime.Today.AddHours(8), EndTime = DateTime.Today.AddHours(16) },
                    new Shift { ShiftId = Guid.NewGuid(), StartTime = DateTime.Today.AddHours(16), EndTime = DateTime.Today.AddHours(24) },
                    new Shift { ShiftId = Guid.NewGuid(), StartTime = DateTime.Today.AddDays(1).AddHours(8), EndTime = DateTime.Today.AddDays(1).AddHours(16) }
                };
                dbContext.Shifts.AddRange(shifts);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}

