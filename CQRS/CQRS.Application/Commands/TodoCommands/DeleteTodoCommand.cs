using CQRS.Data.Configurtion.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Application.Commands.TodoCommands
{
    public class DeleteTodoCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteTodoHandler : IRequestHandler<DeleteTodoCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteTodoHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _applicationDbContext.Todos.FindAsync(request.Id);
            _applicationDbContext.Todos.Remove(todo);
            await _applicationDbContext.SaveChanges(cancellationToken);
            return Unit.Value;
        }
    }
}
