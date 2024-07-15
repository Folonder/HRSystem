using System.Reflection;
using Application;
using Infrastructure;
using Infrastructure.Configs;
using Infrastructure.Data;
using Presentation.Mappers;
using Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Configuration
    .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
    .AddJsonFile("appsettings.json", false)
    .AddEnvironmentVariables();

builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(DatabaseConfig.Section));

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


var app = builder.Build();

// Инициализация бд изначальными значениями
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();