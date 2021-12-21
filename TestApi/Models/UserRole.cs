using System.ComponentModel.DataAnnotations;

namespace TestApi.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
