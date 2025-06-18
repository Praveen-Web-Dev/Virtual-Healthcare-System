using System.ComponentModel.DataAnnotations;

namespace GpReg.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required,StringLength(100)]
        public string FullName { get; set; }
        [Required, EmailAddress]

        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
