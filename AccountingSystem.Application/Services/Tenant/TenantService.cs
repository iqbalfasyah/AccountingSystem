
using System.Threading;

namespace AccountingSystem.Application.Services.Tenant
{
    public class TenantService : ITenantService
    {
        private static readonly AsyncLocal<int?> _tenantId = new();

        public int? GetTenant() => _tenantId.Value;

        public void SetTenant(int tenantId) => _tenantId.Value = tenantId;
    }

}
