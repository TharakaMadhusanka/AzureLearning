| Action/ Feature          | PL  | SECD |
| ------------------------ | --- | ---- |
| Download File            | ❎  | ❎   |
| Submit Clearance Request | ❎  | ❌   |
| View Clearance Request   | ❎  | ❌   |
| Edit Clearance Request   | ❎  | ❌   |

1. Where are those role permissions defined?
2. What are the different roles? PL, SECD etc.
3. Who are the different users can submit clearance requests for a project ? only PL?

Ref: [Custom Authorization Attribute](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iard?view=aspnetcore-10.0)

- [Checklist](https://mathieu.fenniak.net/the-api-checklist/)

# Feature Flags

- Feature Flag Table

`public class FeatureFlag
{
    public Guid Id { get; set; }
    public string Key { get; set; }     // "new-dashboard"
    public bool IsEnabled { get; set; }
    public string? AllowedRoles { get; set; }   // CSV: Admin,Manager
    public string? AllowedTenants { get; set; } // Optional
    public DateTime? StartsAt { get; set; }
    public DateTime? EndsAt { get; set; }
}
`

- Expose API to retrieve all the enabled features based on the ROLE Permission & Feature Flags.
  - Retrieve enabled features set on FE Load.
- Implement API Authorization (As in grouping APIs for feature wise, Group wise etc.)

# API Authorization Rules

- Project is allowed to view only set of authorized users (ex. only the project's PL can view/ edit, not different PL)
- Each API should be segementized based on accessible groups/ roles

# API implementation check list

| Category               | Considerations / Notes                                                                |
| ---------------------- | ------------------------------------------------------------------------------------- |
| API Contract & Design  | Clear resource naming, predictable verbs, versioning, error model, pagination/sorting |
| Versioning             | /api/v1/, /api/v2/, backward compatibility                                            |
| Security               | OAuth2/Entra ID, role & policy auth, feature flags, rate limiting, CORS               |
| Validation & Contracts | Input validation (FluentValidation), prevent bad data, protect DB                     |
| Error Handling         | Standard format with code, message, traceId                                           |
| Observability          | Correlation ID, structured logging, metrics, health checks, OpenTelemetry             |
| Performance            | Caching, compression, pagination, async I/O, bulk endpoints, SQL indexing             |
| Backward Compatibility | Optional new fields, new endpoints for breaking changes                               |
| Idempotency            | Idempotent endpoints for critical actions (use Idempotency-Key header)                |
| Rate Limiting          | Protect from brute force / DOS attacks                                                |
| Documentation          | Swagger/OpenAPI, example payloads, error cases, version notes                         |
| Security Headers       | CSP, HSTS, X-Frame-Options                                                            |
| Background Processing  | Use queues, workers, Kafka/Service Bus                                                |
| Testing                | Unit, integration, contract, load, chaos testing                                      |
| Feature Flags          | Safe rollout, toggle features for roles, tenants, environment                         |

- Logging: Serilog

| Log Type                            | Best Storage                                  | Notes                                                               |
| ----------------------------------- | --------------------------------------------- | ------------------------------------------------------------------- |
| **Debug / Info / Performance logs** | **File / JSON**                               | Use **Serilog file sink**, ship to ELK / Seq / Grafana              |
| **Warning / Error / Critical**      | **File + optional DB**                        | Critical errors + audit logs can also go to MSSQL for easy querying |
| **Audit logs / Compliance logs**    | **DB**                                        | Must be persisted, easy to query, rarely deleted                    |
| **Metrics / Tracing**               | **Prometheus / OpenTelemetry / App Insights** | Separate from logging; do not store metrics in DB logs              |
