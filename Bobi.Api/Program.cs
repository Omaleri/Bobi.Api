using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Services;
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
builder.Services.AddScoped<IBuildAppService, BuildAppService>();
builder.Services.AddScoped<ICityAppService, CityAppService>();
builder.Services.AddScoped<IDeviceAppService, DeviceAppService>();
builder.Services.AddScoped<INumberAppService, NumberAppService>();
builder.Services.AddScoped<IProvinceAppService, ProvinceAppService>();
builder.Services.AddScoped<IStreetAppService, StreetAppService>();
builder.Services.AddScoped<ITownAppService, TownAppService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<IVoiceAppService, VoiceAppService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

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
