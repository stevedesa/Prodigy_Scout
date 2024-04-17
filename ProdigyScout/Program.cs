using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProdigyScout.Data;
using ProdigyScout.Data.SeedData;
using ProdigyScout.Interfaces;
using ProdigyScout.Repository;
using ProdigyScout.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ProdigyScoutContextConnection") ?? throw new InvalidOperationException("Connection string 'ProdigyScoutContextConnection' not found.");

builder.Services.AddDbContext<ProdigyScoutContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ProdigyScoutContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// SendGrid Email Service Config.
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// Repository Pattern.
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedDatabase.Initialize(services, Assembly.GetExecutingAssembly());
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
