using System;

namespace CQRS.Data.Configurtion.Contracts
{
    public interface IDateTime
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
