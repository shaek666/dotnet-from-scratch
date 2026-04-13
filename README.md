# dotnet-from-scratch

Written in C#, this is a from-scratch learning project built to understand what .NET and ASP.NET Core are doing under the hood. The goal is not to replace the framework. The goal is to rebuild enough of it by hand to understand attributes, delegates, transient, scoped, singleton lifetimes, routing, controllers, and the rest of the moving parts at the lowest level.

## Why this exists

I wanted a project that forces me to actually understand the pieces people usually take for granted in .NET.

This repo is where I experiment with:

- Attributes and reflection
- Delegates and route handlers
- Dependency injection and service lifetimes
- Controller discovery
- Custom HTTP method attributes
- A lightweight request pipeline
- A tiny web host built with plain C#

## What is in here

- `ServiceCollection` and `ServiceProvider` for registering and resolving services
- `Transient`, `Singleton`, and `Scoped` lifetimes
- `HttpGet` and `HttpPost` attributes for controller actions
- A simple `Router` and `Endpoint` model
- A minimal `WebApplication` builder and runner
- A lightweight `TcpServer`
- Example controllers and services to test the whole flow

## What I am learning here

- How service registration turns into object creation
- Why `Transient`, `Singleton`, and `Scoped` behave differently
- How attributes can drive behavior at runtime
- How delegates can stand in for route handlers
- How a controller can be discovered and mapped without the normal ASP.NET Core pipeline
- How a request turns into a response with just C# and the base class library

## Run it

```bash
dotnet run
```

Then hit:

```text
GET /codecamp
```

The app starts a small server and responds through the custom routing layer.

## Notes

This is intentionally small and direct. I am using it to learn the real mechanics of .NET, not to build a production framework. The point is to make the hidden stuff visible.