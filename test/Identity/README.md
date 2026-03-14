# Identity Test Architecture

- `UnitTests/Application`: handler/controller/behavior tests with Moq + FluentAssertions
- `UnitTests/Domain`: entity integrity tests for anemic model
- `UnitTests/Infrastructure`: isolated infrastructure service tests
- `IntegrationTests/Authentication`: end-to-end auth flows via API host
- `IntegrationTests/Persistence`: repository/database behavior with EF InMemory
- `IntegrationTests/API`: endpoint-level flows across Identity APIs
- `Builders`: fluent builders for realistic test data
- `Fixtures`: shared API host and auth fixture infrastructure
- `TestUtilities`: shared HTTP/json helpers
