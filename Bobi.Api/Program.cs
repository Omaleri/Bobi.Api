using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Services;
using Bobi.Api.Domain.Address;
using Bobi.Api.Domain.Build;
using Bobi.Api.Domain.User;
using Bobi.Api.MongoDb.Repositories.Base;
using Bobi.Api.MongoDb.Repositories.Interfaces;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddScoped<IBuildAppService, BuildAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<Build>, BuildRepository>();
builder.Services.AddScoped<ICityAppService, CityAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<City>, CityRepository>();
builder.Services.AddScoped<IDeviceAppService, DeviceAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<Device>, DeviceRepository>();
builder.Services.AddScoped<INumberAppService, NumberAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<Number>, NumberRepository>();
builder.Services.AddScoped<IProvinceAppService, ProvinceAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<Province>, ProvinceRepository>();
builder.Services.AddScoped<IStreetAppService, StreetAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<Street>, StreetRepository>();
builder.Services.AddScoped<ITownAppService, TownAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<Town>, TownRepository>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<User>, UserRepository>();
builder.Services.AddScoped<IVoiceAppService, VoiceAppService>();
builder.Services.AddScoped<Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<Voice>, VoiceRepository>();
//builder.Services.AddScoped(typeof(Bobi.Api.MongoDb.Repositories.Interfaces.IRepository<>), typeof(Bobi.Api.MongoDb.Repositories.Base.IRepository<>));

var app = builder.Build();

app.UseCors("MyPolicy");
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "I am Alive!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
