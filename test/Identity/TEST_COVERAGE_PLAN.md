# Identity Module Test Coverage Plan

## Dependency map (public behavior focus)
- API Controllers -> IDispatcher, report services, auth policies
- Application Handlers -> IUnitOfWork repositories/services + mapping
- Infrastructure Services -> JwtTokenGenerator, OrderingQuery, PermissionService
- Persistence Repositories -> ApplicationDbContext (EF Core)
- Domain Entities -> state containers with audit fields

## Priority 1
- Command handlers: CreateUser, DeleteUser, UpdateRole, LoginRefreshToken
- Query handlers: Login, GetAllUser, GetPermissionsByRoleId
- Authentication services: JwtTokenGenerator, PermissionAuthorizationHandler / PermissionService

## Priority 2
- Repositories: GenericRepository, UserRepository, MenuRepository, PermissionRepository
- Domain entities: constructor/state integrity checks (anemic model verification)

## Priority 3
- API endpoints: login, refresh token, revoke token, create user, role assignment, permission retrieval

## Coverage strategy
- UnitTests/Application: handler orchestration, success/failure/edge/dependency failure paths
- UnitTests/Infrastructure: pure service logic (ordering, token creation)
- UnitTests/Domain: entity integrity and property behavior
- IntegrationTests/Persistence: repository behavior with InMemory EF Core
- IntegrationTests/API + Authentication: end-to-end API flows via WebApplicationFactory

## Naming convention
`MethodName_StateUnderTest_ExpectedBehavior`

## Target
- 70%+ on Identity module focus areas with incremental expansion path.
