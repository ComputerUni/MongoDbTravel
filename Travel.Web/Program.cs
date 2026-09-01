using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using QuestPDF.Infrastructure;
using System.Reflection;
using Travel.Web.Entities;
using Travel.Web.Services.BannerServices;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.ContactServices;
using Travel.Web.Services.DashboardServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.FavoriteServices;
using Travel.Web.Services.IWhyUsServices;
using Travel.Web.Services.LookupServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReportServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;
using Travel.Web.Services.UserServices;
using Travel.Web.Settings;
using Travel.Web.Validations.CommentValidations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));

builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWhyUsService, WhyUsService>();
builder.Services.AddScoped<IContactService, ContactService>();

//builder.Services.AddScoped<IRouteService, RouteService>();

builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddMongoDbStores<AppUser, AppRole, Guid>(
        builder.Configuration["DatabaseSettings:ConnectionString"],
        builder.Configuration["DatabaseSettings:DatabaseName"]
    ).AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            );


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }

}

QuestPDF.Settings.License = LicenseType.Community;

app.Run();
