# TypeGuard

<div align="center">

[![CI/CD](https://github.com/DoubledDoge/TypeGuard/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/DoubledDoge/TypeGuard/actions/workflows/ci-cd.yml)
[![CodeQL](https://github.com/DoubledDoge/TypeGuard/actions/workflows/codeql.yml/badge.svg)](https://github.com/DoubledDoge/TypeGuard/actions/workflows/codeql.yml)
[![NuGet](https://img.shields.io/nuget/v/TypeGuard.Core.svg?label=NuGet)](https://www.nuget.org/packages/TypeGuard.Core)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0%20%7C%2010.0-512BD4)](https://dotnet.microsoft.com)
[![Downloads](https://img.shields.io/nuget/dt/TypeGuard.Core.svg)](https://www.nuget.org/packages/TypeGuard.Core)

A fluent, type-safe user input handler and validation library for .NET with platform-specific implementations for Console, WinForms, WPF, Avalonia, MAUI, Uno, Blazor, and ASP.NET.

</div>

---

## 📑 Table of Contents

- [🚀 Features](#-features)
- [📦 Installation](#-installation)
- [📚 Usage Guide](#-usage-guide)
- [🖥️ Platform Guide](#-platform-guide)
- [🏗️ Architecture](#-architecture)
- [🔌 Extending TypeGuard](#-extending-typeguard)
- [💻 Requirements](#-requirements)
- [📄 License](#-license)
- [🤝 Contributing](#-contributing)

---

## 🚀 Features

- **Type-Safe Validation** - Built-in input handlers + validators for all common C# types
- **Composable Rules** - Chain validation rules, similarly to LINQ
- **Async Support** - Full async/await support with cancellation tokens
- **Extensible** - Add custom handlers and rules with ease
- **Multi-Platform** - Dedicated implementations for Console, WinForms, WPF, Avalonia, MAUI, Uno, Blazor, and ASP.NET

---

## 📦 Installation

Install only the package for your platform. `TypeGuard.Core` is included automatically as a dependency.

### Console
```bash
dotnet add package TypeGuard.Console
```

### WinForms
```bash
dotnet add package TypeGuard.Winforms
```

### WPF
```bash
dotnet add package TypeGuard.Wpf
```

### Avalonia
```bash
dotnet add package TypeGuard.Avalonia
```

### MAUI
```bash
dotnet add package TypeGuard.Maui
```

### Uno
```bash
dotnet add package TypeGuard.Uno
```

### Blazor
```bash
dotnet add package TypeGuard.Blazor
```

### ASP.NET

ASP.NET applications use `TypeGuard.Core` directly via `Validator<T>` and `ValidationResult`, which are included automatically as a dependency of any platform package, or can be installed on their own:

```bash
dotnet add package TypeGuard.Core
```

### Manual Installation
```bash
dotnet add reference path/to/TypeGuard.Console/TypeGuard.Console.csproj  # or your platform project
```

---

## 📚 Usage Guide

### Simple Input Handling

```csharp
int count = await Guard.Int("How many items?").GetAsync();

string username = await Guard.String("Enter username").GetAsync();

DateTime date = await Guard.DateTime("Enter a date", "dd/MM/yyyy").GetAsync();
```

### Adding Rules

```csharp
int age = await Guard.Int("Enter your age")
    .WithRange(18, 120)
    .GetAsync();

string username = await Guard.String("Choose a username")
    .WithLengthRange(3, 20)
    .WithNoDigits()
    .GetAsync();
```

### Custom Validation Rules

```csharp
string password = await Guard.String("Create a password")
    .WithLengthRange(8, null)
    .WithRegex(@"[A-Z]", "Must contain at least one uppercase letter")
    .WithRegex(@"[a-z]", "Must contain at least one lowercase letter")
    .WithRegex(@"\d", "Must contain at least one digit")
    .WithCustomRule(
        p => p.Distinct().Count() >= 5,
        "Must contain at least 5 unique characters")
    .GetAsync();
```

### DateTime Handling

```csharp
DateTime appointment = await Guard.DateTime("Enter appointment date", "dd/MM/yyyy")
    .WithRange(DateTime.Today, DateTime.Today.AddMonths(6))
    .GetAsync();

DateTime birthday = await Guard.DateTime("Enter birthday")
    .WithCustomRule(
        d => DateTime.Today.Year - d.Year >= 18,
        "Must be 18 or older")
    .GetAsync();

DateTime meeting = await Guard.DateTime("Select meeting date", "yyyy-MM-dd")
    .WithWeekday()
    .GetAsync();
```

---

## 🖥️ Platform Guide

### Console

```csharp
using TypeGuard.Console;

int age = await Guard.Int("Enter your age")
    .WithRange(1, 120)
    .GetAsync();

string name = await Guard.String("Enter your name").GetAsync();
```

### WinForms

```csharp
using TypeGuard.Winforms;

private readonly Guard _guard;

public MyForm()
{
    InitializeComponent();
    _guard = new Guard(inputTextBox, promptLabel, errorLabel);
}

private async void submitButton_Click(object sender, EventArgs e)
{
    int age = await _guard.Int("Enter your age")
        .WithRange(1, 120)
        .GetAsync();
}
```

### WPF

```csharp
using TypeGuard.Wpf;

private readonly Guard _guard;

public MyWindow()
{
    InitializeComponent();
    _guard = new Guard(inputTextBox, promptBlock, errorBlock);
}

private async void SubmitButton_Click(object sender, RoutedEventArgs e)
{
    int age = await _guard.Int("Enter your age")
        .WithRange(1, 120)
        .GetAsync();
}
```

### Avalonia

```csharp
using TypeGuard.Avalonia;

private readonly Guard _guard;

public MyView()
{
    InitializeComponent();
    _guard = new Guard(InputTextBox, PromptBlock, ErrorBlock);
}

private async void SubmitButton_Click(object sender, RoutedEventArgs e)
{
    int age = await _guard.Int("Enter your age")
        .WithRange(1, 120)
        .GetAsync();
}
```

### MAUI

```csharp
using TypeGuard.Maui;

_guard = new Guard(InputEntry, PromptLabel, ErrorLabel, SubmitButton);

int age = await _guard.Int("Enter your age")
    .WithRange(1, 120)
    .GetAsync();
```

### Blazor

```html
@inject Guard Guard

<InputText @bind-Value="Guard.Input.Value" />
<p>@Guard.Output.PromptMessage</p>
<p style="color: red">@Guard.Output.ErrorMessage</p>

@code {
    protected override void OnInitialized()
    {
        Guard.Output.OnStateChanged = StateHasChanged;
    }

    private async Task SubmitAsync()
    {
        int age = await Guard.Int("Enter your age")
            .WithRange(1, 120)
            .GetAsync();
    }
}
```

### Uno

```csharp
using TypeGuard.Uno;

private readonly Guard _guard;

public MyPage()
{
    InitializeComponent();
    _guard = new Guard(InputTextBox, PromptBlock, ErrorBlock);
}

private async void SubmitButton_Click(object sender, RoutedEventArgs e)
{
    int age = await _guard.Int("Enter your age")
        .WithRange(1, 120)
        .GetAsync();
}
```

### ASP.NET

ASP.NET applications use `TypeGuard.Core` directly. Since the framework owns the I/O loop via model binding, there is no `Guard` entry point.

```csharp
using TypeGuard.Core;
using TypeGuard.Core.Rules;

ValidationResult result = new Validator<int>()
    .AddRule(new RangeRule<int>(1, 120))
    .Validate(model.Age);

if (!result.IsValid)
    return BadRequest(result.ErrorMessage);
```

---

## 🏗️ Architecture

```
TypeGuard.Core              → Platform-agnostic logic
├── Abstractions            → Interfaces
├── Handlers                → Type-specific handlers
├── Rules                   → Validation rules
└── Builders                → Fluent API builders

TypeGuard.Console           → Console implementation
├── ConsoleInput            → Reads from Console.ReadLine()
├── ConsoleOutput           → Writes prompts and errors
└── Guard                   → Main entry point

TypeGuard.Winforms          → WinForms implementation (TextBox, TextBlock)
├── WinformsInput           → Reads from Control.Text
├── WinformsOutput          → Writes prompts and errors
└── Guard                   → Main entry point

TypeGuard.Wpf               → WPF implementation (TextBox, TextBlock)
├── WpfInput                → Reads from Textbox.Text
├── WpfOutput               → Writes prompts and errors
└── Guard                   → Main entry point

TypeGuard.Avalonia          → Avalonia implementation (TextBox, TextBlock)
├── AvaloniaInput           → Reads from Textbox.Text
├── AvaloniaOutput          → Writes prompts and errors
└── Guard                   → Main entry point

TypeGuard.Maui              → MAUI implementation (Entry, Label, Button)
├── MauiInput               → Reads from Entry.Text
├── MauiOutput              → Writes prompts and errors
└── Guard                   → Main entry point

TypeGuard.Blazor            → Blazor implementation (InputText, DI-injected Guard)
├── BlazorInput             → Reads from InputText.Value
├── BlazorOutput            → Writes prompts and errors
└── Guard                   → Main entry point

TypeGuard.Uno               → Uno implementation (TextBox, TextBlock)
├── UnoInput                → Reads from TextBox.Text
├── UnoOutput               → Writes prompts and errors
└── Guard                   → Main entry point

TypeGuard.Core (ASP.NET)    
├── Validator<T>            → Validates already-bound values against configured rules
└── ValidationResult        → Carries success or failure state with an error message
```

---

## 🔌 Extending TypeGuard

### Creating Custom Rules

```csharp
using TypeGuard.Core.Interfaces;

public class EmailRule : IValidatorRule<string>
{
    public bool IsValid(string value) =>
        value.Contains('@') && value.Contains('.');

    public string ErrorMessage => "Must be a valid email address";
}
 
string email = await Guard.String("Enter email")
    .WithCustomRule(new EmailRule())
    .GetAsync();

// Note: Email Rule already exists but this is just an example of how to create your own custom rule.
```

---

## 💻 Requirements

- .NET 9.0 or .NET 10.0
- Windows, macOS, or Linux (certain packages may have OS restrictions)

---

## 📄 License

See [LICENCE](LICENSE) for details. (MIT Licence)

---

## 🤝 Contributing

Contributions are welcome! You can:

- Report bugs or request features via [Issues](https://github.com/DoubledDoge/TypeGuard/issues)
- Submit pull requests
- Suggest improvements to the API
- Adding new type handlers/builders/rules
