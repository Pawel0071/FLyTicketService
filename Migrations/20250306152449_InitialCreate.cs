using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FLyTicketService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aircrafts",
                columns: table => new
                {
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.AircraftId);
                });

            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IATA = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    AirlineName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    AirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AirportName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IATA = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ICAO = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Altitude = table.Column<double>(type: "float", nullable: false),
                    Timezone = table.Column<int>(type: "int", nullable: false),
                    DST = table.Column<int>(type: "int", nullable: false),
                    Continent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.AirportId);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Group = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "AircraftSeats",
                columns: table => new
                {
                    AircraftSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false),
                    OutOfService = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AircraftSeats", x => x.AircraftSeatId);
                    table.ForeignKey(
                        name: "FK_AircraftSeats_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "AircraftId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FlightSchedules",
                columns: table => new
                {
                    FlightScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AirlineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AircraftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FlightId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Departure = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Arrival = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    OriginAirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationAirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSchedules", x => x.FlightScheduleId);
                    table.ForeignKey(
                        name: "FK_FlightSchedules_Aircrafts_AircraftId",
                        column: x => x.AircraftId,
                        principalTable: "Aircrafts",
                        principalColumn: "AircraftId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightSchedules_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightSchedules_Airports_DestinationAirportId",
                        column: x => x.DestinationAirportId,
                        principalTable: "Airports",
                        principalColumn: "AirportId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightSchedules_Airports_OriginAirportId",
                        column: x => x.OriginAirportId,
                        principalTable: "Airports",
                        principalColumn: "AirportId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK_Discounts_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "FlightSeats",
                columns: table => new
                {
                    FlightSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FlightScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Locked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightSeats", x => x.FlightSeatId);
                    table.ForeignKey(
                        name: "FK_FlightSeats_FlightSchedules_FlightScheduleId",
                        column: x => x.FlightScheduleId,
                        principalTable: "FlightSchedules",
                        principalColumn: "FlightScheduleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightSeats_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "TicketId");
                });

            migrationBuilder.CreateTable(
                name: "Conditions",
                columns: table => new
                {
                    ConditionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Property = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ConditionType = table.Column<int>(type: "int", nullable: false),
                    ConditionValue = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conditions", x => x.ConditionId);
                    table.ForeignKey(
                        name: "FK_Conditions_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "DiscountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Aircrafts",
                columns: new[] { "AircraftId", "Model", "RegistrationNumber" },
                values: new object[,]
                {
                    { new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), "Boeing 747-8", "SP-LQQ" },
                    { new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), "Boeing 777-300ER", "SP-LLA" },
                    { new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), "Boeing 787-9", "SP-LGG" },
                    { new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), "Airbus A380", "SP-LKK" },
                    { new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), "Airbus A320Neo", "SP-LBG" },
                    { new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), "Boeing 737-MAX", "SP-LAA" },
                    { new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), "Boeing 787-9", "SP-LCC" },
                    { new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), "Boeing 737 MAX", "SP-LRA" },
                    { new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), "Airbus A380", "SP-LLL" },
                    { new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), "Airbus A320", "SP-LOO" },
                    { new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), "Boeing 777-300ER", "SP-LRR" },
                    { new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), "Boeing 767", "SP-LPP" }
                });

            migrationBuilder.InsertData(
                table: "Airlines",
                columns: new[] { "AirlineId", "AirlineName", "Country", "IATA" },
                values: new object[,]
                {
                    { new Guid("170cabf2-3f5c-405c-8cb1-404221a89ce5"), "Virgin Australia", "Australia", "VA" },
                    { new Guid("1debf6bf-7ffe-4940-9f9f-1453723c8be0"), "Delta Airlines", "USA", "DL" },
                    { new Guid("2a9f9251-4b04-4b4b-8cea-ddef0b7c4a3d"), "American Airlines", "USA", "AA" },
                    { new Guid("4b75bb34-292c-483c-ac54-ecff52e2e8c7"), "Emirates", "UAE", "EK" },
                    { new Guid("6ab28b27-803f-42d4-b888-c69f5aae6b26"), "Singapore Airlines", "Singapore", "SQ" },
                    { new Guid("79daabab-2cd1-4eaa-be7e-c5440701bec0"), "Cathay Pacific", "Hong Kong", "CX" },
                    { new Guid("abf220db-9971-4c89-bebe-67464b33d677"), "Qantas", "Australia", "QF" },
                    { new Guid("b24c296f-11eb-4962-9a17-15f4c7877007"), "Air India", "India", "AI" },
                    { new Guid("c808fb02-9a6c-4b9e-a7e5-ea375417990c"), "Qatar Airways", "Qatar", "QR" },
                    { new Guid("e0008ad3-7e18-46ea-9366-ab65f5532517"), "LOT", "Poland", "LO" }
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "AirportId", "AirportName", "Altitude", "City", "Continent", "Country", "DST", "IATA", "ICAO", "Latitude", "Longitude", "Timezone" },
                values: new object[,]
                {
                    { new Guid("0d8080f8-eb5e-4693-a23c-f365f86c93c9"), "Hartsfield-Jackson Atlanta International Airport", 1026.0, "Atlanta", "North America", "United States", 1, "ATL", "KATL", 33.636699999999998, -84.428100000000001, 5 },
                    { new Guid("19ade48f-9b18-4025-af9a-1e8cb1ebcdf0"), "Leonardo da Vinci–Fiumicino Airport", 13.0, "Rome", "Europe", "Italy", 0, "FCO", "LIRF", 41.8003, 12.238899999999999, 3 },
                    { new Guid("25058de8-3cdc-47bf-a810-73da1e3da28b"), "Charles de Gaulle Airport", 392.0, "Paris", "Europe", "France", 0, "CDG", "LFPG", 49.009700000000002, 2.5478999999999998, 3 },
                    { new Guid("2b6d45e2-6327-4471-8b85-8802108b4f4f"), "San Francisco International Airport", 13.0, "San Francisco", "North America", "United States", 1, "SFO", "KSFO", 37.6188, -122.375, 11 },
                    { new Guid("34ae11d9-6da2-47ec-8ef5-975943bd3885"), "Munich Airport", 1487.0, "Munich", "Europe", "Germany", 0, "MUC", "EDDM", 48.3538, 11.786099999999999, 3 },
                    { new Guid("3950e8fd-f921-4583-8633-1260c8903823"), "Warsaw Chopin Airport", 362.0, "Warsaw", "Europe", "Poland", 0, "WAW", "EPWA", 52.165700000000001, 20.967099999999999, 3 },
                    { new Guid("3f34fb41-0a96-46bc-9a8e-2ecf188b7525"), "Amsterdam Airport Schiphol", -11.0, "Amsterdam", "Europe", "Netherlands", 0, "AMS", "EHAM", 52.308599999999998, 4.7638999999999996, 3 },
                    { new Guid("441e7474-38b7-47e9-bbd5-d73fcba80072"), "Miami International Airport", 8.0, "Miami", "North America", "United States", 1, "MIA", "KMIA", 25.7959, -80.287000000000006, 5 },
                    { new Guid("4fdda5be-0c8d-4d8e-bbfd-830826728e5e"), "Denver International Airport", 5431.0, "Denver", "North America", "United States", 1, "DEN", "KDEN", 39.861699999999999, -104.67310000000001, 9 },
                    { new Guid("5269bf59-8137-4d8e-a920-ef677fa4665f"), "Adolfo Suárez Madrid–Barajas Airport", 1998.0, "Madrid", "Europe", "Spain", 0, "MAD", "LEMD", 40.471899999999998, -3.5626000000000002, 3 },
                    { new Guid("52f3823e-7f55-4570-9775-33494fb75999"), "Barcelona–El Prat Airport", 12.0, "Barcelona", "Europe", "Spain", 0, "BCN", "LEBL", 41.297400000000003, 2.0832999999999999, 3 },
                    { new Guid("547cb4f5-b7ee-4fe1-9136-591e01419a3a"), "Seattle-Tacoma International Airport", 433.0, "Seattle", "North America", "United States", 1, "SEA", "KSEA", 47.450200000000002, -122.30880000000001, 11 },
                    { new Guid("55fc4114-5f92-498b-940b-71c8e867c0b4"), "Dallas/Fort Worth International Airport", 607.0, "Dallas-Fort Worth", "North America", "United States", 1, "DFW", "KDFW", 32.896799999999999, -97.037999999999997, 7 },
                    { new Guid("67305102-2acb-47a0-ad5c-503e04108beb"), "Hamad International Airport", 13.0, "Doha", "Asia", "Qatar", 5, "DOH", "OTHH", 25.273099999999999, 51.6081, 30 },
                    { new Guid("69be65ff-ad04-46f6-8a12-3831e972df92"), "Frankfurt Airport", 364.0, "Frankfurt", "Europe", "Germany", 0, "FRA", "EDDF", 50.0379, 8.5622000000000007, 3 },
                    { new Guid("6ff33a43-120d-4b64-972c-f022adecc684"), "Los Angeles International Airport", 125.0, "Los Angeles", "North America", "United States", 1, "LAX", "KLAX", 33.942500000000003, -118.4081, 11 },
                    { new Guid("7599b5af-afe1-4039-a24f-5495b8f2b5a4"), "O'Hare International Airport", 672.0, "Chicago", "North America", "United States", 1, "ORD", "KORD", 41.9786, -87.904799999999994, 7 },
                    { new Guid("7bcf290a-3207-414d-94b1-d2cf9924bd50"), "Beijing Capital International Airport", 116.0, "Beijing", "Asia", "China", 5, "PEK", "ZBAA", 40.080100000000002, 116.58459999999999, 6 },
                    { new Guid("7fedf0f9-0911-406c-bc91-11e26e683048"), "Melbourne Airport", 132.0, "Melbourne", "Oceania", "Australia", 5, "MEL", "YMML", -37.673299999999998, 144.8433, 18 },
                    { new Guid("825ef8d0-1a5a-4301-9425-af33f43b0583"), "Brisbane Airport", 13.0, "Brisbane", "Oceania", "Australia", 5, "BNE", "YBBN", -27.3842, 153.11750000000001, 18 },
                    { new Guid("876862ea-cda9-4cca-80a1-f606ea923ca4"), "Cape Town International Airport", 151.0, "Cape Town", "Africa", "South Africa", 5, "CPT", "FACT", -33.964799999999997, 18.601700000000001, 39 },
                    { new Guid("8def7ee1-d119-4853-b75f-246886f6e1b7"), "John Paul II International Airport Kraków–Balice", 791.0, "Kraków", "Europe", "Poland", 0, "KRK", "EPKK", 50.0777, 19.784800000000001, 3 },
                    { new Guid("972e65fc-12b5-4bcb-b44d-070eb0591fe5"), "Tokyo Narita International Airport", 41.0, "Tokyo", "Asia", "Japan", 5, "NTR", "RJAA", 35.771999999999998, 140.392, 17 },
                    { new Guid("b00918c2-1896-4d9f-b5d8-71d61fb62b97"), "Istanbul Airport", 325.0, "Istanbul", "Europe", "Turkey", 5, "IST", "LTFM", 41.275300000000001, 28.751899999999999, 38 },
                    { new Guid("b7ad2fb8-4f9a-400f-aa18-13c2ad766b4b"), "O. R. Tambo International Airport", 5558.0, "Johannesburg", "Africa", "South Africa", 5, "JNB", "FAOR", -26.133800000000001, 28.2425, 39 },
                    { new Guid("d6d0d8e5-69c1-4cea-8b46-59b029bbc91e"), "Hong Kong International Airport", 28.0, "Hong Kong", "Asia", "Hong Kong", 5, "HKG", "VHHH", 22.308900000000001, 113.91459999999999, 41 },
                    { new Guid("dc182e37-d43d-4bbc-8f98-a1a31561719f"), "Zurich Airport", 1416.0, "Zurich", "Europe", "Switzerland", 0, "ZRH", "LSZH", 47.464700000000001, 8.5492000000000008, 3 },
                    { new Guid("e927ec8f-78b0-40cd-8954-26ccd5a23a79"), "Heathrow Airport", 83.0, "London", "Europe", "United Kingdom", 0, "LHR", "EGLL", 51.470599999999997, -0.46189999999999998, 29 },
                    { new Guid("ee0df3c8-706c-4093-9536-8da5ca6e9016"), "Sydney Kingsford Smith Airport", 6.0, "Sydney", "Oceania", "Australia", 5, "SYD", "YSSY", -33.946100000000001, 151.1772, 18 },
                    { new Guid("f9453ea3-c4c3-412e-b7c2-4360905105c5"), "John F. Kennedy International Airport", 13.0, "New York", "North America", "United States", 1, "JFK", "KJFK", 40.641300000000001, -73.778099999999995, 5 },
                    { new Guid("f94903fc-7e8b-48a9-b46b-0bb6ccb28624"), "Addis Ababa Bole International Airport", 7630.0, "Addis Ababa", "Africa", "Ethiopia", 5, "ADD", "HAAB", 8.9770000000000003, 38.798999999999999, 40 },
                    { new Guid("f94ac45b-dfaa-45dc-a743-1894ba5a1924"), "Cairo International Airport", 382.0, "Cairo", "Africa", "Egypt", 5, "CAI", "HECA", 30.1219, 31.4056, 21 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "DiscountId", "Description", "Name", "TicketId", "Value" },
                values: new object[,]
                {
                    { new Guid("c4b192a2-41dc-4f3f-b484-fb8647692db9"), "Discount applied if the flight destination is in Africa on Thursday.", "Thursday Africa Discount", null, 0m },
                    { new Guid("f33611eb-613b-466f-8be5-82b0e8866f28"), "Discount applied if the purchase date matches the tenant's birthday.", "Birthday Discount", null, 0m }
                });

            migrationBuilder.InsertData(
                table: "AircraftSeats",
                columns: new[] { "AircraftSeatId", "AircraftId", "Class", "OutOfService", "SeatNumber" },
                values: new object[,]
                {
                    { new Guid("00541896-b902-4c91-a071-d095e411ac0e"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "9E" },
                    { new Guid("0065a9a0-333f-4d6d-b113-1a984d2f05fd"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "11F" },
                    { new Guid("006f49b3-f895-4eae-af0d-778600b115c8"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 2, false, "2A" },
                    { new Guid("0092a8c1-cf4e-4c34-aa61-6cdeb3a28f99"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "7C" },
                    { new Guid("00fc0107-5d52-4b2e-b55b-2182fac38ae4"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "2F" },
                    { new Guid("017239a1-972b-45c0-8598-8a1c6cc1f696"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "3B" },
                    { new Guid("01ad3345-1e81-4357-ba16-bb965db90685"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 3, false, "1C" },
                    { new Guid("02d474e2-acf7-4100-a61d-f6450e184938"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "5F" },
                    { new Guid("031010fd-74f0-4735-965e-9c3d5821cb64"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "5B" },
                    { new Guid("03d63ad5-d30c-4001-84fd-1bc890230f1e"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "4B" },
                    { new Guid("03e71eb7-cd3d-4ce8-8aef-b5fc09eb3c53"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 3, false, "1B" },
                    { new Guid("03ee9aaf-47e2-4b41-ae8c-57f140f064eb"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "8B" },
                    { new Guid("04ca6b0f-70a0-4947-bcdc-5707783ef9f6"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "4B" },
                    { new Guid("04cf10bc-893f-4a13-bf47-854c663a8441"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "10A" },
                    { new Guid("05d6aaa1-09d0-479d-a75b-5015b5587edd"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "10B" },
                    { new Guid("0645c5ca-15be-4be9-8cea-166070744681"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "6F" },
                    { new Guid("06cc71c5-b420-4d3a-b95d-b7d1d4774c9d"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "7F" },
                    { new Guid("075424aa-f390-4a81-b644-02e82802de0a"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "10D" },
                    { new Guid("076dd5ba-c530-490d-85be-5307e22c3ba6"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "3D" },
                    { new Guid("07a5fba4-28e9-496d-ad45-ec3f61adb5f9"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "5A" },
                    { new Guid("07d457cd-e36b-4539-8a6f-9fb779a50111"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "6C" },
                    { new Guid("07e79d94-db77-4c54-ab61-4992b6dd2a99"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "10D" },
                    { new Guid("08247f3f-4919-4986-8713-9d4a1a52acd5"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "7E" },
                    { new Guid("0881abcb-ce4f-49be-abdf-666b1a4bf035"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "8C" },
                    { new Guid("08aa724f-d3de-4888-994b-4f8dec99bd12"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "6D" },
                    { new Guid("09776f1e-ad02-476a-ad9b-6cde039ff10f"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "9F" },
                    { new Guid("09d302b4-a237-4f7c-a499-20e82d1a4ce2"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 3, false, "1F" },
                    { new Guid("09fc1caa-5135-439d-9ac3-ea694e8b204d"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "3B" },
                    { new Guid("0a1daf06-a896-43ed-9fc4-950f7a967d92"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "10B" },
                    { new Guid("0aced2e2-17fe-4758-bbb6-fd562b506f37"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "10C" },
                    { new Guid("0bd2a5ac-6c99-47e2-a789-4de4016956f1"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "10B" },
                    { new Guid("0cae2962-c4af-46b7-b438-749276960752"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "5D" },
                    { new Guid("0d07db3c-26d5-4ee1-912b-6d23f085d9b4"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "7A" },
                    { new Guid("0d310587-95dd-42f2-a371-9d1726d38f13"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "4D" },
                    { new Guid("0d3c1e3a-c79e-4f49-a619-f03f7a5ad15d"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "11E" },
                    { new Guid("0d52718b-b151-489d-8e48-71cd09ba6fc6"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "5A" },
                    { new Guid("0dd13d7c-f4e2-443a-a49c-06433f4da0c6"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "2F" },
                    { new Guid("0dde4cc2-e9ee-4e00-a989-9e61d9b54232"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 2, false, "2C" },
                    { new Guid("0e2ccf0a-c1ce-42f0-8cca-dcd50d6dc2e1"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "5E" },
                    { new Guid("0e489460-8755-49a9-a33b-8f84bc5d571e"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "8E" },
                    { new Guid("0eb82100-6550-40de-8e78-e70c903ea96b"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "5E" },
                    { new Guid("10114f69-f393-4d96-8dc5-0aed5c34945d"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "3A" },
                    { new Guid("11f16dd9-66ee-4c51-bafc-7d0ed4fc8516"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "11D" },
                    { new Guid("12c4deb6-1a3f-4bb9-a862-b13cf5d94e91"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "4E" },
                    { new Guid("12ec2064-b63f-4b18-b807-77bdc38247bf"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 2, false, "2A" },
                    { new Guid("13707666-5df1-462b-b074-0236bde81324"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "7B" },
                    { new Guid("13d43177-4533-491e-8e0b-4ae3263de319"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "8F" },
                    { new Guid("13fb527a-04e1-4a04-ac5e-8ef684d1aec0"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "6B" },
                    { new Guid("14ac3a3b-1624-4857-9c70-f47a1f223e82"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "10E" },
                    { new Guid("14be204d-fcba-4c94-b9c7-60928daeb4db"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "6E" },
                    { new Guid("1550f94d-ce45-403c-b6c9-a56321a5b33e"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "8C" },
                    { new Guid("156161b0-5eb1-484a-8ed7-4659b78eecc0"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "9E" },
                    { new Guid("16064c4e-b9a2-4a81-af02-e137a9d6aeca"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "9C" },
                    { new Guid("16609095-ec1e-4718-9237-090883c1ed53"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 3, false, "1C" },
                    { new Guid("168e00c0-03a7-4ebd-84de-962b771ae262"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "4E" },
                    { new Guid("178b9eef-dd0d-45a9-9d6b-356604bad44a"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "3A" },
                    { new Guid("17d8e1da-3fb6-48c5-8a39-b4e1172e8f52"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "6C" },
                    { new Guid("1906fa7b-8d7b-4a1e-b0ce-fb7a6cb78a33"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "9A" },
                    { new Guid("193e896c-0e85-4e80-9bd0-7b1b638eaa59"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "8B" },
                    { new Guid("1a34227e-8763-484f-9f93-a35448156840"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "3A" },
                    { new Guid("1a47b86f-b58d-47af-a503-5fc639aba821"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "4B" },
                    { new Guid("1b0f774d-e602-4e11-b7c5-35ceff65e3d1"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "6C" },
                    { new Guid("1b5da293-ab4e-47cf-842f-a2342d3c9852"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "3B" },
                    { new Guid("1cceb3d6-03dd-46b5-8b11-2c49b05f3702"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "11D" },
                    { new Guid("1d0a9a2b-899d-4677-9a64-85b3e9b40ed4"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "4B" },
                    { new Guid("1d0c89e7-6606-4d2d-b013-9670727f7195"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "9F" },
                    { new Guid("1d803262-f2d1-4eab-9ed0-28435152e1ef"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 3, false, "1A" },
                    { new Guid("1dd845dc-9dba-4054-bf91-b4a0d84fcfd5"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "4E" },
                    { new Guid("1eb48212-3b66-4cad-9dd8-8167fbe15808"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "4B" },
                    { new Guid("1f7ccb5b-3d9f-4f59-badc-c6ac1d05c1ae"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 3, false, "1E" },
                    { new Guid("1fbb95ca-b836-444f-8901-cc15d4d7fae3"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "8C" },
                    { new Guid("1fe94815-5f26-4826-b04d-115427e05324"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "2D" },
                    { new Guid("2082a727-93d0-44aa-9154-1b5022f7a787"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "6B" },
                    { new Guid("20b7ebf1-44d6-43f4-a2fe-58dc24a55a27"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "5B" },
                    { new Guid("21740d6d-cbac-4390-9266-6b4478a571cb"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "6B" },
                    { new Guid("22897bbb-fff4-4a7c-a601-7fae2a50b58d"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "2E" },
                    { new Guid("23e6d794-d350-4a65-a0a6-c1afcab7a1ab"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "6A" },
                    { new Guid("2413eccf-eee8-4cdd-b651-fc1194ce1443"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "7E" },
                    { new Guid("2468dac2-ce9d-4b40-972c-42ccc871d27b"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "3B" },
                    { new Guid("256326a0-d018-4faf-a97c-6068f263f2c8"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "6F" },
                    { new Guid("25d2809e-d7f0-4dbf-afd3-a3cd303c80e7"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "11A" },
                    { new Guid("25de7c3e-ab19-428a-90d8-70af29f78071"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "11C" },
                    { new Guid("26016af8-7053-44e5-898f-5b3625b50a0c"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "5B" },
                    { new Guid("2643a3bd-a50d-4f41-8ef8-a55d135f3736"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "10C" },
                    { new Guid("2778b4a0-0b2c-4eba-951e-33e035633f8f"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "5C" },
                    { new Guid("27eb1512-b029-4c05-bd70-ced018b012a7"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "4F" },
                    { new Guid("2853d616-f47b-46b7-ba24-72d0f52dc94f"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "10D" },
                    { new Guid("29abe4bd-cad3-40d8-8ba6-248b1425ad29"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 3, false, "1E" },
                    { new Guid("29bb168c-25ea-4ab0-8279-5b14028e239b"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "6F" },
                    { new Guid("2a131562-2d7c-4d50-8afc-72c0fe3cb707"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 3, false, "1D" },
                    { new Guid("2a783b2e-1dc0-49bc-a860-fb3a79f17b54"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "9F" },
                    { new Guid("2a79cd33-145e-4a1c-9249-a3732a366fc3"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "11D" },
                    { new Guid("2a976c86-f270-44b9-afc3-c73980703aa3"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "10E" },
                    { new Guid("2b0d9e06-396c-48de-b9e9-f0bd503f0bb8"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "4D" },
                    { new Guid("2b18e38d-ac0d-4492-9519-fe1909abc2d4"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "7F" },
                    { new Guid("2b3570e6-1fd9-46df-a640-76c2611af222"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "11F" },
                    { new Guid("2b388519-6b5a-4e82-b40d-7474b470bb4f"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "2E" },
                    { new Guid("2b831fcc-7b00-4d4e-ac9a-4093240711fb"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "4C" },
                    { new Guid("2b8e69a2-885e-4f6f-bc9d-4e2444caa009"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "8F" },
                    { new Guid("2c27e1c9-054f-43e0-8bb6-d8e180ea568d"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "10F" },
                    { new Guid("2c5c0dcf-7f32-426b-a4a1-5bfa297907e9"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "4C" },
                    { new Guid("2da20c17-1e5d-4a26-9f56-9f6b3523a931"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "11E" },
                    { new Guid("2dc1147e-5118-4b0a-826a-cfd84462f294"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 3, false, "1A" },
                    { new Guid("2de4b3fd-e798-4955-af9f-339ce535bd41"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "7C" },
                    { new Guid("2e249b3d-3a6a-45b5-914b-85294b140aaf"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "9D" },
                    { new Guid("2eafd3dc-c928-49e8-8a3d-0c2335750e63"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "8E" },
                    { new Guid("2fee1583-0215-4d92-a02f-0b484b903f22"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "5C" },
                    { new Guid("302e171b-aeff-4f8c-aad4-2402c32ecd90"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "7B" },
                    { new Guid("3077c580-ef5d-462d-be42-7825df24c3b7"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "3C" },
                    { new Guid("31216987-df23-46af-86a7-a38b28719287"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, true, "3B" },
                    { new Guid("319c50bf-3ff1-423f-b243-8b44b10967aa"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "11A" },
                    { new Guid("31c87efd-fe2a-4072-877a-270d6cf6e424"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "5C" },
                    { new Guid("321d09e4-f2ae-4bcc-851a-1f57e4abac92"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "8A" },
                    { new Guid("3268ddc0-8e8f-4c5d-b178-175d719a212a"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "5C" },
                    { new Guid("32a9869f-89b7-4e97-af98-856f2fb23db0"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "4D" },
                    { new Guid("3348049d-bad7-4957-9f6b-2c1cbdd20bb5"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "8B" },
                    { new Guid("33bde12c-d829-43fc-a3c5-f434b856690a"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "5E" },
                    { new Guid("34372f63-e117-45b6-90fd-5bbcb79aacc9"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "9D" },
                    { new Guid("3440546e-60d4-4016-af54-eda42211b60f"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "5E" },
                    { new Guid("34e7288e-4338-4a86-be57-8b2b8186e7cc"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "8B" },
                    { new Guid("3575e342-ed30-4d0c-9eeb-d4b8d2dfda91"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "1F" },
                    { new Guid("3588ab9d-95f4-4509-ab6e-f212d6ddc809"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "11F" },
                    { new Guid("36d4015a-6c8c-4631-ae2f-714fbbf137c3"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "4F" },
                    { new Guid("36d4220a-4789-46c7-af3f-cbc06852a0a2"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "8D" },
                    { new Guid("36e981e9-4945-4773-8d73-4aa20a611f79"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "7C" },
                    { new Guid("3719c814-09c6-4021-8680-40594d1346c5"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "4A" },
                    { new Guid("371be532-dcc1-4d99-b082-cff6e0f51796"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "2A" },
                    { new Guid("3722a7ab-5c36-49a7-8555-a07a4b6d441f"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "4C" },
                    { new Guid("3804f634-22a7-471d-9bc8-babcee1410e1"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "7D" },
                    { new Guid("384bd3c8-6629-43ac-8c67-131906899ceb"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "10F" },
                    { new Guid("39147d32-61e0-4ff7-8375-1772f2495707"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "6A" },
                    { new Guid("394663e2-50f3-473d-9f9c-be137d91ebf4"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "5B" },
                    { new Guid("397c43d2-50ed-4adb-b3d0-8304b83c862a"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "5A" },
                    { new Guid("39f2c87c-1526-4f48-90ff-20a70ec7d623"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "8A" },
                    { new Guid("3a820f8d-5b01-4698-896a-2e93bec2e9c8"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "6B" },
                    { new Guid("3b236d32-c5f8-470d-b4ae-8fc33e2bd6e3"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "4A" },
                    { new Guid("3ba61cb2-683f-4b2f-972d-9f656ed065a9"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "11D" },
                    { new Guid("3bd56154-4e0a-4747-b794-e2df931d4f15"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 3, false, "1C" },
                    { new Guid("3bdd1f39-e876-4e70-ba68-6d70fdd8a561"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "9A" },
                    { new Guid("3bfa54a5-3bed-4e18-b737-884688301d28"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "5F" },
                    { new Guid("3c923a22-a257-484f-8f9a-9f44adc65367"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "7A" },
                    { new Guid("3c95d403-af8a-46ee-b750-36d75e360f0b"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "5C" },
                    { new Guid("3c9b0e09-3d9a-408d-8bd3-d2872db92543"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 3, false, "1F" },
                    { new Guid("3ca5ecbb-3225-4d42-a3a8-ce5d905f87b6"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 2, false, "2F" },
                    { new Guid("3cec7f55-600b-4267-a1c6-c5943e5314af"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "5D" },
                    { new Guid("3d416699-d0e0-435a-969f-6cebadcf2ed5"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "5D" },
                    { new Guid("3d518133-798e-41bd-a0ab-acb3b9e849c0"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "11C" },
                    { new Guid("3de11219-dc6e-4df8-b9a8-3eedd909fc4c"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "4B" },
                    { new Guid("3e7afbcd-5ca9-48c2-ae07-3c27d0a16a1b"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "10E" },
                    { new Guid("3f168c1a-dc47-48fb-b349-15db2af5822b"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "9C" },
                    { new Guid("3f6b5dbf-01fd-46b2-a78a-a6f344a07168"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "8C" },
                    { new Guid("40cc8f4b-ec7e-4e36-a6cc-5179d1fe77a8"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "9F" },
                    { new Guid("40e79ad7-3adb-4947-a511-25a97adfa97a"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "4B" },
                    { new Guid("411aa57e-d4da-494a-9ef2-834a875bf7ff"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "1D" },
                    { new Guid("417df507-49b3-411e-aede-25ed674a5c3b"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "3F" },
                    { new Guid("4181395a-e0fd-4ef5-8958-cc0907dcbfc9"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "8B" },
                    { new Guid("42956081-45ff-4ead-b13f-71201daebd2d"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "10D" },
                    { new Guid("437ee8b1-f70e-4a92-9794-8a39331f861c"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "6F" },
                    { new Guid("43c57686-2c0f-4185-bd2a-8c6b53acefc0"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "4C" },
                    { new Guid("43c5cf9e-8849-4f62-8a5a-496641f958a2"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "5C" },
                    { new Guid("43ffa62c-68e5-4d74-bdb4-ab4ece5773dc"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "6B" },
                    { new Guid("44370ce0-d780-4f90-97f5-2c14d62ba084"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 3, false, "1B" },
                    { new Guid("4445849d-46b4-4844-a963-b4ef2b231354"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "11F" },
                    { new Guid("4488cb76-5312-4472-85b8-475bb14279a6"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "6E" },
                    { new Guid("452bd7bc-86e9-4ea4-978d-9d5e6103b458"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "8D" },
                    { new Guid("456bfeaa-df2a-45ce-9173-b47a6d554349"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "8D" },
                    { new Guid("45850d8c-4b2b-4ee4-8c1d-762530f6a9aa"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "5F" },
                    { new Guid("45890609-85bf-44b2-b9c8-8616f3fd7861"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "3A" },
                    { new Guid("45f969b0-6dbe-4b32-83ed-b69e00bddd94"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "6E" },
                    { new Guid("46e70a14-b296-4e88-b580-8d937bb1e0bd"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "11C" },
                    { new Guid("47e3cd88-2534-47b6-bcfd-3f54d735c6ec"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "5B" },
                    { new Guid("481301d2-b537-42b1-9b7f-d04a9e487d10"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "11D" },
                    { new Guid("483aa2df-903c-42c3-ae8d-51a287c761d2"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "10F" },
                    { new Guid("4851ec39-ecf7-410f-8372-b2dd7e3d78ee"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "9B" },
                    { new Guid("48abe2e1-a535-4a2e-ac3d-709990c6889b"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "9F" },
                    { new Guid("497bade5-534b-4039-80ec-e8dc1f2d5874"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "10C" },
                    { new Guid("49acac61-7da7-407f-8b1a-f44f17c90e17"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "9D" },
                    { new Guid("4a865ed3-f50d-40c6-ab96-9d399cea0288"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 3, false, "1D" },
                    { new Guid("4ae1705b-4893-4bc3-be88-bdc308f9fd36"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 3, false, "1A" },
                    { new Guid("4b275781-1087-4ed3-80cd-76f32a48eeb2"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "6A" },
                    { new Guid("4b88f6ae-06d4-486d-9a3b-957281784843"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "11B" },
                    { new Guid("4bfe92db-8812-4d89-aaff-4f12978b79b6"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "4B" },
                    { new Guid("4c0d0d5d-9ed5-47ce-a756-fdaa501e57ad"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "3D" },
                    { new Guid("4cb43e95-8924-40c0-b3c4-46a86eb5235e"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "7D" },
                    { new Guid("4cc5aff9-0d30-4a96-90c1-75df032c2121"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "3B" },
                    { new Guid("4cd2cfe9-258f-46bd-bc9f-1b817b52da52"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 3, false, "1D" },
                    { new Guid("4de81d7a-8b1c-4783-953b-e243a112b429"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "3C" },
                    { new Guid("4dea068c-785f-4efe-b596-d7484cd51f35"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "5D" },
                    { new Guid("4ded80b1-9c65-4e14-a66c-01140c48286f"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "3F" },
                    { new Guid("4e294056-e9cf-4ff6-ae69-56e7700c42d5"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "3C" },
                    { new Guid("4e437492-fa96-4974-945e-12b2b13a796a"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "8C" },
                    { new Guid("4e732bc0-8aec-4e62-8415-3ff144507d1d"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "8A" },
                    { new Guid("4f96b3a3-a978-42b7-9d8a-8ee06b0a3caa"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "3B" },
                    { new Guid("4fd20f5f-ae89-4f09-8fbf-13a61ee5748d"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "6D" },
                    { new Guid("4ff1118a-9acc-44cc-9a3a-17c11fee9d86"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "6B" },
                    { new Guid("50f4d264-8618-47c8-90db-fa2976e76909"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "9E" },
                    { new Guid("516c9e94-55d8-4234-8f76-a52d26c53292"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "8F" },
                    { new Guid("51c845f1-7911-47d0-aa08-edbd8867e97a"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "11A" },
                    { new Guid("51ec7d10-206c-484b-8f9f-5611f49c9f8f"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "2B" },
                    { new Guid("5220aaab-5f1e-4661-aab5-7e039fb9eeb8"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "6F" },
                    { new Guid("52bc206f-5ae1-4411-8704-1c29b87085a6"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "5C" },
                    { new Guid("533f51d5-eb23-463b-ae4b-51b8064e7c51"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "10E" },
                    { new Guid("54271284-6bc7-4914-8ce7-c0a45c3dac19"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "6E" },
                    { new Guid("54ccaeca-f83b-4309-bb1a-fa39be32e992"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "4D" },
                    { new Guid("55960898-d4e7-4d41-b62d-f3d5fea62326"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "9E" },
                    { new Guid("55fd9b0a-a38c-4b17-9fbe-d194ff376e81"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 3, false, "1C" },
                    { new Guid("563dc7f0-0c8f-4356-9ab4-c39601d2557b"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "7B" },
                    { new Guid("564ba401-9e39-471a-aa1c-9d45f3bbc06d"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "6D" },
                    { new Guid("57872fca-0e5a-4afa-9955-54d6451e240c"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "3F" },
                    { new Guid("57cc79a0-6702-446e-bbb7-811e7ff56dcc"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "5F" },
                    { new Guid("57ef7c69-f101-45d6-a30b-6bdb8d4a25a8"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "1E" },
                    { new Guid("5858c2b0-647a-42c3-868f-5bfd97af5f1f"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "8D" },
                    { new Guid("58afaa66-eb5a-4596-b853-38248d588e25"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 2, false, "2D" },
                    { new Guid("591e388f-4229-4a58-ae52-44ac2c7df7b5"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "9E" },
                    { new Guid("5a1fd5e0-1fb1-49c3-9d39-c4fa97074ef2"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "8D" },
                    { new Guid("5a632226-7e60-4233-83c1-a976dbbf89c5"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "7F" },
                    { new Guid("5af3fcca-b73c-41cf-9843-ac8cf812db29"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "9B" },
                    { new Guid("5af93747-fa85-4a4c-bf93-205f1cd5fd9e"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "8A" },
                    { new Guid("5b7ebcf3-c00b-404d-8056-5c58a374f29e"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "8E" },
                    { new Guid("5bfae002-192d-4b39-b6fe-d4037a624e0a"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 3, false, "1A" },
                    { new Guid("5eac06e4-9fb9-4516-8698-324b3a360676"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "8E" },
                    { new Guid("5f934684-be58-424d-a695-0ba1a9a0431b"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "8F" },
                    { new Guid("5fe41de9-82b9-474b-8652-ea02425bc6f1"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "4D" },
                    { new Guid("601e80e0-e432-4b25-8592-cb7586f6a2a7"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 3, false, "1B" },
                    { new Guid("60b330a2-5240-4902-9a02-fbd62dfba1cd"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "4F" },
                    { new Guid("60c12ec0-3e9e-45c1-a558-2cf3d46e770e"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "6E" },
                    { new Guid("61779ae8-7742-477c-bc30-d94edc7d5237"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "6B" },
                    { new Guid("6183bea4-e022-4dfc-8795-9b25a209a40c"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 2, false, "2A" },
                    { new Guid("6325d57c-20ee-4286-8de3-1c87c3cbfd06"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "5F" },
                    { new Guid("6381e144-a0af-4d23-9ee3-20d27cae579b"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 3, false, "1B" },
                    { new Guid("63e78c94-7683-4bd4-be25-7c9d3d8eec34"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "2F" },
                    { new Guid("63f33f3f-ccd8-4dc3-becd-cf87873aad80"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "11A" },
                    { new Guid("650f57bd-0d65-4fd9-95ad-c85f39ea659d"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "8C" },
                    { new Guid("658e1cc5-754d-483c-a935-8a271d4654f4"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "7D" },
                    { new Guid("66421c5a-0e34-4d3d-8e25-24d37f4639c0"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "7C" },
                    { new Guid("66506aca-32eb-4baa-adf1-884f75465635"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "4C" },
                    { new Guid("66fd662e-da86-483b-9d35-5ef799a1f914"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "2B" },
                    { new Guid("6736ed6c-b802-489c-b176-cddfd5e3a3e1"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 3, false, "1F" },
                    { new Guid("6905ea0b-6b10-40db-bf61-442773fd91e2"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "10F" },
                    { new Guid("69848974-9e86-4a63-a452-460f062a0faa"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "7E" },
                    { new Guid("69d33982-1321-4222-8326-a202f787a2d8"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "5B" },
                    { new Guid("6a60022f-32d6-4eab-b4f8-e08ae816061d"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "5A" },
                    { new Guid("6acd5a8a-5774-4734-a889-e33326d997f6"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "9A" },
                    { new Guid("6ba37ec6-e538-4b14-b873-0e5c952af2aa"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "1A" },
                    { new Guid("6bcb4d70-e9ae-48ab-92fb-7528e2adb5b1"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "3A" },
                    { new Guid("6cbf324d-0eb6-46ea-8e04-3d2ff7ec4a31"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "5B" },
                    { new Guid("6d8601c8-3d27-485d-8d71-fbb64e369744"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "10C" },
                    { new Guid("6daa30aa-3258-469d-8b0b-c477ab1a07e9"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "11C" },
                    { new Guid("6e2a03a6-f4a5-4a64-8303-edf1ebdb617c"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "9A" },
                    { new Guid("6ed63572-c126-4b2c-9ec3-6ba9b4ba364b"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "10A" },
                    { new Guid("6eed78d1-e139-4840-aec7-aac73d4ada05"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "4F" },
                    { new Guid("6f8791fb-1fa0-447c-916d-d1da904ad0e3"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "4C" },
                    { new Guid("6fc46095-a5f6-4b14-a52b-7bb18f82e9b6"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "6C" },
                    { new Guid("70459b04-7fc3-498e-8ab1-a6259a2568a8"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "8F" },
                    { new Guid("70f8082e-3833-495b-9880-1b85d943a615"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "11A" },
                    { new Guid("71b11db0-8f8d-43fa-a595-a3d063e2f3ca"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "11D" },
                    { new Guid("71d204ee-77cc-42b7-9914-d062440b8abc"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "4C" },
                    { new Guid("7206c309-2bf7-4ae3-9fb0-20b7801efe3b"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 3, false, "1B" },
                    { new Guid("72087925-0b75-4232-bf46-08f30c4403bc"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "10D" },
                    { new Guid("726df720-08e1-487b-b78f-a737d5ad4fd1"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 3, false, "1A" },
                    { new Guid("73119ebd-c382-4f72-953b-340dd3ad0176"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "11D" },
                    { new Guid("731f977d-0093-4277-8bb8-d39632d0f0ee"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "7F" },
                    { new Guid("7332d035-8408-487c-ac7f-e357ae5a3c8d"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "6E" },
                    { new Guid("74872e06-88a7-474f-a556-7efed294ebce"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "6B" },
                    { new Guid("755b3289-d29d-4a0e-aba9-b1965c23686a"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, true, "7B" },
                    { new Guid("7597bcf5-56ec-467c-8357-7c92a6678955"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "5E" },
                    { new Guid("75a387f1-81d7-4b87-966f-74d221b5019d"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "10A" },
                    { new Guid("761c35c5-aae6-4d78-8bf6-06a54760cdb6"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "6E" },
                    { new Guid("767dd607-8582-4606-80d9-de79d704cba3"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "6C" },
                    { new Guid("77ffca40-2a16-4fb3-8fc0-50586ef686bd"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "10E" },
                    { new Guid("78d55571-c6dd-4a51-937e-4f4a8db1ec9a"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "8C" },
                    { new Guid("798cddb8-a566-4ff1-a130-24e5066b9f1c"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 3, false, "1A" },
                    { new Guid("799dc1c2-7448-435f-bc85-162d34b2b227"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "11B" },
                    { new Guid("79f65480-3a07-46b4-add3-9f278caf512d"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "5A" },
                    { new Guid("7abf42aa-96d2-420f-b147-67b468002f0c"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "5E" },
                    { new Guid("7ae1dcb8-c1c2-467d-9c7d-fb58b7325090"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "8E" },
                    { new Guid("7b28b792-5ee5-415a-bfa9-464ff027fbd7"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "8A" },
                    { new Guid("7b59745b-a6c8-49eb-a7be-2d71ab3c0265"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "10D" },
                    { new Guid("7b7a7bbe-6339-4722-9701-9acaadefac23"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "5D" },
                    { new Guid("7bad07c3-6b81-4546-b63d-db12f1baac25"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "7F" },
                    { new Guid("7c18dc09-656f-4e5b-a100-fc258e496f02"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "6A" },
                    { new Guid("7c865e13-c7bb-4c5a-a3e1-38fa318793e2"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "8D" },
                    { new Guid("7c9f69ab-5b13-4f1c-8d07-77a15a54984d"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "6A" },
                    { new Guid("7cc42464-e813-4853-a3fa-c80e504c7c1b"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "11E" },
                    { new Guid("7cfac8eb-d4cf-4751-8935-d733da5a29c1"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "6F" },
                    { new Guid("7dbc5e71-a532-43b5-af2d-9f03ddf598b2"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "5A" },
                    { new Guid("7e719246-018b-4855-820c-b09360edd06b"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "11F" },
                    { new Guid("7e814b95-2c28-449f-b68e-eef69e4556e6"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "6D" },
                    { new Guid("7f17e391-42ec-4039-a864-5944f93e22d7"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "7A" },
                    { new Guid("7f8b83d2-d612-4971-9124-d33bdab18166"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "2C" },
                    { new Guid("7fde5e26-44a8-4e6e-88c4-a291ee8a42d0"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 3, false, "1B" },
                    { new Guid("807c3283-b50e-4840-ae59-1a54a91c6664"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "11A" },
                    { new Guid("8081b2f0-6236-4dbd-8885-044efe330ebd"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 3, false, "1E" },
                    { new Guid("80a056f1-0fcc-43b8-b590-a209326eb88e"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "4D" },
                    { new Guid("80c894d5-1423-4f42-94a1-e16837b68617"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "3E" },
                    { new Guid("8107a480-d492-4941-aec1-8042c04367a0"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "7A" },
                    { new Guid("811114cf-fb6c-4ace-8e68-882e4f5a6c76"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "4B" },
                    { new Guid("8197e860-35bc-47de-9df4-78f30a17d613"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "2A" },
                    { new Guid("81b99e74-778b-44f6-8d88-6c1156c3fc32"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "6C" },
                    { new Guid("81d54088-3266-4f15-8feb-976d2357ca1a"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "4E" },
                    { new Guid("82551992-be4a-45e3-845f-98c84c9ccb9e"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "9C" },
                    { new Guid("827bfcd5-5905-4648-9a2d-7fdf3474034f"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "9A" },
                    { new Guid("8292c92b-0a6f-4113-a75f-1691ad6ec579"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "4C" },
                    { new Guid("8298aee4-ed74-4bcd-8ee4-ae68087f03ca"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "9D" },
                    { new Guid("83ef6b69-78ea-4d24-b1f7-a022ad9e3c9b"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "8F" },
                    { new Guid("84228dee-3859-4c99-9283-d93840f582ba"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "4A" },
                    { new Guid("846cf520-b10e-4564-8d71-7f924a20f2bf"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "5F" },
                    { new Guid("850bdc4f-77cb-4c00-a3a7-1244fc4e9687"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "11E" },
                    { new Guid("85882223-3ed2-4b31-bec2-68b1d3bc07d5"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "6D" },
                    { new Guid("85950f81-83a2-47c0-8d70-1503cc1cce4d"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 2, false, "2E" },
                    { new Guid("85f4a57f-342b-4059-af21-71875e800b39"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "4B" },
                    { new Guid("8648b38d-ef66-4ccf-8f37-936bf3bd4785"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "6A" },
                    { new Guid("866b2b1e-ca9f-4b9a-b11b-4cc224fbd5ab"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "11E" },
                    { new Guid("8686dfba-0649-463d-95a6-b5479ea1ea24"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "10A" },
                    { new Guid("86e80fb3-c8fc-4acd-a72a-30d5993dacab"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "8A" },
                    { new Guid("870ca322-022c-40c9-ae67-d866c48db3ee"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "5C" },
                    { new Guid("876455aa-a551-4f2e-8e69-8c005876ba1d"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "7E" },
                    { new Guid("87e9d194-48ee-4b64-bf8f-c7baeb088729"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "7C" },
                    { new Guid("87f85cfc-bac8-41c4-abd9-432db743a18e"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "6A" },
                    { new Guid("88303771-1a0b-488a-a1dc-aab7f3b4507b"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "8A" },
                    { new Guid("88542528-a19e-4de9-bdbc-215d34d12bab"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "10F" },
                    { new Guid("88a4e3a1-5f70-4e89-8636-a18dbc183d0c"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "6C" },
                    { new Guid("88b59ed1-f869-401b-9119-ef26e155f48a"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "3D" },
                    { new Guid("8969ba4d-380c-4425-bfd1-9876ea12e1e1"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "5A" },
                    { new Guid("896e6b8e-fc68-4df0-bee0-d2e21518ce77"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "7D" },
                    { new Guid("89ab684e-8bf6-492f-bc52-a4693b6d3260"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "4A" },
                    { new Guid("89cbd79e-1341-4607-bda3-6b4c2d8474e5"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "9C" },
                    { new Guid("8a116556-8748-4a93-b1c1-7ba344c240e9"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 3, false, "1B" },
                    { new Guid("8a3b2bd3-778d-455c-961d-e80051c07a8e"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "4D" },
                    { new Guid("8b16fc09-1d98-47fa-a6b0-18073f83b653"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "9A" },
                    { new Guid("8ba44233-c972-45ec-be1b-d85d55df37ab"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "10E" },
                    { new Guid("8bafc3fe-8027-484f-8d36-d9eb8ba706c0"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "10C" },
                    { new Guid("8bb695ea-035b-4e0b-8b89-512c59a869d7"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "7F" },
                    { new Guid("8bfb33cf-66f7-4519-a103-3864d3a96cdb"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "3B" },
                    { new Guid("8c7aae8b-9f6e-4075-9c81-f372b6e798fd"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "9D" },
                    { new Guid("8d2da010-0e71-4e52-8927-cf40e5ca8dec"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "5E" },
                    { new Guid("8e716b7b-487a-421c-b369-855537025b66"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 2, false, "2A" },
                    { new Guid("8ef6b6fc-340b-4b4f-85b0-9f2bd5e9bf2a"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "5A" },
                    { new Guid("8f0ca92e-c163-4281-8084-b035272b199a"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "10A" },
                    { new Guid("8f1d99aa-1bb5-4f51-8e41-f643db0a69f1"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "3A" },
                    { new Guid("8f2c7acb-9ed6-4359-af1b-dcdcff0f0bc9"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "9E" },
                    { new Guid("8f3bb01b-7093-4dd9-8892-9d3a56dcaa79"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "3B" },
                    { new Guid("9008b915-0fee-474d-8f4c-43cb2917ed87"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "4C" },
                    { new Guid("903cf31c-334f-4b17-bc26-f58093538a67"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "6C" },
                    { new Guid("90fc2dc0-47a0-4ebd-8652-ee7876a5d67b"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "5F" },
                    { new Guid("915b6c57-7e23-4761-809c-18ae2461049c"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "8E" },
                    { new Guid("918adef1-0a6a-48c4-94aa-dfa3bb01fe79"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "11F" },
                    { new Guid("91e76d46-196b-41da-bd9c-941fb7d172f2"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "5F" },
                    { new Guid("924d4c8d-5167-4ae6-8c8d-e37d4bf960ad"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 2, false, "2A" },
                    { new Guid("931801b3-7359-4d78-bddf-e72ddc75a770"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "3D" },
                    { new Guid("931a5e7d-5e00-4b46-8b53-3295324e1a5c"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "11B" },
                    { new Guid("935da26f-4808-4907-9f06-f9c19311b916"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "11C" },
                    { new Guid("938d23bb-2fdf-40b9-8b0a-e6fb5064e480"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "9B" },
                    { new Guid("939f7c9b-fd0f-45aa-aec9-45fed137e19b"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "9F" },
                    { new Guid("93ed2213-0a1f-43f6-b5b9-bc7565752909"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "9B" },
                    { new Guid("943b132e-b34b-459a-acf0-7e79d2aa45cb"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "4D" },
                    { new Guid("9558a492-4342-4148-9c0a-78290d340b43"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 3, false, "1A" },
                    { new Guid("9560e1d4-dc7e-41bd-9dd5-5c569707b844"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "4D" },
                    { new Guid("95e0f213-581f-4756-b2cb-0257c4dd6626"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "6B" },
                    { new Guid("9608ae37-68aa-4de9-ad76-1c9b95629e7b"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "6A" },
                    { new Guid("963560a1-70f8-4501-8b50-273f9dfe9130"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "8E" },
                    { new Guid("966d358b-8580-42b1-a4c7-d8edfe322bb7"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "5A" },
                    { new Guid("98b22916-8f71-477b-a9a2-306c5d969b72"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "5D" },
                    { new Guid("98b3638e-c8bc-4787-b144-eb6d27896ea7"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "3C" },
                    { new Guid("992aa843-b875-4497-acb0-53a8dddc13bc"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "7E" },
                    { new Guid("99b1a096-238f-4c2a-b5b7-a49d4b7235e4"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "3C" },
                    { new Guid("99b85607-7da8-4e6f-9d6b-70d5e4a3d621"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "6A" },
                    { new Guid("99fcae64-1c75-437e-8635-aebf8bcc2789"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 2, false, "2F" },
                    { new Guid("9a54306e-8f43-428e-b83d-ea603e16301d"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "3B" },
                    { new Guid("9a569ed1-b2e4-4809-9c4c-9fb0f8b1e781"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "5D" },
                    { new Guid("9a8fe7d0-8a46-4900-bc82-00b14cf14295"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "4E" },
                    { new Guid("9bad5540-655d-4802-a816-f45a44f9682b"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "6D" },
                    { new Guid("9bddd1a6-4a00-4d8b-a3b2-3988ca3a3d51"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "8C" },
                    { new Guid("9c29330b-78c8-4cad-bbcf-4acee91a090d"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "7A" },
                    { new Guid("9d123b7b-be05-49a6-b9c0-9c5fa9c63920"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "10C" },
                    { new Guid("9d312420-8cce-4ab1-b43c-aba4c34b98b0"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 3, false, "1B" },
                    { new Guid("9d9cc667-291b-4ea1-b1c5-ac86eb728348"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "3E" },
                    { new Guid("9e178b39-7f57-4ddc-ae33-a096203a37cd"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "6F" },
                    { new Guid("9e3f0063-737d-4e95-8e06-6bc395b32a7b"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "7E" },
                    { new Guid("9e449572-b729-465e-a1e3-2193699dcae0"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "4F" },
                    { new Guid("9f28363b-d73d-4889-adb5-edf315f14951"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "10A" },
                    { new Guid("9f28479a-29a9-452e-a08a-ffd3f4a16a20"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "7A" },
                    { new Guid("a00cf005-ae10-4c8e-a05e-1c1c553f6afa"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "9E" },
                    { new Guid("a0344511-9d46-4b5e-924e-ecc9af82dbd8"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 2, false, "2D" },
                    { new Guid("a078f7c5-f84f-4504-9e9b-c68896155f95"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "10B" },
                    { new Guid("a123c6d8-024f-4a91-9932-da6557fa6e87"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "4A" },
                    { new Guid("a1ab2fcc-e0b5-430a-9139-0cb14198bbe1"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "5C" },
                    { new Guid("a1b9d81d-9f22-4962-a646-0733ddd245ae"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "1C" },
                    { new Guid("a1bd2c1d-8d0d-47ec-b9c5-e16a6c711857"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "9C" },
                    { new Guid("a2457dec-f55a-4346-a098-2be7abdd9579"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "7A" },
                    { new Guid("a24d7e31-9f0d-4707-a6da-60f9d2035ec9"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "7C" },
                    { new Guid("a2d42cb3-1467-402a-8c90-3da3b2581e12"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "8F" },
                    { new Guid("a343ee3f-9def-489e-b0e2-4d8f5698d6af"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "6A" },
                    { new Guid("a3aeab03-ac16-47f1-8daf-48d1c6c2983d"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "2A" },
                    { new Guid("a6152bb5-6108-425b-9dee-3ca1621268b7"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "6D" },
                    { new Guid("a63a649f-63c9-4623-bea2-5b976f9dd607"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "5F" },
                    { new Guid("a656e2fe-baef-40cc-a52b-97a2f3f3483f"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "2E" },
                    { new Guid("a65b5f1c-5bf0-49cb-9ffa-a81df44353b8"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "4B" },
                    { new Guid("a6c4812f-e90a-4b0b-b3ce-c700224c6835"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "11E" },
                    { new Guid("a7216a18-1e08-49dc-84d3-4fb97c4c7605"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "9B" },
                    { new Guid("a7928842-5367-4b2d-9315-8e5b8766f4c9"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "5D" },
                    { new Guid("a7d97dcf-0498-4084-bf2b-643033e5d4b6"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "3E" },
                    { new Guid("a86adf5d-583c-46d1-b72c-df5dc8d87fbd"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "6D" },
                    { new Guid("a88c7644-f6c2-4f25-ba9c-9adc90d0ba38"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "3A" },
                    { new Guid("a924d14f-acf3-4b94-8f41-a06f41b93cb2"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "3B" },
                    { new Guid("ab22957b-a8c4-4dde-97e5-90833e164162"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "8F" },
                    { new Guid("ac45d958-7b1e-4b0c-88fe-edd9eecdecb6"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "8B" },
                    { new Guid("ac944468-32db-472d-a39d-14d454aaaab3"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "3A" },
                    { new Guid("acf7d40f-5f94-4b3d-8a74-fc7a885e3f04"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, true, "7A" },
                    { new Guid("ad629fbc-5742-42e4-957a-b4a51a1844c4"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "8A" },
                    { new Guid("ae4531ed-d65b-4d31-ad00-cdf90a14b908"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "5E" },
                    { new Guid("ae733a96-98af-44f5-99ee-ffae60fdded7"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "7C" },
                    { new Guid("ae93dafa-c93f-4b1d-97c3-ce5d96c0d49e"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "7E" },
                    { new Guid("af92a23f-3391-4f02-bade-d00138bb0796"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "5B" },
                    { new Guid("b01636bf-38b3-4e82-aeb7-f67c144e63ea"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "5F" },
                    { new Guid("b04ec60e-ee6c-4605-843d-8938d3969bea"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "8D" },
                    { new Guid("b123cc1c-6e1c-4149-9278-d7c2a319afdf"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "2D" },
                    { new Guid("b1a801fc-fc23-4a27-b358-3c2ee3a3ba29"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "5C" },
                    { new Guid("b2548a00-bc96-48fb-bafd-1b5d3fea76c6"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "7C" },
                    { new Guid("b356fa0f-95d9-4ddd-bf07-c2ef73a9d36b"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "4F" },
                    { new Guid("b35fbd4d-4c34-4b6a-bd00-5aa6ce6af035"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "8C" },
                    { new Guid("b3a7a72a-8655-491c-b6cc-e577949eae76"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "4E" },
                    { new Guid("b4626801-aaea-4f28-90d5-1c30e81993b9"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "4A" },
                    { new Guid("b55f95b9-f6b6-4cd3-9272-fffdf695819c"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "9C" },
                    { new Guid("b56061ae-9523-4d1b-9ca5-ce748ce24acb"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 2, false, "2E" },
                    { new Guid("b59f863f-e12e-41a3-a321-b0d6781c20e3"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "4D" },
                    { new Guid("b5a85517-d5c3-457c-885e-db7084725cdb"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "11C" },
                    { new Guid("b611964a-e82f-4b4f-93b5-9f1d9f4bee08"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "4C" },
                    { new Guid("b62b3a9d-045a-4c4f-a99b-65f97c9e3428"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "9D" },
                    { new Guid("b66dbf91-0508-4ea5-bc40-4bac318cdc25"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "5D" },
                    { new Guid("b6a3e652-b37a-4484-a25f-61ab6cde3e05"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "7B" },
                    { new Guid("b6bea0be-8c2b-40ef-8247-61339b801389"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "10B" },
                    { new Guid("b6cf676e-5b31-478b-a6bb-b536f86d9177"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "11F" },
                    { new Guid("b6e20a5e-d103-4db2-9a42-5dd8d55bb058"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "9A" },
                    { new Guid("b894d371-82c4-45b3-ac8a-6d5cf25a2ecd"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "9B" },
                    { new Guid("b8adb810-2f50-41f6-b6cd-0c1d1183a7ff"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "5D" },
                    { new Guid("b8be8bf9-e459-41ad-87cf-80aa7a3495c8"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "5E" },
                    { new Guid("b95ce61a-d72b-4173-919b-fd1f081e9dc5"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "8F" },
                    { new Guid("b9827bb1-e3b2-490a-964b-99e480c5d602"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 3, false, "1E" },
                    { new Guid("b983844e-8e83-401d-bed3-00121b03c09b"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "5C" },
                    { new Guid("b9a5e8cb-9d59-4396-80e7-ced4eaf4375e"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 2, false, "2B" },
                    { new Guid("b9d4bb5b-fb21-49f8-917b-2ae72b44cc94"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "6C" },
                    { new Guid("bbf1d8e2-e8fd-4817-b11b-02c66ed6fc15"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "4F" },
                    { new Guid("bc2051a1-65b3-4bb7-add2-a5c0f2e44519"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "6E" },
                    { new Guid("bd81c58b-a73c-4dc8-92fc-50950dcc5215"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "10D" },
                    { new Guid("bd9efc43-15ac-4ba1-821d-dfd035c8be2c"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "2B" },
                    { new Guid("be69f42c-901e-4712-acad-d6b4539be5df"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "11A" },
                    { new Guid("bebfee2c-c35c-4010-a444-51e69d7788a1"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "9B" },
                    { new Guid("c04a07c1-b266-409e-8138-aae5b46ce2de"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "4F" },
                    { new Guid("c04f09e9-13fa-4d50-94d6-68cca0ecbdb4"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "5E" },
                    { new Guid("c0eb838d-ad76-4583-9196-f6a2675b2ab2"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "4C" },
                    { new Guid("c127e6e7-4197-4de7-8534-035cf6fc99d8"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 2, false, "2A" },
                    { new Guid("c296c08b-ec1a-4a7d-85fd-e5464db4af58"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "11B" },
                    { new Guid("c29e8e3d-0c15-4996-80aa-43144396741e"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "9C" },
                    { new Guid("c3302e95-f104-44bf-a906-21dc65720c84"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "7B" },
                    { new Guid("c349b615-47a5-4e08-be87-51ddebb71fff"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "3D" },
                    { new Guid("c3838061-b13c-46c9-a7f3-208005150b03"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "4E" },
                    { new Guid("c3f5f51d-1de1-435d-b292-f3c99122e6ea"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "5B" },
                    { new Guid("c4ed4de9-07be-4cd5-b28e-8008313257ff"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "6F" },
                    { new Guid("c517dc17-38ec-46c8-9efb-8753d464ebf2"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "5B" },
                    { new Guid("c524d7f6-5dfa-4274-8fe0-0123e6a85777"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "6D" },
                    { new Guid("c532699b-8ece-4a24-9ada-094d3a00d7cc"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "4F" },
                    { new Guid("c540da05-6926-469b-abe1-ddceee27ee22"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "5A" },
                    { new Guid("c5a23702-ead2-44ef-9e37-9664baa9a668"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 2, false, "2A" },
                    { new Guid("c5ee21c1-1918-43a1-b37e-5962919e0135"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "4F" },
                    { new Guid("c73c3d3a-aa53-4ab2-863a-efa1caf03324"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "4E" },
                    { new Guid("c7441855-8b8d-484f-9654-806f4f553c6d"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "4B" },
                    { new Guid("c74b9bee-04a2-40c8-ba3d-6ffc230806fc"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "8D" },
                    { new Guid("c79bd12a-1d56-476d-bbc6-e3dcd86b7e49"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 3, false, "1B" },
                    { new Guid("c82c02fa-e2b1-4bf8-aa74-494c21becdb2"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "10C" },
                    { new Guid("c87953d1-a626-441c-809e-4952b4db7afd"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 3, false, "1B" },
                    { new Guid("c8dc30b2-a69b-4fc9-9ba1-4267a6ffb058"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 2, false, "2C" },
                    { new Guid("c9022755-ba0a-4ec7-af1b-4b2268586c15"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "8A" },
                    { new Guid("ca69564b-624d-4c73-b08f-84851db2daba"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "6D" },
                    { new Guid("cac2393c-a093-416c-b016-b70e1c19b32c"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "10B" },
                    { new Guid("cad91ee2-4fca-45ee-825f-e2b847df14e0"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "7F" },
                    { new Guid("cb41d864-a817-4017-8ac1-fbf41c0c0539"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "9A" },
                    { new Guid("cc0f442b-8142-4dda-a270-482d84ef24c5"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "7E" },
                    { new Guid("cd941dee-34b7-482e-bb3e-249e4b90624d"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "5F" },
                    { new Guid("cde62d1e-e28e-4aae-a794-d13ab551b388"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "8E" },
                    { new Guid("ce809af9-b995-4766-b5d3-ec6818bc97b8"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "5C" },
                    { new Guid("ce91791e-bbd5-4980-867e-e16b95da4afa"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "7F" },
                    { new Guid("cfe813d8-11df-473c-8018-e0c5eec53f90"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 2, false, "3E" },
                    { new Guid("cff50a43-b04d-4d9d-bd0a-ea2dd2cff798"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "7F" },
                    { new Guid("d0c7af9c-62ac-4bd9-b7f4-bfc0543b4230"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "3A" },
                    { new Guid("d1047708-3f09-4143-9f3b-ae232cb3c2c6"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "9E" },
                    { new Guid("d1a2a3c3-7b6e-4ef3-b383-b12448928feb"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "6C" },
                    { new Guid("d2027bce-e329-4565-9fe4-341b579ba707"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "4D" },
                    { new Guid("d2ac1c6f-08bf-4cb7-a76d-1cfc214f5382"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "10B" },
                    { new Guid("d53e09db-7898-4fe7-83b8-17eaa2c2cfbf"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "6C" },
                    { new Guid("d5645acb-cae8-4b9d-b706-ac7352eae141"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "6D" },
                    { new Guid("d5665a0e-7c45-469c-9982-c189903311a8"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 2, false, "2B" },
                    { new Guid("d570d853-da8a-439a-aefc-e70e747d5f94"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "7C" },
                    { new Guid("d60237c6-e8c4-4be5-9d1d-72907c4c6ca2"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "7D" },
                    { new Guid("d690f5fb-126c-4090-ae33-6ad9f7f4f9b6"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "2C" },
                    { new Guid("d6e3d607-f882-4bc8-b2d9-096073efc055"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 3, false, "1A" },
                    { new Guid("d6f2eb97-d663-4314-a25b-0616f75151b8"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 3, false, "1B" },
                    { new Guid("d78a96e4-df44-443e-a3ba-10d756494cd4"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "3A" },
                    { new Guid("d7f271d4-612c-4a3c-a23d-ddd8f97b16dc"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "6F" },
                    { new Guid("d8112cd2-fd03-451b-be75-f57f46e50789"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 2, false, "3F" },
                    { new Guid("d83c17f0-3cbe-4f6d-a487-af5108df6ebd"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "4E" },
                    { new Guid("d875bee5-9924-4285-b139-d6d05ef2dc80"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "6D" },
                    { new Guid("d8a3cd1f-2328-4f4c-80a9-175c5affd81d"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "7F" },
                    { new Guid("da8a99a5-acff-4f87-8ddb-2f60b63524ed"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "3E" },
                    { new Guid("db6d88e9-3c4a-4b55-a43f-fd34cd393128"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "10F" },
                    { new Guid("dbc90ae6-0109-4a73-872f-0779de432b67"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "4E" },
                    { new Guid("dc31e5d9-74f7-4977-bbd6-988c23b0ae38"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "5D" },
                    { new Guid("dc464cfb-bf73-4e31-bf5f-5736b109ca5d"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 2, false, "2A" },
                    { new Guid("dc8b5c52-836c-4b66-b959-07303904559f"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "7B" },
                    { new Guid("dd0785cc-aa2a-4f69-94a4-56250a2387eb"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "7D" },
                    { new Guid("de263bac-66b0-46bf-83fd-44549e449fd8"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "7E" },
                    { new Guid("dfd8635b-0f4e-4715-a2ad-985872d10f73"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "9D" },
                    { new Guid("e1a2c849-53df-4695-8080-ced764ae36ff"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "10E" },
                    { new Guid("e1a2d091-8b58-4699-a160-38d7c60a07a3"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "4F" },
                    { new Guid("e212fc9e-6856-4a09-a221-01f994f87109"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "4A" },
                    { new Guid("e23329f4-fb09-4b01-8bca-03e1220b989f"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "6F" },
                    { new Guid("e23c0737-ff65-4b45-b336-f805b6bbc663"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "3A" },
                    { new Guid("e26f3eb8-dd0d-4621-b039-33c771b650ff"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "8B" },
                    { new Guid("e2ab14f7-74a6-496e-9c39-b5af247b4d6d"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "5A" },
                    { new Guid("e30e19d8-37d9-430c-bcc0-7bc32e8ee8e6"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "5E" },
                    { new Guid("e33a2bb5-a981-4347-a106-192747933b56"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 3, false, "1A" },
                    { new Guid("e34834e9-a433-422c-aa2e-2bdc2c55a907"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "4A" },
                    { new Guid("e3ecd538-29a8-42f2-b251-9247476b9e11"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "3A" },
                    { new Guid("e3fe12c3-dcf7-4fb4-80ed-fd77707e3ab6"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "9D" },
                    { new Guid("e443c300-8f5c-4af9-ad36-b89fba8bfabf"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "6C" },
                    { new Guid("e506bdc6-8c50-4f4f-b45c-1f682e683e17"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "10B" },
                    { new Guid("e59ec8ea-1ee7-4e51-95c2-8eb02d1bf2dd"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "9F" },
                    { new Guid("e5b59ce1-7342-47a0-a76c-928e08e04de7"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "11C" },
                    { new Guid("e6e629ee-91dc-4310-8ab5-dd564ae685ce"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "11B" },
                    { new Guid("e6ff1d4c-a6d9-4aed-96a3-66fd17229d36"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "6E" },
                    { new Guid("e714247b-78e5-4aa7-8ee8-18bf3110a29f"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "9C" },
                    { new Guid("e73b373e-3256-44b9-bbbc-f8befd59b7e8"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "4C" },
                    { new Guid("e75eabec-e1e3-411d-a3a6-6c77b1cf7a40"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "10D" },
                    { new Guid("e787a2f8-1c8a-4666-8e2e-d59000632d79"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "8D" },
                    { new Guid("e856b72d-d715-49ce-b1ba-68d4de2414b2"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "7B" },
                    { new Guid("e8d1aa83-cede-43a2-b44a-9eb398f60e3a"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "7D" },
                    { new Guid("e9179a01-eb35-45ea-a72f-e69c0eb51bac"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "9F" },
                    { new Guid("ea46e200-db64-4941-8c5c-bb46fa7903c6"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "11E" },
                    { new Guid("eab2da83-32f0-4453-9280-2d496a7b86f9"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "3B" },
                    { new Guid("eafaa6be-f3d9-45df-924b-a3a0e6211497"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "4A" },
                    { new Guid("eb441036-fb87-4a78-b35b-2bb7536f863b"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "2D" },
                    { new Guid("ebc377c6-460a-43c9-84f0-80bbbc8c7e81"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 3, false, "1D" },
                    { new Guid("eccc04ad-49be-4ea6-bce8-41077bdad212"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "10C" },
                    { new Guid("ed3bc1d0-e977-48fa-b40b-52d770090e2b"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "9B" },
                    { new Guid("ed615487-9a54-4242-8c62-db9e177f310a"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "6A" },
                    { new Guid("edb3f580-7e91-43eb-9eae-6451d281f133"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "7C" },
                    { new Guid("ef4bd5f5-6a23-4732-bf7e-202ea8158883"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "11B" },
                    { new Guid("efb08107-e132-4aba-9d69-280df460caf3"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "7D" },
                    { new Guid("f03f3341-5742-46e2-a018-d24c03e3955b"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "6E" },
                    { new Guid("f07f5c72-cc43-482a-aa01-c1eb546263d9"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "6F" },
                    { new Guid("f0f60bb1-8cc4-4ea4-9d3f-003969fef210"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 0, false, "6B" },
                    { new Guid("f173e32a-52b4-462f-bfb7-bbdf80440d99"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 3, false, "1F" },
                    { new Guid("f1cfd43f-4e3d-4ebe-9feb-796fa3b545f1"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "4E" },
                    { new Guid("f1f9caa0-8d6a-4664-9330-07655b762e55"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "5B" },
                    { new Guid("f238f514-024d-41bf-adc8-f680e9723c14"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "10A" },
                    { new Guid("f2695d83-b289-46e8-bc46-5a2a3398c122"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "5A" },
                    { new Guid("f2ddec5f-a130-4d98-b2a7-55d358ef7293"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "10A" },
                    { new Guid("f30adc3f-ffad-4470-9fef-c43f14e78abe"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "6B" },
                    { new Guid("f319d03d-74fa-4811-8cc4-c7c9fddbe23c"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "4A" },
                    { new Guid("f3545dc2-e2b6-4415-963b-53781e4cd46b"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "4A" },
                    { new Guid("f35daa86-b990-4a44-a6eb-81d45b250ae4"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "4E" },
                    { new Guid("f48fc725-16b4-4896-8d2b-7fc6721d737f"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "5F" },
                    { new Guid("f4db7025-7057-4880-9ee7-36d646eb87fd"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 2, false, "2A" },
                    { new Guid("f4de7706-268a-488e-8e34-0e98cc24750f"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "5E" },
                    { new Guid("f52685de-255a-409b-b0c5-0d4fe3c8dbae"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "6E" },
                    { new Guid("f53ec4be-58bb-4356-aa32-230080449791"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "8E" },
                    { new Guid("f651d730-224f-4307-a61f-26813fed0b00"), new Guid("11aa3ade-d8e5-494a-a0aa-0fbf253df21f"), 0, false, "6B" },
                    { new Guid("f65777c2-9966-4e26-86bc-fa2a6a3adf5f"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "4F" },
                    { new Guid("f666e623-b140-48c6-a67f-4db74a00e92c"), new Guid("eba84182-f8f2-40dd-8612-4e47fe735ce8"), 0, false, "7A" },
                    { new Guid("f68aefe1-13ad-4fed-87a1-3328e9cda6bc"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 0, false, "11B" },
                    { new Guid("f6d0fffb-2435-424d-a829-2cdd68a47cc1"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "7B" },
                    { new Guid("f7255390-a1ee-4dc2-8a49-b21abefda90c"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "1B" },
                    { new Guid("f771c526-95a2-45d6-bfb1-ead8cb89c1d9"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "7D" },
                    { new Guid("f7be200f-1e8e-48f0-87ec-ba60b23bbe44"), new Guid("22632089-8b6d-48bc-8db1-b8aa8e364168"), 0, false, "8B" },
                    { new Guid("f8d43f2e-f546-42af-9845-af70acfc37f4"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 3, false, "1A" },
                    { new Guid("f9104bfc-3673-4f09-b562-e4269dbd2a3f"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "8B" },
                    { new Guid("f9eb6662-50f8-444c-ba2f-27c9df668732"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "7E" },
                    { new Guid("fa1762bf-4f85-4828-b900-8aac8ed6bfb9"), new Guid("7f37cef3-2959-4023-a339-2fcc9a7b69de"), 0, false, "7D" },
                    { new Guid("fa18f2c6-8509-46d5-b65d-67fb0addfb97"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 3, false, "2C" },
                    { new Guid("fa1ba0cb-19a0-40d8-9740-22c113315f6b"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "6A" },
                    { new Guid("fa967238-4212-4f82-9d75-6c2da321ec36"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "10E" },
                    { new Guid("faabdf9e-715b-4f21-b93d-aab01bb82fdd"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "6E" },
                    { new Guid("faf4cb56-7177-49da-8853-3332c8096e50"), new Guid("d0476af9-1672-49f6-8fdf-d74732ae86a8"), 0, false, "10F" },
                    { new Guid("faf763af-a09c-4869-a876-aa57cdffaeb0"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "7B" },
                    { new Guid("fb0f2c32-b95c-4e4a-bda7-7a5616babc2c"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "3F" },
                    { new Guid("fce7d5fa-338d-45b4-9097-f610030f0502"), new Guid("c9acfc93-f41a-476d-9f0d-43d92315a54b"), 0, false, "5B" },
                    { new Guid("fd7ad289-92b2-4c01-9899-93cc8ee19bca"), new Guid("e6458593-a017-4684-8a9f-c6b5bb95dc3e"), 0, false, "4D" },
                    { new Guid("fdd60d94-9914-4a1e-82d8-148ba371f928"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "6F" },
                    { new Guid("fe679045-d262-46e8-a594-de680008ddab"), new Guid("9e216b53-0ba1-4975-88f1-af73dd56c0ea"), 2, false, "4A" },
                    { new Guid("fe87c0ff-c9e2-4d2f-88f5-86e70fced097"), new Guid("e40a19a5-582b-4e56-9409-d9c4c18331e2"), 0, false, "7A" },
                    { new Guid("fe920575-3fcc-474d-a8db-7022e9d88c9d"), new Guid("b3fb016d-06cb-405c-ad64-adf591475a78"), 0, false, "5D" },
                    { new Guid("fed8cf2c-74d5-4f31-9398-f1dc1fb2a4cd"), new Guid("f06e709a-645f-4500-81af-674ecf0dd6f1"), 0, false, "10F" },
                    { new Guid("ff5587b7-72b6-40d2-a0fc-098fef4a2ec5"), new Guid("97046ae8-a72a-49c3-887c-8fc9a04b660c"), 3, false, "1A" }
                });

            migrationBuilder.InsertData(
                table: "Conditions",
                columns: new[] { "ConditionId", "Category", "ConditionType", "ConditionValue", "DiscountId", "Property" },
                values: new object[,]
                {
                    { new Guid("132307ff-e1c1-4409-96dc-0817c2ab0e5a"), 5, 0, "Thursday", new Guid("c4b192a2-41dc-4f3f-b484-fb8647692db9"), "self" },
                    { new Guid("33a3dc6c-18db-455f-a444-37081860229a"), 1, 0, "Africa", new Guid("c4b192a2-41dc-4f3f-b484-fb8647692db9"), "Continent" },
                    { new Guid("b0280dd4-a7d0-4253-9afc-fc4b4b3ed60d"), 0, 0, null, new Guid("f33611eb-613b-466f-8be5-82b0e8866f28"), "Birthday" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aircrafts_RegistrationNumber",
                table: "Aircrafts",
                column: "RegistrationNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AircraftSeats_AircraftId",
                table: "AircraftSeats",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_Airport_IATA",
                table: "Airports",
                column: "IATA",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Airport_ICAO",
                table: "Airports",
                column: "ICAO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conditions_DiscountId",
                table: "Conditions",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_TicketId",
                table: "Discounts",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSchedules_AircraftId",
                table: "FlightSchedules",
                column: "AircraftId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSchedules_AirlineId",
                table: "FlightSchedules",
                column: "AirlineId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSchedules_DestinationAirportId",
                table: "FlightSchedules",
                column: "DestinationAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSchedules_OriginAirportId",
                table: "FlightSchedules",
                column: "OriginAirportId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeats_FlightScheduleId",
                table: "FlightSeats",
                column: "FlightScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightSeats_TicketId",
                table: "FlightSeats",
                column: "TicketId",
                unique: true,
                filter: "[TicketId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TenantId",
                table: "Tickets",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftSeats");

            migrationBuilder.DropTable(
                name: "Conditions");

            migrationBuilder.DropTable(
                name: "FlightSeats");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "FlightSchedules");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Aircrafts");

            migrationBuilder.DropTable(
                name: "Airlines");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
