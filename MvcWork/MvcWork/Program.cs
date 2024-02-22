using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MvcWork.Models;
using System;
using System.IO;

var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var userDataFilePath = Path.Combine(desktopFolder, "users.txt");
var noteDataFilePath = Path.Combine(desktopFolder, "notes.txt"); 


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
}); 

var fileUserService = new FileUserService(userDataFilePath);
var fileNoteService = new FileNoteService(noteDataFilePath, fileUserService);

builder.Services.AddSingleton<FileUserService>(fileUserService);
builder.Services.AddSingleton<FileNoteService>(fileNoteService);

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
