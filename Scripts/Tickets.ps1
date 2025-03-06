param (
    [string]$ServerHost = "localhost",
    [int]$Port = 32801
)
# Define the API endpoints and headers
$BaseUrl = "https://$($ServerHost):$($Port)/api/Ticket"

# Helper function to send HTTP requests
function Invoke-ApiRequest {
    param (
        [string]$Url ,
        [string]$Method,
        [hashtable]$Headers,
        [object]$Body
    )
    $JsonBody = if ($Body) { $Body | ConvertTo-Json -Depth 10 -Compress } else { $null }
    Write-Host $TicketUrl
    Invoke-RestMethod -Uri $Url  -Method $Method -Headers $Headers -Body $JsonBody
}

# Headers
$headers = @{
    "Content-Type" = "application/json"
}

# Operations

# Reserve a ticket
function Reserve-Ticket {
    param (
        [string]$FlightId,
        [string]$SeatNo,
        [guid]$TenantId
    )
    $TicketUrl = "$BaseUrl/reserve?flightId=$FlightId&seatNo=$SeatNo&tenantId=$TenantId"
    Invoke-ApiRequest -Url $TicketUrl -Method "POST" -Headers $headers
}

# Sell a ticket
function Sell-Ticket {
    param (
        [string]$FlightId,
        [string]$SeatNo,
        [guid]$TenantId,
        [array]$Discounts
    )
    $TicketUrl = "$BaseUrl/sell?flightId=$FlightId&seatNo=$SeatNo&tenantId=$TenantId"
    Invoke-ApiRequest -Url $TicketUrl -Method "POST" -Headers $headers -Body $Discounts
}

# Sell a reserved ticket
function Sell-Reserved-Ticket {
    param (
        [string]$TicketNumber
    )
    $TicketUrl = "$BaseUrl/sell-reserved?ticketNumber=$TicketNumber"
    Invoke-ApiRequest -Url $TicketUrl -Method "POST" -Headers $headers
}

# Cancel a ticket
function Cancel-Ticket {
    param (
        [string]$TicketNumber
    )
    $TicketUrl = "$BaseUrl/$TicketNumber"
    Invoke-ApiRequest -Url $TicketUrl -Method "DELETE" -Headers $headers
}

# Get ticket details
function Get-Ticket {
    param (
        [string]$TicketNumber
    )
    $TicketUrl = "$BaseUrl/$TicketNumber"
    Invoke-ApiRequest -Url $TicketUrl -Method "GET" -Headers $headers
}

# Get a list of tickets
function Get-Tickets {
    param (
        [string]$FlyNumber,
        [guid]$TenantId,
        [datetime]$Departure,
        [string]$OriginIATA,
        [string]$DestinationIATA
    )
    $TicketUrl = "$BaseUrl?flyNumber=$FlyNumber&tenantId=$TenantId&departure=$Departure&originIATA=$OriginIATA&destinationIATA=$DestinationIATA"
    Invoke-ApiRequest -Url $TicketUrl -Method "GET" -Headers $headers
}

# Get all applicable discounts for a ticket
function Get-All-Discounts {
    Invoke-ApiRequest -Url "$BaseUrl/all-discounts" -Method "GET" -Headers $headers
}

# Apply discounts to a ticket
function Apply-Discount {
    param (
        [string]$TicketNumber,
        [array]$Discounts
    )
    $TicketUrl = "$BaseUrl/apply-discount?ticketNumber=$TicketNumber"
    Invoke-ApiRequest -Url $TicketUrl -Method "POST" -Headers $headers -Body $Discounts
}

# Check if a discount can be applied
function Can-Apply-Discount {
    param (
        [string]$TicketNumber,
        [hashtable]$Discount
    )
    $TicketUrl = "$BaseUrl/can-apply-discount/$TicketNumber"
    Invoke-ApiRequest -Url $TicketUrl -Method "GET" -Headers $headers -Body $Discount
}

$TenantUrl = "https://$($ServerHost):$($Port)/api/Tenant"

