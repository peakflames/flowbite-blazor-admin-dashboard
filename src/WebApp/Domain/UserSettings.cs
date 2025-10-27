using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

public class UserSettings
{
    [Required]
    public string FirstName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Phone]
    public string Phone { get; set; } = "";

    public string? Birthday { get; set; }

    public string Organization { get; set; } = "";

    public string Role { get; set; } = "";

    public string Department { get; set; } = "";

    public string ZipCode { get; set; } = "";

    public string Country { get; set; } = "";

    public string City { get; set; } = "";

    public string Address { get; set; } = "";
}
