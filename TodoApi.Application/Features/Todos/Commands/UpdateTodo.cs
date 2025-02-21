using MediatR;
using TodoApi.Application.Data;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Features.Todos.Commands;

public record UpdateTodoCommand(Guid Id, string Title, string Description, bool IsCompleted) : IRequest<Todo>;

public class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, Todo>
{
    private readonly TodoDbContext _context;

    public UpdateTodoHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Todo> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);

        if (todo == null)
            throw new Exception("Todo not found");

        todo.Title = request.Title;
        todo.Description = request.Description;

        if (request.IsCompleted && !todo.IsCompleted)
        {
            todo.IsCompleted = true;
            todo.CompletedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return todo;
    }
}