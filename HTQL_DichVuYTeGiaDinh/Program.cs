using HTQL_DichVuYTeGiaDinh.Repository;
using HTQL_DichVuYTeGiaDinh.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using DinkToPdf;
using DinkToPdf.Contracts;
using System;
using System.IO;
using System.Reflection;
using HTQL_DichVuYTeGiaDinh.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình các dịch vụ SignalR
builder.Services.AddSignalR();

//Connect Db
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionStr = builder.Configuration.GetConnectionString("SqlServer");
    options.UseSqlServer(connectionStr);
});

// Thêm dịch vụ Session
builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ để lưu trữ session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(120); // Thời gian hết hạn session
    options.Cookie.HttpOnly = true; // Chỉ có thể truy cập cookie thông qua HTTP
    options.Cookie.IsEssential = true; // Đảm bảo cookie được gửi ngay cả khi không có đồng ý của người dùng
});

// Add dịch vụ EmailService
builder.Services.AddScoped<EmailService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình dịch vụ cho DinkToPdf
builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));


var app = builder.Build();
app.UseRouting();


// Sử dụng middleware Session
app.UseSession();

app.UseMiddleware<UserInfoMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();


app.UseAuthentication();

app.UseAuthorization();
// Cấu hình routing cho các Area
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
// Cấu hình routing cho các controller thông thường
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=TrangChu}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<TuVanHub>("/tuVanHub");
});


app.Run();
