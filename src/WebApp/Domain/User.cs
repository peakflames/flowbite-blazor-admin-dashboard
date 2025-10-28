using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain;

/// <summary>
/// Represents a user in the CRUD users management system.
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Full name of the user.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = "";

    /// <summary>
    /// Avatar image filename.
    /// </summary>
    public string Avatar { get; set; } = "";

    /// <summary>
    /// Email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = "";

    /// <summary>
    /// Biography or description of the user.
    /// </summary>
    public string Biography { get; set; } = "";

    /// <summary>
    /// Job position or role of the user.
    /// </summary>
    [Required(ErrorMessage = "Position is required")]
    public string Position { get; set; } = "";

    /// <summary>
    /// Country where the user is located.
    /// </summary>
    public string Country { get; set; } = "";

    /// <summary>
    /// Current status of the user (Active, Offline).
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// First name extracted from full name (for form editing).
    /// </summary>
    public string FirstName
    {
        get
        {
            var parts = Name.Split(' ', 2);
            return parts.Length > 0 ? parts[0] : "";
        }
        set
        {
            var lastName = LastName;
            Name = string.IsNullOrEmpty(lastName) ? value : $"{value} {lastName}";
        }
    }

    /// <summary>
    /// Last name extracted from full name (for form editing).
    /// </summary>
    public string LastName
    {
        get
        {
            var parts = Name.Split(' ', 2);
            return parts.Length > 1 ? parts[1] : "";
        }
        set
        {
            var firstName = FirstName;
            Name = string.IsNullOrEmpty(firstName) ? value : $"{firstName} {value}";
        }
    }
}
