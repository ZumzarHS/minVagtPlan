using Microsoft.EntityFrameworkCore;
using minVagtPlan.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

// Add Entity Framework Core and configure the SQL Server database context.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("myVagtPlanDb")));

//// Add ASP.NET Core Identity services
//builder.Services.AddIdentity<IdentityUser, IdentityRole>() // You can replace IdentityUser with a custom user class if needed
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders(); // This adds default token providers for things like password reset

var app = builder.Build();

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

//// Enable authentication and authorization
//app.UseAuthentication(); // This middleware must be placed before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
