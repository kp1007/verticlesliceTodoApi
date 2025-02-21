using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Application.Features.Todos.Commands;
using TodoApi.Application.Features.Todos.Queries;
using TodoApi.Domain.Entities;

namespace TodoApi.Api.Features.Todos;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        var todos = await _mediator.Send(new GetTodosQuery());
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetTodo(Guid id)
    {
        var todo = await _mediator.Send(new GetTodoByIdQuery(id));
        if (todo == null) return NotFound();
        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodo(CreateTodoCommand command)
    {
        var todo = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Todo>> UpdateTodo(Guid id, UpdateTodoCommand command)
    {
        if (id != command.Id) return BadRequest();

        try
        {
            var todo = await _mediator.Send(command);
            return Ok(todo);
        }
        catch (Exception)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteTodoCommand(id));
            return NoContent();
        }
        catch (Exception)
        {
            return NotFound();
        }
    }
}