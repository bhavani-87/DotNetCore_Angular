using ContactsAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() // Allow all origins
                   .AllowAnyMethod() // Allow any HTTP method
                   .AllowAnyHeader(); // Allow any headers
        });
});

var app = builder.Build();

// Use the CORS middleware
app.UseCors("AllowAllOrigins");
app.UseMiddleware<GlobalException>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
