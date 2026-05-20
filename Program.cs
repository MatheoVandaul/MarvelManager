using Microsoft.AspNetCore.Authentication.Cookies;
using MarvelManager.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<UserService>();
// Add services to the container.
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddScoped<CharacterService>();
builder.Services.AddScoped<TeamService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
