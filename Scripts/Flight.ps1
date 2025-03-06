param (
    [string]$ServerHost = "localhost",
    [int]$Port = 32799
)
# Define the API endpoints and headers
$BaseUrl = "https://$($ServerHost):$($Port)/api/"
$TenantUrl = $BaseUrl+"Tenant"
$FlightScheduleUrl = $BaseUrl+"FlightSchedule"
$TicketUrl = $BaseUrl+"Ticket"

$headers = @{
    "Content-Type" = "application/json"
}

# Helper function to create a flight schedule
function New-FlightSchedule($airlineIATA, $number, $suffix, $aircraft, $departure, $arrival, $origin, $destination, [decimal]$price, [int]$days, [int]$occurrence) {
    return @{
        AirlineIATA = $airlineIATA
        Number = $number
        NumberSuffix = $suffix
        AircraftRegistrationNumber = $aircraft
        Departure = $departure
        Arrival = $arrival
        OriginIATA = $origin
        DestinationIATA = $destination
        Price = $price
        DaysOfWeek = $days
        Occurrence = $occurrence
        FlightScheduleId = [guid]::NewGuid()
    }
}

# Define flight data
$flights = @(
    # Europe to America
    New-FlightSchedule "AA" "00101" "AAA" "SP-LRA" "2025-03-06T08:00:00Z" "2025-03-06T14:00:00Z" "CDG" "JFK" 50 1 1
    New-FlightSchedule "EK" "00202" "BAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T15:00:00Z" "WAW" "LAX" 55 2 1
    New-FlightSchedule "DL" "00303" "CAA" "SP-LLA" "2025-03-08T10:00:00Z" "2025-03-08T16:00:00Z" "FRA" "ORD" 60 3 1
    New-FlightSchedule "QF" "00404" "DAA" "SP-LGG" "2025-03-09T11:00:00Z" "2025-03-09T17:00:00Z" "FCO" "MIA" 65 4 1
    New-FlightSchedule "DL" "00505" "EAA" "SP-LAA" "2025-03-10T12:00:00Z" "2025-03-10T18:00:00Z" "AMS" "ATL" 70 5 1

    # Europe to Africa
    New-FlightSchedule "AA" "00601" "FAA" "SP-LRA" "2025-03-06T08:00:00Z" "2025-03-06T12:00:00Z" "CDG" "CPT" 45 1 1
    New-FlightSchedule "EK" "00702" "GAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T13:00:00Z" "WAW" "JNB" 50 2 1
    New-FlightSchedule "LO" "00803" "HAA" "SP-LLA" "2025-03-08T10:00:00Z" "2025-03-08T14:00:00Z" "FRA" "CAI" 40 3 1
    New-FlightSchedule "AA" "00904" "IAA" "SP-LBG" "2025-03-09T11:00:00Z" "2025-03-09T15:00:00Z" "FCO" "CPT" 55 4 1
    New-FlightSchedule "DL" "00105" "JAA" "SP-LAA" "2025-03-10T12:00:00Z" "2025-03-10T16:00:00Z" "AMS" "CAI" 30 5 1

    # Europe to Asia
    New-FlightSchedule "LO" "01101" "KAA" "SP-LBG" "2025-03-06T08:00:00Z" "2025-03-06T18:00:00Z" "CDG" "PEK" 70 1 1
    New-FlightSchedule "LO" "01202" "LAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T19:00:00Z" "WAW" "NTR" 75 4 1
    New-FlightSchedule "LO" "01303" "MAA" "SP-LAA" "2025-03-08T10:00:00Z" "2025-03-08T20:00:00Z" "FRA" "PEK" 80 6 1

    # America to Africa
    New-FlightSchedule "QR" "01401" "NAA" "SP-LBG" "2025-03-06T08:00:00Z" "2025-03-06T18:00:00Z" "JFK" "CPT" 30 1 1
    New-FlightSchedule "QR" "01502" "OAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T19:00:00Z" "ATL" "JNB" 45 6 1

    # Europe to Australia
    New-FlightSchedule "QF" "00806" "LAA" "SP-LLA" "2025-03-11T10:00:00Z" "2025-03-11T22:00:00Z" "CDG" "SYD" 25 1 1
    New-FlightSchedule "AA" "00907" "MAA" "SP-LRA" "2025-03-12T12:00:00Z" "2025-03-12T23:30:00Z" "LHR" "MEL" 25 2 1

    # America to Australia
    New-FlightSchedule "VA" "01008" "NAA" "SP-LGG" "2025-03-13T08:00:00Z" "2025-03-13T18:30:00Z" "LAX" "SYD" 50 3 1
    New-FlightSchedule "QF" "01109" "OAA" "SP-LRA" "2025-03-14T09:30:00Z" "2025-03-14T20:00:00Z" "JFK" "BNE" 40 4 1

    # Asia to Australia
    New-FlightSchedule "CX" "01210" "PAA" "SP-LLA" "2025-03-15T10:00:00Z" "2025-03-15T16:00:00Z" "NTR" "SYD" 25 5 1
    
    # America to Europe
    New-FlightSchedule "DL" "01301" "PAA" "SP-LGG" "2025-03-16T08:30:00Z" "2025-03-16T16:30:00Z" "ATL" "AMS" 85 2 1
    New-FlightSchedule "AA" "01402" "QAA" "SP-LPP" "2025-03-17T10:00:00Z" "2025-03-17T18:00:00Z" "JFK" "LHR" 95 3 1

    # Asia to Europe
    New-FlightSchedule "SQ" "01503" "RAA" "SP-LKK" "2025-03-18T12:00:00Z" "2025-03-18T20:00:00Z" "HKG" "FRA" 30 4 1
    New-FlightSchedule "QR" "01604" "SAA" "SP-LLL" "2025-03-19T08:00:00Z" "2025-03-19T18:00:00Z" "DOH" "CDG" 35 5 1

    # Africa to Europe
    New-FlightSchedule "QF" "01705" "TAA" "SP-LPP" "2025-03-20T09:00:00Z" "2025-03-20T17:00:00Z" "ADD" "FCO" 70 6 1
    New-FlightSchedule "EK" "01806" "UAA" "SP-LQQ" "2025-03-21T11:00:00Z" "2025-03-21T19:00:00Z" "JNB" "LHR" 90 7 1

    # Europe to Asia
    New-FlightSchedule "QF" "01907" "VAA" "SP-LOO" "2025-03-22T07:00:00Z" "2025-03-22T17:00:00Z" "LHR" "NTR" 25 1 1
    New-FlightSchedule "AA" "02008" "WAA" "SP-LPP" "2025-03-23T08:30:00Z" "2025-03-23T18:30:00Z" "CDG" "PEK" 30 2 1

    # Australia to Europe
    New-FlightSchedule "QF" "02109" "XAA" "SP-LQQ" "2025-03-24T14:00:00Z" "2025-03-24T23:00:00Z" "SYD" "FRA" 50 3 1
    New-FlightSchedule "VA" "02210" "YAA" "SP-LRR" "2025-03-25T13:00:00Z" "2025-03-25T22:00:00Z" "MEL" "LHR" 40 4 1
)

# Add each flight using POST method
foreach ($flight in $flights) {
    $flightJson = $flight | ConvertTo-Json -Depth 10
    $response = Invoke-RestMethod -Uri "$FlightScheduleUrl" -Method Post -Headers $headers -Body $flightJson

    if ($response -eq $true) {
        Write-Host "Flight successfully scheduled: $($flight.FlightScheduleId)"
    } else {
        Write-Host "Failed to schedule flight: $($flight.FlightScheduleId)"
    }
}

