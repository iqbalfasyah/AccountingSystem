namespace AccountingSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Hashed Password
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
