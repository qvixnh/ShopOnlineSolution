using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.Api.Data;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//***IMPORTANT INSTRUCTION HERE - MUST CONFIGURE CONNECTION BEFORE RUNNING MIGRATIONS

builder.Services.AddDbContextPool<ShopOnlineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection"))
);

//implement the code to register the product repository  with the dependency injection system
//using add trasient would mean that a new instance of the relevant object provided to every class that required the relevant object to be injected
//AddScope = 
builder.Services.AddScoped<IProductRepository, ProductRepository>();
//app building
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//
app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7135", "https://localhost:7135")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
    
    );
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
