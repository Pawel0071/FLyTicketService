#!/bin/bash

# Test concurrent reservations - symulacja dwóch osób rezerwujących to samo miejsce jednocześnie

BASE_URL="http://localhost:5042/api/Ticket"

# Kolory
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}=========================================="
echo "Testing Concurrent Reservations"
echo -e "==========================================${NC}"

# Pobierz ID dwóch różnych tenantów
echo -e "${YELLOW}Getting tenant IDs...${NC}"
tenants=$(curl -s http://localhost:5042/api/Tenant)
tenant1=$(echo "$tenants" | jq -r '.[0].tenantId')
tenant2=$(echo "$tenants" | jq -r '.[1].tenantId')

echo "Tenant 1: $tenant1"
echo "Tenant 2: $tenant2"

# Test 1: Dwie osoby próbują zarezerwować to samo miejsce jednocześnie
echo -e "\n${BLUE}Test 1: Two users trying to reserve the same seat simultaneously${NC}"
echo "Flight: AA 00101 AAA, Seat: 1A"
echo "User 1 (Tenant: $tenant1) and User 2 (Tenant: $tenant2)"

# Uruchom oba requesty równolegle w tle
(
  response=$(curl -s -w "\n%{http_code}" -X POST "${BASE_URL}/reserve?flightId=AA%2000101%20AAA&seatNo=1A&tenantId=$tenant1")
  http_code=$(echo "$response" | tail -n1)
  body=$(echo "$response" | sed '$d')
  
  if [ "$http_code" = "200" ] || [ "$http_code" = "201" ]; then
    echo -e "${GREEN}✓ User 1: SUCCESS (HTTP $http_code)${NC}"
    echo "$body" | jq -r '.data.ticketNumber' > /tmp/user1_ticket.txt
  elif [ "$http_code" = "409" ]; then
    echo -e "${YELLOW}✗ User 1: CONFLICT (HTTP $http_code) - Seat already reserved${NC}"
  else
    echo -e "${RED}✗ User 1: FAILED (HTTP $http_code)${NC}"
    echo "$body"
  fi
) &

(
  response=$(curl -s -w "\n%{http_code}" -X POST "${BASE_URL}/reserve?flightId=AA%2000101%20AAA&seatNo=1A&tenantId=$tenant2")
  http_code=$(echo "$response" | tail -n1)
  body=$(echo "$response" | sed '$d')
  
  if [ "$http_code" = "200" ] || [ "$http_code" = "201" ]; then
    echo -e "${GREEN}✓ User 2: SUCCESS (HTTP $http_code)${NC}"
    echo "$body" | jq -r '.data.ticketNumber' > /tmp/user2_ticket.txt
  elif [ "$http_code" = "409" ]; then
    echo -e "${YELLOW}✗ User 2: CONFLICT (HTTP $http_code) - Seat already reserved${NC}"
  else
    echo -e "${RED}✗ User 2: FAILED (HTTP $http_code)${NC}"
    echo "$body"
  fi
) &

# Czekaj aż oba requesty się zakończą
wait

echo ""
echo -e "${BLUE}Expected Result:${NC} Only ONE user should successfully reserve the seat"
echo -e "${BLUE}The other should get HTTP 409 Conflict${NC}"

# Test 2: Sprawdź czy miejsce jest faktycznie zarezerwowane tylko raz
echo -e "\n${BLUE}Test 2: Verify seat is reserved only once${NC}"
sleep 1

tickets=$(curl -s "${BASE_URL}")
seat_1a_tickets=$(echo "$tickets" | jq '[.[] | select(.flightId == "AA 00101 AAA" and .seatNumber == "1A")]')
count=$(echo "$seat_1a_tickets" | jq 'length')

echo "Tickets for AA 00101 AAA, Seat 1A: $count"

if [ "$count" = "1" ]; then
  echo -e "${GREEN}✓ PASS: Exactly one ticket created (concurrency control working!)${NC}"
  ticket_owner=$(echo "$seat_1a_tickets" | jq -r '.[0].tenant')
  echo "  Reserved by: $ticket_owner"
elif [ "$count" = "0" ]; then
  echo -e "${RED}✗ FAIL: No tickets created${NC}"
else
  echo -e "${RED}✗ FAIL: $count tickets created (should be only 1 - RACE CONDITION!)${NC}"
  echo "$seat_1a_tickets" | jq '.'
fi

# Test 3: Próba sprzedaży tego samego zarezerwowanego biletu przez dwie osoby
echo -e "\n${BLUE}Test 3: Two users trying to sell the same reserved ticket${NC}"

# Pobierz numer biletu zarezerwowanego
reserved_ticket=$(echo "$seat_1a_tickets" | jq -r '.[0].ticketNumber')
echo "Reserved ticket: $reserved_ticket"

if [ -n "$reserved_ticket" ] && [ "$reserved_ticket" != "null" ]; then
  # Dwie osoby próbują sprzedać ten sam bilet
  (
    response=$(curl -s -w "\n%{http_code}" -X PUT "${BASE_URL}/${reserved_ticket}/sell")
    http_code=$(echo "$response" | tail -n1)
    
    if [ "$http_code" = "200" ]; then
      echo -e "${GREEN}✓ Sell attempt 1: SUCCESS (HTTP $http_code)${NC}"
    elif [ "$http_code" = "409" ]; then
      echo -e "${YELLOW}✗ Sell attempt 1: CONFLICT (HTTP $http_code)${NC}"
    else
      echo -e "${RED}✗ Sell attempt 1: FAILED (HTTP $http_code)${NC}"
    fi
  ) &

  (
    response=$(curl -s -w "\n%{http_code}" -X PUT "${BASE_URL}/${reserved_ticket}/sell")
    http_code=$(echo "$response" | tail -n1)
    
    if [ "$http_code" = "200" ]; then
      echo -e "${GREEN}✓ Sell attempt 2: SUCCESS (HTTP $http_code)${NC}"
    elif [ "$http_code" = "409" ]; then
      echo -e "${YELLOW}✗ Sell attempt 2: CONFLICT (HTTP $http_code)${NC}"
    else
      echo -e "${RED}✗ Sell attempt 2: FAILED (HTTP $http_code)${NC}"
    fi
  ) &

  wait
  
  echo ""
  echo -e "${BLUE}Expected Result:${NC} Only ONE sell should succeed"
fi

echo -e "\n${GREEN}=========================================="
echo "Concurrent Reservation Test Complete"
echo -e "==========================================${NC}"
