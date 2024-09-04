using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserCategorySystem.Models;
#nullable disable

[Table("User")]
public class User
{
    public User()
    {
        UserCategories = new HashSet<UserCategory>();
    }

    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(20, ErrorMessage = "Maximum limit is 20 characters")]
    public string Name { get; set; }
    
    public string Phone { get; set; }

    [EmailAddress(ErrorMessage = "Enter valid email address")]
    public string Email { get; set; }

    public string Password { get; set; }

    public bool IsAdmin { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserCategory> UserCategories { get; set; }
}