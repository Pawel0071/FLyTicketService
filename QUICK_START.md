# 🚀 Quick Start Guide - FLyTicketService

Szybki przewodnik po uruchomieniu i testowaniu aplikacji FLyTicketService.

## 📋 Wymagania wstępne

- ✅ .NET 8.0 SDK
- ✅ SQL Server (lokalny lub Docker)
- ✅ Visual Studio Code / Visual Studio / Rider (opcjonalnie)

## 🔧 Krok 1: Uruchomienie SQL Server

### Opcja A: Docker Compose (najłatwiejsza!) 🐳

```bash
# Uruchom wszystko jedną komendą!
docker-compose up -d

# Sprawdź status
docker-compose ps

# Logi
docker-compose logs -f

# Zatrzymaj
docker-compose down
```

✅ **Co się uruchomi?**
- SQL Server 2022 na porcie 1433
- Automatyczny healthcheck
- Persystentne dane (volume)
- Sieć Docker dla komunikacji

### Opcja B: Docker (ręcznie)

```bash
# Uruchom kontener SQL Server
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" \
   -p 1433:1433 --name mssql-flyticket \
   -d mcr.microsoft.com/mssql/server:2022-latest

# Sprawdź czy działa
docker ps | grep mssql
```

### Opcja C: SQL Server lokalny

Upewnij się że SQL Server działa i jest dostępny na porcie 1433.

## 🗄️ Krok 2: Konfiguracja Connection String

Edytuj `appsettings.json` lub `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FlyTicket;User=sa;Password=YourStrong@Password123;TrustServerCertificate=True"
  }
}
```

## 🏗️ Krok 3: Utworzenie bazy danych

```bash
cd /Volumes/Data/Repositories/FlyTicketService

# Przywróć pakiety NuGet
dotnet restore

# Zastosuj migracje i utwórz bazę
dotnet ef database update
```

✅ **Co zostanie utworzone?**
- Baza danych `FlyTicket`
- Wszystkie tabele (Aircrafts, Airlines, Airports, Tenants, FlightSchedules, etc.)
- **Seed data** z plików JSON:
  - ~30 portów lotniczych
  - ~10 linii lotniczych  
  - ~10 samolotów
  - Przykładowe rabaty

## ▶️ Krok 4: Uruchomienie aplikacji

```bash
# Zbuduj projekt
dotnet build

# Uruchom aplikację
dotnet run
```

Aplikacja będzie dostępna pod:
- 🌐 HTTP: `http://localhost:5000`
- 🔒 HTTPS: `https://localhost:5001`
- 📚 Swagger: `https://localhost:5001/swagger`

## 🧪 Krok 5: Testowanie API

### Opcja A: Swagger UI (najłatwiejsze)

1. Otwórz przeglądarkę: `https://localhost:5001/swagger`
2. Zaakceptuj certyfikat (Development)
3. Testuj endpointy bezpośrednio w przeglądarce

### Opcja B: Skrypt bash (macOS/Linux)

```bash
cd Scripts
chmod +x test-api.sh
./test-api.sh localhost 5000
```

### Opcja C: PowerShell (Windows)

```powershell
# Pobierz dane
.\Scripts\GetData.ps1 -Server localhost -Port 5000

# Dodaj przykładowych tenantów
.\Scripts\tenant.ps1 -Server localhost -Port 5000

# Dodaj loty
.\Scripts\Flight.ps1 -ServerHost localhost -Port 5000
```

### Opcja D: curl

```bash
# Pobierz wszystkich tenantów
curl -X GET "https://localhost:5001/api/Tenant" -k

# Pobierz wszystkie loty
curl -X GET "https://localhost:5001/api/FlightSchedule" -k

# Dodaj nowego tenanta
curl -X POST "https://localhost:5001/api/Tenant" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Jan Kowalski",
    "address": "ul. Testowa 1",
    "group": "GroupA",
    "birthDate": "1990-01-01",
    "phone": "123456789",
    "email": "jan@example.com"
  }' -k
```

## 🎫 Przykładowy workflow - Rezerwacja biletu

### 1. Pobierz dostępne loty

```bash
curl -X GET "https://localhost:5001/api/FlightSchedule" -k
```

