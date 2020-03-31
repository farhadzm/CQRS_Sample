using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRS.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, string[]> Failures { get; }
        public ValidationException()
            : base("One ore more validation failures have accurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }
        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            var failureGroups = failures.GroupBy(a => a.PropertyName, a => a.ErrorMessage);
            foreach (var item in failureGroups)
            {
                var propertyName = item.Key;
                var propertyFailures = item.ToArray();
                Failures.Add(propertyName, propertyFailures);
            }
        }
    }
}
