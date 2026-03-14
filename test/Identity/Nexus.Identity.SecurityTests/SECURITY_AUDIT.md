# Identity Module Security Attack Surface Audit

## Authentication
- `AuthController.Login` and `AuthController.LoginRefreshToken` are anonymous endpoints and have no visible rate-limit or lockout control.
- `LoginHandler` returns failure responses but does not implement account lockout or progressive delay for repeated invalid credentials.
- `AuthController.RevokeRefreshToken` is callable without `[Authorize]`.

## Authorization
- `UserController` has the controller-level `[Authorize]` attribute commented out, exposing user listing/update/delete operations.
- `UserRoleController` and role-management surfaces rely on authenticated access but do not enforce role-based attributes for admin-only operations.

## JWT / Token Security
- JWTs use symmetric signing and validation checks for issuer/audience/signature/lifetime.
- No anti-replay state is enforced for access tokens (same token can be validated repeatedly until expiry).

## Injection and Input Handling
- Commands such as `CreateUserCommand` and `LoginQuery` do not define explicit validators in the Identity module.
- User input appears to be mapped directly into entities, so script payloads can persist and become stored-XSS vectors if rendered downstream.

## Sensitive Data Exposure
- Static configuration includes a hard-coded SQL connection string and JWT secret in `appsettings.json`.
