using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence;
using TSchedule.Persistence.Interfaces;
using TSchedule.Persistence.Managers;
using TSchedule.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAnnouncementsService, AnnouncementsService>();
builder.Services.AddSingleton<IClassroomsService, ClassroomsService>();
builder.Services.AddSingleton<IGroupsService, GroupsService>();
builder.Services.AddSingleton<ILicensesService, LicensesService>();
builder.Services.AddSingleton<IProductsService, ProductsService>();
builder.Services.AddSingleton<ISpecialtiesService, SpecialtiesService>();
builder.Services.AddSingleton<ISubjectsService, SubjectsService>();
builder.Services.AddSingleton<ITeachersService, TeachersService>();
builder.Services.AddSingleton<IUsersService, UsersService>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(ConnectionManager.Default.GetConnectionString()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
