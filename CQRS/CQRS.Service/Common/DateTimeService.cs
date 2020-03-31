using CQRS.Data.Configurtion.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Service
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
