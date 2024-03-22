using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class UserModel
    {

        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        public string? UrlImage { get; set; } = null!;

        public string? Phone { get; set; } = null!;

        [StringLength(1000)]
        public string? Biography { get; set; } = null!;

        public int? AddressId { get; set; }

        public AccountDetailsAddressModel? Address { get; set; }
    }
}
