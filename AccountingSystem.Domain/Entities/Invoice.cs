using AccountingSystem.Domain.Enum;
using BCrypt.Net;

namespace AccountingSystem.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public string CustomerName { get; set; }
        public InvoiceStatusEnum Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
