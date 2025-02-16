using AccountingSystem.Application.Services.Tenant;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITenantService tenantService)
    {
        var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();
        if (!string.IsNullOrEmpty(tenantId) && int.TryParse(tenantId, out int id))
        {
            tenantService.SetTenant(id);
        }

        await _next(context);
    }
}
