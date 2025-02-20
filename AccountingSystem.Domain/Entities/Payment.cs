using AccountingSystem.Domain.Enum;
using BCrypt.Net;

namespace AccountingSystem.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
