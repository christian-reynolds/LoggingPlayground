using Microsoft.AspNetCore.Http;
using Serilog;
using System.Linq;

namespace Logging
{
    public static class LogEnricher
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);

            // This is handled in MyLogger.cs via Nuget package
            //diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress.ToString());
            //diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
        }
    }
}
