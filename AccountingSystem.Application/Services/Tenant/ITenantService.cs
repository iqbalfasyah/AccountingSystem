namespace AccountingSystem.Application.Services.Tenant
{
    public interface ITenantService
    {
        int? GetTenant();
        void SetTenant(int tenantId);
    }
}
