using System.Text;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Data;
using FastTrackEServices.Implementation;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.ServiceResolver;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

// Dependency Injections
// Shoe Color's resources will not have a dedicated module
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IRestOperation, ClientRest>();
builder.Services.AddScoped<IRestOperation, ShoeRest>();
builder.Services.AddScoped<IRestOperation, ShoeRepairRest>();
builder.Services.AddScoped<IRestOperation, OwnedShoeRest>();
builder.Services.AddScoped<IRestOperation, OrderCartRest>();


// DI for helper algorithms
builder.Services.AddSingleton<ITransform, CollectionToStringArray>();


//Routing
builder.Services.AddControllers();


var app = builder.Build();

// Adds enpoints to controller functions to the endpoint builder interface
app.MapControllers();
app.UseStaticFiles();
app.Run();
