using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Influencerhub.DAL.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        public Guid? RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        public bool IsVerified { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public string? EmailVerificationToken { get; set; }
        public bool IsBlocked { get; set; } = false;
        public DateTime? EmailVerificationTokenExpiry { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpireTimeRefreshToken { get; set; } = DateTime.UtcNow;
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }
    }
}
