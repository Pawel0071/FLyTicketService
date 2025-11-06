# Test Coverage Summary

## Podsumowanie testów jednostkowych

Projekt FlyTicketService zawiera kompletny zestaw testów jednostkowych pokrywających wszystkie główne komponenty systemu.

## Struktura testów

```
FlyTicketService.Tests/
├── Controllers/
│   └── TicketControllerTests.cs          (12 testów)
├── Services/
│   ├── TicketServiceTests.cs             (4 testy)
│   ├── FlightPriceServiceTests.cs        (5 testów)
│   ├── GroupStrategyTests.cs             (4 testy)
│   ├── GroupStrategyFactoryTests.cs      (5 testów)
│   ├── TenantServiceTests.cs             (7 testów)
│   ├── FlightScheduleServiceTests.cs     (8 testów)
│   └── DiscountServiceTests.cs           (9 testów)
├── Middleware/
│   └── ExceptionHandlingMiddlewareTests.cs (5 testów)
└── Shared/
    └── OperationResultTests.cs           (16 testów)
```

## Statystyki testów

- **Całkowita liczba testów**: 75
- **Pokryte komponenty**: 10
- **Kategorie testów**:
  - Kontrolery: 12 testów
  - Serwisy biznesowe: 42 testy
  - Middleware: 5 testów
  - Shared/Utilities: 16 testów

## Testy według komponentów

### 1. TicketControllerTests (12 testów)
Testy API dla operacji na biletach:
- ✅ Rezerwacja biletu (sukces i błędy)
- ✅ Sprzedaż biletu (z rabatami)
- ✅ Sprzedaż zarezerwowanego biletu
- ✅ Anulowanie biletu
- ✅ Pobieranie biletu
- ✅ Lista biletów z filtrami
- ✅ Dostępne rabaty dla biletu
- ✅ Lista wszystkich rabatów

### 2. TicketServiceTests (4 testy)
Testy logiki biznesowej biletów:
- ✅ Pobieranie istniejącego biletu
- ✅ Obsługa nieistniejącego biletu
- ✅ Anulowanie zarezerwowanego biletu
- ✅ Próba anulowania sprzedanego biletu (conflict)

### 3. FlightPriceServiceTests (5 testów)
Testy systemu cenowego i rabatów:
- ✅ Pobieranie wszystkich rabatów
- ✅ Aplikowanie pojedynczych rabatów
- ✅ Aplikowanie wielu rabatów
- ✅ Sprawdzanie warunków rabatowych
- ✅ Filtrowanie rabatów według warunków

### 4. GroupStrategyTests (4 testy)
Testy wzorca Strategy dla grup klientów:
- ✅ GroupA - pełne rabaty
- ✅ GroupA - zwracanie wszystkich rabatów
- ✅ GroupB - cena bez rabatu
- ✅ GroupB - pusta lista rabatów

### 5. GroupStrategyFactoryTests (5 testów)
Testy fabryki strategii:
- ✅ Zwracanie strategii dla GroupA
- ✅ Zwracanie strategii dla GroupB
- ✅ Obsługa nieprawidłowej grupy
- ✅ Walidacja konstruktora (pusta lista)
- ✅ Walidacja konstruktora (null)

### 6. TenantServiceTests (7 testów)
Testy zarządzania klientami:
- ✅ Pobieranie istniejącego klienta
- ✅ Obsługa nieistniejącego klienta
- ✅ Tworzenie nowego klienta
- ✅ Błąd podczas tworzenia
- ✅ Aktualizacja klienta
- ✅ Usuwanie klienta
- ✅ Lista wszystkich klientów

### 7. FlightScheduleServiceTests (8 testów)
Testy harmonogramów lotów:
- ✅ Pobieranie lotu po numerze
- ✅ Obsługa nieistniejącego lotu
- ✅ Filtrowanie lotów (origin, destination, date)
- ✅ Lista wszystkich lotów
- ✅ Tworzenie nowego lotu
- ✅ Aktualizacja lotu
- ✅ Usuwanie lotu
- ✅ Pobieranie dostępnych miejsc

### 8. DiscountServiceTests (9 testów)
Testy systemu rabatowego:
- ✅ Pobieranie rabatu po ID
- ✅ Obsługa nieistniejącego rabatu
- ✅ Lista wszystkich rabatów
- ✅ Tworzenie nowego rabatu
- ✅ Błąd podczas tworzenia
- ✅ Aktualizacja rabatu
- ✅ Usuwanie rabatu
- ✅ Filtrowanie według kategorii
- ✅ Walidacja warunków rabatowych

