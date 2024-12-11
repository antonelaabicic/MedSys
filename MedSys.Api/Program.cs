using MedSys.Api.Mapping;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PostgresContext>(options =>
{
    options.UseNpgsql("name=ConnectionStrings:AppConnStr");
});

builder.Services.AddScoped<ICheckupRepository, CheckupRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ICheckupTypeRepository, CheckupTypeRepository>();
builder.Services.AddScoped<IRepository<MedicalDocument>, MedicalDocumentRepository>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
