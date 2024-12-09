using MedSys.Api.Mapping;
using MedSys.BL.DALModels;
using MedSys.BL.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PostgresContext>(options =>
{
    options.UseNpgsql("name=ConnectionStrings:AppConnStr");
});

builder.Services.AddScoped<IRepository<CheckupType>, CheckupTypeRepository>();
builder.Services.AddScoped<IRepository<Checkup>, CheckupRepository>();
builder.Services.AddScoped<IRepository<MedicalDocument>, MedicalDocumentRepository>();
builder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();