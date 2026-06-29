# Watchman Devotional API 🛡️⛪

A scalable, robust backend REST API engineered with **.NET 8** and **C#** to power the Watchman Portal application. Built using a decoupling architecture with the Repository Pattern and Data Transfer Objects (DTOs), this system administers modern content distribution workflows, user roles, and interactive quizzes for ministry management.

## 🚀 Key Technical Features Built

* **Secure Admin Onboarding Workflow**: Designed an administrative provisioning system allowing root administrators to securely register regional ministry managers with automated type-safe input checks.
* **Decoupled Architecture (DTOs & Services)**: Abstracted persistent entities by enforcing clear data transfer boundaries via strong-typed structures (`RegisterDto`, `LoginDto`, `QuizSubmissionDto`).
* **Interactive Quiz Engine**: Structured backend controllers to handle, score, and persist user-submitted responses safely through dedicated API data layers.
* **Daily Manna Devotional Manager**: Created complete CRUD management endpoints with strict route protection allowing authorized administrators to seamlessly update text blocks, target release dates, and media metadata references.

---

## 🛠️ Tech Stack & Architecture

* **Framework**: .NET 8 Web API
* **Language**: C#
* **Patterns**: Repository Pattern, Single Responsibility Principle, Controller-Service-Repository separation.
* **Security**: Role-Based Access Control (RBAC), input formatting rules, and environment isolation (`.gitignore` structured to dynamically secure sensitive metadata maps).

---

## 📂 System Architecture Breakdown

```text
WatchmanDevotional/
├── Controllers/         # Exposes clean REST API endpoints (Auth, Devotionals, Quizzes)
├── Data/                # Database contexts, migrations, and tracking instances
├── DTOs/                # Structural payload maps preventing over-posting bugs
├── Interfaces/          # Abstraction contracts decoupled from raw repository execution
├── Models/              # Core business domain schema definitions
└── Services/            # Business logic orchestration boundaries
