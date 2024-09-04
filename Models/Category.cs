using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserCategorySystem.Models;
#nullable disable

[Table("Category")]
public class Category
{
    public Category()
    {
        SubCategories = new HashSet<SubCategory>();
        UserCategories = new HashSet<UserCategory>();
    }

    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50,ErrorMessage = "Maximum limit is 50 characters")]
    public string Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<SubCategory> SubCategories { get; set; }
    [JsonIgnore]
    public virtual ICollection<UserCategory> UserCategories { get; set; }
}