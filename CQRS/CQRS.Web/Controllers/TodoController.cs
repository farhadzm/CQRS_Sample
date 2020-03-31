using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CQRS.Application.Commands.TodoCommands;
using CQRS.Application.Queries.TodoQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CQRS.Web.Controllers
{
    public class TodoController : ApiBaseController
    {
        private readonly Lazy<IHttpContextAccessor> _httpContextAccessor;

        public TodoController(IMediator mediator, Lazy<IHttpContextAccessor> httpContextAccessor)
            : base(mediator)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetTodoByIdQuery { Id = id });
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int userId = Convert.ToInt32(_httpContextAccessor.Value.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            GetAllTodoByUserIdQuery request = new GetAllTodoByUserIdQuery { UserId = userId };
            var result = await _mediator.Send(request);
            return Ok(result);

        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update(EditTodoCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Post(CreateTodoCommand createTodoCommand)
        {
            var result = await _mediator.Send(createTodoCommand);
            return Ok(result);
        }
        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(DeleteTodoCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}