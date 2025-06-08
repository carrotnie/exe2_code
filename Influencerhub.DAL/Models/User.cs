
using System;
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
        public string Email { get; set; }

        public Guid? RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role? Role { get; set; }

        /// Đánh dấu admin đã duyệt user (phê duyệt thủ công)
        public bool IsVerified { get; set; } = false;


        /// Đánh dấu user đã xác thực email (qua link hoặc mã xác thực)
        public bool IsEmailVerified { get; set; } = false;


        /// Token xác thực email (link/mail)
        public string? EmailVerificationToken { get; set; }

        public bool IsBlocked { get; set; } = false;

        /// Thời hạn hiệu lực của token xác thực email
        public DateTime? EmailVerificationTokenExpiry { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpireTimeRefreshToken { get; set; } = DateTime.UtcNow;

        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }

    }
}
