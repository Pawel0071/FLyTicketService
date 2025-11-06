#!/bin/bash

# Flight.sh - Add sample flight schedules to FlyTicketService
# Usage: ./Flight.sh [host] [port]

SERVER=${1:-localhost}
PORT=${2:-5000}
BASE_URL="http://${SERVER}:${PORT}/api"
FLIGHT_URL="${BASE_URL}/FlightSchedule"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo "=========================================="
echo "FlyTicketService - Add Flight Schedules"
echo "Base URL: $BASE_URL"
echo "=========================================="
echo ""

# Function to add flight schedule
add_flight() {
    local airlineIATA=$1
    local number=$2
    local suffix=$3
    local aircraft=$4
    local departure=$5
    local arrival=$6
    local origin=$7
    local destination=$8
    local price=$9
    local days=${10}
    local occurrence=${11}
    
    local flightId="${airlineIATA} ${number} ${suffix}"
    echo -e "${YELLOW}Adding flight:${NC} $flightId ($origin → $destination)"
    
    json_data=$(cat <<EOF
{
  "airlineIATA": "$airlineIATA",
  "number": "$number",
  "numberSuffix": "$suffix",
  "aircraftRegistrationNumber": "$aircraft",
  "departure": "$departure",
  "arrival": "$arrival",
  "originIATA": "$origin",
  "destinationIATA": "$destination",
  "price": $price,
  "daysOfWeek": $days,
  "occurrence": $occurrence
}
EOF
)
    
    response=$(curl -s -w "\n%{http_code}" -X POST "$FLIGHT_URL" \
        -H "Content-Type: application/json" \
        -d "$json_data" 2>&1)
    
    http_code=$(echo "$response" | tail -1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Success${NC} (HTTP $http_code)"
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)"
        echo "Response: $body"
    fi
    echo ""
}

echo "Adding flight schedules..."
echo ""

# Europe to America
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "EUROPE → AMERICA"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "AA" "00101" "AAA" "SP-LRA" "2025-03-06T08:00:00Z" "2025-03-06T14:00:00Z" "CDG" "JFK" 50 1 1
add_flight "EK" "00202" "BAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T15:00:00Z" "WAW" "LAX" 55 2 1
add_flight "DL" "00303" "CAA" "SP-LLA" "2025-03-08T10:00:00Z" "2025-03-08T16:00:00Z" "FRA" "ORD" 60 3 1
add_flight "QF" "00404" "DAA" "SP-LGG" "2025-03-09T11:00:00Z" "2025-03-09T17:00:00Z" "FCO" "MIA" 65 4 1
add_flight "DL" "00505" "EAA" "SP-LAA" "2025-03-10T12:00:00Z" "2025-03-10T18:00:00Z" "AMS" "ATL" 70 5 1

# Europe to Africa
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "EUROPE → AFRICA"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "AA" "00601" "FAA" "SP-LRA" "2025-03-06T08:00:00Z" "2025-03-06T12:00:00Z" "CDG" "CPT" 45 1 1
add_flight "EK" "00702" "GAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T13:00:00Z" "WAW" "JNB" 50 2 1
add_flight "LO" "00803" "HAA" "SP-LLA" "2025-03-08T10:00:00Z" "2025-03-08T14:00:00Z" "FRA" "CAI" 40 3 1
add_flight "AA" "00904" "IAA" "SP-LBG" "2025-03-09T11:00:00Z" "2025-03-09T15:00:00Z" "FCO" "CPT" 55 4 1
add_flight "DL" "00105" "JAA" "SP-LAA" "2025-03-10T12:00:00Z" "2025-03-10T16:00:00Z" "AMS" "CAI" 30 5 1

# Europe to Asia
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "EUROPE → ASIA"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "LO" "01101" "KAA" "SP-LBG" "2025-03-06T08:00:00Z" "2025-03-06T18:00:00Z" "CDG" "PEK" 70 1 1
add_flight "LO" "01202" "LAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T19:00:00Z" "WAW" "NTR" 75 4 1
add_flight "LO" "01303" "MAA" "SP-LAA" "2025-03-08T10:00:00Z" "2025-03-08T20:00:00Z" "FRA" "PEK" 80 6 1

