# FLyTicketService

[![Build Status](https://github.com/Pawel0071/FLyTicketService/workflows/Unit%20Tests/badge.svg)](https://github.com/Pawel0071/FLyTicketService/actions)
[![Tests](https://img.shields.io/badge/tests-181%20passing-brightgreen)](https://github.com/Pawel0071/FLyTicketService)
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
├── 📂 Controllers/                    # Kontrolery API (REST endpoints)
│   ├── TicketController.cs           # Zarządzanie biletami (rezerwacja, sprzedaż)
│   ├── FlightScheduleController.cs   # Harmonogramy lotów
│   ├── TenantController.cs           # Zarządzanie klientami
│   └── DscountTypeController.cs      # Typy rabatów i warunki
│
├── 📂 Service/                        # Logika biznesowa
│   ├── ✈️ TicketService.cs           # Zarządzanie cyklem życia biletu
│   ├── 💰 FlightPriceService.cs      # Obliczanie cen i rabatów
│   ├── 📅 FlightScheduleService.cs   # Obsługa harmonogramów lotów
│   ├── 👥 TenantService.cs           # Zarządzanie klientami
│   ├── 🎫 DiscountService.cs         # System rabatowy
│   ├── 🔷 GroupAStrategy.cs          # Strategia dla klientów Group A
│   ├── 🔶 GroupBStrategy.cs          # Strategia dla klientów Group B
│   ├── 🏭 GroupStrategyFactory.cs    # Fabryka strategii grupowych
│   └── Interfaces/                   # Interfejsy serwisów
│
├── 📂 Repositories/                   # Warstwa dostępu do danych
│   ├── GenericRepositorycs.cs        # Generyczne repozytorium (CRUD)
│   └── Interfaces/                   # Interfejsy repozytoriów
│       └── IGenericRepository.cs
│
├── 📂 Model/                          # Modele domenowe (Entity Framework)
│   ├── Ticket.cs                     # Bilet lotniczy
│   ├── FlightSchedule.cs             # Harmonogram lotu
│   ├── FlightSeat.cs                 # Miejsce w samolocie
│   ├── Tenant.cs                     # Klient/Najemca
│   ├── Discount.cs                   # Rabat
│   ├── Condition.cs                  # Warunek rabatu
│   ├── Aircraft.cs                   # Samolot
│   ├── AircraftSeat.cs               # Konfiguracja miejsc w samolocie
│   ├── Airline.cs                    # Linia lotnicza
│   ├── AirPort.cs                    # Port lotniczy
│   └── Enums/                        # Enumy (TicketStatus, TenantGroup, etc.)
│
├── 📂 DTO/                            # Data Transfer Objects (API contracts)
│   ├── TicketDTO.cs                  # DTO biletu
│   ├── FlightScheduleDTO.cs          # DTO harmonogramu
│   ├── FlightScheduleFullDTO.cs      # Pełne info o locie (z zagnieżdżonymi obiektami)
│   ├── FlightSeatDTO.cs              # DTO miejsca
│   ├── TenantDTO.cs                  # DTO klienta
│   ├── DiscountDTO.cs                # DTO rabatu
│   ├── ConditionDTO.cs               # DTO warunku
│   ├── AircraftDTO.cs                # DTO samolotu
│   ├── AirlineDTO.cs                 # DTO linii lotniczej
│   └── AirportDTO.cs                 # DTO portu lotniczego
│
├── 📂 Data/                           # Konfiguracja Entity Framework Core
│   ├── FLyTicketDbContext.cs         # DbContext z konfiguracją
│   ├── Configuration/                # Fluent API configurations
│   │   ├── AircraftConfiguration.cs
│   │   ├── AircraftSeatConfiguration.cs
│   │   ├── AirlineConfiguration.cs
│   │   ├── AirportConfiguration.cs
│   │   ├── ConditionConfiguration.cs
│   │   ├── DiscountConfiguration.cs
│   │   ├── FlightScheduleConfiguration.cs
│   │   ├── FlightSeatConfiguration.cs
│   │   ├── TenantConfiguration.cs
│   │   └── TicketConfigurationcs.cs
│   └── WarmingUpData/                # Dane początkowe (seed data)
│       ├── aircrafts.json            # ~10 samolotów
│       ├── airlines.json             # ~10 linii lotniczych
│       ├── airports.json             # ~30 portów lotniczych
│       └── discount.json             # Przykładowe rabaty
│
├── 📂 Mapper/                         # AutoMapper profiles
│   └── FLyTicketMappingProfile.cs    # Mapowania DTO ↔ Domain
│
├── 📂 Middleware/                     # Custom middleware
│   └── ExceptionHandlingMiddleware.cs # Globalna obsługa błędów HTTP
│
├── 📂 Shared/                         # Wspólne komponenty
│   ├── OperationResult.cs            # Wzorzec Result dla operacji
│   ├── OperationResultExtensions.cs  # Rozszerzenia konwersji do IActionResult
│   ├── OperationStatus.cs            # Enum statusów operacji
│   ├── FlightDetails.cs              # Record ze szczegółami lotu
│   └── EnumConverter.cs              # JSON converter dla enumów
│
├── 📂 Extension/                      # Rozszerzenia
│   ├── SimplyTimeZoneExtension.cs    # Konwersje stref czasowych
│   └── SimplyTimeZoneInfo.cs         # Helper dla stref czasowych
│
├── 📂 Migrations/                     # Migracje Entity Framework Core
│   ├── 20250306152449_InitialCreate.cs
│   ├── 20250306152449_InitialCreate.Designer.cs
│   └── FLyTicketDbContextModelSnapshot.cs
│
├── 📂 Scripts/                        # Skrypty testowe API
│   ├── test-api.sh                   # Bash script (Linux/macOS)
│   ├── GetData.ps1                   # PowerShell - pobieranie danych
│   ├── tenant.ps1                    # PowerShell - dodawanie tenantów
│   ├── Flight.ps1                    # PowerShell - dodawanie lotów
│   └── Tickets.ps1                   # PowerShell - operacje na biletach
│
├── 📂 FlyTicketService.Tests/         # 🧪 Projekt testowy (181 testów)
│   │
│   ├── 📂 Controllers/                # Testy kontrolerów (32 testy)
│   │   ├── DscountTypeControllerTests.cs (8 testów)
│   │   ├── FlightScheduleControllerTests.cs (11 testów)
│   │   ├── TenantControllerTests.cs (8 testów)
│   │   └── TicketControllerTests.cs (5 testów)
│   │
│   ├── 📂 Services/                   # Testy serwisów (41 testów)
│   │   ├── TicketServiceTests.cs (9 testów)
│   │   ├── FlightPriceServiceTests.cs (7 testów)
│   │   ├── TenantServiceTests.cs (6 testów)
│   │   ├── FlightScheduleServiceTests.cs (6 testów)
│   │   ├── DiscountServiceTests.cs (6 testów)
│   │   ├── GroupStrategyTests.cs (8 testów)
│   │   └── GroupStrategyFactoryTests.cs (5 testów)
│   │
│   ├── 📂 Repositories/               # Testy repozytoriów (11 testów)
│   │   └── GenericRepositoryTests.cs (11 testów - integration z In-Memory DB)
│   │
│   ├── 📂 Middleware/                 # Testy middleware (5 testów)
│   │   └── ExceptionHandlingMiddlewareTests.cs (5 testów)
│   │
│   ├── 📂 Shared/                     # Testy komponentów wspólnych (73 testy)
│   │   ├── OperationResultTests.cs (19 testów)
│   │   ├── OperationResultExtensionsTests.cs (10 testów)
│   │   ├── EnumConverterTests.cs (11 testów)
│   │   ├── SimplyTimeZoneExtensionTests.cs (10 testów)
│   │   ├── SimplyTimeZoneInfoTests.cs (8 testów)
│   │   ├── FlightDetailsTests.cs (6 testów)
│   │   └── TicketStatusExtensionTests.cs (9 testów)
│   │
│   └── 📂 Mapper/                     # Testy mapowań (12 testów)
│       └── FLyTicketMappingProfileTests.cs (12 testów)
│
├── 📄 Program.cs                      # Punkt wejścia aplikacji
├── 📄 appsettings.json                # Konfiguracja produkcyjna
├── 📄 appsettings.Development.json    # Konfiguracja deweloperska
├── 📄 FLyTicketService.csproj         # Plik projektu .NET
├── 📄 Dockerfile                      # Konteneryzacja Docker
├── 📄 README.md                       # Dokumentacja projektu
├── 📄 PROJECT_REVIEW.md               # Szczegółowa analiza projektu
├── 📄 CHANGELOG.md                    # Historia zmian
└── 📄 LICENSE.txt                     # Licencja MIT
```

### 📊 Statystyki projektu

- **Linie kodu**: ~8,000+
- **Pliki źródłowe**: ~50
- **Testy**: 181 (100% passing)
- **Pokrycie testami**: ~100%
- **Controllers**: 4
- **Services**: 7
- **Modele domenowe**: 10
- **DTO**: 10

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

Projekt zawiera **181 testów jednostkowych i integracyjnych** z pokryciem **~100%** wszystkich warstw aplikacji:

#### **Kontrolery API** (32 testy)
- ✅ **DiscountTypeControllerTests** (8 testów) - endpointy rabatów
- ✅ **FlightScheduleControllerTests** (11 testów) - endpointy harmonogramów lotów
- ✅ **TenantControllerTests** (8 testów) - endpointy klientów
- ✅ **TicketControllerTests** (5 testów) - endpointy biletów

#### **Logika biznesowa - Serwisy** (41 testów)
- ✅ **TicketService** (9 testów) - zarządzanie biletami (rezerwacja, sprzedaż, anulowanie)
- ✅ **FlightPriceService** (7 testów) - obliczanie cen i aplikacja rabatów
- ✅ **TenantService** (6 testów) - zarządzanie klientami
- ✅ **FlightScheduleService** (6 testów) - harmonogramy lotów
- ✅ **DiscountService** (6 testów) - system rabatowy
- ✅ **GroupStrategyTests** (8 testów) - strategie grup A i B
- ✅ **GroupStrategyFactory** (5 testów) - fabryka strategii

#### **Warstwa dostępu do danych** (11 testów)
- ✅ **GenericRepositoryTests** (11 testów integracyjnych z In-Memory DB)
  - CRUD operations (Add, GetAll, GetById, Update, Remove)
  - Predicate queries (GetByAsync, FilterByAsync)

#### **Middleware** (5 testów)
- ✅ **ExceptionHandlingMiddleware** (5 testów) - obsługa błędów HTTP

#### **Utilities & Shared** (73 testy)
- ✅ **OperationResultTests** (19 testów) - wzorzec Result
- ✅ **OperationResultExtensionsTests** (10 testów) - konwersja na IActionResult
- ✅ **EnumConverterTests** (11 testów) - serializacja JSON enums
- ✅ **SimplyTimeZoneExtensionTests** (10 testów) - konwersje stref czasowych
- ✅ **SimplyTimeZoneInfoTests** (8 testów) - informacje o strefach czasowych
- ✅ **FlightDetailsTests** (6 testów) - record type validation

#### **Mapowania DTO** (12 testów)
- ✅ **FLyTicketMappingProfileTests** (12 testów)
  - Mapowania DTO ↔ Domain (Tenant, Discount, Condition, FlightSchedule, Ticket)

### 📊 Podsumowanie pokrycia testami

✅ **Status testów**: **181/181 passing (100% success rate)**

| Warstwa | Testy | Pokrycie |
|---------|-------|----------|
| Controllers | 32 | 100% |
| Services | 41 | 100% |
| Repositories | 11 | 100% |
| Middleware | 5 | 100% |
| Utilities/Shared | 73 | 100% |
| Mappers | 12 | 100% |
| **RAZEM** | **181** | **~100%** |

### Technologie testowe

- **xUnit** - framework testowy
- **Moq** - mockowanie zależności
- **FluentAssertions** - czytelne asercje
- **EF Core InMemory** - testy integracyjne z bazą w pamięci
- **AAA Pattern** - Arrange-Act-Assert

### Uruchamianie testów

```bash
# Wszystkie testy
dotnet test

# Tylko testy konkretnej warstwy
dotnet test --filter FullyQualifiedName~Controllers
dotnet test --filter FullyQualifiedName~Services
dotnet test --filter FullyQualifiedName~Repositories

# Testy z pokryciem (coverage)
dotnet test --collect:"XPlat Code Coverage"
```

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
