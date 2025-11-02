using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Components;
using WebApp.Domain;

namespace WebApp.Pages.Components;

public partial class ProductDrawer : ComponentBase
{
  private readonly List<Product> Products = new()
  {
    new Product
    {
      Id = 1,
      Name = "iPhone 15 Pro",
      Price = "1199",
      Technology = "APPLE",
      Category = "SMARTPHONE",
      Description = "Latest iPhone with titanium design and A17 Pro chip",
      Discount = "5"
    },
    new Product
    {
      Id = 2,
      Name = "MacBook Air M3",
      Price = "1299",
      Technology = "APPLE",
      Category = "LAPTOP",
      Description = "Lightweight laptop with M3 chip and 18-hour battery life",
      Discount = "10"
    },
    new Product
    {
      Id = 3,
      Name = "Samsung Galaxy S24",
      Price = "899",
      Technology = "SAMSUNG",
      Category = "SMARTPHONE",
      Description = "AI-powered smartphone with excellent camera",
      Discount = ""
    }
  };

  private readonly IReadOnlyList<OptionItem> TechnologyOptions = new[]
  {
    new OptionItem("APPLE", "Apple"),
    new OptionItem("SAMSUNG", "Samsung"),
    new OptionItem("GOOGLE", "Google"),
    new OptionItem("MICROSOFT", "Microsoft")
  };

  private readonly IReadOnlyList<OptionItem> CategoryOptions = new[]
  {
    new OptionItem("SMARTPHONE", "Smartphone"),
    new OptionItem("LAPTOP", "Laptop"),
    new OptionItem("TABLET", "Tablet"),
    new OptionItem("ACCESSORY", "Accessory")
  };

  private readonly IReadOnlyList<OptionItem> DiscountOptions = new[]
  {
    new OptionItem("5", "5% Off"),
    new OptionItem("10", "10% Off"),
    new OptionItem("15", "15% Off"),
    new OptionItem("20", "20% Off"),
    new OptionItem("25", "25% Off")
  };

  private ProductFormModel _formModel = new();
  private bool _isDrawerOpen;
  private int? _editingProductId;
  private string _drawerTitle = "Add New Product";
  private string _submitLabel => _editingProductId.HasValue ? "Save Changes" : "Add Product";

  private void OpenAddProduct()
  {
    _editingProductId = null;
    _drawerTitle = "Add New Product";
    _formModel = new ProductFormModel();
    _isDrawerOpen = true;
  }

  private void EditProduct(Product product)
  {
    _editingProductId = product.Id;
    _drawerTitle = "Edit Product";
    _formModel = ProductFormModel.FromProduct(product);
    _isDrawerOpen = true;
  }

  private void DuplicateProduct(Product product)
  {
    _editingProductId = null;
    _drawerTitle = "Duplicate Product";
    _formModel = ProductFormModel.FromProduct(product);
    _formModel.Name = $"Copy of {product.Name}";
    _formModel.Discount = "";
    _isDrawerOpen = true;
  }

  private void OpenSmartphoneAppleTemplate() => CreateFromTemplate("SMARTPHONE", "APPLE");

  private void OpenSmartphoneSamsungTemplate() => CreateFromTemplate("SMARTPHONE", "SAMSUNG");

  private void OpenLaptopAppleTemplate() => CreateFromTemplate("LAPTOP", "APPLE");

  private void CreateFromTemplate(string category, string technology)
  {
    _editingProductId = null;
    _drawerTitle = $"Add New {FormatTitle(category)}";
    _formModel = new ProductFormModel
    {
      Category = category,
      Technology = technology,
      Description = $"New {category.ToLower()} product"
    };
    _isDrawerOpen = true;
  }

  private void DeleteProduct(int productId)
  {
    var index = Products.FindIndex(p => p.Id == productId);
    if (index >= 0)
    {
      Products.RemoveAt(index);
      StateHasChanged();
    }
  }

  private void CloseDrawer()
  {
    _isDrawerOpen = false;
  }

  private void HandleValidSubmit()
  {
    if (_editingProductId.HasValue)
    {
      var existing = Products.FirstOrDefault(p => p.Id == _editingProductId.Value);
      if (existing is not null)
      {
        existing.Name = _formModel.Name;
        existing.Price = _formModel.Price;
        existing.Technology = _formModel.Technology;
        existing.Category = _formModel.Category;
        existing.Discount = _formModel.Discount;
        existing.Description = _formModel.Description;
      }
    }
    else
    {
      var nextId = Products.Count == 0 ? 1 : Products.Max(p => p.Id) + 1;
      Products.Add(new Product
      {
        Id = nextId,
        Name = _formModel.Name,
        Price = _formModel.Price,
        Technology = _formModel.Technology,
        Category = _formModel.Category,
        Discount = _formModel.Discount,
        Description = _formModel.Description
      });
    }

    CloseDrawer();
  }

  private static string FormatTitle(string value) =>
    string.IsNullOrWhiteSpace(value)
      ? "Product"
      : string.Join(' ', value.ToLower().Split(' ').Select(UppercaseFirst));

  private static string UppercaseFirst(string input) =>
    string.IsNullOrEmpty(input)
      ? input
      : char.ToUpperInvariant(input[0]) + input[1..];

  private sealed class ProductFormModel
  {
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    public string Price { get; set; } = string.Empty;

    [Required(ErrorMessage = "Technology/Brand is required")]
    public string Technology { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = string.Empty;

    public string Discount { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public static ProductFormModel FromProduct(Product product) => new()
    {
      Name = product.Name,
      Price = product.Price,
      Technology = product.Technology,
      Category = product.Category,
      Discount = product.Discount,
      Description = product.Description
    };
  }

  private readonly record struct OptionItem(string Value, string Label);
}
