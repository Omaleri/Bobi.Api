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


builder.Services.AddScoped<IAddressAppService, AddressAppService>();
builder.Services.AddScoped<IRepository<Address>, AddressRepository>();
builder.Services.AddScoped<IBuildAppService, BuildAppService>();
builder.Services.AddScoped<IRepository<Build>, BuildRepository>();
builder.Services.AddScoped<ICityAppService, CityAppService>();
builder.Services.AddScoped<IRepository<City>, CityRepository>();
builder.Services.AddScoped<IDeviceAppService, DeviceAppService>();
builder.Services.AddScoped<IRepository<Device>, DeviceRepository>();
builder.Services.AddScoped<INumberAppService, NumberAppService>();
builder.Services.AddScoped<IRepository<Number>, NumberRepository>();
builder.Services.AddScoped<IProvinceAppService, ProvinceAppService>();
builder.Services.AddScoped<IRepository<Province>, ProvinceRepository>();
builder.Services.AddScoped<IStreetAppService, StreetAppService>();
builder.Services.AddScoped<IRepository<Street>, StreetRepository>();
builder.Services.AddScoped<ITownAppService, TownAppService>();
builder.Services.AddScoped<IRepository<Town>, TownRepository>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IVoiceAppService, VoiceAppService>();
builder.Services.AddScoped<IRepository<Voice>, VoiceRepository>();


var app = builder.Build();

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
