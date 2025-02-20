using AccountingSystem.Domain.Enum;
using BCrypt.Net;

namespace AccountingSystem.Domain.Entities
{
    public class ChartOfAccount
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string Name { get; set; }
        public Guid Code { get; set; }
        public AccountTypeEnum Type { get; set; }
        public int ParentId { get; set; }
        public ChartOfAccount Parent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
