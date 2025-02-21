# Todo API with Vertical Slice Architecture and CQRS

This project demonstrates implementing a Todo API using Vertical Slice Architecture (VSA) and CQRS patterns in .NET 8. It serves as both a working example and a learning resource.

## What is Vertical Slice Architecture?

Vertical Slice Architecture organizes code around features rather than technical layers. Instead of traditional horizontal layers (Controllers, Services, Repositories), we organize code by business functionality ("vertical slices").

### Benefits of VSA:
- **Feature-Centric**: Each feature is self-contained and independent
- **Better Maintainability**: Changes to a feature are localized
- **Reduced Coupling**: Features don't depend on shared abstractions
- **Easier to Understand**: Code organization matches business functionality
- **Simplified Testing**: Features can be tested in isolation

## Project Structure

```
TodoApi/
├── TodoApi.Api/
│   ├── Features/
│   │   └── Todos/
│   │       └── TodosController.cs
│   └── Program.cs
├── TodoApi.Application/
│   ├── Data/
│   │   └── TodoDbContext.cs
│   └── Features/
│       └── Todos/
│           ├── Commands/
│           │   ├── CreateTodo.cs
│           │   ├── UpdateTodo.cs
│           │   └── DeleteTodo.cs
│           └── Queries/
│               ├── GetTodos.cs
│               └── GetTodoById.cs
└── TodoApi.Domain/
    └── Entities/
        └── Todo.cs
```

## Key Concepts

### 1. CQRS (Command Query Responsibility Segregation)
- **Commands**: Modify state (Create, Update, Delete)
- **Queries**: Read state (Get, List)
- Benefits: Separation of read and write operations, optimization opportunities

### 2. MediatR
- Implements mediator pattern
- Decouples request handlers from controllers
- Enables CQRS implementation

### 3. Vertical Slices
- Each feature contains its own:
  - Controllers
  - Commands/Queries
  - DTOs
  - Validation
  - Business Logic

## Getting Started

1. Clone the repository
2. Run these commands:
```bash
dotnet restore
dotnet build
cd TodoApi.Api
dotnet run
```
3. Access Swagger UI at `https://localhost:7000/swagger`

## Implementation Guide

### 1. Create Solution and Projects
```bash
dotnet new sln -n TodoApi
dotnet new webapi -n TodoApi.Api
dotnet new classlib -n TodoApi.Application
dotnet new classlib -n TodoApi.Domain
```

### 2. Add Required Packages
```bash
# TodoApi.Api
dotnet add package Microsoft.EntityFrameworkCore.InMemory

# TodoApi.Application
dotnet add package MediatR
dotnet add package Microsoft.EntityFrameworkCore
```

### 3. Implement Domain Entity
```csharp
public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
```

### 4. Create Commands and Queries
Example Command:
```csharp
public record CreateTodoCommand(string Title, string Description) : IRequest<Todo>;

public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, Todo>
{
    private readonly TodoDbContext _context;

    public CreateTodoHandler(TodoDbContext context) => _context = context;

    public async Task<Todo> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync(cancellationToken);
        return todo;
    }
}
```

### 5. Configure Services
```csharp
// Program.cs
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseInMemoryDatabase("TodoDb"));

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(TodoDbContext).Assembly);
});
```

## Best Practices

1. **Keep Slices Independent**
   - Avoid sharing code between slices
   - Each slice should be self-contained

2. **Use CQRS Appropriately**
   - Commands: Modify state
   - Queries: Read state
   - Keep handlers focused and simple

3. **Validation**
   - Add validation in command/query handlers
   - Use FluentValidation for complex validation

4. **Error Handling**
   - Implement consistent error handling
   - Use custom exceptions for business rules

## Extending the Project

Here are some ways to enhance the project for learning:

1. **Add Validation**
   ```csharp
   builder.Services.AddFluentValidation(fv => 
       fv.RegisterValidatorsFromAssembly(typeof(CreateTodoValidator).Assembly));
   ```

2. **Implement Logging**
   ```csharp
   public class CreateTodoHandler
   {
       private readonly ILogger<CreateTodoHandler> _logger;
       
       public async Task<Todo> Handle(CreateTodoCommand request, ...)
       {
           _logger.LogInformation("Creating new todo: {Title}", request.Title);
           // ... implementation
       }
   }
   ```

3. **Add Authentication**
   ```csharp
   builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options => { ... });
   ```

## Common Questions

1. **Why use MediatR?**
   - Decouples controllers from handlers
   - Enables CQRS pattern
   - Simplifies cross-cutting concerns

2. **VSA vs Traditional Layered Architecture?**
   - VSA organizes by feature
   - Reduces coupling
   - More maintainable
   - Better matches business domains

3. **When to use CQRS?**
   - Different read/write requirements
   - Complex domain logic
   - Need for separate optimization

## Resources for Learning

1. **Books**
   - "Clean Architecture" by Robert C. Martin
   - "Domain-Driven Design" by Eric Evans

2. **Online Resources**
   - [Jimmy Bogard's Blog](https://jimmybogard.com/)
   - [MSDN Architecture Guides](https://docs.microsoft.com/en-us/dotnet/architecture/)

## Contributing

Feel free to submit issues and enhancement requests!

## License

MIT License
