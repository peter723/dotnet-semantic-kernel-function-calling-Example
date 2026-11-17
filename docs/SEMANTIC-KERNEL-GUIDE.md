# Semantic Kernel guide

## What Semantic Kernel does

Semantic Kernel orchestrates model calls and registered application functions. It does not automatically understand your database or expose every method in your codebase.

## Plugins and functions

See `src/EventsAssistant.Infrastructure/SemanticKernel/EventsPlugin.cs`.

- `[KernelFunction]` exposes a method as a tool.
- `[Description]` describes the tool and each parameter to the model.
- The model decides whether to request a tool based on the user's message and the available descriptions.
- Semantic Kernel invokes the selected C# method and passes the result back to the model.

## Automatic function calling

The chat assistant uses `FunctionChoiceBehavior.Auto()`. The Kernel can handle tool requests during the chat completion call. Only functions registered on that Kernel are available.

## Why the plugin calls EventService

The plugin delegates to an Application service instead of injecting DbContext. That keeps business rules reusable from controllers, background jobs, and AI tools. Persistence can change without rewriting the plugin.

## Add a tool to your project

1. Implement the use-case in Application.
2. Add repository or external-service abstractions as needed.
3. Implement infrastructure dependencies.
4. Add a focused plugin method with `[KernelFunction]` and clear descriptions.
5. Register the plugin with the Kernel.
6. Test business logic independently from the model interaction.

Function Calling is useful for invoking defined operations and retrieving live structured data. RAG is useful for retrieving relevant context from larger document collections. They can be combined.
