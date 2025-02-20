using AccountingSystem.Domain.Enum;
using BCrypt.Net;

namespace AccountingSystem.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public int AccountId { get; set; }
        public ChartOfAccount Account { get; set; }
        public TransactionTypeEnum Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
