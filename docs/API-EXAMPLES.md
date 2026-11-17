# API examples

## List upcoming events

```http
GET https://localhost:7241/api/events
```

## Get event details

```http
GET https://localhost:7241/api/events/1
```

## Book a seat

```http
POST https://localhost:7241/api/events/1/bookings
Content-Type: application/json

{ "attendeeName": "Peter" }
```

## Chat

```http
POST https://localhost:7241/api/chat
Content-Type: application/json

{
  "messages": [
    { "role": "user", "content": "What events are coming up?" }
  ]
}
```

Send previous user and assistant messages with each request to preserve context; this demo endpoint is stateless. Do not send a system role: server instructions are controlled by the application.
