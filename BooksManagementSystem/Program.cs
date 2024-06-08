using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BooksManagementSystem.Data;
using BooksManagementSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BooksManagementSystemContextConnection") 
    ?? throw new InvalidOperationException("Connection string 'BooksManagementSystemContextConnection' not found.");




builder.Services.AddDbContext<BooksManagementSystemContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<BooksManagementSystemUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;//this configuration does not work and ignored by ASP.NET. left for reference
    options.SignIn.RequireConfirmedAccount = false;//this configuration does not work and ignored by ASP.NET. left for reference
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BooksManagementSystemContext>();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;//this configuration does not work and ignored by ASP.NET. left for reference
    options.SignIn.RequireConfirmedAccount = false;//this configuration does not work and ignored by ASP.NET. left for reference
});



builder.Services.AddControllers(option =>
{
    //to prevent the issue when empty/null collection that reprresents foreign keys in model prevents passing validation
    option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});



// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseAuthentication(); ;

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


app.Run();

