using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
