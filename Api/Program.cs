using Api.ConfigurationOptions;
using Infrastructure.DateTimes;

#region Services
var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);
services.Configure<AppSettings>(configuration);

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();



//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddProblemDetails();

//services.AddCors(options =>
//{
//    options.AddPolicy("AllowedOrigins", builder => builder
//        .WithOrigins(appSettings.CORS.AllowedOrigins)
//        .AllowAnyMethod()
//        .AllowAnyHeader());

//    options.AddPolicy("SignalRHubs", builder => builder
//        .WithOrigins(appSettings.CORS.AllowedOrigins)
//        .AllowAnyHeader()
//        .WithMethods("GET", "POST")
//        .AllowCredentials());

//    options.AddPolicy("AllowAnyOrigin", builder => builder
//        .AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader());

//    options.AddPolicy("CustomPolicy", builder => builder
//        .AllowAnyOrigin()
//        .WithMethods("Get")
//        .WithHeaders("Content-Type"));
//});

services.AddDateTimeProvider();

#endregion

#region App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion