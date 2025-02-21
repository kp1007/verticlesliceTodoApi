using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseInMemoryDatabase("TodoDb"));

// Register MediatR - new way for .NET 8
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(TodoApi.Application.Data.TodoDbContext).Assembly);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();