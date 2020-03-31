using AutoMapper;
using CQRS.Application.Common.Mappings;
using CQRS.Data.Configurtion.Contracts;
using CQRS.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Application.Commands.TodoCommands
{
    public class EditTodoCommandDtoResponse : IMapFrom<Todo>
    {
        public int Id { get; set; }
        public bool Done { get; set; }
        public string Title { get; set; }
    }
    public class EditTodoCommand : IRequest<EditTodoCommandDtoResponse>
    {
        public int Id { get; set; }
        public bool Done { get; set; }
        public string Title { get; set; }
    }
    public class EditTodoHandler : IRequestHandler<EditTodoCommand, EditTodoCommandDtoResponse>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public EditTodoHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<EditTodoCommandDtoResponse> Handle(EditTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _applicationDbContext.Todos.FindAsync(request.Id);
            todo.Title = request.Title;
            todo.Done = request.Done;
            _applicationDbContext.Todos.Update(todo);
            await _applicationDbContext.SaveChanges(cancellationToken);
            return _mapper.Map<EditTodoCommandDtoResponse>(todo);
        }
    }
}
