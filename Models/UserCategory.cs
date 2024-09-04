using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
#nullable disable

namespace UserCategorySystem.Models;

[Table("UserCategory")]
public class UserCategory
{
    [Key]
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [InverseProperty("UserCategories")]
    [JsonIgnore]
    public virtual Category Category { get; set; }

    [ForeignKey(nameof(UserId))]
    [InverseProperty("UserCategories")]
    [JsonIgnore]
    public virtual User User { get; set; }
}