using Microsoft.EntityFrameworkCore;
using minVagtPlan.Data;
using Microsoft.AspNetCore.Identity;
using minVagtPlan.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Configure ApplicationDbContext for domain entities
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("myVagtPlanDb")));

// Add ASP.NET Core Identity services
builder.Services.AddDefaultIdentity<VagtPlanUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
    options.AddPolicy("AdminOrEmployee", policy => policy.RequireRole("Admin", "Employee"));
});

// Configure authorization policies
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Employee", "AdminOnly");
    options.Conventions.AuthorizeFolder("/Shift", "AdminOnly");
    options.Conventions.AuthorizeFolder("/Assignment", "AdminOnly");
    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage", "AdminOnly");
});

var app = builder.Build();

// Seed the database with roles and an admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 403)
    {
        context.HttpContext.Response.Redirect("/Identity/Account/AccessDenied");
    }
    else if (context.HttpContext.Response.StatusCode == 401)
    {
        context.HttpContext.Response.Redirect("/Identity/Account/Login");
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
