# ChitChat Backend

Real-time chat backend built with **.NET 8, ASP.NET Core and SignalR**. Personal project exploring real-time communication, presence tracking and a permission-based messaging model.

Frontend repository: [chit-chat-frontend](https://github.com/ViniiSouza/chit-chat-frontend)

> Part of the early commit history lives in the frontend repository; this repo was split out later to separate backend and frontend.

## Features

- **Real-time messaging** over SignalR (WebSockets): private one-to-one conversations with server-pushed `MessageReceived` and `NewConversation` events.
- **Presence tracking**: online/offline status broadcast to subscribed clients, with `LastSeen` persisted on disconnect and a limit of 3 concurrent connections per user.
- **Message requests**: users with private profiles can only be messaged after accepting an invite. Acceptance creates mutual messaging permissions.
- **Contacts**: address book independent from conversations.
- **JWT authentication** for both REST endpoints and the SignalR hub (token via query string on `/hubs` paths, standard SignalR pattern).
- **Paginated message history**.

## Architecture

Two projects in a layered design:

```
ChatAPI (presentation)
  └─ Controllers: Authentication · User · Conversation
Chat (class library)
  ├─ Domain        entities + repository/service interfaces
  ├─ Application   app services · DTOs · AutoMapper profiles
  ├─ Infra         EF Core DbContext · generic Repository<T> · Unit of Work · Fluent API mappings
  ├─ Hubs          ChatHub + in-memory connection tracking
  └─ Security      TokenService (JWT) · DI registration · JwtMiddleware
```

**Patterns:** Repository + Unit of Work, generic base repository/service, DTO layer with AutoMapper (profiles auto-registered by reflection), interface-driven DI, Fluent API entity configuration.

**Stack:** .NET 8 · ASP.NET Core · SignalR · Entity Framework Core 8 (code-first, full migration history) · SQL Server · AutoMapper · JWT Bearer.

## Running locally

Prerequisites: .NET 8 SDK and a SQL Server instance.

1. Set the `UserConnection` connection string and the JWT `Secret` in `ChatAPI/appsettings.Development.json`.
2. Apply migrations: `dotnet ef database update` (creates the `LiveChat` database).
3. `cd ChatAPI && dotnet run`. API at `https://localhost:7180` / `http://localhost:5180`, Swagger UI at `/swagger` in Development.
4. Start the [frontend](https://github.com/ViniiSouza/chit-chat-frontend), register a user and log in.

## Status and roadmap

Functionally complete for private chats; never deployed (hosting a always-on SignalR backend was outside the budget for a side project). Honest gaps I'd address next:

- Group conversations (modeled in the domain via `EChatType.Group` and member addition/removal messages, but not implemented).
- Typing indicators.
- Redis backplane for SignalR: connection tracking is currently in-memory (static dictionaries), so it only scales to a single instance.
- Externalize configuration (connection string and JWT secret out of appsettings, URLs to environment variables).
