# FLyTicketService

[![Build Status](https://github.com/Pawel0071/FLyTicketService/workflows/Unit%20Tests/badge.svg)](https://github.com/Pawel0071/FLyTicketService/actions)
[![Tests](https://img.shields.io/badge/tests-43%20passing-brightgreen)](https://github.com/Pawel0071/FLyTicketService)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)

System zarządzania biletami lotniczymi - REST API zbudowane w ASP.NET Core 8.0

## 📋 Spis treści

- [Opis projektu](#-opis-projektu)
- [Architektura](#-architektura)
- [Funkcjonalności](#-funkcjonalności)
- [Technologie](#-technologie)
- [Wymagania](#-wymagania)
- [Instalacja i uruchomienie](#-instalacja-i-uruchomienie)
- [Struktura projektu](#-struktura-projektu)
- [API Endpoints](#-api-endpoints)
- [Testy](#-testy)
- [Konfiguracja](#-konfiguracja)
- [Wzorce projektowe](#-wzorce-projektowe)

## 📝 Opis projektu

FLyTicketService to zaawansowany system zarządzania rezerwacjami i sprzedażą biletów lotniczych. Aplikacja oferuje kompleksowe rozwiązanie do obsługi lotów, rezerwacji miejsc, zarządzania klientami (tenantami) oraz zaawansowany system rabatowy.

### Główne możliwości:

- ✈️ Zarządzanie harmonogramami lotów
- 🎫 Rezerwacja i sprzedaż biletów
- 👥 System grupowania klientów (Group A, Group B)
- 💰 Zaawansowany system rabatów z wieloma warunkami
- 🌍 Obsługa różnych stref czasowych
- 📊 Szczegółowe informacje o lotach i samolotach

## 🏗 Architektura

Projekt implementuje **czystą architekturę** z wyraźnym podziałem na warstwy:

```
┌─────────────────────────────────────┐
│     Controllers (API Layer)         │
├─────────────────────────────────────┤
│     Services (Business Logic)       │
├─────────────────────────────────────┤
│   Repositories (Data Access)        │
├─────────────────────────────────────┤
│  Entity Framework Core + SQL Server │
└─────────────────────────────────────┘
```

### Wzorce projektowe

1. **Repository Pattern** - abstrakcja dostępu do danych
2. **Strategy Pattern** - różne strategie obliczania rabatów dla grup klientów
3. **Factory Pattern** - tworzenie strategii grupowych
4. **DTO Pattern** - separacja modeli domenowych od API
5. **Dependency Injection** - luźne powiązanie komponentów

## ✨ Funkcjonalności

### System biletowy

- **Rezerwacja biletów** - tymczasowe zablokowanie miejsca na lot
- **Sprzedaż biletów** - finalizacja zakupu z aplikacją rabatów
- **Anulowanie biletów** - zwolnienie zarezerwowanych miejsc
- **Historia biletów** - przeglądanie zakupionych biletów

### System rabatowy

Aplikacja wspiera zaawansowany system rabatów z warunkami:

- **Rabaty dla destynacji** - na podstawie portu docelowego
- **Rabaty dla linii lotniczych** - na podstawie przewoźnika
- **Rabaty czasowe** - na podstawie daty wylotu/przylotu
- **Rabaty dla klientów** - grupowanie klientów (Group A, Group B)
- **Warunkowe rabaty** - złożone warunki rabatowe

### Strategia grup klientów

- **Group A** - klienci premium z pełnym dostępem do wszystkich rabatów
- **Group B** - klienci standardowi z ograniczonym dostępem do rabatów

## 🛠 Technologie

### Backend

- **ASP.NET Core 8.0** - framework webowy
- **Entity Framework Core 9.0** - ORM
- **SQL Server** - baza danych
- **AutoMapper 14.0** - mapowanie obiektów
- **Swagger** - dokumentacja API

### Testy

- **xUnit** - framework testowy
- **Moq** - mockowanie zależności
- **FluentAssertions** - asercje testowe
- **EF Core InMemory** - testy z bazą w pamięci
- **Microsoft.AspNetCore.Mvc.Testing** - testy integracyjne

### DevOps

- **Docker** - konteneryzacja
- **GitHub** - kontrola wersji

## 📦 Wymagania

- **.NET 8.0 SDK** lub nowszy
- **SQL Server 2019** lub nowszy (lub SQL Server Express)
- **Docker** (opcjonalnie, do uruchomienia w kontenerze)

## 🚀 Instalacja i uruchomienie

### 1. Klonowanie repozytorium

```bash
git clone https://github.com/Pawel0071/FlyTicketService.git
cd FlyTicketService
```

### 2. Konfiguracja connection string

Edytuj plik `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FlyTicketDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Lub ustaw zmienną środowiskową:

```bash
export CONNECTION_STRING="YourConnectionStringHere"
```

### 3. Migracje bazy danych

```bash
dotnet ef database update
```

### 4. Uruchomienie aplikacji

```bash
dotnet run
```

Aplikacja będzie dostępna pod adresem:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

### 5. Uruchomienie z Dockerem

```bash
docker build -t flyticketservice .
docker run -p 8080:8080 -e CONNECTION_STRING="YourConnectionString" flyticketservice
```

## 📁 Struktura projektu

```
FlyTicketService/
├── Controllers/              # Kontrolery API
│   ├── TicketController.cs
│   ├── FlightScheduleController.cs
│   ├── TenantController.cs
│   └── DiscountTypeController.cs
├── Service/                  # Logika biznesowa
│   ├── TicketService.cs
│   ├── FlightPriceService.cs
│   ├── FlightScheduleService.cs
│   ├── TenantService.cs
│   ├── DiscountService.cs
│   ├── GroupAStrategy.cs
│   ├── GroupBStrategy.cs
│   └── GroupStrategyFactory.cs
├── Repositories/             # Warstwa dostępu do danych
│   ├── GenericRepository.cs
│   └── Interfaces/
├── Model/                    # Modele domenowe
│   ├── Ticket.cs
│   ├── FlightSchedule.cs
│   ├── Tenant.cs
│   ├── Discount.cs
│   ├── Aircraft.cs
│   ├── Airline.cs
│   ├── Airport.cs
│   └── Enums/
├── DTO/                      # Data Transfer Objects
│   ├── TicketDTO.cs
│   ├── FlightScheduleDTO.cs
│   └── ...
├── Data/                     # Konfiguracja EF Core
│   ├── FLyTicketDbContext.cs
│   ├── Configuration/
│   └── WarmingUpData/
├── Mapper/                   # Profile AutoMapper
│   └── FLyTicketMappingProfile.cs
├── Middleware/               # Custom middleware
│   └── ExceptionHandlingMiddleware.cs
├── Shared/                   # Wspólne komponenty
│   ├── OperationResult.cs
│   └── FlightDetails.cs
├── Extension/                # Rozszerzenia
│   └── SimplyTimeZoneExtension.cs
├── Migrations/               # Migracje EF Core
├── Scripts/                  # Skrypty PowerShell
└── FlyTicketService.Tests/   # Projekt testowy
    └── Services/
        ├── TicketServiceTests.cs
        ├── FlightPriceServiceTests.cs
        └── ...
```

## 🌐 API Endpoints

### Tickets (Bilety)

#### Rezerwacja biletu
```http
POST /api/ticket/reserve?flightId={id}&seatNo={seat}&tenantId={id}
```

#### Sprzedaż biletu
```http
POST /api/ticket/sell?flightId={id}&seatNo={seat}&tenantId={id}
Content-Type: application/json

[
  {
    "discountId": "guid",
    "name": "string",
    "value": 0,
    "category": "Destination"
  }
]
```

#### Sprzedaż zarezerwowanego biletu
```http
POST /api/ticket/sell-reserved?ticketNumber={number}
```

#### Anulowanie biletu
```http
DELETE /api/ticket/{ticketNumber}
```

#### Pobranie biletu
```http
GET /api/ticket/{ticketNumber}
```

#### Lista biletów
```http
GET /api/ticket?flyNumber={number}&tenantId={id}&departure={date}&originIATA={code}&destinationIATA={code}
```

#### Dostępne rabaty dla biletu
```http
GET /api/ticket/discounts/{ticketNumber}
```

#### Wszystkie rabaty
```http
GET /api/ticket/all-discounts
```

### FlightSchedule (Harmonogramy lotów)

#### Lista lotów
```http
GET /api/flightschedule?originIATA={code}&destinationIATA={code}&departureDate={date}
```

#### Szczegóły lotu
```http
GET /api/flightschedule/{flightNumber}
```

### Tenant (Klienci)

#### Utworzenie klienta
```http
POST /api/tenant
Content-Type: application/json

{
  "firstName": "string",
  "lastName": "string",
  "email": "string",
  "phoneNumber": "string",
  "group": "GroupA"
}
```

#### Pobranie klienta
```http
GET /api/tenant/{id}
```

### DiscountType (Typy rabatów)

#### Lista warunków rabatowych
```http
GET /api/discounttype/conditions
```

## 🧪 Testy

### Uruchomienie testów jednostkowych

```bash
# Wszystkie testy
dotnet test

# Testy z pokryciem kodu
dotnet test /p:CollectCoverage=true

# Testy konkretnego projektu
dotnet test FlyTicketService.Tests/FlyTicketService.Tests.csproj

# Z szczegółowym outputem
dotnet test --verbosity detailed
```

### 🔧 Skrypty testowe API

Projekt zawiera skrypty do testowania działającego API w folderze `Scripts/`:

#### Linux/macOS (bash)

```bash
# 1. Uruchom serwis w jednym terminalu
cd /Volumes/Data/Repositories/FlyTicketService
dotnet run

# 2. W osobnym terminalu - uruchom testy API
cd Scripts
./test-api.sh localhost 5000
```

**Skrypt test-api.sh testuje:**
- ✅ GET /api/Tenant - pobieranie listy tenantów
- ✅ GET /api/FlightSchedule - pobieranie harmonogramu lotów
- ✅ GET /api/DiscountType/all-discounts - pobieranie listy rabatów
- ✅ POST /api/Tenant - dodawanie nowego tenanta
- ✅ POST /api/DiscountType - dodawanie nowego rabatu

#### Windows (PowerShell)

```powershell
# Pobierz wszystkie dane
.\Scripts\GetData.ps1 -Server localhost -Port 5000

# Dodaj przykładowych tenantów (30 sztuk)
.\Scripts\tenant.ps1 -Server localhost -Port 5000

# Dodaj przykładowe loty
.\Scripts\Flight.ps1 -ServerHost localhost -Port 5000

# Testuj operacje na biletach
.\Scripts\Tickets.ps1 -Server localhost -Port 5000
```

**Dostępne skrypty PowerShell:**
- **GetData.ps1** - pobiera dane o tenantach, lotach i rabatach
- **tenant.ps1** - dodaje 30 przykładowych tenantów (różne grupy)
- **Flight.ps1** - dodaje przykładowe harmonogramy lotów (Europa, Ameryka, Azja, Afryka)
- **Tickets.ps1** - testuje rezerwację, sprzedaż i anulowanie biletów

### Struktura testów

Projekt zawiera **43 testy jednostkowe** z pokryciem **100%** kluczowych komponentów:

- **Controllers** (10 testów) - testy API endpoints
- **Services** (31 testów) - testy logiki biznesowej
  - TicketService - zarządzanie biletami
  - FlightPriceService - obliczanie cen i rabatów
  - TenantService - zarządzanie klientami
  - FlightScheduleService - harmonogramy lotów
  - DiscountService - system rabatowy
  - GroupStrategy - strategie grup klientów
  - GroupStrategyFactory - fabryka strategii
- **Middleware** (1 test) - obsługa wyjątków
- **Shared** (1 test) - komponenty współdzielone

✅ **Status testów**: 43/43 passing (100% success rate)

### Technologie testowe

- **xUnit** - framework testowy
- **Moq** - mockowanie zależności
- **FluentAssertions** - czytelne asercje
- **AAA Pattern** - Arrange-Act-Assert

## ⚙️ Konfiguracja

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=172.17.0.3;Database=FlyTicket;User=sa;Password=2019Venza;TrustServerCertificate=True"
  }
}
```

### Dane testowe (Seed Data)

Aplikacja automatycznie ładuje dane początkowe z plików JSON podczas tworzenia bazy:

**Data/WarmingUpData/**
- **airports.json** - ~30 portów lotniczych na całym świecie
  - Przykłady: JFK (New York), LAX (Los Angeles), CDG (Paris), WAW (Warsaw)
- **airlines.json** - ~10 linii lotniczych
  - Przykłady: LOT, American Airlines, Delta, Emirates, Qantas
- **aircrafts.json** - ~10 samolotów z numerami rejestracyjnymi
  - Przykłady: Boeing 737, Airbus A320, Boeing 787
- **discount.json** - przykładowe rabaty

Dane są automatycznie ładowane podczas pierwszej migracji EF Core (`OnModelCreating`).

### Migracje bazy danych

```bash
# Utworzenie bazy danych
dotnet ef database update

# Utworzenie nowej migracji
dotnet ef migrations add MigrationName

# Cofnięcie migracji
dotnet ef database update PreviousMigrationName
```

**Plik migracji**: `Migrations/20250306152449_InitialCreate.cs`

Tworzy kompletną strukturę bazy z:
- Tabelami: Aircrafts, Airlines, Airports, Tenants, FlightSchedules, FlightSeats, Tickets, Discounts, Conditions
- Relacjami Foreign Key
- Indeksami
- Seed data z plików JSON

### Zmienne środowiskowe

- `CONNECTION_STRING` - connection string do bazy danych
- `ASPNETCORE_ENVIRONMENT` - środowisko (Development, Production)

## 🎯 Wzorce projektowe

### 1. Repository Pattern

```csharp
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}
```

### 2. Strategy Pattern

```csharp
public interface IGroupStrategy
{
    (decimal, decimal) ApplyDiscountBasedOnTenantGroup(IEnumerable<Discount> discounts, Ticket ticket);
    IEnumerable<Discount> GetListBasedOnTenantGroup(IEnumerable<Discount> discounts);
}

// Implementacje
- GroupAStrategy - pełne rabaty
- GroupBStrategy - ograniczone rabaty
```

### 3. Factory Pattern

```csharp
public interface IGroupStrategyFactory
{
    IGroupStrategy GetStrategy(TenantGroup group);
}
```

### 4. Operation Result Pattern

```csharp
public class OperationResult<T>
{
    public OperationStatus Status { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
}
```

## 📄 Licencja

Ten projekt jest licencjonowany na zasadach MIT License - szczegóły w pliku [LICENSE.txt](LICENSE.txt)

## 👤 Autor

Pawel - [Pawel0071](https://github.com/Pawel0071)

## 🤝 Współpraca

Contributions, issues i feature requests są mile widziane!

1. Fork projektu
2. Utwórz branch dla feature (`git checkout -b feature/AmazingFeature`)
3. Commit zmian (`git commit -m 'Add some AmazingFeature'`)
4. Push do brancha (`git push origin feature/AmazingFeature`)
5. Otwórz Pull Request

## 📮 Kontakt

W razie pytań lub sugestii, proszę o otwarcie issue na GitHubie.

---

⭐ Jeśli podoba Ci się ten projekt, zostaw gwiazdkę na GitHubie!
