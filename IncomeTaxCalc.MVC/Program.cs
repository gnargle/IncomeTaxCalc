using IncomeTaxCalc.Database;
using IncomeTaxCalc.Database.Data;
using IncomeTaxCalc.Database.Repositories;
using IncomeTaxCalc.Database.Repositories.Interfaces;
using IncomeTaxCalc.Services;
using IncomeTaxCalc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TaxCalcContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("TaxCalcContext")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<ITaxBandService, TaxBandService>();
builder.Services.AddSingleton<IRegionService, RegionService>();
builder.Services.AddSingleton<ITaxCalculatorServiceFactory, TaxCalculatorServiceFactory>();
builder.Services.AddSingleton<ITaxBandRepository, TaxBandRepository>();
builder.Services.AddSingleton<IRegionRepository, RegionRepository>();
builder.Services.AddSingleton<ITaxCalculatorService, TaxCalculatorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<TaxCalcContext>();
    DbInitialiser.Initialise(context);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TaxCalc}/{action=Index}/{id?}");

app.Run();
