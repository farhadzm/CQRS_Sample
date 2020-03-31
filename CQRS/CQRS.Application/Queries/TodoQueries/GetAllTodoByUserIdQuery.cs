using AutoMapper;
using AutoMapper.QueryableExtensions;
using CQRS.Application.Common.Mappings;
using CQRS.Data.Configurtion.Contracts;
using CQRS.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Application.Queries.TodoQueries
{
    public class GetAllTodoByUserIdDtoResponse:IMapFrom<Todo>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
    }
    public class GetAllTodoByUserIdQuery : IRequest<List<GetAllTodoByUserIdDtoResponse>>
    {
        public int UserId { get; set; }
    }
    public class GetAllTodoByUserIdHandler : IRequestHandler<GetAllTodoByUserIdQuery, List<GetAllTodoByUserIdDtoResponse>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public GetAllTodoByUserIdHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<List<GetAllTodoByUserIdDtoResponse>> Handle(GetAllTodoByUserIdQuery request, CancellationToken cancellationToken)
        {
            var data = await _applicationDbContext.Todos
                .Where(a => a.UserCreatorId == request.UserId)
                .ProjectTo<GetAllTodoByUserIdDtoResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
            return data;
        }
    }
}
