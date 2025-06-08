using System;

namespace Influencerhub.Common.DTO
{
    /// <summary>
    /// DTO dùng khi xác thực email qua link (token trên URL)
    /// </summary>
    public class EmailVerificationDTO
    {
        public string Token { get; set; }
    }
}
