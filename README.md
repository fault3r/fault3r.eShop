
### A .NET Microservices Architecture Showcase

`fault3r.eShop` is a learning-oriented software project designed to demonstrate software architecture concepts, backend technologies, and the organization of a distributed application using the .NET ecosystem.

Rather than focusing only on application features, this project aims to make the underlying architecture visible: how services are organized, how infrastructure is configured, and how different technologies can work together in a microservices-based system.

## 🎯 Project Goals

- Explore modern software architecture patterns in the .NET ecosystem.
- Demonstrate how a system can be divided into independently organized services.
- Explore Clean Architecture and separation of concerns.
- Understand communication between services and supporting infrastructure.
- Demonstrate infrastructure configuration using Docker Compose.
- Provide a practical codebase for studying architectural decisions and backend engineering techniques.

## 🛠️ Technology Stack

| Technology | Purpose |
|---|---|
| C# / .NET | Application development |
| PostgreSQL | Relational database infrastructure |
| Redis | In-memory data store |
| RabbitMQ | Message broker and messaging infrastructure |
| Docker Compose | Local infrastructure orchestration |

The infrastructure technologies listed above are configured in the repository's Docker Compose file. Their exact usage by individual services should be confirmed against the application code.

## 📁 Repository Structure

```text
fault3r.eShop/
├── src/
│   └── services/
├── docker-compose.yml
├── .env
└── .gitignore
```

### `src/`

Contains the application's source code. This is the primary location for exploring the implementation and architectural organization of the system.

### `src/services/`

Contains the service projects that make up the application.

In a microservices architecture, each service should have a clearly defined responsibility and an explicit approach to communication with other services. The exact purpose, internal layers, and dependencies of each service are documented in the service-specific sections below.

> **Service documentation:** Add an entry for every actual service in this directory, including its business responsibility, internal project structure, technologies, and communication patterns.

### `docker-compose.yml`

Defines the local infrastructure containers required by the development environment.

The current configuration includes:

- **PostgreSQL:** relational database server.
- **Redis:** in-memory data store.
- **RabbitMQ:** message broker with its management interface.

Persistent Docker volumes are configured for the infrastructure services so their data can survive container recreation.

### `.env`

Contains environment variables used by Docker Compose to configure the infrastructure services, including database credentials, database name, and exposed ports.

For local development, use your own environment-specific values. Do not commit production credentials or real secrets to source control.

### `.gitignore`

Defines files and directories that Git should exclude from version control, such as generated build artifacts, local development files, and environment-specific configuration when covered by its rules.

## 🏛️ Architectural Overview

This repository is intended to serve as a practical way to explore the relationship between application architecture and infrastructure.

The main architectural areas to document are:

- **Services:** individual application components and their responsibilities.
- **Application layers:** the separation of business rules, application use cases, and external concerns, where implemented.
- **Persistence:** how services access and manage their data.
- **Inter-service communication:** how services exchange requests or messages.
- **Infrastructure:** how databases, caches, and message brokers support the application.
- **Dependency management:** how application components depend on abstractions and implementations.

The exact patterns used by each service should be understood from its implementation rather than assumed from the repository's overall architecture label.

## 🚧 Project Status

This repository is an evolving exploration of software architecture and backend technologies. Refer to the implementation of each service to determine which patterns and capabilities are currently implemented.


---

**The goal of `fault3r.eShop` is to make software architecture easier to explore through code, clear separation of responsibilities, and practical examples of backend technologies.**
