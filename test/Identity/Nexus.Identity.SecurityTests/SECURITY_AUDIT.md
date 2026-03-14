# Identity Module Security Attack Surface Audit (OWASP Top 10 aligned)

## A01: Broken Access Control
- `UserController` has the controller-level `[Authorize]` attribute commented out, exposing listing/update/delete operations.
- `AuthController.RevokeRefreshToken` can be called without `[Authorize]`.
- Role-assignment endpoints are authenticated but do not enforce explicit role-based authorization.

## A02: Cryptographic Failures
- JWT secret is embedded in source configuration (`appsettings.json`) rather than external secret storage.
- Generated JWT includes `sub` but not `ClaimTypes.NameIdentifier`, which can cause identity-binding mismatches in controllers relying on `NameIdentifier`.

## A03: Injection
- Login flow accepts attacker-controlled values; SQLi payload attempts should be continuously tested.
- User creation flow currently accepts script payloads unchanged, enabling potential stored-XSS if rendered downstream.

## A04: Insecure Design
- No dedicated FluentValidation validators were identified for key security-sensitive flows (`LoginQuery`, `CreateUserCommand`).
- Login flow lacks brute-force protections such as lockout, throttling, or progressive delays.

## A05: Security Misconfiguration
- CORS policy config uses wildcard origin (`WithOrigins("*")`) together with broad method/header allowance.
- Hard-coded connection string and JWT settings remain in `appsettings.json`.

## A07: Identification and Authentication Failures
- Anonymous login and refresh-token endpoints have no visible anti-automation controls.

## A09: Security Logging and Monitoring Failures
- Exception messages are returned to API clients in handlers, risking internal detail disclosure.

## A10: Server-Side Request Forgery
- No direct SSRF surface was identified in the Identity module during static scan.
