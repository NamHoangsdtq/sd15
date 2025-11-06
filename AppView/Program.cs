using AppView.IServices;
using AppView.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddSession(cfg =>
{
    cfg.IdleTimeout = new TimeSpan(1,0,0);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}");

IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "../Rotativa/Windows");
app.Run();
