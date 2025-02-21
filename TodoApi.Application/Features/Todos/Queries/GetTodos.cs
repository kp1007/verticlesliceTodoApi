using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Data;
using TodoApi.Domain.Entities;

namespace TodoApi.Application.Features.Todos.Queries;

public record GetTodosQuery : IRequest<IEnumerable<Todo>>;

public class GetTodosHandler : IRequestHandler<GetTodosQuery, IEnumerable<Todo>>
{
    private readonly TodoDbContext _context;

    public GetTodosHandler(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Todo>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        return await _context.Todos.ToListAsync(cancellationToken);
    }
}