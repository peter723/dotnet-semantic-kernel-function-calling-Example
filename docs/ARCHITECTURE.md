# Architecture

This repository has two paths: the minimal root console sample, and the layered ASP.NET Core API under `src/`.

## Request flow

```text
HTTP -> ChatController -> IChatAssistant -> SemanticKernelChatAssistant
     -> Kernel chooses tool -> EventsPlugin -> EventService
     -> IEventRepository -> EfEventRepository -> AppDbContext -> SQLite
```

The model never connects to the database or runs SQL. It sees only registered tool descriptions, parameters, and results.

| Project | Responsibility |
|---|---|
| Domain | Entities and core business behavior |
| Application | Use cases, contracts, repository and assistant abstractions |
| Infrastructure | EF Core, SQLite, Semantic Kernel, OpenRouter |
| API | HTTP endpoints, startup, dependency injection |

## Production considerations

- Add authentication and authorization before exposing booking tools.
- Enforce permissions and business rules in backend code, never only in prompts.
- Use a transaction or atomic conditional update for seat reservation; this simple sample can oversell under concurrent requests.
- Use EF Core migrations instead of `EnsureCreated`.
- Persist conversation history and scope it to the authenticated user.
- Add rate limiting, observability, retries, and explicit confirmation for consequential actions.
