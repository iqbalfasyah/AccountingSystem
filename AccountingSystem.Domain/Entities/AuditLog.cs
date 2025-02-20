using AccountingSystem.Domain.Enum;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AccountingSystem.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public JsonDocument OldData { get; set; }
        public JsonDocument NewData { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
