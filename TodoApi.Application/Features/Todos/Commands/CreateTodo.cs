using MediatR;
using TodoApi.Application.Data;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Features.Todos.Commands;

public record CreateTodoCommand(string Title, string Description) : IRequest<Todo>;

public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, Todo>
{
    private readonly TodoDbContext _context;

    public CreateTodoHandler(TodoDbContext context)
    {
        _context = context;
    }

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