# .NET Semantic Kernel Function Calling — Learning Repository

A practical reference for developers who want to understand **Microsoft Semantic Kernel** and integrate **Function Calling / Tool Calling** into a .NET backend.

This repository contains two learning paths:

1. **Root console sample** — the smallest possible Kernel + plugin example.
2. **Layered ASP.NET Core Web API** — a fuller example with controllers, application services, domain entities, EF Core, SQLite, and Semantic Kernel.

## Start here

| If you want to learn... | Open |
|---|---|
| The smallest working concept | Root `Program.cs` and `Plugins/EventsPlugin.cs` |
| How tools are exposed to the model | `src/EventsAssistant.Infrastructure/SemanticKernel/EventsPlugin.cs` |
| How the Kernel calls the model and handles tools | `src/EventsAssistant.Infrastructure/SemanticKernel/SemanticKernelChatAssistant.cs` |
| How tool calls reach business logic | `src/EventsAssistant.Application/Services/EventService.cs` |
| How data is persisted | `src/EventsAssistant.Infrastructure/Persistence/` |
| How an API exposes chat and normal backend endpoints | `src/EventsAssistant.Api/Controllers/` |
| Architecture and design decisions | `docs/ARCHITECTURE.md` |
| Semantic Kernel concepts | `docs/SEMANTIC-KERNEL-GUIDE.md` |
| Sample HTTP requests | `docs/API-EXAMPLES.md` |

## Architecture

```text
src/
├── EventsAssistant.Domain/
│   └── Entities/
├── EventsAssistant.Application/
│   ├── Abstractions/
│   ├── Contracts/
│   └── Services/
├── EventsAssistant.Infrastructure/
│   ├── Persistence/
│   └── SemanticKernel/
└── EventsAssistant.Api/
    └── Controllers/
```

The model does **not** connect directly to the database and does not execute SQL. It can only request functions registered in Semantic Kernel. A plugin delegates to an application service, which applies business rules and uses a repository implemented with EF Core.

## Function-calling flow

1. The user sends a message to `POST /api/chat`.
2. `ChatController` validates the request and calls `IChatAssistant`.
3. `SemanticKernelChatAssistant` builds chat history and sends it to the model.
4. The model can request one of the registered functions.
5. Semantic Kernel invokes the corresponding method in `EventsPlugin`.
6. The plugin calls `EventService`, which uses the repository and domain entity.
7. EF Core reads or writes SQLite data.
8. The tool result is returned to the model, which writes the final response.

## Requirements

- .NET 10 SDK
- OpenRouter API key
- Internet connection for model requests

## Run the Web API example

### 1. Configure the API key

PowerShell:

```powershell
$env:OpenRouter__ApiKey="YOUR_OPENROUTER_API_KEY"
```

Bash:

```bash
export OpenRouter__ApiKey="YOUR_OPENROUTER_API_KEY"
```

The model defaults to `google/gemini-2.5-flash`. Override it if needed:

```powershell
$env:OpenRouter__Model="google/gemini-2.5-flash"
```

### 2. Restore and run

From the repository root:

```bash
dotnet restore src/EventsAssistant.Api/EventsAssistant.Api.csproj
dotnet run --project src/EventsAssistant.Api/EventsAssistant.Api.csproj
```

The API creates and seeds a local SQLite database named `events-assistant.db` on startup. OpenAPI JSON is available at `/openapi/v1.json` in Development.

## API endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/events` | List upcoming events |
| GET | `/api/events/{id}` | Get event details |
| POST | `/api/events/{id}/bookings` | Create a booking directly |
| POST | `/api/chat` | Chat with the AI assistant |

See [API examples](docs/API-EXAMPLES.md).

## Available AI tools

| Function | Purpose |
|---|---|
| `get_upcoming_events` | Gets upcoming events and available seats |
| `get_event_details` | Gets details for an event ID |
| `book_event` | Books a seat for an attendee |

The functions live in `EventsPlugin.cs`. The plugin does not contain booking rules or database code; it delegates to `EventService`.

## Important notes

- The API sample uses SQLite and EF Core so you can follow data from HTTP request to database.
- `EnsureCreated` is used to keep first run simple. Use EF Core migrations in real applications.
- Booking demonstrates layered design, not production-grade concurrency control. Concurrent bookings require a transaction or atomic conditional update to prevent overselling.
- Add authentication and authorization before exposing booking tools in a real application.
- Prompts and tool descriptions are not security boundaries. Always enforce permissions and business rules in backend code.
- The chat API is stateless. Send previous user/assistant messages with each request to preserve context.
- Never commit API keys. Use environment variables or a secret manager.

## License

This project is an educational reference. Add a license file if you want to specify reuse terms.
