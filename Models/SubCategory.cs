using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserCategorySystem.Models;
#nullable disable

[Table("SubCategory")]
public class SubCategory
{
    [Key]
    public int Id { get; set; }

    public int CategoryId { get; set; }

    [Required]
    public string Name { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [InverseProperty("SubCategories")]
    [JsonIgnore]
    public virtual Category Category { get; set; }
}