using Bobi.Api.Application.Contracts.Interfaces;
using Bobi.Api.Application.Services;
using Bobi.Api.Domain.Address;
using Bobi.Api.EntityFrameworkCore.Repositories.Base;
using Bobi.Api.EntityFrameworkCore.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Bobi.Api.Domain;
using Bobi.Api.EntityFrameworkCore;
using Bobi.Api.EntityFrameworkCore.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MongoDB:ConnectionString");

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<BobiDbContext>();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "I am Alive!");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();