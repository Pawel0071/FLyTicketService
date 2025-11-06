#!/bin/bash

# GetData.sh - Fetch all data from FlyTicketService API
# Usage: ./GetData.sh [host] [port]

SERVER=${1:-localhost}
PORT=${2:-5000}
BASE_URL="http://${SERVER}:${PORT}/api"

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo "=========================================="
echo "FlyTicketService - Get All Data"
echo "Base URL: $BASE_URL"
echo "=========================================="
echo ""

# Function to make API calls
api_get() {
    local endpoint=$1
    local description=$2
    
    echo -e "${YELLOW}Fetching:${NC} $description"
    echo "Endpoint: GET $endpoint"
    
    response=$(curl -s -X GET "$endpoint" \
        -H "Content-Type: application/json" 2>&1)
    
    if [ $? -eq 0 ]; then
        echo -e "${GREEN}✓ Success${NC}"
        echo "$response" | jq '.' 2>/dev/null || echo "$response"
    else
        echo "✗ Failed"
        echo "Error: $response"
    fi
    echo ""
}

# Get Tenants
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "TENANTS"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
api_get "${BASE_URL}/Tenant" "All Tenants"

# Get Flight Schedules
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "FLIGHT SCHEDULES"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
api_get "${BASE_URL}/FlightSchedule" "All Flight Schedules"

# Get All Discounts
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "DISCOUNTS"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
api_get "${BASE_URL}/Ticket/all-discounts" "All Available Discounts"

echo "=========================================="
echo "Data Fetch Complete"
echo "=========================================="
