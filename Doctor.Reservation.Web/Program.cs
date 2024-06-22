using Doctor.Reservation.Domain;
using Doctor.Reservation.Repository;
using Doctor.Reservation.Services.Services;
using Doctor.Reservation.Web.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IClock, Clock>();
builder.Services.AddScoped<ITimeBoxRepository, TimeBoxRepository>();
builder.Services.AddScoped<ITimeBoxService, TimeBoxService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilters));
});

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