# TypeGuard

## 📑 Table of Contents

- [🚀 Features](#-features)
- [📦 Installation](#-installation)
- [🎯 Quick Start](#-quick-start)
- [📚 Usage Guide](#-usage-guide)
- [🔌 Extending TypeGuard](#-extending-typeguard)
- [💻 Requirements](#-requirements)s
- [📄 License](#-license)
- [🤝 Contributing](#-contributing)

## 🚀 Features

-  **Type-Safe Validation** — Built-in validators for `int`, `string`, `DateTime`, and more coming soon
-  **Automatic Retry Logic** — Invalid input automatically re-prompts the user
-  **Composable Rules** — Chain validation rules (Similar to LINQ)
-  **Async Support** — Full async/await support with cancellation tokens
-  **Extensible** — Easy to add as many custom validators and rules as you want!
-  **Multi-Platform Ready** — Separate implementations for Console, WinForms, WPF (coming soon)
-  **Colored Console Output** — Integration with ConsolePrism for beautiful error messages

---

## 📦 Installation

### NuGet Package

For console applications:
```bash
dotnet add package TypeGuard.Console
```

(More to come!)

The core library (`TypeGuard.Core`) is included automatically as a dependency.

### Manual Installation
1. Clone the repository
2. Add references to your project:
```bash
dotnet add reference path/to/TypeGuard.Core/TypeGuard.Core.csproj
dotnet add reference path/to/TypeGuard.Console/TypeGuard.Console.csproj
```

## 🎯 Quick Start

```csharp
using TypeGuard.Console;

// Simple integer input
int age = await TypeGuard.GetIntAsync("Enter your age");

// String input with validation
string name = await TypeGuard
    .ForString("Enter your name")
    .WithNoDigits()
    .WithLengthRange(2, 50)
    .GetAsync();

// DateTime with specific format
DateTime birthday = await TypeGuard
    .ForDateTime("Enter your birthday", "dd/MM/yyyy")
    .WithRange(new DateTime(1900, 1, 1), DateTime.Today)
    .GetAsync();

// Integer with range validation
int score = await TypeGuard
    .ForInt("Enter score")
    .WithRange(0, 100)
    .GetAsync();
```

## 📚 Usage Guide

### Simple Validation

```csharp
// Integer input
int count = await TypeGuard.GetIntAsync("How many items?");
int countSync = TypeGuard.GetInt("How many items?"); // Synchronous version

// String input
string username = await TypeGuard.GetStringAsync("Enter username");
string usernameSync = TypeGuard.GetString("Enter username");
```

### Validation with Rules

```csharp
// Age validation: must be between 18 and 120
int age = await TypeGuard
    .ForInt("Enter your age")
    .WithRange(18, 120)
    .GetAsync();

// Username validation: 3-20 characters, no digits
string username = await TypeGuard
    .ForString("Choose a username")
    .WithLengthRange(3, 20)
    .WithNoDigits()
    .GetAsync();

// Email validation using regex
string email = await TypeGuard
    .ForString("Enter your email")
    .WithRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", "Invalid email format")
    .GetAsync();
```

### Custom Validation Rules

```csharp
// Even numbers only (Might be dedicated rule in the future)
int evenNumber = await TypeGuard
    .ForInt("Enter an even number")
    .WithCustomRule(x => x % 2 == 0, "Must be an even number")
    .GetAsync();

// Password strength
string password = await TypeGuard
    .ForString("Create password")
    .WithLengthRange(8, null)
    .WithRegex(@"[A-Z]", "Must contain at least one uppercase letter")
    .WithRegex(@"[a-z]", "Must contain at least one lowercase letter")
    .WithRegex(@"\d", "Must contain at least one digit")
    .WithCustomRule(
        p => p.Distinct().Count() >= 5,
        "Must contain at least 5 unique characters")
    .GetAsync();
```

### DateTime Validation

(No time support yet)

```csharp
// Strict format validation
DateTime appointmentDate = await TypeGuard
    .ForDateTime("Enter appointment date", "dd/MM/yyyy")
    .WithRange(DateTime.Today, DateTime.Today.AddMonths(6))
    .GetAsync();

// Flexible format (accepts various date formats)
DateTime birthday = await TypeGuard
    .ForDateTime("Enter birthday", format: null) // null = flexible parsing
    .WithCustomRule(
        d => DateTime.Today.Year - d.Year >= 18,
        "Must be 18 or older")
    .GetAsync();

// Business days only
DateTime meetingDate = await TypeGuard
    .ForDateTime("Select meeting date", "yyyy-MM-dd")
    .WithCustomRule(
        d => d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday,
        "Must be a weekday")
    .GetAsync();
```

## 🏗️ Architecture

```
TypeGuard.Core          → Platform-agnostic validation logic
├── Abstractions        → Interfaces (IValidator, IInputProvider, IOutputProvider)
├── Validators          → Type-specific validators (IntValidator, StringValidator, etc.)
├── Rules               → Validation rules (RangeRule, RegexRule, CustomRule, etc.)
└── Builders            → Fluent API builders

TypeGuard.Console       → Console-specific implementation
├── ConsoleInput   → Reads from Console.ReadLine()
├── ConsoleOutput  → Writes errors/prompts with ConsolePrism
└── TypeGuard              → Main entry point for developers
```

## 🔌 Extending TypeGuard

### Creating Custom Validators

```csharp
public class DecimalValidator : ValidatorBase<decimal>
{
    public DecimalValidator(
        IInputProvider inputProvider,
        IOutputProvider outputProvider,
        string prompt)
        : base(inputProvider, outputProvider, prompt)
    {
    }

    protected override bool TryParse(string? input, out decimal value, out string? errorMessage)
    {
        if (decimal.TryParse(input, out value))
        {
            errorMessage = null;
            return true;
        }

        errorMessage = "Please enter a valid decimal number";
        value = default;
        return false;
    }
}
```

### Creating Custom Rules

```csharp
public class EmailRule : IValidationRule<string>
{
    public bool IsValid(string value)
    {
        return value.Contains('@') && value.Contains('.');
    }

    public string ErrorMessage => "Must be a valid email address";
}

// Usage
string email = await TypeGuard
    .ForString("Enter email")
    .WithCustomRule(new EmailRule())
    .GetAsync();
```

## 💻 Requirements

- .NET 9.0 or higher
- **TypeGuard.Console** requires ConsolePrism for colored output (Dependency)
- Works on Windows, macOS, and Linux

## 📄 License
s
MIT License — see [LICENSE](LICENSE) file for details

## 🤝 Contributing

Contributions welcome! Feel free to:
- Report bugs or request features via Issues
- Submit pull requests
- Suggest improvements to the API

## 🗺️ Roadmap

Current version: **v0.1.0** (Pre-release)

**Planned Features:**
-  WinForms implementation
-  WPF implementation
-  TimeSpan validator
-  Decimal/Float validators
-  Collection validators
-  Comprehensive unit tests?
-  Full XML documentation
-  Interactive demo application?

---

Last Updated: 2025
