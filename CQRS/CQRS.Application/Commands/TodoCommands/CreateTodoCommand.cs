using AutoMapper;
using CQRS.Application.Common.Mappings;
using CQRS.Data.Configurtion.Contracts;
using CQRS.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Application.Commands.TodoCommands
{
    public class CreateTodoCommand : IRequest<Todo>, IMapTo<Todo>
    {
        public string Title { get; set; }
        public bool Done { get; set; }
    }
    public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, Todo>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTodoHandler> _logger;

        public CreateTodoHandler(IApplicationDbContext applicationDbContext, IMapper mapper, ILogger<CreateTodoHandler> logger)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Todo> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = _mapper.Map<Todo>(request);
            _applicationDbContext.Todos.Add(todo);
            await _applicationDbContext.SaveChanges(cancellationToken);
            _logger.LogInformation($"A new Todo created by UserId={todo.UserCreatorId}, Title={request.Title}");
            return todo;
        }
    }
}
