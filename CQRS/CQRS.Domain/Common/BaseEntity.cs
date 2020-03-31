
namespace CQRS.Domain.Common
{
    public interface IBaseEntity
    {
    }
    public abstract class BaseEntity<T> : IBaseEntity
    {
        public T Id { get; set; }
    }
    public abstract class BaseEntity : BaseEntity<int>
    {
    }
}
