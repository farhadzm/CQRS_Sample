using CQRS.Application.Commands.TodoCommands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Application.Validators.TodoValidators
{
    public class EditTodoValidator : AbstractValidator<EditTodoCommand>
    {
        public EditTodoValidator()
        {
            RuleFor(a => a.Title).NotEmpty().MaximumLength(250);
            RuleFor(a => a.Id).NotNull().NotEqual(0);
        }
    }
}
