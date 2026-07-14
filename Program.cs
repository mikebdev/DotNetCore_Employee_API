using Employee.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;


// Simple API with Angular FrontEND, This could be done with repository pattern but keeping it simple.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
