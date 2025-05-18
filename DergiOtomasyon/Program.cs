using AutoMapper;
using DergiOtomasyon.AutoMapper;
using DergiOtomasyon.Models;
using DergiOtomasyon.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MagazineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;  
}
);
//builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddScoped<AutoPenaltyService>();
builder.Services.AddHostedService<AutoPenaltyBackgroundService>();
builder.Services.AddScoped<SubscriptionReminder>();
builder.Services.AddHostedService<SubscriptionReminderHostedService>();
builder.Services.AddScoped<IEmailService, DergiOtomasyon.Service.EmailService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddHostedService<SubscriptionBackground>();
builder.Services.AddScoped<SubscriptionRenew>();
builder.Services.AddHostedService<SubscriptionRenewBackground>();
builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = "User";
}).AddCookie("User", options =>
{
    options.LoginPath = "/Login/Index";
    options.AccessDeniedPath = "/AccessDenied";
}).AddCookie("Admin", optins =>
{
    optins.LoginPath = "/Admin/Login/";
    optins.AccessDeniedPath = "/AccesDenied";

}
);
     

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
app.UseSession();
 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
