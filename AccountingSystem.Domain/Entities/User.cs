using AccountingSystem.Domain.Enum;
using BCrypt.Net;

namespace AccountingSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public UserRoleEnum Role { get; set; } = UserRoleEnum.Admin;
        public void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
