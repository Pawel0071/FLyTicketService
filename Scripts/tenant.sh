#!/bin/bash

# tenant.sh - Add sample tenants to FlyTicketService
# Usage: ./tenant.sh [host] [port]

SERVER=${1:-localhost}
PORT=${2:-5042}
BASE_URL="http://${SERVER}:${PORT}/api"
TENANT_URL="${BASE_URL}/Tenant"

# Colors
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo "=========================================="
echo "FlyTicketService - Add Sample Tenants"
echo "Base URL: $BASE_URL"
echo "=========================================="
echo ""

# Function to add tenant
add_tenant() {
    local name=$1
    local address=$2
    local group=$3
    local birthday=$4
    local phone=$5
    local email=$6
    
    echo -e "${YELLOW}Adding tenant:${NC} $name"
    
    json_data=$(cat <<EOF
{
  "Name": "$name",
  "Address": "$address",
  "Group": "$group",
  "Birthday": "$birthday",
  "Phone": "$phone",
  "Email": "$email"
}
EOF
)
    
    response=$(curl -s -w "\n%{http_code}" -X POST "$TENANT_URL" \
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

# Add 30 sample tenants
echo "Adding 30 sample tenants..."
echo ""

add_tenant "John Doe" "123 Elm Street" "GroupA" "1985-06-15" "123-456-7890" "john.doe@example.com"
add_tenant "Jane Smith" "456 Maple Avenue" "GroupB" "1990-03-22" "987-654-3210" "jane.smith@example.com"
add_tenant "Alice Brown" "789 Oak Boulevard" "GroupA" "1978-11-07" "555-123-4567" "alice.brown@example.com"
add_tenant "Bob Johnson" "321 Pine Road" "GroupB" "1982-04-19" "444-789-1234" "bob.johnson@example.com"
add_tenant "Liam Wilson" "12 Baker Street" "GroupA" "1988-01-12" "111-222-3333" "liam.wilson@example.com"
add_tenant "Emma Davis" "34 King Street" "GroupB" "1992-02-23" "222-333-4444" "emma.davis@example.com"
add_tenant "Noah Miller" "56 Queen Street" "GroupA" "1980-03-14" "333-444-5555" "noah.miller@example.com"
add_tenant "Olivia Taylor" "78 Prince Street" "GroupB" "1985-04-25" "444-555-6666" "olivia.taylor@example.com"
add_tenant "William Anderson" "90 Duke Street" "GroupA" "1990-05-16" "555-666-7777" "william.anderson@example.com"
add_tenant "Sophia Thomas" "12 Duchess Street" "GroupB" "1982-06-27" "666-777-8888" "sophia.thomas@example.com"
add_tenant "James Jackson" "34 Earl Street" "GroupA" "1987-07-18" "777-888-9999" "james.jackson@example.com"
add_tenant "Isabella White" "56 Baron Street" "GroupB" "1993-08-29" "888-999-0000" "isabella.white@example.com"
add_tenant "Benjamin Harris" "78 Viscount Street" "GroupA" "1981-09-10" "999-000-1111" "benjamin.harris@example.com"
add_tenant "Mia Martin" "90 Marquess Street" "GroupB" "1986-10-21" "000-111-2222" "mia.martin@example.com"
add_tenant "Lucas Thompson" "12 Count Street" "GroupA" "1991-11-12" "111-222-3333" "lucas.thompson@example.com"
add_tenant "Amelia Garcia" "34 Baroness Street" "GroupB" "1983-12-23" "222-333-4444" "amelia.garcia@example.com"
add_tenant "Henry Martinez" "56 Viscountess Street" "GroupA" "1989-01-14" "333-444-5555" "henry.martinez@example.com"
add_tenant "Evelyn Robinson" "78 Countess Street" "GroupB" "1994-02-25" "444-555-6666" "evelyn.robinson@example.com"
add_tenant "Alexander Clark" "90 Earl Street" "GroupA" "1984-03-16" "555-666-7777" "alexander.clark@example.com"
add_tenant "Harper Rodriguez" "12 Duke Street" "GroupB" "1995-04-27" "666-777-8888" "harper.rodriguez@example.com"
add_tenant "Daniel Lewis" "34 Prince Street" "GroupA" "1986-05-18" "777-888-9999" "daniel.lewis@example.com"
add_tenant "Ella Lee" "56 Queen Street" "GroupB" "1996-06-29" "888-999-0000" "ella.lee@example.com"
add_tenant "Matthew Walker" "78 King Street" "GroupA" "1987-07-10" "999-000-1111" "matthew.walker@example.com"
add_tenant "Avery Hall" "90 Baker Street" "GroupB" "1997-08-21" "000-111-2222" "avery.hall@example.com"
add_tenant "David Allen" "12 Elm Street" "GroupA" "1988-09-12" "111-222-3333" "david.allen@example.com"
add_tenant "Scarlett Young" "34 Maple Avenue" "GroupB" "1998-10-23" "222-333-4444" "scarlett.young@example.com"
add_tenant "Joseph Hernandez" "56 Oak Boulevard" "GroupA" "1989-11-14" "333-444-5555" "joseph.hernandez@example.com"
add_tenant "Grace King" "78 Pine Road" "GroupB" "1999-12-25" "444-555-6666" "grace.king@example.com"
add_tenant "Samuel Wright" "90 Birch Street" "GroupA" "1990-01-16" "555-666-7777" "samuel.wright@example.com"
add_tenant "Victoria Scott" "12 Cedar Street" "GroupB" "2000-02-27" "666-777-8888" "victoria.scott@example.com"

echo "=========================================="
echo "✓ Finished adding 30 tenants"
echo "=========================================="
