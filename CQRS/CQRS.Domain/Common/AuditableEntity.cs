using CQRS.Domain.Entities;
using System;

namespace CQRS.Domain.Common
{
    public class AuditableEntity : BaseEntity
    {
        public int UserCreatorId { get; set; }
        public int? UserEditorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
