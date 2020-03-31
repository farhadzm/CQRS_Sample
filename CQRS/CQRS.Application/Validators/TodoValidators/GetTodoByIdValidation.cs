using FluentValidation;
namespace CQRS.Application.Queries.TodoQueries
{
    public class GetTodoByIdValidation : AbstractValidator<GetTodoByIdQuery>
    {
        public GetTodoByIdValidation()
        {
            RuleFor(a => a.Id).NotEmpty();
        }
    }
}
