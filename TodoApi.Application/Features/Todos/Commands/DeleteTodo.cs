using MediatR;
using TodoApi.Application.Data;

namespace TodoApi.Application.Features.Todos.Commands;

public record DeleteTodoCommand(Guid Id) : IRequest;

public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand>
{
    private readonly TodoDbContext _context;

    public DeleteTodoHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);

        if (todo == null)
            throw new Exception("Todo not found");

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync(cancellationToken);
    }
}