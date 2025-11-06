#!/bin/bash

# Tickets.sh - Test ticket operations for FlyTicketService
# Usage: ./Tickets.sh [host] [port]

SERVER=${1:-localhost}
PORT=${2:-5000}
BASE_URL="http://${SERVER}:${PORT}/api"
TICKET_URL="${BASE_URL}/Ticket"
TENANT_URL="${BASE_URL}/Tenant"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

echo "=========================================="
echo "FlyTicketService - Ticket Operations Test"
echo "Base URL: $BASE_URL"
echo "=========================================="
echo ""

# Function to reserve a ticket
reserve_ticket() {
    local flight_id=$1
    local seat_no=$2
    local tenant_id=$3
    
    # URL encode spaces in flight_id
    local encoded_flight_id=$(echo "$flight_id" | sed 's/ /%20/g')
    
    echo -e "${YELLOW}Reserving:${NC} Flight '$flight_id', Seat '$seat_no', Tenant: $tenant_id" >&2
    
    response=$(curl -s -w "\n%{http_code}" -X POST \
        "${TICKET_URL}/reserve?flightId=${encoded_flight_id}&seatNo=${seat_no}&tenantId=${tenant_id}" \
        -H "Content-Type: application/json" 2>&1)
    
    http_code=$(echo "$response" | tail -1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Reserved${NC}" >&2
        if command -v jq &> /dev/null; then
            echo "$body" | jq '.' >&2
        else
            echo "$body" >&2
        fi
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)" >&2
        echo "Response: $body" >&2
    fi
    echo "" >&2
    echo "$body"
}

# Function to sell a ticket
sell_ticket() {
    local flight_id=$1
    local seat_no=$2
    local tenant_id=$3
    local discounts=$4
    
    # URL encode spaces in flight_id
    local encoded_flight_id=$(echo "$flight_id" | sed 's/ /%20/g')
    
    echo -e "${YELLOW}Selling:${NC} Flight '$flight_id', Seat '$seat_no', Tenant: $tenant_id" >&2
    
    response=$(curl -s -w "\n%{http_code}" -X POST \
        "${TICKET_URL}/sell?flightId=${encoded_flight_id}&seatNo=${seat_no}&tenantId=${tenant_id}" \
        -H "Content-Type: application/json" \
        -d "$discounts" 2>&1)
    
    http_code=$(echo "$response" | tail -1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Sold${NC}" >&2
        if command -v jq &> /dev/null; then
            echo "$body" | jq '.' >&2
        else
            echo "$body" >&2
        fi
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)" >&2
        echo "Response: $body" >&2
    fi
    echo "" >&2
    echo "$body"
}

# Function to sell a reserved ticket
sell_reserved_ticket() {
    local ticket_number=$1
    
    echo -e "${YELLOW}Selling reserved ticket:${NC} $ticket_number" >&2
    
    response=$(curl -s -w "\n%{http_code}" -X POST \
        "${TICKET_URL}/sell-reserved?ticketNumber=${ticket_number}" \
        -H "Content-Type: application/json" 2>&1)
    
    http_code=$(echo "$response" | tail -1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Sold reserved ticket${NC}" >&2
        if command -v jq &> /dev/null; then
            echo "$body" | jq '.' >&2
        else
            echo "$body" >&2
        fi
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)" >&2
        echo "Response: $body" >&2
    fi
    echo "" >&2
    echo "$body"
}

# Function to cancel a ticket
cancel_ticket() {
    local ticket_number=$1
    
    echo -e "${YELLOW}Canceling ticket:${NC} $ticket_number" >&2
    
    response=$(curl -s -w "\n%{http_code}" -X DELETE \
        "${TICKET_URL}/${ticket_number}" \
        -H "Content-Type: application/json" 2>&1)
    
    http_code=$(echo "$response" | tail -1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Canceled${NC}" >&2
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)" >&2
        echo "Response: $body" >&2
    fi
    echo "" >&2
}

# Function to get ticket details
get_ticket() {
    local ticket_number=$1
    
    echo -e "${YELLOW}Getting ticket:${NC} $ticket_number" >&2
    
    response=$(curl -s -w "\n%{http_code}" -X GET \
        "${TICKET_URL}/${ticket_number}" \
        -H "Content-Type: application/json" 2>&1)
    
    http_code=$(echo "$response" | tail -1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Retrieved${NC}" >&2
        if command -v jq &> /dev/null; then
            echo "$body" | jq '.' >&2
        else
            echo "$body" >&2
        fi
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)" >&2
        echo "Response: $body" >&2
    fi
    echo "" >&2
    echo "$body"
}

# Function to apply discount
apply_discount() {
    local ticket_number=$1
    local discounts=$2
    
    echo -e "${YELLOW}Applying discounts to:${NC} $ticket_number" >&2
    
    response=$(curl -s -w "\n%{http_code}" -X POST \
        "${TICKET_URL}/apply-discount?ticketNumber=${ticket_number}" \
        -H "Content-Type: application/json" \
        -d "$discounts" 2>&1)
    
    http_code=$(echo "$response" | tail -1)
    body=$(echo "$response" | sed '$d')
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Discounts applied${NC}" >&2
        if command -v jq &> /dev/null; then
            echo "$body" | jq '.' >&2
        else
            echo "$body" >&2
        fi
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)" >&2
        echo "Response: $body" >&2
    fi
    echo "" >&2
}

