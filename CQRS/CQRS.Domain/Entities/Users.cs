using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CQRS.Domain.Entities
{
    public class Users : IdentityUser<int>
    {
        public Users()
        {
            this.TodoCreators = new List<Todo>();
            this.TodoEditors = new List<Todo>();
        }
        public string FullName { get; set; }
        public ICollection<Todo> TodoCreators { get; set; }
        public ICollection<Todo> TodoEditors { get; set; }
    }
}
