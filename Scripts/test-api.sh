#!/bin/bash

# Test API script for macOS/Linux
# Usage: ./test-api.sh [host] [port]

SERVER=${1:-localhost}
PORT=${2:-5000}
BASE_URL="http://${SERVER}:${PORT}/api"

echo "=========================================="
echo "Testing FlyTicketService API"
echo "Base URL: $BASE_URL"
echo "=========================================="
echo ""

# Colors for output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to test endpoint
test_endpoint() {
    local method=$1
    local endpoint=$2
    local description=$3
    local data=$4
    
    echo -e "${YELLOW}Testing:${NC} $description"
    echo "Endpoint: $method $endpoint"
    
    if [ -z "$data" ]; then
        response=$(curl -s -w "\n%{http_code}" -X $method "$endpoint" \
            -H "Content-Type: application/json" 2>&1)
    else
        response=$(curl -s -w "\n%{http_code}" -X $method "$endpoint" \
            -H "Content-Type: application/json" \
            -d "$data" 2>&1)
    fi
    
    http_code=$(echo "$response" | tail -n1)
    body=$(echo "$response" | head -n-1)
    
    if [ "$http_code" -ge 200 ] && [ "$http_code" -lt 300 ]; then
        echo -e "${GREEN}✓ Success${NC} (HTTP $http_code)"
        echo "Response: ${body:0:200}..."
    else
        echo -e "${RED}✗ Failed${NC} (HTTP $http_code)"
        echo "Error: $body"
    fi
    echo ""
}

# Test 1: Get all tenants
test_endpoint "GET" "${BASE_URL}/Tenant" "Get all tenants"

# Test 2: Get all flight schedules
test_endpoint "GET" "${BASE_URL}/FlightSchedule" "Get all flight schedules"

# Test 3: Get all discounts
test_endpoint "GET" "${BASE_URL}/DiscountType/all-discounts" "Get all discounts"

# Test 4: Add a tenant
TENANT_DATA='{
  "name": "Test User",
  "address": "123 Test Street",
  "group": "GroupA",
  "birthDate": "1990-01-01",
  "phone": "123-456-7890",
  "email": "test@example.com"
}'
test_endpoint "POST" "${BASE_URL}/Tenant" "Add new tenant" "$TENANT_DATA"

# Test 5: Add a discount
DISCOUNT_DATA='{
  "name": "Test Discount",
  "value": 10,
  "description": "Test discount for testing"
}'
test_endpoint "POST" "${BASE_URL}/DiscountType" "Add new discount" "$DISCOUNT_DATA"

echo "=========================================="
echo "API Testing Complete"
echo "=========================================="
