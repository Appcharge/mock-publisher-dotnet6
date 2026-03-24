# BUGBOT — mock-publisher-dotnet6

> Watches for routing defects, controller correctness, and configuration hygiene in this .NET 6 mock publisher service.

## Code Quality
- **Duplicate route segments**: Check `[Route]` attributes for double slashes — e.g. `/mocker//health` instead of `/mocker/health`.
- **Malformed path attributes**: Verify all route strings have no trailing slashes or typos before merging.

## Checklist
- [ ] All controller `[Route]` attributes are free of double or trailing slashes
- [ ] Route paths match the intended API surface exactly
- [ ] CI includes a route validation or lint step to catch path defects automatically
