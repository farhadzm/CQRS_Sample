using FluentValidation;
namespace CQRS.Application.Commands.TodoCommands
{
    public class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoValidator()
        {
            RuleFor(a => a.Title).NotEmpty();
        }
    }
}