Zapisz `flightId` z odpowiedzi, np. `"FL123"`

### 2. Pobierz dostępne miejsca dla lotu

```bash
curl -X GET "https://localhost:5001/api/FlightSchedule/FL123" -k
```

Sprawdź wolne miejsca w sekcji `seats`, wybierz `seatNumber`, np. `"1A"`

### 3. Pobierz ID tenanta

```bash
curl -X GET "https://localhost:5001/api/Tenant" -k
```

Zapisz `tenantId` z odpowiedzi

### 4. Zarezerwuj bilet

```bash
curl -X POST "https://localhost:5001/api/Ticket/reserve?flightId=FL123&seatNo=1A&tenantId=<TENANT_GUID>" -k
```

### 5. Sprzedaj bilet z rabatem

```bash
curl -X POST "https://localhost:5001/api/Ticket/sell?flightId=FL123&seatNo=1A&tenantId=<TENANT_GUID>" \
  -H "Content-Type: application/json" \
  -d '[
    {
      "name": "EarlyBird",
      "value": 10,
      "description": "Early booking discount"
    }
  ]' -k
```

## 🧪 Testy jednostkowe

```bash
# Uruchom wszystkie testy
cd /Volumes/Data/Repositories/FlyTicketService.Tests
dotnet test

# Z szczegółami
dotnet test --verbosity detailed

# Z pokryciem kodu
dotnet test --collect:"XPlat Code Coverage"
```

✅ **Oczekiwany rezultat**: 43/43 testy passing

## 📊 Monitorowanie

### Logi aplikacji

Logi są wyświetlane w konsoli podczas działania:

```
info: FLyTicketService.Services.TicketService[0]
      Reserving ticket for flight: FL123
info: FLyTicketService.Services.FlightPriceService[0]
      Calculating price with discounts...
```

### Swagger UI

Pełna dokumentacja API z możliwością testowania:
- 📍 URL: `https://localhost:5001/swagger`
- 🔍 Wszystkie endpointy z opisami
- 🧪 Try it out - testuj bezpośrednio
- 📝 Modele request/response

## 🐛 Rozwiązywanie problemów

### Problem: Błąd połączenia z bazą danych

```
Microsoft.Data.SqlClient.SqlException: A network-related or instance-specific error...
```

**Rozwiązanie:**
1. Sprawdź czy SQL Server działa: `docker ps` lub `services.msc`
2. Zweryfikuj connection string w `appsettings.json`
3. Sprawdź firewall/port 1433

### Problem: Błąd migracji

```
Build failed. Error: ...
```

**Rozwiązanie:**
```bash
# Usuń poprzednią bazę
dotnet ef database drop --force

# Zastosuj migracje ponownie
dotnet ef database update
```

### Problem: Port zajęty

```
Failed to bind to address https://127.0.0.1:5001
```

**Rozwiązanie:**
```bash
# Zmień port w launchSettings.json lub użyj zmiennej:
ASPNETCORE_URLS="https://localhost:5002;http://localhost:5003" dotnet run
```

## 📚 Następne kroki

1. ✅ Przeczytaj pełną dokumentację: [README.md](../README.md)
2. 🔍 Eksploruj API przez Swagger UI
3. 🧪 Uruchom testy jednostkowe
4. 🛠️ Testuj API za pomocą skryptów z folderu `Scripts/`
5. 📖 Zapoznaj się z architekturą w sekcji wzorców projektowych

## 💡 Przydatne komendy

```bash
# Sprawdź wersję .NET
dotnet --version

# Wyczyść build
dotnet clean

# Rebuild projektu
dotnet build --no-incremental

# Uruchom w trybie watch (auto-reload)
dotnet watch run

# Lista zainstalowanych pakietów
dotnet list package

# Aktualizacja pakietów
dotnet restore
```

---

🎉 **Gratulacje!** Masz teraz działający system rezerwacji biletów lotniczych!

Jeśli masz problemy, sprawdź:
- 📖 [README.md](../README.md) - pełna dokumentacja
- 🐛 [Issues na GitHubie](https://github.com/Pawel0071/FLyTicketService/issues)
- 📧 Kontakt: otwórz issue z pytaniem
