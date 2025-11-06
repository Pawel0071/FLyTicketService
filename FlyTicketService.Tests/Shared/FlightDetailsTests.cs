using FluentAssertions;
using FLyTicketService.Model;
using FLyTicketService.Model.Enums;
using FLyTicketService.Shared;
using Xunit;

namespace FlyTicketService.Tests.Shared
{
    public class FlightDetailsTests
    {
        [Fact]
        public void Constructor_WithAllParameters_CreatesFlightDetails()
        {
            // Arrange
            var airline = new Airline
            {
                AirlineId = Guid.NewGuid(),
                AirlineName = "Test Airlines",
                IATA = "TA",
                Country = "Test Country"
            };

            var aircraft = new Aircraft
            {
                AircraftId = Guid.NewGuid(),
                Model = "Boeing 737",
                RegistrationNumber = "TEST123"
            };

            var origin = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Test Origin Airport",
                City = "Origin City",
                Country = "Origin Country",
                IATA = "ORI",
                ICAO = "ORIG",
                Timezone = SimplyTimeZone.UTC,
                Continent = "Europe"
            };

            var destination = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Test Destination Airport",
                City = "Destination City",
                Country = "Destination Country",
                IATA = "DST",
                ICAO = "DEST",
                Timezone = SimplyTimeZone.CET,
                Continent = "Europe"
            };

            // Act
            var flightDetails = new FlightDetails(airline, aircraft, origin, destination);

            // Assert
            flightDetails.Airline.Should().Be(airline);
            flightDetails.Aircraft.Should().Be(aircraft);
            flightDetails.Origin.Should().Be(origin);
            flightDetails.Destination.Should().Be(destination);
        }

        [Fact]
        public void Constructor_WithNullValues_CreatesFlightDetailsWithNulls()
        {
            // Act
            var flightDetails = new FlightDetails(null, null, null, null);

            // Assert
            flightDetails.Airline.Should().BeNull();
            flightDetails.Aircraft.Should().BeNull();
            flightDetails.Origin.Should().BeNull();
            flightDetails.Destination.Should().BeNull();
        }

        [Fact]
        public void Record_Equality_WithSameValues_AreEqual()
        {
            // Arrange
            var airline = new Airline
            {
                AirlineId = Guid.NewGuid(),
                AirlineName = "Test Airlines",
                IATA = "TA",
                Country = "Test Country"
            };

            var aircraft = new Aircraft
            {
                AircraftId = Guid.NewGuid(),
                Model = "Boeing 737",
                RegistrationNumber = "TEST123"
            };

            var origin = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Test Airport",
                City = "Test City",
                Country = "Test Country",
                IATA = "TST",
                ICAO = "TEST",
                Timezone = SimplyTimeZone.UTC,
                Continent = "Europe"
            };

            var destination = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Test Destination",
                City = "Dest City",
                Country = "Dest Country",
                IATA = "DST",
                ICAO = "DEST",
                Timezone = SimplyTimeZone.CET,
                Continent = "Europe"
            };

            var flightDetails1 = new FlightDetails(airline, aircraft, origin, destination);
            var flightDetails2 = new FlightDetails(airline, aircraft, origin, destination);

            // Act & Assert
            flightDetails1.Should().Be(flightDetails2);
            flightDetails1.GetHashCode().Should().Be(flightDetails2.GetHashCode());
        }

        [Fact]
        public void Record_Equality_WithDifferentAirline_AreNotEqual()
        {
            // Arrange
            var airline1 = new Airline
            {
                AirlineId = Guid.NewGuid(),
                AirlineName = "Airline 1",
                IATA = "A1",
                Country = "Country 1"
            };

            var airline2 = new Airline
            {
                AirlineId = Guid.NewGuid(),
                AirlineName = "Airline 2",
                IATA = "A2",
                Country = "Country 2"
            };

            var aircraft = new Aircraft
            {
                AircraftId = Guid.NewGuid(),
                Model = "Boeing 737",
                RegistrationNumber = "TEST123"
            };

            var airport = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Test Airport",
                City = "Test City",
                Country = "Test Country",
                IATA = "TST",
                ICAO = "TEST",
                Timezone = SimplyTimeZone.UTC,
                Continent = "Europe"
            };

            var flightDetails1 = new FlightDetails(airline1, aircraft, airport, airport);
            var flightDetails2 = new FlightDetails(airline2, aircraft, airport, airport);

            // Act & Assert
            flightDetails1.Should().NotBe(flightDetails2);
        }

        [Fact]
        public void Record_WithExpression_CreatesModifiedCopy()
        {
            // Arrange
            var airline1 = new Airline
            {
                AirlineId = Guid.NewGuid(),
                AirlineName = "Original Airline",
                IATA = "OA",
                Country = "Original Country"
            };

            var airline2 = new Airline
            {
                AirlineId = Guid.NewGuid(),
                AirlineName = "New Airline",
                IATA = "NA",
                Country = "New Country"
            };

            var aircraft = new Aircraft
            {
                AircraftId = Guid.NewGuid(),
                Model = "Airbus A320",
                RegistrationNumber = "NEW456"
            };

            var airport = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Test Airport",
                City = "Test City",
                Country = "Test Country",
                IATA = "TST",
                ICAO = "TEST",
                Timezone = SimplyTimeZone.UTC,
                Continent = "Europe"
            };

            var original = new FlightDetails(airline1, aircraft, airport, airport);

            // Act
            var modified = original with { Airline = airline2 };

            // Assert
            modified.Airline.Should().Be(airline2);
            modified.Aircraft.Should().Be(aircraft);
            modified.Origin.Should().Be(airport);
            modified.Destination.Should().Be(airport);
            original.Airline.Should().Be(airline1); // Original unchanged
        }

        [Fact]
        public void Deconstruct_ReturnsAllProperties()
        {
            // Arrange
            var airline = new Airline
            {
                AirlineId = Guid.NewGuid(),
                AirlineName = "Test Airlines",
                IATA = "TA",
                Country = "Test Country"
            };

            var aircraft = new Aircraft
            {
                AircraftId = Guid.NewGuid(),
                Model = "Boeing 747",
                RegistrationNumber = "TEST789"
            };

            var origin = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Origin Airport",
                City = "Origin City",
                Country = "Origin Country",
                IATA = "ORI",
                ICAO = "ORIG",
                Timezone = SimplyTimeZone.EST,
                Continent = "North America"
            };

            var destination = new Airport
            {
                AirportId = Guid.NewGuid(),
                AirportName = "Destination Airport",
                City = "Dest City",
                Country = "Dest Country",
                IATA = "DST",
                ICAO = "DEST",
                Timezone = SimplyTimeZone.PST,
                Continent = "North America"
            };

            var flightDetails = new FlightDetails(airline, aircraft, origin, destination);

            // Act
            var (airlineResult, aircraftResult, originResult, destinationResult) = flightDetails;

            // Assert
            airlineResult.Should().Be(airline);
            aircraftResult.Should().Be(aircraft);
            originResult.Should().Be(origin);
            destinationResult.Should().Be(destination);
        }
    }
}
