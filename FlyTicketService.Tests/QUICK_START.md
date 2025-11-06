# Przewodnik szybkiego startu - Testy

## Przygotowanie środowiska

### 1. Wymagania
- .NET 8.0 SDK
- Visual Studio Code lub Visual Studio 2022
- Terminal (bash/zsh/PowerShell)

### 2. Sklonowanie repozytorium
```bash
git clone https://github.com/Pawel0071/FlyTicketService.git
cd FlyTicketService
```

## Uruchomienie testów

### Metoda 1: Wszystkie testy
```bash
dotnet test
```

### Metoda 2: Z Visual Studio Code
1. Otwórz folder projektu w VS Code
2. Zainstaluj rozszerzenie "C# Dev Kit"
3. W zakładce "Testing" kliknij "Run All Tests"

### Metoda 3: Z Visual Studio 2022
1. Otwórz `FLyTicketService.sln`
2. Build Solution (Ctrl+Shift+B)
3. Test > Run All Tests (Ctrl+R, A)

## Uruchomienie konkretnych testów

### Testy dla konkretnej klasy
```bash
# Tylko testy TicketService
dotnet test --filter "FullyQualifiedName~TicketServiceTests"

# Tylko testy kontrolerów
dotnet test --filter "FullyQualifiedName~Controllers"
```

### Pojedynczy test
```bash
dotnet test --filter "FullyQualifiedName~GetTicketAsync_WhenTicketExists_ReturnsTicket"
```

### Testy według kategorii
```bash
# Wszystkie testy serwisów
dotnet test --filter "FullyQualifiedName~Services"

# Wszystkie testy middleware
dotnet test --filter "FullyQualifiedName~Middleware"
```

## Pokrycie kodu

### Generowanie raportu pokrycia
```bash
# Instalacja narzędzia
dotnet tool install -g dotnet-reportgenerator-globaltool

# Uruchomienie testów z pokryciem
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura

# Generowanie raportu HTML
reportgenerator -reports:FlyTicketService.Tests/coverage.cobertura.xml -targetdir:coverage-report -reporttypes:Html

# Otwarcie raportu
open coverage-report/index.html  # macOS
start coverage-report/index.html  # Windows
xdg-open coverage-report/index.html  # Linux
```

## Przykładowy output

```
Test run for FlyTicketService.Tests.dll (.NET 8.0)
Microsoft (R) Test Execution Command Line Tool Version 17.8.0

Starting test execution, please wait...
A total of 1 test files matched the specified pattern.

Passed!  - Failed:     0, Passed:    75, Skipped:     0, Total:    75, Duration: 2s
```

## Debugowanie testów

### Visual Studio Code
1. Ustaw breakpoint w kodzie testu
2. Kliknij prawym na test i wybierz "Debug Test"

### Visual Studio 2022
1. Ustaw breakpoint w kodzie testu
2. Kliknij prawym na test i wybierz "Debug Test"

### Z terminala
```bash
# Uruchom z debuggerem
dotnet test --logger "console;verbosity=detailed"
```

## Watch mode - automatyczne uruchamianie

```bash
# Uruchom testy w trybie watch (automatyczne uruchamianie po zmianach)
dotnet watch test
```

## Continuous Integration

### GitHub Actions przykład
```yaml
name: Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 8.0.x
      - name: Restore dependencies
        run: dotnet restore
      - name: Build
        run: dotnet build --no-restore
      - name: Test
        run: dotnet test --no-build --verbosity normal
```

## Rozwiązywanie problemów

### Problem: Błędy kompilacji
```bash
# Wyczyść i odbuduj
dotnet clean
dotnet restore
dotnet build
```

### Problem: Testy nie są wykrywane
```bash
# Sprawdź czy projekt testowy ma odpowiednie zależności
dotnet list package --project FlyTicketService.Tests/FlyTicketService.Tests.csproj
```

### Problem: Błędy z IntelliSense
1. Zamknij VS Code
2. Usuń folder `.vs` i `obj/bin` foldery
3. Uruchom `dotnet restore`
4. Otwórz ponownie VS Code

## Przydatne komendy

```bash
# Lista wszystkich testów
dotnet test --list-tests

# Uruchom tylko nieudane testy
dotnet test --filter "TestCategory!=Success"

# Testy z timeoutem
dotnet test --blame-hang-timeout 60s

# Równoległe uruchamianie
dotnet test --parallel

# Bez buildowania
dotnet test --no-build
```

## Best Practices

1. ✅ Uruchamiaj testy przed commitem
2. ✅ Używaj watch mode podczas developmentu
3. ✅ Sprawdzaj pokrycie kodu regularnie
4. ✅ Debuguj testy gdy nie przechodzą
5. ✅ Uruchamiaj wszystkie testy przed pushem do repo

## Dodatkowe zasoby

- [xUnit Documentation](https://xunit.net/)
- [Moq Documentation](https://github.com/moq/moq4)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [.NET Testing Documentation](https://docs.microsoft.com/en-us/dotnet/core/testing/)

---

**Pytania?** Otwórz issue na GitHubie!