# Get tenants
echo -e "${BLUE}Getting tenants...${NC}"
tenants_response=$(curl -s "${TENANT_URL}")
if command -v jq &> /dev/null; then
    echo "$tenants_response" | jq '.[0:5]'  # Show first 5
else
    echo "$tenants_response"
fi
echo ""

# Get all discounts
echo -e "${BLUE}Getting all discounts...${NC}"
discounts_response=$(curl -s "${TICKET_URL}/all-discounts")
if command -v jq &> /dev/null; then
    echo "$discounts_response" | jq '.'
else
    echo "$discounts_response"
fi
echo ""

# Extract tenant IDs (using jq if available)
if command -v jq &> /dev/null; then
    john_doe=$(echo "$tenants_response" | jq -r '.[] | select(.name=="John Doe") | .tenantId')
    jane_smith=$(echo "$tenants_response" | jq -r '.[] | select(.name=="Jane Smith") | .tenantId')
    alice_brown=$(echo "$tenants_response" | jq -r '.[] | select(.name=="Alice Brown") | .tenantId')
    bob_johnson=$(echo "$tenants_response" | jq -r '.[] | select(.name=="Bob Johnson") | .tenantId')
    liam_wilson=$(echo "$tenants_response" | jq -r '.[] | select(.name=="Liam Wilson") | .tenantId')
    william_anderson=$(echo "$tenants_response" | jq -r '.[] | select(.name=="William Anderson") | .tenantId')
    mia_martin=$(echo "$tenants_response" | jq -r '.[] | select(.name=="Mia Martin") | .tenantId')
    amelia_garcia=$(echo "$tenants_response" | jq -r '.[] | select(.name=="Amelia Garcia") | .tenantId')
    
    # Extract discount IDs
    birthday_discount=$(echo "$discounts_response" | jq -c '[.[] | select(.name=="Birthday Discount")]')
    thursday_africa=$(echo "$discounts_response" | jq -c '[.[] | select(.name=="Thursday Africa Discount")]')
    all_discounts=$(echo "$discounts_response" | jq -c '.')
else
    echo -e "${RED}Warning: jq not installed. Using first tenant ID for all operations.${NC}"
    john_doe=$(echo "$tenants_response" | grep -o '"tenantId":"[^"]*"' | head -n1 | cut -d'"' -f4)
    jane_smith=$john_doe
    alice_brown=$john_doe
    bob_johnson=$john_doe
    liam_wilson=$john_doe
    william_anderson=$john_doe
    mia_martin=$john_doe
    amelia_garcia=$john_doe
    birthday_discount='[]'
    thursday_africa='[]'
    all_discounts='[]'
fi

echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "TICKET OPERATIONS - TEST SCENARIOS"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

# Example 1: Reserve ticket
echo "Example 1: Reserve ticket"
ticket1=$(reserve_ticket "AA 00101 AAA" "7A" "$john_doe")
ticket1_number=$(echo "$ticket1" | jq -r '.ticketNumber' 2>/dev/null || echo "")

# Example 2: Reserve another ticket
echo "Example 2: Reserve another ticket"
ticket2=$(reserve_ticket "EK 00702 GAA" "2B" "$jane_smith")
ticket2_number=$(echo "$ticket2" | jq -r '.ticketNumber' 2>/dev/null || echo "")

# Example 3: Reserve ticket for another flight
echo "Example 3: Reserve ticket for another flight"
ticket3=$(reserve_ticket "LO 00803 HAA" "4C" "$alice_brown")
ticket3_number=$(echo "$ticket3" | jq -r '.ticketNumber' 2>/dev/null || echo "")

# Example 4: Sell ticket after reservation (with birthday discount)
echo "Example 4: Sell ticket with birthday discount"
ticket4=$(sell_ticket "DL 00303 CAA" "1A" "$bob_johnson" "$birthday_discount")

# Example 5: Sell ticket without reservation (with Thursday Africa discount)
echo "Example 5: Sell ticket with Thursday Africa discount"
ticket5=$(sell_ticket "EK 00202 BAA" "3C" "$liam_wilson" "$thursday_africa")

# Example 6: Sell reserved ticket
if [ -n "$ticket2_number" ] && [ "$ticket2_number" != "null" ]; then
    echo "Example 6: Sell reserved ticket"
    ticket6=$(sell_reserved_ticket "$ticket2_number")
fi

# Example 7: Cancel ticket
if [ -n "$ticket1_number" ] && [ "$ticket1_number" != "null" ]; then
    echo "Example 7: Cancel ticket"
    cancel_ticket "$ticket1_number"
fi

# Example 8: Get ticket details
if [ -n "$ticket3_number" ] && [ "$ticket3_number" != "null" ]; then
    echo "Example 8: Get ticket details"
    ticket7=$(get_ticket "$ticket3_number")
fi

# Example 9: Reserve ticket for another flight
echo "Example 9: Reserve ticket for another flight"
ticket8=$(reserve_ticket "QF 00404 DAA" "2C" "$mia_martin")

# Example 10: Sell ticket with birthday discount
echo "Example 10: Sell ticket for another flight"
ticket9=$(sell_ticket "AA 00904 IAA" "3D" "$amelia_garcia" "$birthday_discount")

# Example 11: Apply discounts to ticket
if [ -n "$ticket3_number" ] && [ "$ticket3_number" != "null" ]; then
    echo "Example 11: Apply all discounts to ticket"
    apply_discount "$ticket3_number" "$all_discounts"
fi

echo "=========================================="
echo "✓ Finished ticket operations test"
echo "=========================================="
