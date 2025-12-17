#### Implementing a Domain‑Driven Design .NET Microservices Architecture with Clean Code.
📦 src  
 └── 📂 services  
     └── 📂 UserService  
         ├── 🐳 Dockerfile  
         ├── 📚 docs  
         │   ├── 📘 Domain Driven Design  
         │   ├── 🧼 Clean Architecture  
         │   └── 🧩 Microservices Architecture  
         ├── 📂 src  
         │   ├── 🌐 UserService.Api  
         │   │   ├── ⚙️ appsettings.json  
         │   │   ├── 📂 Controllers  
         │   │   │   └── 👤 UserController.cs  
         │   │   ├── 📂 DTOs  
         │   │   │   └── 📂 User  
         │   │   │       └── 📝 SignUpUserDto.cs  
         │   │   ├── 🧾 log.txt  
         │   │   ├── 🧱 Middlewares  
         │   │   │   ├── 🔗 CrossCuttingMiddleware.cs  
         │   │   │   └── 🛑 ExceptionHandlingMiddleware.cs  
         │   │   ├── 🚀 Program.cs  
         │   │   ├── 📄 UserService.Api.csproj  
         │   │   ├── 🔌 Interfaces  
         │   │   │   └── 🧩 ISignUpUserService.cs  
         │   │   ├── 🔐 Security  
         │   │   │   └── 🔑 IPasswordHasher.cs  
         │   │   ├── 🧠 UseCases  
         │   │   │   └── 🧩 SignUpUser  
         │   │   │       ├── 📨 SignUpUserCommand.cs  
         │   │   │       ├── 🛠️ SignUpUserCommandHandler.cs  
         │   │   │       ├── 🧪 SignUpUserService.cs  
         │   │   │       └── ✔️ SignUpUserValidator.cs  
         │   │   └── 📄 UserService.Application.csproj  
         │   ├── 🧬 UserService.Domain  
         │   │   ├── 🧱 Abstractions  
         │   │   │   ├── 🧩 AggregateRoot.cs  
         │   │   │   ├── 🧩 DomainEvent.cs  
         │   │   │   ├── 🧩 Entity.cs  
         │   │   │   └── 🧩 ValueObject.cs  
         │   │   ├── 🧩 Aggregates  
         │   │   │   └── 👤 UserAggregate  
         │   │   │       ├── 🎉 Events  
         │   │   │       │   ├── 🟢 UserActivatedEvent.cs  
         │   │   │       │   ├── ✨ UserCreatedEvent.cs  
         │   │   │       │   ├── 📧 UserEmailChangedEvent.cs  
         │   │   │       │   ├── 📝 UserFullNameChangedEvent.cs  
         │   │   │       │   ├── 🔒 UserLockedEvent.cs  
         │   │   │       │   ├── 🔑 UserPasswordChangedEvent.cs  
         │   │   │       │   └── 🎭 UserRoleChangedEvent.cs  
         │   │   │       └── 👤 User.cs  
         │   │   ├── 📦 Common  
         │   │   │   └── 📄 Result.cs  
         │   │   ├── 🚨 Exceptions  
         │   │   │   ├── 📁 Abstraction  
         │   │   │   ├── ❗ DomainException.cs  
         │   │   │   ├── 📁 Common  
         │   │   │   ├── 📁 ValueObjects  
         │   │   ├── 🏭 Factories  
         │   │   │   └── 🏗️ UserFactory.cs  
         │   │   ├── 🔌 Interfaces  
         │   │   ├── 📤 Outbox  
         │   │   ├── 🗄️ Repositories  
         │   │   ├── 🛠️ Services  
         │   │   ├── 🔄 UnitOfWork  
         │   │   ├── 📄 UserService.Domain.csproj  
         │   │   └── 🧩 ValueObjects  
         │   └── 🏗️ UserService.Infrastructure  
         │       ├── 🔗 CrossCutting  
         │       ├── 🧩 DependencyInjection  
         │       ├── 🚨 Exceptions  
         │       ├── 📬 Messaging  
         │       ├── 🗄️ Persistence  
         │       ├── 🗃️ Repositories  
         │       ├── 🔐 Security  
         │       ├── ⚙️ Settings  
         │       ├── 🔄 UnitOfWork  
         │       └── 📄 UserService.Infrastructure.csproj  
         └── 🧩 UserService.sln  