function Get-Tenants {
    Write-Host "Pobieranie listy tenantów..."
    $Response = Invoke-RestMethod -Uri $TenantUrl -Method Get -ContentType "application/json"
    Write-Host "Lista tenantów:"
    $Response | ForEach-Object { 
        Write-Host "Name: $($_.Name), Address: $($_.Address), Group: $($_.Group), BirthDate: $($_.BirthDate), Phone: $($_.Phone), Email: $($_.Email)" 
    }
    return $Response
}

# Sample Operations (10 Examples)

$Tenants = Get-Tenants
Write-Output $Tenants
$discounts = Get-All-Discounts 
Write-Output $discounts 

# Przykład 1: Rezerwacja biletu
$ticket1 = Reserve-Ticket -FlightId "QR 01604 SAA" -SeatNo "7A" -TenantId ($Tenants | Where-Object { $_.Name -eq "John Doe" }).TenantId
Write-Output $ticket1

# Przykład 2: Rezerwacja kolejnego biletu
$ticket2 = Reserve-Ticket -FlightId "EK 00702 GAA" -SeatNo "2B" -TenantId ($Tenants | Where-Object { $_.Name -eq "Jane Smith" }).TenantId
Write-Output $ticket2

# Przykład 3: Rezerwacja biletu dla innego lotu
$ticket3 = Reserve-Ticket -FlightId "LO 01202 LAA" -SeatNo "4C" -TenantId ($Tenants | Where-Object { $_.Name -eq "Alice Brown" }).TenantId
Write-Output $ticket3

# Przykład 4: Sprzedaż biletu po rezerwacji
$ticket4 = Sell-Ticket -FlightId "QR 01604 SAA" -SeatNo "1A" -TenantId ($Tenants | Where-Object { $_.Name -eq "Bob Johnson" }).TenantId -Discounts @($discounts | Where-Object { $_.Name -eq "Birthday Discount" })
Write-Output $ticket4

# Przykład 5: Sprzedaż biletu bez rezerwacji
$ticket5 = Sell-Ticket -FlightId "EK 00702 GAA" -SeatNo "2B" -TenantId ($Tenants | Where-Object { $_.Name -eq "Liam Wilson" }).TenantId -Discounts @($discounts | Where-Object { $_.Name -eq "Thursday Africa Discount" })
Write-Output $ticket5

# Przykład 6: Sprzedaż kolejnego biletu po rezerwacji
$ticket6 = Sell-Reserved-Ticket -TicketNumber "$($ticket2.TicketNumber)"
Write-Output $ticket6

# Przykład 7: Anulowanie biletu
Cancel-Ticket -TicketNumber "$($ticket1.TicketNumber)"

# Przykład 8: Pobranie szczegółów biletu
$ticket7 = Get-Ticket -TicketNumber "$($ticket3.TicketNumber)"
Write-Output $ticket7

# Przykład 9: Pobranie listy biletów dla lotu
$tickets = Get-Tickets -FlyNumber "LO 01202 LAA" -TenantId ($Tenants | Where-Object { $_.Name -eq "William Anderson" }).TenantId -Departure (Get-Date "2025-03-15") -OriginIATA "WAW" -DestinationIATA "JFK"
Write-Output $tickets

# Przykład 10: Nadanie zniżek do biletu
Apply-Discount -TicketNumber "$($ticket7.TicketNumber)" -Discounts @($discounts)

# Przykład 11: Rezerwacja biletu dla kolejnego lotu
$ticket8 = Reserve-Ticket -FlightId "QF 00806 LAA" -SeatNo "2C" -TenantId ($Tenants | Where-Object { $_.Name -eq "Mia Martin" }).TenantId
Write-Output $ticket8

# Przykład 12: Sprzedaż kolejnego biletu dla innego lotu
$ticket9 = Sell-Ticket -FlightId "QF 00806 LAA" -SeatNo "2C" ($Tenants | Where-Object { $_.Name -eq "Amelia Garcia" }).TenantId  -Discounts @($discounts | Where-Object { $_.Name -eq "Birthday Discount" })
Write-Output $ticket9

Can-Apply-Discount -TicketNumber "$($ticket7.TicketNumber)"-Discount @($discounts | Where-Object { $_.Name -eq "Birthday Discount" })
