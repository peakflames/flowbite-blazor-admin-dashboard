using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

/// <summary>
/// Represents a product in the CRUD products management system.
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier for the product.
    /// </summary>
    [Required(ErrorMessage = "Product ID is required")]
    public int Id { get; set; }

    /// <summary>
    /// Name of the product.
    /// </summary>
    [Required(ErrorMessage = "Product name is required")]
    public string Name { get; set; } = "";

    /// <summary>
    /// Category of the product (e.g., HTML Templates, UI Kit, Dashboard, Component Library).
    /// </summary>
    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = "";

    /// <summary>
    /// Technology or brand associated with the product (e.g., Angular, React JS, Vue, Svelte).
    /// </summary>
    [Required(ErrorMessage = "Technology/Brand is required")]
    public string Technology { get; set; } = "";

    /// <summary>
    /// Description of the product.
    /// </summary>
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; } = "";

    /// <summary>
    /// Price of the product (e.g., "$149", "$129").
    /// </summary>
    [Required(ErrorMessage = "Price is required")]
    public string Price { get; set; } = "";

    /// <summary>
    /// Discount applied to the product (e.g., "No", "10%", "25%").
    /// </summary>
    public string Discount { get; set; } = "No";
}
