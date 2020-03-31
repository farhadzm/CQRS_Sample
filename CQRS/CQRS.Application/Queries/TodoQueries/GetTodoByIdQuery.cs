using AutoMapper;
using AutoMapper.QueryableExtensions;
using CQRS.Application.Common.Mappings;
using CQRS.Data.Configurtion.Contracts;
using CQRS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Application.Queries.TodoQueries
{
    public class GetTodoByIdDto : IMapFrom<Todo>
    {
        public string Title { get; set; }
        public bool Done { get; set; }
    }
    public class GetTodoByIdQuery : IRequest<GetTodoByIdDto>
    {
        public int Id { get; set; }
    }
    public class GetTodoByIdHandler : IRequestHandler<GetTodoByIdQuery, GetTodoByIdDto>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetTodoByIdHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public Task<GetTodoByIdDto> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            var todo = _applicationDbContext
                .Todos
                .Where(a => a.Id == request.Id)
                .ProjectTo<GetTodoByIdDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            return todo;
        }
    }
}
