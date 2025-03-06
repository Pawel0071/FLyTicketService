param (
    [string]$Server = "localhost",
    [int]$Port = 32799
)

$BaseUrl = "https://$($Server):$($Port)/api/"
$TenantUrl = $BaseUrl+"Tenant"
$FlightScheduleUrl = $BaseUrl+"FlightSchedule"
$TicketUrl = $BaseUrl+"Ticket"

function Get-Tenants {
    Write-Host "Pobieranie listy tenantów..."
    $Response = Invoke-RestMethod -Uri $TenantUrl -Method Get -ContentType "application/json"
    Write-Host "Lista tenantów:"
    $Response | ForEach-Object { 
        Write-Host "Name: $($_.Name), Address: $($_.Address), Group: $($_.Group), BirthDate: $($_.BirthDate), Phone: $($_.Phone), Email: $($_.Email)" 
    }
}

function Get-FlightSchedule {
    Write-Host "Pobieranie listy lotów..."
    $Response = Invoke-RestMethod -Uri $FlightScheduleUrl -Method Get -ContentType "application/json"
    Write-Host "Lista lotów:"
    $Response | ForEach-Object { 
        Write-Host "Id: $($_.FlightId) Type: $($_.Aircraft.Model) RegistrationNumber: $($_.Aircraft.RegistrationNumber), Departure: $($_.Departure), Arrival: $($_.Arrival), Origin: $($_.Origin.AirportName), Destination: $($_.Destination.AirportName)" 
    }
}

function Get-All-Discounts {
    Write-Host "Pobieranie listy zniżek..."
    $Response = Invoke-RestMethod -Uri "$($TicketUrl)/all-discounts" -Method Get -ContentType "application/json"
    Write-Host "Lista zniżek:"
    $Response | ForEach-Object { 
        Write-Host "DiscountId: $($_.DiscountId), Description: $($_.Name), Amount: $($_.Value)" 
    }
}


Get-Tenants
Get-FlightSchedule
Get-All-Discounts