# America to Africa
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "AMERICA → AFRICA"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "QR" "01401" "NAA" "SP-LBG" "2025-03-06T08:00:00Z" "2025-03-06T18:00:00Z" "JFK" "CPT" 30 1 1
add_flight "QR" "01502" "OAA" "SP-LBG" "2025-03-07T09:00:00Z" "2025-03-07T19:00:00Z" "ATL" "JNB" 45 6 1

# Europe to Australia
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "EUROPE → AUSTRALIA"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "QF" "00806" "LAA" "SP-LLA" "2025-03-11T10:00:00Z" "2025-03-11T22:00:00Z" "CDG" "SYD" 25 1 1
add_flight "AA" "00907" "MAA" "SP-LRA" "2025-03-12T12:00:00Z" "2025-03-12T23:30:00Z" "LHR" "MEL" 25 2 1

# America to Australia
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "AMERICA → AUSTRALIA"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "VA" "01008" "NAA" "SP-LGG" "2025-03-13T08:00:00Z" "2025-03-13T18:30:00Z" "LAX" "SYD" 50 3 1
add_flight "QF" "01109" "OAA" "SP-LRA" "2025-03-14T09:30:00Z" "2025-03-14T20:00:00Z" "JFK" "BNE" 40 4 1

# Asia to Australia
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "ASIA → AUSTRALIA"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "CX" "01210" "PAA" "SP-LLA" "2025-03-15T10:00:00Z" "2025-03-15T16:00:00Z" "NTR" "SYD" 25 5 1

# America to Europe
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "AMERICA → EUROPE"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "DL" "01301" "PAA" "SP-LGG" "2025-03-16T08:30:00Z" "2025-03-16T16:30:00Z" "ATL" "AMS" 85 2 1
add_flight "AA" "01402" "QAA" "SP-LPP" "2025-03-17T10:00:00Z" "2025-03-17T18:00:00Z" "JFK" "LHR" 95 3 1

# Asia to Europe
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "ASIA → EUROPE"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "SQ" "01503" "RAA" "SP-LKK" "2025-03-18T12:00:00Z" "2025-03-18T20:00:00Z" "HKG" "FRA" 30 4 1
add_flight "QR" "01604" "SAA" "SP-LLL" "2025-03-19T08:00:00Z" "2025-03-19T18:00:00Z" "DOH" "CDG" 35 5 1

# Africa to Europe
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "AFRICA → EUROPE"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "QF" "01705" "TAA" "SP-LPP" "2025-03-20T09:00:00Z" "2025-03-20T17:00:00Z" "ADD" "FCO" 70 6 1
add_flight "EK" "01806" "UAA" "SP-LQQ" "2025-03-21T11:00:00Z" "2025-03-21T19:00:00Z" "JNB" "LHR" 90 7 1

# Europe to Asia (Additional)
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "EUROPE → ASIA (Additional)"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "QF" "01907" "VAA" "SP-LOO" "2025-03-22T07:00:00Z" "2025-03-22T17:00:00Z" "LHR" "NTR" 25 1 1
add_flight "AA" "02008" "WAA" "SP-LPP" "2025-03-23T08:30:00Z" "2025-03-23T18:30:00Z" "CDG" "PEK" 30 2 1

# Australia to Europe
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "AUSTRALIA → EUROPE"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
add_flight "QF" "02109" "XAA" "SP-LQQ" "2025-03-24T14:00:00Z" "2025-03-24T23:00:00Z" "SYD" "FRA" 50 3 1
add_flight "VA" "02210" "YAA" "SP-LRR" "2025-03-25T13:00:00Z" "2025-03-25T22:00:00Z" "MEL" "LHR" 40 4 1

echo "=========================================="
echo "✓ Finished adding flight schedules"
echo "=========================================="
