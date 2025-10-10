using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Tuan06.Data;

var builder = WebApplication.CreateBuilder(args);

// Begin Khai
//builder.Services.AddSingleton<IProductService, ProductService>();
// End Khai
builder.Services.AddTransient<Tuan06.Data.ProductDAL>();
builder.Services.AddTransient<Tuan06.Data.CategoryDAL>();
builder.Services.AddScoped<Tuan06.Data.UserDAL>();
builder.Services.AddSession();
builder.Services.AddControllersWithViews();
 
// ? Th�m DistributedMemoryCache (n?u kh�ng c� s? b�o l?i khi Session ch?y)
builder.Services.AddDistributedMemoryCache();

// begin phat
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // th?i gian s?ng c?a session
    options.Cookie.HttpOnly = true; // t�ng b?o m?t
    options.Cookie.IsEssential = true; // session ho?t �?ng k? c? khi user t?t consent cookie
});
// end phat
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// begin phat
app.UseSession(); // ph?i �?t tr�?c UseAuthorization v� MapControllerRoute
// end phat
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
