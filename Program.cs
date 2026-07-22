using Employee.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Scalar.AspNetCore;


// Simple API with Angular FrontEND, This could be done with repository pattern but keeping it simple.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



// Cross-Origin Resource Sharing (CORS) configuration to allow requests from Angular frontend running on localhost:4200 and others as needed.
builder.Services.AddCors(opt =>
{ 
 opt.AddPolicy("CorsPolicy", policy =>
    {
        //policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); // Allow any origin for testing purposes, you can restrict it to specific origins in production.
    });
});



builder.Services.AddDbContext<EmployeeDbContext>(opt=>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeConnection")));


// Swagger, we can also use Scalar
builder.Services.AddOpenApi();

// Register the Swagger generator; custom details
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Employee-Management_MSSQLDB_DotnetCoreAPI_Angular_Demo v1 | .NET CORE 10",
        Description = "Manages Employeess",
        TermsOfService = new Uri("https://images3.alphacoders.com/967/thumb-1920-96797.jpg"),
        Contact = new OpenApiContact
        {
            Name = "Employee-Management_MSSQLDB_DotnetCoreAPI_Angular_Demo Administrator",
            Email = "Employee-Management_MSSQLDB_DotnetCoreAPI_Angular_Demo@encom.com",
            Url = new Uri("https://m.media-amazon.com/images/I/71rVfyrUzPL._SL1101_.jpg"),
        },
        License = new OpenApiLicense
        {
            Name = "No license",
            Url = new Uri("https://example.com/license"),
        }
    });
});

// Only works in development
//SwaggerURL: string = this.hostAddress + 'swagger/index.html';
//SwaggerJSON: string = this.hostAddress + '/swagger/v1/swagger.json';
//ScalarURL: string = this.hostAddress + '/scalar';
//SwaggerURL: https://localhost:7004/swagger/index.html
//SwaggerJSON:  https://localhost:7004/swagger/v1/swagger.json
//ScalarURL: https://localhost:7004/scalar


var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy"); // Apply the CORS policy to the request pipeline
app.UseAuthorization();

app.MapControllers();

app.Run();
