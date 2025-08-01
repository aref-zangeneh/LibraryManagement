using FluentValidation;
using LibraryManagement.Api.Middlewares;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Interfaces.Repositories;
using LibraryManagement.Application.Mappings;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Validators;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookCommandValidator>();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// register application services - DI
builder.Services.AddScoped<IBookService, BookService>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IGenericRepository<Book>, GenericRepository<Book>>();

//register automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// add swagger - using Swashbuckle.AspNetCore
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management Project API", Version = "v1" });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