### 9. ExceptionHandlingMiddlewareTests (5 testów)
Testy middleware obsługi błędów:
- ✅ Wywołanie następnego delegata (brak błędu)
- ✅ Zwracanie HTTP 500 przy wyjątku
- ✅ Logowanie błędów
- ✅ Zwracanie komunikatu błędu
- ✅ Obsługa różnych typów wyjątków

### 10. OperationResultTests (16 testów)
Testy wzorca Result:
- ✅ Konstruktor i ustawianie właściwości
- ✅ IsSuccessStatusCode dla różnych statusów
- ✅ GetResult dla różnych statusów HTTP
- ✅ Konwersja statusu na kod HTTP
- ✅ Obsługa null message/data
- ✅ Testy parametryzowane (Theory)

## Uruchomienie testów

### Wszystkie testy
```bash
dotnet test
```

### Z szczegółowym outputem
```bash
dotnet test --verbosity detailed
```

### Z pokryciem kodu
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Konkretny plik testowy
```bash
dotnet test --filter "FullyQualifiedName~TicketServiceTests"
```

### Konkretny test
```bash
dotnet test --filter "FullyQualifiedName~GetTicketAsync_WhenTicketExists_ReturnsTicket"
```

## Wzorce testowe

### Arrange-Act-Assert (AAA)
Wszystkie testy używają wzorca AAA dla czytelności:
```csharp
[Fact]
public async Task GetTicketAsync_WhenTicketExists_ReturnsTicket()
{
    // Arrange - przygotowanie danych testowych
    var ticketNumber = "TEST123";
    var ticket = CreateTestTicket(ticketNumber);

    // Act - wykonanie testowanej akcji
    var result = await _sut.GetTicketAsync(ticketNumber);

    // Assert - weryfikacja rezultatu
    result.Should().NotBeNull();
    result.Status.Should().Be(OperationStatus.Ok);
}
```

### Mockowanie zależności
Używamy Moq do mockowania zależności:
```csharp
_ticketRepositoryMock
    .Setup(x => x.GetByIdAsync(ticketId))
    .ReturnsAsync(ticket);
```

### FluentAssertions
Używamy FluentAssertions dla czytelnych asercji:
```csharp
result.Should().NotBeNull();
result.Status.Should().Be(OperationStatus.Ok);
result.Data.Should().HaveCount(2);
```

## Pokrycie kodu

Docelowe pokrycie kodu według warstw:
- **Controllers**: 90%+
- **Services**: 85%+
- **Middleware**: 80%+
- **Shared/Utilities**: 95%+

## Best Practices

1. ✅ **Izolacja testów** - każdy test jest niezależny
2. ✅ **Mockowanie zależności** - testy jednostkowe bez zależności zewnętrznych
3. ✅ **Czytelne nazwy** - `MethodName_Scenario_ExpectedBehavior`
4. ✅ **Wzorzec AAA** - Arrange-Act-Assert
5. ✅ **FluentAssertions** - czytelne asercje
6. ✅ **Theory/InlineData** - testy parametryzowane
7. ✅ **Test helpers** - metody pomocnicze dla tworzenia danych testowych

## Dalszy rozwój testów

### Testy integracyjne
Można rozważyć dodanie:
- Testy z WebApplicationFactory
- Testy z prawdziwą bazą danych (testcontainers)
- Testy end-to-end całych scenariuszy

### Testy wydajnościowe
- Testy obciążeniowe dla API
- Testy wydajności zapytań do bazy danych

### Testy kontraktowe
- Pact dla weryfikacji API
- Swagger/OpenAPI validation

## Continuous Integration

Konfiguracja CI/CD powinna zawierać:
```yaml
- name: Run tests
  run: dotnet test --no-build --verbosity normal /p:CollectCoverage=true

- name: Upload coverage
  uses: codecov/codecov-action@v3
```

## Raporty

Generowanie raportów pokrycia:
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutput=./coverage/ /p:CoverletOutputFormat=cobertura
reportgenerator -reports:coverage/coverage.cobertura.xml -targetdir:coverage/report -reporttypes:Html
```

---

**Uwaga**: Testy są gotowe do uruchomienia po naprawieniu problemu z kompilacją głównego projektu (zduplikowane atrybuty assembly).
