param (
    [string]$Server = "localhost",
    [int]$Port = 32799
)

$BaseUrl = "https://$($Server):$($Port)/api/"
$TenantUrl = $BaseUrl+"Tenant"
$FlightScheduleUrl = $BaseUrl+"FlightSchedule"
$TicketUrl = $BaseUrl+"Ticket"

# Funkcja dodająca tenantów
function Add-Tenants {
    # Przygotuj tenantów
    $Tenants = @(
        @{ Name = "John Doe"; Address = "123 Elm Street"; Group = "GroupA"; BirthDate = "1985-06-15"; Phone = "123-456-7890"; Email = "john.doe@example.com" },
        @{ Name = "Jane Smith"; Address = "456 Maple Avenue"; Group = "GroupB"; BirthDate = "1990-03-22"; Phone = "987-654-3210"; Email = "jane.smith@example.com" },
        @{ Name = "Alice Brown"; Address = "789 Oak Boulevard"; Group = "GroupA"; BirthDate = "1978-11-07"; Phone = "555-123-4567"; Email = "alice.brown@example.com" },
        @{ Name = "Bob Johnson"; Address = "321 Pine Road"; Group = "GroupB"; BirthDate = "1982-04-19"; Phone = "444-789-1234"; Email = "bob.johnson@example.com" },
        @{ Name = "Liam Wilson"; Address = "12 Baker Street"; Group = "GroupA"; BirthDate = "1988-01-12"; Phone = "111-222-3333"; Email = "liam.wilson@example.com" },
        @{ Name = "Emma Davis"; Address = "34 King Street"; Group = "GroupB"; BirthDate = "1992-02-23"; Phone = "222-333-4444"; Email = "emma.davis@example.com" },
        @{ Name = "Noah Miller"; Address = "56 Queen Street"; Group = "GroupA"; BirthDate = "1980-03-14"; Phone = "333-444-5555"; Email = "noah.miller@example.com" },
        @{ Name = "Olivia Taylor"; Address = "78 Prince Street"; Group = "GroupB"; BirthDate = "1985-04-25"; Phone = "444-555-6666"; Email = "olivia.taylor@example.com" },
        @{ Name = "William Anderson"; Address = "90 Duke Street"; Group = "GroupA"; BirthDate = "1990-05-16"; Phone = "555-666-7777"; Email = "william.anderson@example.com" },
        @{ Name = "Sophia Thomas"; Address = "12 Duchess Street"; Group = "GroupB"; BirthDate = "1982-06-27"; Phone = "666-777-8888"; Email = "sophia.thomas@example.com" },
        @{ Name = "James Jackson"; Address = "34 Earl Street"; Group = "GroupA"; BirthDate = "1987-07-18"; Phone = "777-888-9999"; Email = "james.jackson@example.com" },
        @{ Name = "Isabella White"; Address = "56 Baron Street"; Group = "GroupB"; BirthDate = "1993-08-29"; Phone = "888-999-0000"; Email = "isabella.white@example.com" },
        @{ Name = "Benjamin Harris"; Address = "78 Viscount Street"; Group = "GroupA"; BirthDate = "1981-09-10"; Phone = "999-000-1111"; Email = "benjamin.harris@example.com" },
        @{ Name = "Mia Martin"; Address = "90 Marquess Street"; Group = "GroupB"; BirthDate = "1986-10-21"; Phone = "000-111-2222"; Email = "mia.martin@example.com" },
        @{ Name = "Lucas Thompson"; Address = "12 Count Street"; Group = "GroupA"; BirthDate = "1991-11-12"; Phone = "111-222-3333"; Email = "lucas.thompson@example.com" },
        @{ Name = "Amelia Garcia"; Address = "34 Baroness Street"; Group = "GroupB"; BirthDate = "1983-12-23"; Phone = "222-333-4444"; Email = "amelia.garcia@example.com" },
        @{ Name = "Henry Martinez"; Address = "56 Viscountess Street"; Group = "GroupA"; BirthDate = "1989-01-14"; Phone = "333-444-5555"; Email = "henry.martinez@example.com" },
        @{ Name = "Evelyn Robinson"; Address = "78 Countess Street"; Group = "GroupB"; BirthDate = "1994-02-25"; Phone = "444-555-6666"; Email = "evelyn.robinson@example.com" },
        @{ Name = "Alexander Clark"; Address = "90 Earl Street"; Group = "GroupA"; BirthDate = "1984-03-16"; Phone = "555-666-7777"; Email = "alexander.clark@example.com" },
        @{ Name = "Harper Rodriguez"; Address = "12 Duke Street"; Group = "GroupB"; BirthDate = "1995-04-27"; Phone = "666-777-8888"; Email = "harper.rodriguez@example.com" },
        @{ Name = "Daniel Lewis"; Address = "34 Prince Street"; Group = "GroupA"; BirthDate = "1986-05-18"; Phone = "777-888-9999"; Email = "daniel.lewis@example.com" },
        @{ Name = "Ella Lee"; Address = "56 Queen Street"; Group = "GroupB"; BirthDate = "1996-06-29"; Phone = "888-999-0000"; Email = "ella.lee@example.com" },
        @{ Name = "Matthew Walker"; Address = "78 King Street"; Group = "GroupA"; BirthDate = "1987-07-10"; Phone = "999-000-1111"; Email = "matthew.walker@example.com" },
        @{ Name = "Avery Hall"; Address = "90 Baker Street"; Group = "GroupB"; BirthDate = "1997-08-21"; Phone = "000-111-2222"; Email = "avery.hall@example.com" },
        @{ Name = "David Allen"; Address = "12 Elm Street"; Group = "GroupA"; BirthDate = "1988-09-12"; Phone = "111-222-3333"; Email = "david.allen@example.com" },
        @{ Name = "Scarlett Young"; Address = "34 Maple Avenue"; Group = "GroupB"; BirthDate = "1998-10-23"; Phone = "222-333-4444"; Email = "scarlett.young@example.com" },
        @{ Name = "Joseph Hernandez"; Address = "56 Oak Boulevard"; Group = "GroupA"; BirthDate = "1989-11-14"; Phone = "333-444-5555"; Email = "joseph.hernandez@example.com" },
        @{ Name = "Grace King"; Address = "78 Pine Road"; Group = "GroupB"; BirthDate = "1999-12-25"; Phone = "444-555-6666"; Email = "grace.king@example.com" },
        @{ Name = "Samuel Wright"; Address = "90 Birch Street"; Group = "GroupA"; BirthDate = "1990-01-16"; Phone = "555-666-7777"; Email = "samuel.wright@example.com" },
        @{ Name = "Victoria Scott"; Address = "12 Cedar Street"; Group = "GroupB"; BirthDate = "2000-02-27"; Phone = "666-777-8888"; Email = "victoria.scott@example.com" }
    )

    # Dodaj każdego tenanta za pomocą POST
    foreach ($Tenant in $Tenants) {
        $Response = Invoke-RestMethod -Uri $TenantUrl -Method Post -Body ($Tenant | ConvertTo-Json -Depth 10) -ContentType "application/json"
        Write-Host "Dodano tenant: $($Tenant.Name). Odpowiedź API: $Response"
    }
}



# Funkcja pobierająca wszystkich tenantów
function Get-Tenants {
    Write-Host "Pobieranie listy tenantów..."
    $Response = Invoke-RestMethod -Uri $TenantUrl -Method Get -ContentType "application/json"
    Write-Host "Lista tenantów:"
    $Response | ForEach-Object { 
        Write-Host "Name: $($_.Name), Address: $($_.Address), Group: $($_.Group), BirthDate: $($_.BirthDate), Phone: $($_.Phone), Email: $($_.Email)" 
    }
}

# Wywołaj funkcje
Add-Tenants
Get-Tenants
