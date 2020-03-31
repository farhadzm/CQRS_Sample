using CQRS.Domain.Common;

namespace CQRS.Domain.Entities
{
    public class Todo : AuditableEntity
    {
        public string Title { get; set; }
        public bool Done { get; set; }
        public Users UserCreator { get; set; }
        public Users UserEditor { get; set; }
    }
}