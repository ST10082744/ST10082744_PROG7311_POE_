using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10082744_PROG7311_POE_;
using ST10082744_PROG7311_POE_.Areas.Identity.Data;
using ST10082744_PROG7311_POE_.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ST10082744_PROG7311_POE_ContextConnection") ?? throw new InvalidOperationException("Connection string 'ST10082744_PROG7311_POE_ContextConnection' not found.");

builder.Services.AddDbContext<ST10082744_PROG7311_POE_Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ST10082744_PROG7311_POE_User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ST10082744_PROG7311_POE_Context>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();
///seeding roles 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ST10082744_PROG7311_POE_User>>();
        await RoleInitializer.InitializeAsync(roleManager, userManager);
        await DbInitializer.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing roles.");
    }
}


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


app.Run();
