# .NET Semantic Kernel Function Calling Example

A small .NET 10 console application demonstrating how to connect an AI model to backend functionality using **Microsoft Semantic Kernel Function Calling** and **OpenRouter**.

The AI does not access the repository or database directly. Semantic Kernel exposes selected C# methods as tools. The model decides when a tool is useful, Semantic Kernel invokes the method, and the result is sent back to the model to formulate the response.

## What it demonstrates

- Connecting Semantic Kernel to OpenRouter through the OpenAI-compatible API.
- Registering a C# plugin with `[KernelFunction]`.
- Describing tools and their parameters with `[Description]`.
- Enabling automatic function selection with `FunctionChoiceBehavior.Auto()`.
- Returning backend data to the model.
- Executing a booking operation through a backend method.

## Available tools

| Tool | Purpose |
| --- | --- |
| `get_upcoming_events` | Returns upcoming events and available seats. |
| `get_event_details` | Returns details for a specific event ID. |
| `book_event` | Books one seat for an attendee. |

## Requirements

- .NET 10 SDK
- An [OpenRouter](https://openrouter.ai/) API key

## Run the project

### 1. Set your API key

PowerShell:

```powershell
$env:OPENROUTER_API_KEY="your-openrouter-api-key"
```

Bash:

```bash
export OPENROUTER_API_KEY="your-openrouter-api-key"
```

Optionally, choose another model available on OpenRouter:

```powershell
$env:OPENROUTER_MODEL="google/gemini-2.5-flash"
```

### 2. Restore and run

```bash
dotnet restore
dotnet run
```

## Try it

Ask questions such as:

- What events are coming up?
- Tell me more about the AI Engineering Workshop.
- How many seats are available for event 1?
- Book one seat for Peter in event 1.

You can ask in Arabic or English. The system prompt instructs the assistant to respond in the user's language.

## Project structure

```text
.
├── Models/
│   └── Event.cs
├── Plugins/
│   ├── EventsPlugin.cs
│   ├── IEventRepository.cs
│   └── InMemoryEventRepository.cs
├── Program.cs
├── appsettings.json
└── SemanticKernel.OpenRouter.FunctionCalling.csproj
```

## Important notes

- The repository is in-memory and is reset whenever the application restarts. It is only for demonstrating the integration flow.
- In a real application, implement `IEventRepository` using your existing application services or database-backed repository.
- Keep API keys in environment variables or a secret manager. Never commit real credentials.
- Validate user authorization and business rules in the backend method. A tool call is not a replacement for application security.
