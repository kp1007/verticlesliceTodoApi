using MediatR;
using TodoApi.Application.Data;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Features.Todos.Queries;

public record GetTodoByIdQuery(Guid Id) : IRequest<Todo?>;

public class GetTodoByIdHandler : IRequestHandler<GetTodoByIdQuery, Todo?>
{
    private readonly TodoDbContext _context;

    public GetTodoByIdHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<Todo?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);
    }
}