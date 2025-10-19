// Models/ApplicationUser.cs
// This file defines the ApplicationUser entity that extends ASP.NET Core Identity.
// It adds custom properties specific to the NexaCart e-commerce platform.

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomRazorPage.Models
{
    /// <summary>
    /// Custom user class that extends ASP.NET Core IdentityUser.
    /// Adds e-commerce specific properties like shipping address and order history.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's first name for personalized experience.
        /// </summary>
        [PersonalData]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// User's last name for personalized experience.
        /// </summary>
        [PersonalData]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// User's full name computed from first and last name.
        /// </summary>
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// User's shipping address for order delivery.
        /// </summary>
        [PersonalData]
        [Display(Name = "Shipping Address")]
        [MaxLength(200)]
        public string ShippingAddress { get; set; } = string.Empty;

        /// <summary>
        /// User's billing address for payment processing.
        /// </summary>
        [PersonalData]
        [Display(Name = "Billing Address")]
        [MaxLength(200)]
        public string BillingAddress { get; set; } = string.Empty;

        /// <summary>
        /// User's phone number for order notifications.
        /// </summary>
        [PersonalData]
        [Display(Name = "Phone Number")]
        [MaxLength(20)]
        public override string? PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp when the user account was created.
        /// </summary>
        [Display(Name = "Account Created")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Timestamp of the user's last login.
        /// </summary>
        [Display(Name = "Last Login")]
        public DateTime? LastLoginDate { get; set; }

        /// <summary>
        /// Indicates if the user account is active.
        /// </summary>
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// User's preferred language for localization.
        /// </summary>
        [Display(Name = "Preferred Language")]
        [MaxLength(10)]
        public string PreferredLanguage { get; set; } = "en";

        /// <summary>
        /// User's preferred currency for pricing display.
        /// </summary>
        [Display(Name = "Preferred Currency")]
        [MaxLength(3)]
        public string PreferredCurrency { get; set; } = "USD";
    }
}
