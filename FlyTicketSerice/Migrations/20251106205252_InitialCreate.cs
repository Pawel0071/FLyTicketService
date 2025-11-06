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
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    { new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), "Airbus A380", "SP-LLL" },
                    { new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), "Boeing 737-MAX", "SP-LAA" },
                    { new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), "Airbus A320Neo", "SP-LBG" },
                    { new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), "Boeing 777-300ER", "SP-LRR" },
                    { new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), "Boeing 787-9", "SP-LCC" },
                    { new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), "Boeing 747-8", "SP-LQQ" },
                    { new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), "Airbus A320", "SP-LOO" },
                    { new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), "Boeing 737 MAX", "SP-LRA" },
                    { new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), "Boeing 777-300ER", "SP-LLA" },
                    { new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), "Airbus A380", "SP-LKK" },
                    { new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), "Boeing 767", "SP-LPP" },
                    { new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), "Boeing 787-9", "SP-LGG" }
                });

            migrationBuilder.InsertData(
                table: "Airlines",
                columns: new[] { "AirlineId", "AirlineName", "Country", "IATA" },
                values: new object[,]
                {
                    { new Guid("140e9f43-6b0e-4223-9f2f-a607068250c0"), "Delta Airlines", "USA", "DL" },
                    { new Guid("32d7cf76-c843-49d9-94a6-f37806716f96"), "Emirates", "UAE", "EK" },
                    { new Guid("3b5630f9-f5bb-439d-ad98-bfd2756accd2"), "American Airlines", "USA", "AA" },
                    { new Guid("50cdc4ba-8e6f-4a7f-8790-1a88ceee9612"), "Singapore Airlines", "Singapore", "SQ" },
                    { new Guid("7f8beaf9-dd3f-4f40-b7be-532d119e040d"), "Qatar Airways", "Qatar", "QR" },
                    { new Guid("9694c0b0-2d6f-4b56-8a4f-620bdf738bea"), "Cathay Pacific", "Hong Kong", "CX" },
                    { new Guid("a98373c9-19bd-496e-be9f-48d8264def35"), "Air India", "India", "AI" },
                    { new Guid("bfa8bb8c-9b1a-4234-bc6e-3e2640942715"), "LOT", "Poland", "LO" },
                    { new Guid("cf695482-48bc-4d8c-aaad-8d60f1eac323"), "Qantas", "Australia", "QF" },
                    { new Guid("fd877c95-e15b-40cc-ad9f-5497e3902d9b"), "Virgin Australia", "Australia", "VA" }
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "AirportId", "AirportName", "Altitude", "City", "Continent", "Country", "DST", "IATA", "ICAO", "Latitude", "Longitude", "Timezone" },
                values: new object[,]
                {
                    { new Guid("03a0ac6d-b6f4-44c5-a717-4a02b55bf27e"), "Warsaw Chopin Airport", 362.0, "Warsaw", "Europe", "Poland", 0, "WAW", "EPWA", 52.165700000000001, 20.967099999999999, 3 },
                    { new Guid("052066c9-ebea-45e7-973f-049395765cc6"), "Frankfurt Airport", 364.0, "Frankfurt", "Europe", "Germany", 0, "FRA", "EDDF", 50.0379, 8.5622000000000007, 3 },
                    { new Guid("0e80e0cb-e9cc-4fcd-8dfc-26c3c00665c7"), "Istanbul Airport", 325.0, "Istanbul", "Europe", "Turkey", 5, "IST", "LTFM", 41.275300000000001, 28.751899999999999, 38 },
                    { new Guid("17840e0f-ff79-429f-b54b-f129db542522"), "Sydney Kingsford Smith Airport", 6.0, "Sydney", "Oceania", "Australia", 5, "SYD", "YSSY", -33.946100000000001, 151.1772, 18 },
                    { new Guid("29c89269-725b-40a2-93f5-0b98212d519a"), "Seattle-Tacoma International Airport", 433.0, "Seattle", "North America", "United States", 1, "SEA", "KSEA", 47.450200000000002, -122.30880000000001, 11 },
                    { new Guid("2a21ad4d-293d-4660-bb2a-9a0492d2aee7"), "O. R. Tambo International Airport", 5558.0, "Johannesburg", "Africa", "South Africa", 5, "JNB", "FAOR", -26.133800000000001, 28.2425, 39 },
                    { new Guid("3333727f-79c9-4861-8b91-f6439370fb67"), "John F. Kennedy International Airport", 13.0, "New York", "North America", "United States", 1, "JFK", "KJFK", 40.641300000000001, -73.778099999999995, 5 },
                    { new Guid("3f5b683e-794c-4465-9d8e-3072b582ebe2"), "Amsterdam Airport Schiphol", -11.0, "Amsterdam", "Europe", "Netherlands", 0, "AMS", "EHAM", 52.308599999999998, 4.7638999999999996, 3 },
                    { new Guid("48fe72b8-6456-4ba0-8401-2d27ade771c6"), "Los Angeles International Airport", 125.0, "Los Angeles", "North America", "United States", 1, "LAX", "KLAX", 33.942500000000003, -118.4081, 11 },
                    { new Guid("4dcdb1ae-0f8f-4e4a-99b2-c270b785d7b2"), "Adolfo Suárez Madrid–Barajas Airport", 1998.0, "Madrid", "Europe", "Spain", 0, "MAD", "LEMD", 40.471899999999998, -3.5626000000000002, 3 },
                    { new Guid("4fbe2f3b-dd3f-4cad-abe7-f0b6d0e1a67c"), "Dallas/Fort Worth International Airport", 607.0, "Dallas-Fort Worth", "North America", "United States", 1, "DFW", "KDFW", 32.896799999999999, -97.037999999999997, 7 },
                    { new Guid("55ad9f76-10f3-42aa-b0de-d15d2482d07c"), "Hartsfield-Jackson Atlanta International Airport", 1026.0, "Atlanta", "North America", "United States", 1, "ATL", "KATL", 33.636699999999998, -84.428100000000001, 5 },
                    { new Guid("5ccdc3aa-427e-4ecf-b001-2a0cc55fa2bf"), "Leonardo da Vinci–Fiumicino Airport", 13.0, "Rome", "Europe", "Italy", 0, "FCO", "LIRF", 41.8003, 12.238899999999999, 3 },
                    { new Guid("5d977dbd-c2bd-4406-9e79-b82de54bbf10"), "San Francisco International Airport", 13.0, "San Francisco", "North America", "United States", 1, "SFO", "KSFO", 37.6188, -122.375, 11 },
                    { new Guid("63da3d3c-2dda-4bde-a8ad-f8ddfe1f591a"), "Melbourne Airport", 132.0, "Melbourne", "Oceania", "Australia", 5, "MEL", "YMML", -37.673299999999998, 144.8433, 18 },
                    { new Guid("7b6b77ff-e0e8-48b2-974b-52da37b76c15"), "Heathrow Airport", 83.0, "London", "Europe", "United Kingdom", 0, "LHR", "EGLL", 51.470599999999997, -0.46189999999999998, 29 },
                    { new Guid("8626eeb3-4ee2-4674-8709-e221c28ad347"), "Brisbane Airport", 13.0, "Brisbane", "Oceania", "Australia", 5, "BNE", "YBBN", -27.3842, 153.11750000000001, 18 },
                    { new Guid("8d36c64b-f118-414a-9908-a54fd39676db"), "Hamad International Airport", 13.0, "Doha", "Asia", "Qatar", 5, "DOH", "OTHH", 25.273099999999999, 51.6081, 30 },
                    { new Guid("8dc70ab9-0130-4572-a168-f6df06171b80"), "Charles de Gaulle Airport", 392.0, "Paris", "Europe", "France", 0, "CDG", "LFPG", 49.009700000000002, 2.5478999999999998, 3 },
                    { new Guid("a91b108f-323c-4fd0-866e-9a8d16beed1e"), "Zurich Airport", 1416.0, "Zurich", "Europe", "Switzerland", 0, "ZRH", "LSZH", 47.464700000000001, 8.5492000000000008, 3 },
                    { new Guid("b4a44c92-d4dd-4a91-85d6-74a55ed07cb4"), "Cairo International Airport", 382.0, "Cairo", "Africa", "Egypt", 5, "CAI", "HECA", 30.1219, 31.4056, 21 },
                    { new Guid("b810abb5-e418-4212-bf47-9a5723b4f2af"), "Denver International Airport", 5431.0, "Denver", "North America", "United States", 1, "DEN", "KDEN", 39.861699999999999, -104.67310000000001, 9 },
                    { new Guid("d28c803e-32a6-4e65-854a-0f49c002267f"), "Beijing Capital International Airport", 116.0, "Beijing", "Asia", "China", 5, "PEK", "ZBAA", 40.080100000000002, 116.58459999999999, 6 },
                    { new Guid("d65a6595-d262-422c-900f-f46c80825e7b"), "Munich Airport", 1487.0, "Munich", "Europe", "Germany", 0, "MUC", "EDDM", 48.3538, 11.786099999999999, 3 },
                    { new Guid("dd8c0dd0-81f8-4cb4-894f-a0a606527d70"), "John Paul II International Airport Kraków–Balice", 791.0, "Kraków", "Europe", "Poland", 0, "KRK", "EPKK", 50.0777, 19.784800000000001, 3 },
                    { new Guid("e217df27-96d6-430e-8219-fe0f667ddae8"), "Addis Ababa Bole International Airport", 7630.0, "Addis Ababa", "Africa", "Ethiopia", 5, "ADD", "HAAB", 8.9770000000000003, 38.798999999999999, 40 },
                    { new Guid("e54282b0-62cb-4ef8-ac20-e8ac539f075e"), "Cape Town International Airport", 151.0, "Cape Town", "Africa", "South Africa", 5, "CPT", "FACT", -33.964799999999997, 18.601700000000001, 39 },
                    { new Guid("e76f3f9a-0269-436e-a395-e24ef2b595aa"), "Hong Kong International Airport", 28.0, "Hong Kong", "Asia", "Hong Kong", 5, "HKG", "VHHH", 22.308900000000001, 113.91459999999999, 41 },
                    { new Guid("e89b549c-ef7c-4a06-a1e2-328b121b23c5"), "Miami International Airport", 8.0, "Miami", "North America", "United States", 1, "MIA", "KMIA", 25.7959, -80.287000000000006, 5 },
                    { new Guid("f15b8ef1-2b32-4c7c-be0e-93a8d497002d"), "Barcelona–El Prat Airport", 12.0, "Barcelona", "Europe", "Spain", 0, "BCN", "LEBL", 41.297400000000003, 2.0832999999999999, 3 },
                    { new Guid("fcc9975b-b0aa-4d6b-8e6b-6d2efb19e3ec"), "O'Hare International Airport", 672.0, "Chicago", "North America", "United States", 1, "ORD", "KORD", 41.9786, -87.904799999999994, 7 },
                    { new Guid("ff78e335-1dcf-43b5-b440-b983ae811c8e"), "Tokyo Narita International Airport", 41.0, "Tokyo", "Asia", "Japan", 5, "NTR", "RJAA", 35.771999999999998, 140.392, 17 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "DiscountId", "Description", "Name", "TicketId", "Value" },
                values: new object[,]
                {
                    { new Guid("72fd9fbf-ee02-4016-93da-a9ea628c1f42"), "Discount applied if the purchase date matches the tenant's birthday.", "Birthday Discount", null, 0m },
                    { new Guid("d9d0bf82-fa9f-4b73-8ab9-c7077d951df2"), "Discount applied if the flight destination is in Africa on Thursday.", "Thursday Africa Discount", null, 0m }
                });

            migrationBuilder.InsertData(
                table: "AircraftSeats",
                columns: new[] { "AircraftSeatId", "AircraftId", "Class", "OutOfService", "SeatNumber" },
                values: new object[,]
                {
                    { new Guid("0097d626-65fc-4766-91f0-3fd82d473145"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "9C" },
                    { new Guid("00a4128c-73d5-44e5-86a9-233844a0da57"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "5B" },
                    { new Guid("01a1cc1d-089d-4c6f-8fcd-f48bb7132969"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "9E" },
                    { new Guid("01e4510f-4bea-4169-bbdb-521432363396"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "4E" },
                    { new Guid("02d69444-4efe-4711-b88a-e9aaeb9c0065"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "4A" },
                    { new Guid("031f99de-7092-4aa5-a587-2036bcbc93ac"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "10C" },
                    { new Guid("0361e374-b284-4432-87cd-6b669bea683a"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "6B" },
                    { new Guid("03ab014a-7bb4-4c54-b15a-3734c493bfc6"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "5A" },
                    { new Guid("03dab844-2610-4cc4-a9ca-6555f5fdabf1"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "4C" },
                    { new Guid("0449bb38-40d8-4e87-bc77-4a7084b503ee"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "8F" },
                    { new Guid("044e0c88-b92f-4fb0-b4a9-1051e8f5ba3d"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "10F" },
                    { new Guid("04ac083d-4f2c-4ad5-933f-554fc5cea82f"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "6B" },
                    { new Guid("04f4acf9-fc29-4cff-b294-6c8dee3d7501"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "8C" },
                    { new Guid("05ff85bc-5be7-4f95-8355-4f805d5a7d91"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "6B" },
                    { new Guid("065c8d20-72e6-469a-9c49-5a66e8d782ea"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "4C" },
                    { new Guid("06ec219e-9ad4-494d-a5c1-0c9ed1eca70b"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "7A" },
                    { new Guid("0713ed41-b870-4ed8-b22f-3990f8cc22a1"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "4D" },
                    { new Guid("07d0ff08-bb6c-4f43-ad4b-4a477938b4bc"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "6B" },
                    { new Guid("089a7be9-d30a-4848-8780-89441c9c955f"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "5E" },
                    { new Guid("08ada30a-4e87-4262-8923-27c43eb33f78"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "4A" },
                    { new Guid("0a29615e-1263-4cbf-a24c-b2de6b5392a6"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "4F" },
                    { new Guid("0a6a0e16-ccba-4d64-860e-0bf5b86b7d6b"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "9C" },
                    { new Guid("0b9e5b52-d391-4116-9c29-bb8b09136c16"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "4D" },
                    { new Guid("0c431f3f-5095-4f13-8291-f53366bae7aa"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "9F" },
                    { new Guid("0c8269bb-7cec-4989-8526-cb14f7575f5e"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "9C" },
                    { new Guid("0d597c3f-7cb8-4ef8-8672-f21068342f65"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "10F" },
                    { new Guid("0eb8ed99-beac-40ff-bad6-e0639794b5e9"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "10A" },
                    { new Guid("0f3f2782-1e97-45df-b581-f8fb30a7da4c"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "7C" },
                    { new Guid("0fe3a846-02e0-4172-8605-a81a0ad2af02"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "4E" },
                    { new Guid("104607f9-1d91-4032-b0d0-7a8809bfa84a"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 2, false, "2F" },
                    { new Guid("108963e2-2063-490a-a152-e7c275db4dbd"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "10B" },
                    { new Guid("10a6d2d7-0418-4ecb-b098-f411358b426f"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "11F" },
                    { new Guid("1135ce2e-cf33-42de-b353-22ec4d560b62"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "11D" },
                    { new Guid("11bb1ddf-b4d1-432e-a806-2d602e4e6ff4"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "3A" },
                    { new Guid("11e6939c-5438-4250-a5b6-9b85f271e636"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "4C" },
                    { new Guid("12aff332-0ffc-4628-b5ea-548b6a744ac2"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "7B" },
                    { new Guid("13510593-d41c-447f-8d17-9f6927d1f764"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "7F" },
                    { new Guid("13906ef9-41a0-4137-8aaa-a5d27bfa472b"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "11F" },
                    { new Guid("142f4693-c837-4f86-a757-400a01792321"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "7A" },
                    { new Guid("1580a312-5c62-47f4-ab77-617d6c2f2494"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "10D" },
                    { new Guid("15ce3b82-1363-43e0-a039-f7a1bb0507cb"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "7B" },
                    { new Guid("15f35ea9-c634-4247-a4e2-3c66c3315a52"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "5F" },
                    { new Guid("163a63b9-2598-4531-9056-fd99a718927e"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "2B" },
                    { new Guid("16ac650d-27c0-4707-8816-e27d2bd8a903"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "5C" },
                    { new Guid("1727f7fc-729e-4ca6-b43b-f00c6044c4c0"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "5B" },
                    { new Guid("175bedcd-d1e1-413f-87ca-96128d4952b1"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "4C" },
                    { new Guid("17ab60cf-36f8-4a50-92de-76310f93682e"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "6D" },
                    { new Guid("17ba37e5-97f4-4308-b19e-9b3f1169644a"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "6F" },
                    { new Guid("17de3526-8e84-4ea6-8b2f-0b97200e7e53"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "5E" },
                    { new Guid("182bab67-a75c-4b79-967a-83955ac9935b"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 3, false, "1B" },
                    { new Guid("194343b6-639c-4415-ae1e-6bdad298f9c2"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "11C" },
                    { new Guid("197f7f9b-caa9-475d-b6ea-e96176b69490"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "5B" },
                    { new Guid("19b1ce5c-6444-4457-ab2e-3262092bb5d8"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "5F" },
                    { new Guid("1ab64283-d355-4177-986b-f08c99138df0"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 3, false, "1F" },
                    { new Guid("1ae06b01-40da-4d5d-a569-42335e80b33d"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "10F" },
                    { new Guid("1b0bbd0f-53e9-4284-9085-3c0e8a58d44b"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "10B" },
                    { new Guid("1b200ff1-7760-43af-82cb-01872b3795a5"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "6D" },
                    { new Guid("1b548fd9-905e-41bb-a3bd-d3e318ca2b9a"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "5E" },
                    { new Guid("1bc8b138-0574-40df-a5f1-f3a9897e359e"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 2, false, "2A" },
                    { new Guid("1c44c8bf-fc06-4d24-8b12-fa67fa0aec01"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "11B" },
                    { new Guid("1c9bdd6a-dbbc-4877-b1e9-876bd802fb3a"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "6E" },
                    { new Guid("1ca52808-5229-44a8-bc3e-661e3bf7b66a"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 3, false, "1C" },
                    { new Guid("1cec19a9-1150-4a65-89a4-e1f02218933c"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "2A" },
                    { new Guid("1df67982-84a8-498c-8585-f88ab89b72a8"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "3E" },
                    { new Guid("1e42aafc-049b-4dec-ae7a-604253f57836"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "4D" },
                    { new Guid("1eb9ca8b-3e9c-46c4-9f94-c0c316929ae6"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "4D" },
                    { new Guid("1eb9e494-830f-4863-82e5-8333d812ac4b"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "11A" },
                    { new Guid("1f00ee8e-41a0-43d0-8ba4-29e099fa4bfc"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "9A" },
                    { new Guid("1f1d0e5d-5ef0-416f-b288-9a0156b6a8f4"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "7E" },
                    { new Guid("1fb8cf48-e040-4f59-997a-b64d48f5df85"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "11C" },
                    { new Guid("1fe84045-f82c-4495-9589-d21cbb834d2d"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "7E" },
                    { new Guid("1fea79f8-de84-40ad-82ed-12941580e345"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "7A" },
                    { new Guid("20db5d37-3b94-449b-aab4-8a678bd1ca09"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 2, false, "2E" },
                    { new Guid("2180b4cf-45f2-4b38-8e1c-43b046aaa114"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "4A" },
                    { new Guid("21dc1d6b-4133-486d-9232-72e141293eae"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "8C" },
                    { new Guid("22bf27a3-47b3-44e1-abd5-99af9e088efe"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "4E" },
                    { new Guid("23b36098-ef58-4575-a8e9-fffd3abd7441"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "3A" },
                    { new Guid("247e75e3-5cad-4f91-8aa7-de7cdcb8a1fc"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "5C" },
                    { new Guid("24c0d315-7aa5-4042-b63b-f7b6d8ca54e3"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "7A" },
                    { new Guid("25653819-9aab-4cbf-aac8-36e62ae05b80"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 2, false, "2A" },
                    { new Guid("25731146-c142-4518-8ba9-549435a962d2"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "7F" },
                    { new Guid("25a3dd6c-2c27-4a57-8cb8-cb02c2d57703"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "4D" },
                    { new Guid("25da6ac3-3017-49ba-80b0-f4b1359ef417"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "9B" },
                    { new Guid("262b1e8a-b0a9-4c5d-82f3-a516363f6596"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "9A" },
                    { new Guid("262ee40c-1ba8-4d49-83cc-3da1cb2716ec"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "7C" },
                    { new Guid("262fd280-f162-4aff-85d7-c76c7e1deefe"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "8B" },
                    { new Guid("277df0e8-9626-4e00-a9b3-9101965cff4b"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "3F" },
                    { new Guid("2795cf82-68e8-41a2-98f8-092c62bec58e"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "3B" },
                    { new Guid("27e447b1-4ed2-4d67-a21a-059395031786"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 3, false, "1C" },
                    { new Guid("27f9e744-5180-42fd-8285-17a1764b629e"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "2A" },
                    { new Guid("283bc6b7-cf77-44e9-bb87-d55a17350cce"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "10D" },
                    { new Guid("285017aa-4086-4ed3-b330-aae8d5673bc7"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "5A" },
                    { new Guid("29228638-673a-41d4-be79-8a0e9feeaf01"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "7E" },
                    { new Guid("29780f66-1cd8-4e1d-ad6c-e73ace2443ea"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "5D" },
                    { new Guid("29cec334-e1ee-4cc8-b7f9-bde27ca80ebe"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, true, "7A" },
                    { new Guid("29e7253f-241b-4e69-bf8d-31d0b729a3f6"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "3E" },
                    { new Guid("2a3bfa32-2c01-4c12-a964-049dcfe9133d"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "10B" },
                    { new Guid("2a446f77-3aa6-47a6-91e6-64c04273121d"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "5D" },
                    { new Guid("2a58e41d-3ec3-4ea3-bbf6-bad30cc49336"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "9B" },
                    { new Guid("2ad4a26e-364a-4b17-8d53-9c5ae942de27"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "2F" },
                    { new Guid("2b7167fa-4b7d-4dd3-a118-82577055583e"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "6A" },
                    { new Guid("2b7d778e-7e6b-4443-8546-a9778525e78d"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "4D" },
                    { new Guid("2c010a3b-fb47-4e87-8abb-63c6e8ed6969"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "11A" },
                    { new Guid("2c642e0e-564a-4bde-89c7-5d4971e8970a"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "7E" },
                    { new Guid("2c9105fe-85db-44df-b54c-fd6e2ab484c3"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "9B" },
                    { new Guid("2ceac67c-001f-4160-bdf1-10a84befd8fa"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 2, false, "2D" },
                    { new Guid("2d8a4951-c36b-4145-91a8-83c56b75026a"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "6D" },
                    { new Guid("2da12f06-6960-408d-8262-38985bbc8cdb"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 3, false, "1A" },
                    { new Guid("2dc1a8ea-3b05-4866-b7ec-ec726111c97c"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "5C" },
                    { new Guid("2e3cddd7-5a2e-4dea-ac21-b03a4cdc748f"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "6A" },
                    { new Guid("2e44cf32-2d10-4a3f-8392-f60a53e88a93"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "4E" },
                    { new Guid("304ed7c7-deb1-4298-b861-f4ae7546bb55"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "3B" },
                    { new Guid("30d33e03-4faa-4afc-9c25-3e88fe0ed89a"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "10A" },
                    { new Guid("30d5bfd9-aab2-4bf9-a4da-08858e1a21e9"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "8C" },
                    { new Guid("313a7ac2-9fdc-48b8-a252-a77c37454dab"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "11C" },
                    { new Guid("31975a36-79a0-4768-960b-5d3c0e5c2e9b"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "4B" },
                    { new Guid("31ce96bc-3b04-4c42-b17a-5f28d97dcf4a"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "11D" },
                    { new Guid("327aaa74-86a5-415b-afba-b8a57ed377f6"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "11F" },
                    { new Guid("3363cd4d-395c-425a-979e-391976de9701"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "5D" },
                    { new Guid("3384b60f-4f2e-4660-932a-7cf81d96ab37"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "4F" },
                    { new Guid("33cdc424-fa83-48de-b772-3b80b915ebe0"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "4F" },
                    { new Guid("340de62b-4784-4ea3-9451-fceb08b31854"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "4F" },
                    { new Guid("3438f231-5e3f-4cdf-887b-6f661bc9cad5"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "4A" },
                    { new Guid("344b6ac2-a48c-4636-9345-2934ac16f983"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "3D" },
                    { new Guid("344f0d82-45e6-43cb-8823-a3e1dc6f82fb"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "3A" },
                    { new Guid("3657ce91-5a53-41a4-9163-bb2aad592d2a"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "4B" },
                    { new Guid("36687f3f-7943-476d-a690-70336c914d60"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "11E" },
                    { new Guid("36889bbc-ead0-40f5-bfa4-1390e93ef392"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 3, false, "1B" },
                    { new Guid("372be52b-c664-4158-803c-7662d4263bec"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "10E" },
                    { new Guid("37414c25-5550-401b-806c-8f791eb8fd36"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "10B" },
                    { new Guid("38c4a42f-4fb1-40cf-adbc-8f583b6809d3"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "6C" },
                    { new Guid("38f0de74-f258-476c-8cc9-0a8317a879b7"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "7E" },
                    { new Guid("3929dc3a-633b-4459-9c49-913deff79b12"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "6A" },
                    { new Guid("3a1f74e9-400e-4756-b968-257af2fafa93"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "11C" },
                    { new Guid("3a82cc38-04d8-4a92-9d97-33c26f29d828"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "3C" },
                    { new Guid("3a8369e1-bfa0-430c-a4f6-b4e70d9c91f2"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "1C" },
                    { new Guid("3aa70290-b31a-4925-9c53-4d144ac8f742"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "9F" },
                    { new Guid("3acd07a4-148b-4770-a43f-44749c0bffe2"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "4A" },
                    { new Guid("3bd48f44-7ffd-49a4-9898-18def489e072"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "6F" },
                    { new Guid("3c1d2cee-f1a2-4665-a514-b88e2c68ef9c"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "6B" },
                    { new Guid("3c89b8da-189c-4c5c-bbb0-0387e0e5b2fe"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "5F" },
                    { new Guid("3c8afc3f-7773-409c-97e0-5e46abda376e"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "11A" },
                    { new Guid("3cb631c8-5aaf-4dcb-9b1a-85e9bfd6db7b"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 3, false, "1E" },
                    { new Guid("3cc6f2e1-6827-4d92-b93b-38ce9248b1fa"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "1E" },
                    { new Guid("3d2928f0-b008-4776-adb5-40245a0bd93e"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "6C" },
                    { new Guid("3d679a51-52b0-4862-8a40-80f923300699"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "10A" },
                    { new Guid("3da1bee3-3256-4fb9-a51a-33c572fe3805"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 3, false, "1A" },
                    { new Guid("3dad67cc-d986-4c81-ba25-4500fd34ba3d"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "5A" },
                    { new Guid("3e9579ed-bc2f-412f-96ab-08c3c9607e61"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "8F" },
                    { new Guid("3f95c785-b0a6-45c6-a821-16b89decf46c"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "10F" },
                    { new Guid("402b8101-ed3c-4859-87ea-df4a5850e1dc"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 2, false, "2A" },
                    { new Guid("40484fb2-6d5f-4191-9b75-42c6fad07ab4"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "3B" },
                    { new Guid("406c14ba-3bf1-4be5-a97b-eb6159cea576"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "11B" },
                    { new Guid("40976613-c6ec-405a-8eb8-07c28d8d4f34"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "11C" },
                    { new Guid("415f749b-d42a-482b-97ef-877c5e7a60cd"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 3, false, "1F" },
                    { new Guid("417e6e4d-c58b-427f-8410-f704ef3d4777"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 3, false, "1E" },
                    { new Guid("429770ec-47d7-42c0-9663-6aba083d8d62"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "11E" },
                    { new Guid("42de7419-a4da-4461-9e3a-ea63adaa2c60"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "11B" },
                    { new Guid("43337085-3b64-4462-bfc4-437c932d228e"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "5C" },
                    { new Guid("43675d1d-d4ca-4394-a782-ec33dcf1a6c8"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "5A" },
                    { new Guid("438760f1-6358-4a73-b2e2-c1ab4a39151a"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 3, false, "1D" },
                    { new Guid("43b676e0-e327-4d52-b5ac-2080206e01ba"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "7C" },
                    { new Guid("44251e08-75cb-4956-8f6d-4185680a1541"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "9E" },
                    { new Guid("446c8ffc-bd4b-40df-a9c7-70c9806c3183"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "8B" },
                    { new Guid("44754d35-837b-43e9-83f0-cc767d18098f"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "1D" },
                    { new Guid("44d61506-9793-490f-afbf-956308a7574b"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "7D" },
                    { new Guid("45618bb7-082d-475c-bb3b-c9e6f387e243"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "11F" },
                    { new Guid("458a2067-1d07-4aa3-a4ea-2ed85087f133"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "4B" },
                    { new Guid("45f3446a-de4c-4b1a-83e9-00099ce5bfc3"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "6A" },
                    { new Guid("470dcb13-b82c-4a6f-ad0d-47474a5c43af"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "2D" },
                    { new Guid("48114cd6-8045-4240-ba6f-bdd1ea1a7cfc"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "8D" },
                    { new Guid("48bba9b2-b07b-4105-9058-27ceb30159d5"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "2B" },
                    { new Guid("48e2ea2a-2aa1-4e93-825b-ee5c274faaf2"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "5E" },
                    { new Guid("48f612ac-dacd-4b77-a5d2-ae8eadb22c8a"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "5A" },
                    { new Guid("49b126fc-d357-4e9b-99a4-ffbf89f1798e"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "5C" },
                    { new Guid("49b8fe5f-2b6f-4b75-a447-ac7a21d9e15d"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 3, false, "1A" },
                    { new Guid("49d222f6-2903-45b2-aaa8-44aee2620046"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "10E" },
                    { new Guid("4a4f6c5f-66c0-41a2-b954-e50f8d3a02a7"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "10C" },
                    { new Guid("4a602440-0414-44f4-b552-4f4c0fd700a2"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "9E" },
                    { new Guid("4a9ee6d9-e245-4713-8d02-7354b12ca7d9"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "9B" },
                    { new Guid("4acb8b07-47f0-48bc-95fe-a680859cd97e"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "5C" },
                    { new Guid("4b92ef39-d94f-4eff-b8a8-d4e24e1f1f84"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "11E" },
                    { new Guid("4c2b6170-5dc4-4caa-9eb0-2fc8fe2fe336"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 3, false, "1B" },
                    { new Guid("4c9bb1b9-1665-4f2a-8a97-4d19264e200b"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "5B" },
                    { new Guid("4d7b5941-0e04-4a93-9e72-5febf1d2d94d"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "11D" },
                    { new Guid("4d7e43a7-4cd9-4d1f-b10a-267cd331f563"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "5A" },
                    { new Guid("4e728932-65da-4163-be0c-9b25aa4de3da"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "4C" },
                    { new Guid("4f24daf5-1a1c-4edd-a839-c31d4386f76c"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "8E" },
                    { new Guid("4f677f35-f23f-45ad-9eda-90cb765db116"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "8E" },
                    { new Guid("5078e10c-aca7-46c7-8824-6519d9770efb"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "3E" },
                    { new Guid("50a82d93-ae8a-416b-8cbb-100fa0ba7c2a"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "8D" },
                    { new Guid("5135d5ef-fd98-4335-8c00-bb9b89a84edd"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "7D" },
                    { new Guid("526e2ab7-2690-4e6d-9a41-51c909e5cdcd"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "9B" },
                    { new Guid("527deb47-7f0f-4109-a26f-2fb0488048ab"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "9F" },
                    { new Guid("5398f20d-63ba-448b-b302-a0fe25a98581"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "1F" },
                    { new Guid("54b120a7-c52a-4318-8f0a-8172995684f0"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "7D" },
                    { new Guid("54c7d025-fb6b-47f7-997a-05aca5fceb48"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "3C" },
                    { new Guid("54d77156-2db5-4edb-8231-6eeedf4729c1"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "5E" },
                    { new Guid("55b4daf8-8b75-4874-a07d-3dd1a5360700"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "6E" },
                    { new Guid("56a7028a-9c9a-4f07-8897-03bdcdf0c644"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "6F" },
                    { new Guid("56af8f17-6c07-46f0-8ba5-d92edd6186a0"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "9F" },
                    { new Guid("5727eb5c-1e15-4100-8433-12c1165595b3"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "9E" },
                    { new Guid("57574f6d-faab-49b5-b393-d4c69b36986c"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 2, false, "2C" },
                    { new Guid("5870cb17-d9ab-41f7-91f8-695696cd3671"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "6D" },
                    { new Guid("599fd186-50d5-46de-be99-26243ff3bee0"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "3A" },
                    { new Guid("59e14c65-cd38-4aeb-b0b4-4f9c0036480e"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "7F" },
                    { new Guid("5a4e4ecc-cd72-423b-9f70-d180fc5e8d43"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 3, false, "1E" },
                    { new Guid("5ae9b149-cb1c-4684-b430-30a6119e8497"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "10A" },
                    { new Guid("5aef093b-e80f-45a5-a17e-3de8c81d8e79"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "5F" },
                    { new Guid("5b21be7c-766d-433a-b3ab-1849c7d279b6"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "7E" },
                    { new Guid("5b45babe-b09d-49fc-9cb4-bea56e98b05e"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "5B" },
                    { new Guid("5b8be7ee-efff-4c55-9d42-7c472783ea00"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "2B" },
                    { new Guid("5b9e2246-fdcc-4304-9d60-e122ca484143"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "10A" },
                    { new Guid("5ba73ac2-41b2-46be-a2a5-d0e567042035"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "7E" },
                    { new Guid("5bf96c06-e86b-4da0-b9a6-daa49154353c"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "8F" },
                    { new Guid("5c58f756-8807-4023-b2cb-5cef14099b33"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "4B" },
                    { new Guid("5cf3037d-b29f-460f-8536-5d4343e02b9a"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 3, false, "1D" },
                    { new Guid("5d04d8ca-2cd1-4c1c-bca8-5ca2ebde8ab5"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "10C" },
                    { new Guid("5d12f393-502d-4462-9a3f-539a0458545b"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "9A" },
                    { new Guid("5d3698fb-6c75-4ba0-a169-742d8e9d1a2f"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "3B" },
                    { new Guid("5d478d30-b51f-4dba-9021-179c8d95ee07"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "6E" },
                    { new Guid("5d4f145f-8989-4c77-b8ac-205267b7b051"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "4E" },
                    { new Guid("5d8ac25d-b380-4b00-8528-88a55b7ebec2"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "6A" },
                    { new Guid("5dbc4443-6b88-479c-bd42-9680f294aa05"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 3, false, "1A" },
                    { new Guid("5de947fa-ae44-4ad0-968e-f391369625fb"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "6A" },
                    { new Guid("5e33d5f7-1461-4f99-a78f-ca19286b96c2"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "8A" },
                    { new Guid("5e533f0a-9c00-40ae-985c-d7a2ed639492"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "6D" },
                    { new Guid("5e608f83-6cbb-4055-83fe-0fe71b89d2ca"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "6C" },
                    { new Guid("5ea1b76b-412a-4f93-a5cf-6b22503223d7"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "6B" },
                    { new Guid("5ea3cd78-2373-4d54-96fc-51bf42f8101a"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "4A" },
                    { new Guid("5ea88264-6857-4245-bd70-ac1d372faf10"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "9A" },
                    { new Guid("5ec706a2-8133-4855-b47b-580eb58c35f1"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "10B" },
                    { new Guid("5f4fb867-783f-4713-af94-b9826e6dbf00"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "9B" },
                    { new Guid("6080147b-3198-4df0-94d0-bcfa1a886052"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "7D" },
                    { new Guid("60812cd7-7a6e-4906-9f21-5b6e7ec11e93"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "5F" },
                    { new Guid("60ab84c4-6382-45e5-877b-bb2aa4322c5d"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "9E" },
                    { new Guid("630f74d0-1bc1-4bf4-af41-e92c6b246dbf"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "6C" },
                    { new Guid("638abd34-1e34-4ca8-a25c-08499abb16c4"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "5D" },
                    { new Guid("63c1e391-4ae6-4685-9972-c9f045a66f89"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "2E" },
                    { new Guid("63e306b1-3671-45dd-8334-caf72b27526e"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "11E" },
                    { new Guid("644f9077-c15e-487e-bb0f-4edeb89074ca"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "9A" },
                    { new Guid("64527706-0c18-4466-ba71-ec491be4ec62"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "5E" },
                    { new Guid("6491a78b-9656-4663-9c4b-bfd959e742c2"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "9E" },
                    { new Guid("64be7052-1532-4cb3-87ee-1222ca61ea9e"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "6C" },
                    { new Guid("65407ebc-06e0-4904-87d2-cf665cfb3c4a"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "5C" },
                    { new Guid("65b600d8-2aa8-4a87-9dad-06721173edd0"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "8A" },
                    { new Guid("67440118-086f-4da5-8ffd-905400a77c15"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "8F" },
                    { new Guid("6758a686-247a-479a-a964-45581dde56f5"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "8F" },
                    { new Guid("6799fb93-0a8f-4c3e-80a1-c8e0e521091f"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "4D" },
                    { new Guid("67e9678f-2c50-4b06-8054-50513ab0db76"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "6E" },
                    { new Guid("6882024a-dc5b-42c2-ac18-5a99116b3cb0"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "6B" },
                    { new Guid("688db705-8d37-4b61-94b6-2382f584482b"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "11B" },
                    { new Guid("68e2112d-ca18-446e-8bbf-2b0536654b9d"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "4B" },
                    { new Guid("6915cf22-3369-465b-855d-117c4e29e0c3"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 3, false, "1F" },
                    { new Guid("6a189c2b-3b10-4023-b5c6-741785d218e9"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "6D" },
                    { new Guid("6a8c7529-8ffe-44f3-978f-053a20e52e1d"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "6F" },
                    { new Guid("6b336e20-1726-4a5c-99f1-feb406988a63"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "4D" },
                    { new Guid("6c3873b4-dc6b-4d6d-94a1-553adec3836c"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "4D" },
                    { new Guid("6cee862d-f9f4-4c0c-b8c5-e406761df5db"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "4C" },
                    { new Guid("6df1696a-e772-4f81-851e-fbb807e7c369"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "3B" },
                    { new Guid("6e18a0c7-1c97-42b3-acc3-663eb65db6d2"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "5C" },
                    { new Guid("6e9182fb-4e00-4cdc-a9d4-ef52e976c4e1"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "9D" },
                    { new Guid("6f04c359-4911-446a-823a-db92cfd4bb6c"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "11A" },
                    { new Guid("6f173bf8-caee-40b8-909f-132d7e351523"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "7C" },
                    { new Guid("6f2d3fd3-72b5-4c5d-9d48-132474368f3a"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "6A" },
                    { new Guid("6fe03f90-c334-41d9-a62f-b6308631bf51"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 3, false, "1B" },
                    { new Guid("702097d4-808c-4a3c-81bd-c16402b1bdac"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "11D" },
                    { new Guid("702626cb-7850-4ac0-aa6e-fb7ddcd4778b"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "3A" },
                    { new Guid("70b2050b-0105-467c-896e-96148b2f351c"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 3, false, "1A" },
                    { new Guid("70fc5645-3368-415e-b271-48e123de34e1"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "4C" },
                    { new Guid("711926be-3902-4eed-bbe8-a4b6868594e6"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "3D" },
                    { new Guid("71309373-05f4-4f8b-a75c-6c0b4d29d9e1"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "7F" },
                    { new Guid("71ab9419-ff32-46bf-b36d-c68a1fa9ea88"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "7A" },
                    { new Guid("71de40af-7924-40c0-963f-17e32247b35c"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "6A" },
                    { new Guid("7246d7b7-ebe4-4422-bbad-75aed1732bd3"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "8B" },
                    { new Guid("726a23ca-b8e7-4dc3-8e4f-bb32a06167e7"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "8B" },
                    { new Guid("7281b6ca-eacf-4c5e-899c-116319bb5547"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "3B" },
                    { new Guid("730ca535-50d9-4f9c-95e0-fd99998691a6"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "6B" },
                    { new Guid("73244842-2c58-426c-9faa-fddfd9adf44c"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "8C" },
                    { new Guid("75273b7d-2d74-45d9-9bb1-da4f80454745"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "8A" },
                    { new Guid("753e6760-fa99-43ea-ac33-8a5393a2a3d8"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "1A" },
                    { new Guid("7554a6a2-9d14-41e7-8625-675e54f762e3"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "10E" },
                    { new Guid("76dfdfba-4a41-4e47-bff2-7a2880f0eb0e"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "10D" },
                    { new Guid("7748c49a-8272-4624-b349-0143841299ba"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "11F" },
                    { new Guid("77ee457f-b562-45bd-9e3d-20aff48b9956"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "6B" },
                    { new Guid("781b282d-7f52-464d-aee5-afd277f4f377"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "5B" },
                    { new Guid("78cc34fe-1b0d-4c30-872d-238bede21abe"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "7F" },
                    { new Guid("79075254-051a-4e4e-84c9-a6164a23b4a8"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "9C" },
                    { new Guid("79558a99-db3d-47aa-adb4-72e1c6964d82"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "10E" },
                    { new Guid("79ef50e4-508a-40e2-b0a4-63ac430c8a9d"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "7F" },
                    { new Guid("7a402da0-fd0c-4108-a102-69e1395559d3"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "7C" },
                    { new Guid("7a8ff326-e994-4d15-90c2-4da106aad1c9"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "4C" },
                    { new Guid("7ae0e3d2-f2d9-4c82-8477-210651ca97e4"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "4B" },
                    { new Guid("7aed707c-afa8-40d4-82d3-ffb043125dd6"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "4F" },
                    { new Guid("7b31babf-361e-498b-8528-3a04612518d2"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "5E" },
                    { new Guid("7be9c1da-846e-4dd7-a7fb-478064a16a23"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "9C" },
                    { new Guid("7c1eb71c-621a-459a-b6a7-ec42e1effb23"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "8E" },
                    { new Guid("7ea276f1-cca6-412b-a2f7-7672ad961119"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "3D" },
                    { new Guid("7eb84c1f-d7b5-4a43-b557-63d45736304a"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 3, false, "1A" },
                    { new Guid("7ec4462e-7aeb-482b-93c4-9ef14166757b"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "6F" },
                    { new Guid("7f4c9b4a-d63d-40fa-92d9-d50ff43ec6dc"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "5E" },
                    { new Guid("7f6c88eb-d482-4922-8fa7-eb12a2fe01d4"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "6A" },
                    { new Guid("8021adde-e1e6-475f-af5b-8c27884cde1e"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "7D" },
                    { new Guid("80772b24-f875-4b38-a87f-957926d34e51"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 2, false, "2A" },
                    { new Guid("8189e3fb-a2cc-4115-8675-411286d2573d"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "4F" },
                    { new Guid("81bfce31-f8a6-4c5f-90ef-89aceead3424"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "9D" },
                    { new Guid("83bea520-1afa-4d80-a5d9-3a00c02b4c40"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "11A" },
                    { new Guid("83bef74a-a2fd-49cf-aae7-e6388eb33b0b"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "9F" },
                    { new Guid("840ffc0f-6c0f-4f40-9c8e-6b7df0559185"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "4E" },
                    { new Guid("847bf7ce-9b22-4e50-8ce8-436574d697c6"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "7E" },
                    { new Guid("847ebcad-a89f-44e6-bfb7-770d9a7de7db"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "5A" },
                    { new Guid("848d77f8-27fc-4543-8ba4-2d3b259499b1"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "4A" },
                    { new Guid("8591ef22-be7c-445e-98b4-29c77ce4188e"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "3F" },
                    { new Guid("85e0485c-ed0e-44cc-8d70-1b39f47e5cd2"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "8A" },
                    { new Guid("8603e6be-1f78-48e6-9a5e-4f2a1f79b017"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "6E" },
                    { new Guid("86dfdd13-e7d2-4449-aa41-1c1ccac59225"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "6F" },
                    { new Guid("871e9d93-305d-42b4-bffe-d0126cd0866c"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "2F" },
                    { new Guid("8753c4db-f2e3-4876-906b-ad363e80a85a"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "4E" },
                    { new Guid("87e1219f-5964-49b5-8f59-a7d258f6ef78"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "8A" },
                    { new Guid("87e264d8-c1ae-45d7-9aa1-b9b275307a20"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "11E" },
                    { new Guid("8887ece8-6028-46f9-859c-efb1340578f9"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "6D" },
                    { new Guid("89585312-cd95-425b-ad09-a450017f39c6"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "10F" },
                    { new Guid("89a495b9-9bb2-4e2d-aba9-b8b760f59982"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "5A" },
                    { new Guid("8a0ec80f-9947-4f88-94d5-c64364baf913"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "6F" },
                    { new Guid("8ae7533a-4166-4109-8c63-da496b79770c"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 3, false, "1B" },
                    { new Guid("8b05a294-edfd-453e-ae50-f3657f32c287"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "7B" },
                    { new Guid("8b4a16d7-a739-4c95-ae1e-4c361f13d7cf"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "9C" },
                    { new Guid("8bc5d230-5326-4435-840f-7e42ce30a3db"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "4F" },
                    { new Guid("8c621ee2-2694-4b7f-a421-072335e20b0c"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "7F" },
                    { new Guid("8d155d65-8148-41e6-84c2-a5abce98baaf"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 3, false, "1F" },
                    { new Guid("8d750932-9ff2-4e22-8707-584d64fa9c36"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "4C" },
                    { new Guid("8d7f4359-9c77-48f7-93b7-35dae103bc08"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "7B" },
                    { new Guid("8e3a1e84-3666-4049-b1e1-8e5e0f06efb7"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "9D" },
                    { new Guid("8e5e2211-19f1-4991-baad-3b467c969c2b"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "11D" },
                    { new Guid("8f02d535-b7e1-458f-90ef-217796c56f88"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "7A" },
                    { new Guid("8fc344c8-f7ea-4ad4-98cc-0ac1ff6418a1"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "5F" },
                    { new Guid("9051645b-373c-45e0-ab6e-e6128c5ac834"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "3A" },
                    { new Guid("90988559-ef28-4429-9e76-dd82a4433283"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "7F" },
                    { new Guid("9109eafd-9596-419d-804c-21fd3001efba"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "4F" },
                    { new Guid("91b5f51b-93aa-4ab8-8b9b-399e4bbd74ab"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "2F" },
                    { new Guid("91d05c01-4d3e-4c0e-a002-7a19b8fb11b2"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "3E" },
                    { new Guid("926a3525-1d3d-454c-939a-b400a9940c9b"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "11C" },
                    { new Guid("938600a1-cc14-4b23-b915-9194811d6f2e"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "10B" },
                    { new Guid("94437de3-71d1-48e4-b6b2-4b126f52e1d4"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "11A" },
                    { new Guid("947708c2-c0af-4ada-b6dc-6ab0c1477ba2"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "8F" },
                    { new Guid("94a60310-18cf-4b71-a011-e4a508a0bbd1"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "10C" },
                    { new Guid("94f55f5e-0511-4756-a97f-9c4b6147f078"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "4B" },
                    { new Guid("954e6e8c-7f23-420b-bdb2-a82a030e9ebf"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 3, false, "1B" },
                    { new Guid("9645f8ef-1ce9-413f-9d3a-646c93d02248"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "5B" },
                    { new Guid("965df9bb-1e36-4ed4-96aa-0a9a946f00da"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 3, false, "1A" },
                    { new Guid("976b6fc1-4542-410e-aedf-b92a72261a4f"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "4F" },
                    { new Guid("97b90ca5-0235-4b3c-8ed3-d90fad0d9451"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "9D" },
                    { new Guid("97c13de9-f901-4076-8905-104416ea8313"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "9B" },
                    { new Guid("9824aad9-343e-4fc0-b15f-7d5fa54e0247"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "10F" },
                    { new Guid("983451cf-eb46-41b9-9233-58aaf818e087"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "10F" },
                    { new Guid("989d7d62-d255-42bb-b151-b6584a247ac8"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "3B" },
                    { new Guid("98db43c5-6d00-40bd-b77b-6686bea66378"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "4B" },
                    { new Guid("98e70c0a-596b-42dc-a845-9d0d289bfc47"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "7C" },
                    { new Guid("9908ff59-55f8-41c1-ab34-abb062fd0796"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "5B" },
                    { new Guid("99335557-13e2-4d71-9d8c-46bb3a9bf66a"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "6C" },
                    { new Guid("993fbe47-c0fd-4d06-950f-2965cb45ab77"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "3C" },
                    { new Guid("99415760-051c-4618-bc69-7efa0df08da0"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "3F" },
                    { new Guid("998487d5-57e4-4ac6-a2d9-5d165800857e"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "7C" },
                    { new Guid("99978c1d-8c61-46b5-9dbd-5d6b865f45cf"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "6C" },
                    { new Guid("99f3a481-3bf2-4702-8690-f75c92b33f70"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "5F" },
                    { new Guid("99f4bf39-b06a-44f9-b37d-ed5883eea3dd"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 2, false, "2F" },
                    { new Guid("9b790168-2c5e-4fe8-a5e3-c0779ff84f4a"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "2A" },
                    { new Guid("9c51aa21-715d-41cc-a5f0-db5881d7bfa6"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "8D" },
                    { new Guid("9caddf77-dc8a-46f4-8a15-5d6957964288"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "7C" },
                    { new Guid("9ce09f06-5a18-41b6-998c-b0e6f130502c"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 3, false, "1C" },
                    { new Guid("9d36efee-7263-448a-8c73-06a06c0e4b2c"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "10B" },
                    { new Guid("9d78f99b-9c58-4db9-bce4-f3fa63b94af0"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "8D" },
                    { new Guid("9db2fcf4-598f-4031-8ba5-246c062ef21c"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "4F" },
                    { new Guid("9ea252e6-7280-4cad-b1e4-97325fd3ced1"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 2, false, "2E" },
                    { new Guid("9f2192b9-b432-4c81-966c-985a7eedb413"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "10F" },
                    { new Guid("9f4fd379-4638-4d91-ac38-118c03732671"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "5D" },
                    { new Guid("9f716a04-219a-456a-b016-25cd52ceb479"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 3, false, "1A" },
                    { new Guid("9ffe764a-097b-44e8-8e15-939bc253de7f"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "6A" },
                    { new Guid("a0e20320-b5a1-41c8-96fc-e3a7b42f9022"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, true, "3B" },
                    { new Guid("a143fd25-f774-456d-a707-54b1e95f99fc"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "4D" },
                    { new Guid("a19ef078-5188-4a44-b53e-51605800abf6"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "7A" },
                    { new Guid("a257cba8-13bc-434c-9eb0-18cffd8bc4e4"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "3A" },
                    { new Guid("a28b80a4-5641-47c3-97af-71066b1dbe7a"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "5D" },
                    { new Guid("a2da708d-47df-40a9-875c-94fa056b23f4"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "7D" },
                    { new Guid("a32f10e5-92c9-4dd3-9224-22bc66bb0976"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 2, false, "2B" },
                    { new Guid("a413d430-7f41-4e76-92b0-9b7f2b142b41"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "10D" },
                    { new Guid("a4729e74-89e5-47ed-a48e-1ba6284b9439"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "9B" },
                    { new Guid("a5324641-b8d1-4551-b0c4-cde31833755a"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 2, false, "2C" },
                    { new Guid("a5951e2c-1bbe-4baa-9c37-2bff81fba5eb"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "8D" },
                    { new Guid("a5bb2573-03e4-4fad-8d37-9c15064ad6b2"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 3, false, "1A" },
                    { new Guid("a6477904-176a-4386-a617-96c5d5a18ad9"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "4B" },
                    { new Guid("a64c3f2e-5ac0-448a-898e-3f326ebc67f9"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "8D" },
                    { new Guid("a64e482a-a488-4f6b-922b-8247906312ca"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "2E" },
                    { new Guid("a660c187-541d-40b2-9638-4cc3d70571e3"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "4C" },
                    { new Guid("a662fdc1-c6ac-4ac8-9af7-0e6c40f6fb7d"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "8C" },
                    { new Guid("a696e786-0f94-4f80-a7be-4ca0328fbc51"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "10A" },
                    { new Guid("a6afb58b-a8fe-45e7-a9b0-9391e59933bb"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "6A" },
                    { new Guid("a6da99df-f10d-4bf7-9fce-20cd9480a91c"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "8F" },
                    { new Guid("a6f3b6fb-b839-4eef-9782-79d5afccdb60"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "5C" },
                    { new Guid("a75461b1-ec01-420e-b9fd-e4dbac51f9c3"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "6B" },
                    { new Guid("a76dece7-a7cd-4194-9d72-3ebbf8c6a718"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "8E" },
                    { new Guid("a78c0064-dfed-4cc9-9d82-a8281a55f7fb"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "3B" },
                    { new Guid("a7f4fa5d-1433-481f-8a9f-9a55607ffc9c"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "11A" },
                    { new Guid("a85445dd-70c4-435f-8141-ad641f82a68c"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "6E" },
                    { new Guid("a923a2c6-5b51-459d-8812-62d8b7646ff9"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 2, false, "2B" },
                    { new Guid("a9a21545-1c97-4e8b-884c-8cd3ae1193c3"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "7E" },
                    { new Guid("a9b5cf82-c2b2-4339-9ffb-4ba4722a21cb"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "9C" },
                    { new Guid("a9e6fe9d-f424-4e42-86f0-6048a53af210"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "6B" },
                    { new Guid("aa1e7ec4-5656-4e09-9a4a-2cf6bad093fd"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "10E" },
                    { new Guid("aa404f33-b885-4edf-b121-c28adaac0794"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "4F" },
                    { new Guid("aa46ea98-6b96-4a71-9f1a-3d732b67f533"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "4B" },
                    { new Guid("aa9e41c5-1c28-49f4-8a4c-3955091cc182"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "5B" },
                    { new Guid("ab37ec93-fcee-4254-9720-3e2467a63d06"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "7B" },
                    { new Guid("ab41d0e7-9c12-4542-8bbe-a22f76fc4511"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "9F" },
                    { new Guid("ab8f6db9-e351-4490-ad4a-bbde62e0a7ad"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 2, false, "2D" },
                    { new Guid("ac0bce4b-b20a-4576-8ece-226336afd4d9"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "10E" },
                    { new Guid("ac1591ec-aa0e-44c6-a8e7-bdba1529d693"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "3C" },
                    { new Guid("acbdc21e-5b51-47eb-b448-db1d8ab3273e"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "5A" },
                    { new Guid("ade2962b-3bdd-4291-b552-e5ad5c4fb926"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 3, false, "1B" },
                    { new Guid("ae2ce045-9b68-43c4-9ddc-5ab837590f92"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "6E" },
                    { new Guid("aecb9c96-f076-4891-a99f-7ffad733a071"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "4A" },
                    { new Guid("af15c241-ceb2-4ee0-96cb-c2f2dc98ae71"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "8E" },
                    { new Guid("b0c41918-6917-40d0-b6b6-f6e5b134969b"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "6A" },
                    { new Guid("b164b1c1-15ca-4af3-9373-6317108e8cf8"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "8A" },
                    { new Guid("b201455c-aece-401e-bcce-925393141ded"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "11F" },
                    { new Guid("b24efbb9-da96-4911-9451-a47114ac81a4"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 3, false, "1D" },
                    { new Guid("b2be269e-80c2-439d-93d1-a550798ff0b2"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "3B" },
                    { new Guid("b303ca0f-7603-44be-9784-1fadca744be6"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "5B" },
                    { new Guid("b41598f8-0238-4df1-9717-f7b1eda69b19"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "6D" },
                    { new Guid("b48eead5-fae6-4890-b888-a94661c7b1c1"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "4A" },
                    { new Guid("b4e6d9a8-9824-4e10-ac7f-75a0c7ee2644"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "9D" },
                    { new Guid("b527ec7b-b51b-413a-a6bd-3c78014f50e1"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "5B" },
                    { new Guid("b52edb73-de7a-421f-bfd6-0c7c424d765a"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "7D" },
                    { new Guid("b5b07a85-ab96-4699-aa66-d052286df218"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "8D" },
                    { new Guid("b6594872-4bac-4110-b462-4db64b077b72"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "3B" },
                    { new Guid("b67858d3-02ed-49a3-9c09-f5b1aeac14ae"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "8A" },
                    { new Guid("b6829a34-93c6-4673-bbb8-0d7e05240246"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "2C" },
                    { new Guid("b6ae173a-42c9-4c15-8ffd-b5d445ae2327"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "3F" },
                    { new Guid("b71977e0-56d5-401b-916b-1df77340ec32"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "9C" },
                    { new Guid("b7f4a9db-717f-497b-aad3-d93e8e248489"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "7C" },
                    { new Guid("b88b17b4-385d-4dac-ab59-55aa8d8de946"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "5B" },
                    { new Guid("b8a4620a-6687-4cce-b202-73c19fcb80c1"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "6D" },
                    { new Guid("b8dffb11-365b-4ba7-b2b0-7a033b6d04ec"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "10A" },
                    { new Guid("b9082c69-5837-443c-8ca5-1b078dbc428c"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "4D" },
                    { new Guid("b95ae8b7-ce58-448e-a144-83dfa9f7839e"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "7A" },
                    { new Guid("b96bf4de-4bce-4879-82a3-0a8c989f7d94"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "6C" },
                    { new Guid("b99e5299-7fc1-4de8-a476-81f56a6eca6a"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 2, false, "2A" },
                    { new Guid("ba16ea17-34f7-41da-8571-80b6c1cbc68a"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "7C" },
                    { new Guid("bbb14286-92bd-4812-b09c-e76fd6b99f2e"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "3F" },
                    { new Guid("bd9b0109-29a2-48e5-9ddb-ed338124ea31"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "5F" },
                    { new Guid("be37ecd1-8152-46d0-94d6-56cd56d8c32e"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "5E" },
                    { new Guid("be87b08f-d52a-420a-92a2-0e5d6281a9e7"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 3, false, "1C" },
                    { new Guid("be936513-5ae6-4250-aa0a-cf315c5578c1"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "8B" },
                    { new Guid("bf3c633a-85dc-41cf-8703-c0a3b463d88a"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "6F" },
                    { new Guid("bf745f45-59ca-45d1-b6f3-f2131400cee6"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "2C" },
                    { new Guid("bfc09d4f-5f1f-4e71-9598-f519acdea8eb"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "4B" },
                    { new Guid("bfdc9671-47e5-49af-bff0-03cde4585cfc"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "5A" },
                    { new Guid("c03af405-7bcc-4c39-86f4-c35b1799ac82"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "8C" },
                    { new Guid("c0ac8598-0c12-448a-aa9a-f93f3793a48e"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "3D" },
                    { new Guid("c0e69e70-0cd8-4c4d-ad20-870f8ae57b8b"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "8D" },
                    { new Guid("c1346c25-e16c-4278-b7f5-13ce7b5775eb"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 3, false, "1B" },
                    { new Guid("c257ecf4-cab9-4703-a089-e4c0a0222667"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "4E" },
                    { new Guid("c28b5f30-c277-44f1-9bcf-d5b954ba0e2e"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "9D" },
                    { new Guid("c2e372c0-7e28-456b-a5d1-8a10c0a33b96"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "7B" },
                    { new Guid("c35b0d00-1030-4c62-a2b9-a23aa6bd0ec8"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "5D" },
                    { new Guid("c3976bf4-d063-4c17-ae90-57ba88727c61"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 3, false, "1D" },
                    { new Guid("c3a016df-ca0b-45da-97cb-ac4ee8b8bae8"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "8C" },
                    { new Guid("c4b196b9-35d7-456f-9136-dd009e624c70"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "9A" },
                    { new Guid("c4b6f180-32e2-4a7f-88c2-e2ae35edde70"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "5F" },
                    { new Guid("c4f8b277-f3f4-48fd-9c45-e60120a32649"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "10C" },
                    { new Guid("c5e5d5f2-55b8-4e32-a2c7-eadabe332b0e"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 3, false, "1B" },
                    { new Guid("c71743cc-a906-4ff8-abdf-c2f9cb3fc3a5"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "4B" },
                    { new Guid("c738556e-263e-4eee-8d13-360c7d1d7ae3"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "6F" },
                    { new Guid("c786f022-0e3c-45c3-a76e-4ff921e2fe25"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "6C" },
                    { new Guid("c7bafaac-ea71-4e20-8d46-3d20992b2d76"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "6D" },
                    { new Guid("c889aba7-44ef-4665-b941-e6bf5239d802"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "2D" },
                    { new Guid("c92fe3b2-c05e-4b3e-809d-a66d029bc96a"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "5D" },
                    { new Guid("c9b0b2f6-f16e-4044-8447-108039ecee1c"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 3, false, "1B" },
                    { new Guid("c9fb924f-7e7f-4b0c-a5a2-fb6087be0618"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "8D" },
                    { new Guid("ca53d8b0-54b0-43f1-9843-109e15c184ae"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "8B" },
                    { new Guid("ca9d8f92-126b-4bdc-a3c0-421249df1739"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 3, false, "1B" },
                    { new Guid("cb349789-1213-4021-856c-44822132f6fe"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 2, false, "2A" },
                    { new Guid("ccb99729-8f71-4893-ac8b-a6f59732d64d"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "6E" },
                    { new Guid("cdcd9065-4329-4bc1-a5dc-a0c631aa0495"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "6E" },
                    { new Guid("ce7df43f-c040-4827-91d6-b1f5cc5213ab"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "11B" },
                    { new Guid("ce800348-470b-4135-bad3-091223b0f5a2"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "5E" },
                    { new Guid("ce9d1e53-ce80-42da-bfbd-5fd324adf61f"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "4C" },
                    { new Guid("ceb7f656-eb2a-4d7c-bd88-6c279b0972a0"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "7B" },
                    { new Guid("cec4b5db-db98-4a89-9bec-c028eb443d79"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "9E" },
                    { new Guid("cee4fce8-6b7a-4e95-8604-1675685e24d2"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "8B" },
                    { new Guid("cf1faaaa-20c8-46a9-88ac-a2fd3112b143"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "4C" },
                    { new Guid("cf444045-b4cf-4aea-b35a-5a07d7d7f072"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "11B" },
                    { new Guid("cf62e76d-e650-4aee-998c-f7c6a09ce44d"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 3, false, "1E" },
                    { new Guid("cf679aa0-a4ac-41ce-b3ee-88bf38c989fd"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "6E" },
                    { new Guid("d0aed409-5859-4340-8bf9-a24b6c433ce5"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "10C" },
                    { new Guid("d136a852-e52a-4749-a474-fbdff31ca7b2"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "4E" },
                    { new Guid("d1eac417-58af-4697-85bf-89883a5ce538"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "6F" },
                    { new Guid("d27723d9-4b33-480d-9a44-3e7514492aaf"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "6E" },
                    { new Guid("d2910f46-57cb-4f0a-a363-5fe2d3005807"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "6F" },
                    { new Guid("d2a5daa2-d5c1-4d6e-94bd-9c38c213c9e2"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "3C" },
                    { new Guid("d2bcf3e8-3c3f-4092-b4e6-808f7df34178"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "11B" },
                    { new Guid("d3abcc28-8cd3-4ff0-8c9a-99c1bca610ff"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "3A" },
                    { new Guid("d3bea2e1-6655-4e95-a0f3-bc02c5170065"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "8A" },
                    { new Guid("d3cbe764-510d-45f2-8fda-7d144c7d4842"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "5D" },
                    { new Guid("d4715f39-abb1-46d1-9fcf-878fc9c20f17"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "5C" },
                    { new Guid("d541758d-9a3a-4176-a791-4c35513554a7"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "11E" },
                    { new Guid("d55f82c9-2ac6-40b1-b941-56b510323f80"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "5F" },
                    { new Guid("d5b56ae0-7575-4701-b613-1d08327ad355"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "10D" },
                    { new Guid("d5cb18e4-a5a4-4017-9695-79777b5526c8"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "9F" },
                    { new Guid("d788b773-3e4d-4173-90e3-54c7ff7e0e08"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "10C" },
                    { new Guid("d7e40a58-5bf3-4858-ac55-68afbd730d13"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "10C" },
                    { new Guid("d88cc120-4751-46dd-b7c9-cf06a052c12b"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "6F" },
                    { new Guid("d8b16839-3b6d-4641-8867-5f58b26eb4b8"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "4A" },
                    { new Guid("d94764e6-2c3c-4640-a8d4-fa9507b66555"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "8C" },
                    { new Guid("d9686ed3-29b4-4e49-b585-ea7dbcf80dee"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "6C" },
                    { new Guid("d9a60d0b-54a6-4c03-9426-884c24e0fb7a"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "5F" },
                    { new Guid("d9e5dbfa-e35f-4645-974a-2b804add150f"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 3, false, "1A" },
                    { new Guid("d9efa1c5-5d6f-40f4-997a-930295514120"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "7A" },
                    { new Guid("da813baa-5825-4f31-b559-068e56b0364a"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "10E" },
                    { new Guid("db415314-325c-4745-bb12-2d1d576ed99b"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "10D" },
                    { new Guid("dbc76e3e-c8be-4947-a556-d20f15cfb7f5"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "8B" },
                    { new Guid("dc7f9b89-00b0-4496-a1e7-1df3595c62f0"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "5D" },
                    { new Guid("dceadbe2-174b-4e9a-aa08-3af6f562d285"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "6B" },
                    { new Guid("dfdb2482-d5c6-4d30-a1c0-ca81a486884d"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 2, false, "2A" },
                    { new Guid("dfe5bfa7-c348-4bb6-b24b-95f5468b2832"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "3A" },
                    { new Guid("e002c1a6-1ba7-4bbb-8da0-779181620f92"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "7B" },
                    { new Guid("e0068906-62ca-4a5a-854b-3c72a513b56a"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "10B" },
                    { new Guid("e00bfadb-e0b2-4580-8048-a43bda44bb7d"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 3, false, "1A" },
                    { new Guid("e038ad7c-0d42-4d4d-bead-f7bae5e6d53f"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "9D" },
                    { new Guid("e039da2d-cfd6-4cea-b6f2-849e40ce2805"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "5E" },
                    { new Guid("e05f84d7-9cd5-4d18-ae8d-5fcc2ee7c5f4"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "3B" },
                    { new Guid("e0820c02-fde4-40d7-a535-4c1a844b2820"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "5C" },
                    { new Guid("e09ba59d-6a8f-490b-ab17-26a8a1193c00"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "6C" },
                    { new Guid("e0d872f0-907f-4ced-a256-4f1f5224adb2"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "3D" },
                    { new Guid("e0f663b0-fbf4-4166-ac2e-78e12a5e2029"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "3A" },
                    { new Guid("e1b619c0-0a37-4e34-b981-6d1fc06ef200"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "10A" },
                    { new Guid("e1db8768-17eb-4e4d-8c88-7a9a7ff8a350"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "10D" },
                    { new Guid("e1fd97b5-387f-4d9f-98ba-006069e29f94"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "9E" },
                    { new Guid("e2117527-837a-42cf-9f13-6fcf424f68c0"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "10E" },
                    { new Guid("e4146ca2-2b88-48d3-85ee-ce235aee180b"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "5C" },
                    { new Guid("e53ee610-98a8-4e5e-9280-561f0cb06571"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "9A" },
                    { new Guid("e5774666-7aaa-4cd3-ba3f-abf90f59a561"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "11D" },
                    { new Guid("e596496e-c12d-44ff-8ff2-e65cb6e670f4"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "6C" },
                    { new Guid("e5cca0c1-32f3-46e8-a546-73309c1bbe08"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "9F" },
                    { new Guid("e5d7247e-100e-4c5c-b224-d841ecd280b0"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "5A" },
                    { new Guid("e635205b-e783-4440-bcc4-8af8ea944813"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "11F" },
                    { new Guid("e94ea843-2265-41ac-a646-4b67123f1ba5"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "11C" },
                    { new Guid("ea726ba8-5955-448f-9c5e-17516a834259"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "8C" },
                    { new Guid("eacd43e5-ed0f-443b-bc6a-01c593190ded"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "8B" },
                    { new Guid("eb7fa777-1a2c-4a5e-abe9-bb1174c46408"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "2E" },
                    { new Guid("ed27abe0-0b9d-4a43-8f81-5ff8336074be"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 2, false, "2C" },
                    { new Guid("ed8d504f-bebb-4f02-b5a6-07e833e1c470"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 2, false, "2A" },
                    { new Guid("ed8f14d9-7dc8-4eb3-88ef-b4e4ba21787e"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "6D" },
                    { new Guid("ee6dde00-4cef-42e1-a966-601581e504cb"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 2, false, "2A" },
                    { new Guid("ef94bf32-899b-4495-8016-1391584ff68f"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "7D" },
                    { new Guid("eff82e02-7a06-47dc-8716-955c56565560"), new Guid("6bb651ee-3ead-4499-9b0d-fec4202c8b39"), 0, false, "8F" },
                    { new Guid("f02c5d50-eabe-409a-a03e-6ada988f93fe"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "8E" },
                    { new Guid("f0b2fe95-2212-41b1-b972-669d4cbdaf4d"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "9D" },
                    { new Guid("f0d6c8a9-5102-4139-9f2d-f0f7a7412f9e"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "3A" },
                    { new Guid("f17b848e-3b2b-47b1-9188-37ad2d0eb18b"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "3A" },
                    { new Guid("f193acef-3279-43b7-9946-c4c3cde8e696"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "5E" },
                    { new Guid("f22677f4-1ee5-4f74-a389-4e4ccf986a50"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "7F" },
                    { new Guid("f23b5035-d1a2-420f-9bba-a1588a39b42c"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 2, false, "3E" },
                    { new Guid("f2c16ea9-034a-48e8-a590-e1da6a9904d5"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "5D" },
                    { new Guid("f30b36a1-79a7-4c89-bd30-05ccf08040b4"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "11E" },
                    { new Guid("f4431e80-478b-447d-babd-8ea723763f4d"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "4F" },
                    { new Guid("f47c8a5a-9e2d-4d0e-a888-3471fa165d5a"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "4A" },
                    { new Guid("f4e27a83-2126-4a57-a27b-5aadb1674f23"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "8A" },
                    { new Guid("f4e92568-c92b-4cd9-9d32-645e0b3cd722"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "2D" },
                    { new Guid("f52b9d7f-f208-45bb-97aa-6a86d3dc6323"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "4E" },
                    { new Guid("f5d0c2f4-8b8f-43dc-b79b-8a47987878ed"), new Guid("ad4b212a-8ff6-4642-8737-d5113e8ba797"), 0, false, "6E" },
                    { new Guid("f687dd90-db66-4d3f-a043-b32974ab5d8f"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "4E" },
                    { new Guid("f72b4254-560e-4965-b78e-82e2c9193a8f"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, true, "7B" },
                    { new Guid("f75abe59-2af2-44c0-a02b-cc9b7e8e6d59"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "8F" },
                    { new Guid("f864a998-ccc2-4005-b66e-9499b7fa4662"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "7D" },
                    { new Guid("f87dd07b-722b-43f7-a2b3-559bc92f178d"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 3, false, "1B" },
                    { new Guid("f8cb81e8-28fd-401c-806c-cb35f7273497"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "7E" },
                    { new Guid("f9136be6-faee-4420-8671-21c927948f59"), new Guid("80a1a761-091a-4811-a3ba-89d2da3960a4"), 0, false, "10D" },
                    { new Guid("f994093c-c1a8-4736-b6a6-8a1089385c13"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "4E" },
                    { new Guid("fa504d17-2bf7-495d-bde2-0aa971e7d407"), new Guid("249f23b3-4f1e-4729-b80d-39e20a4bf6d7"), 0, false, "8E" },
                    { new Guid("fb8cf3fd-2a48-40ca-bb1d-e0b8c83e51fe"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "8E" },
                    { new Guid("fc326746-f13c-41e2-a511-4796b242eac2"), new Guid("e362a311-4ea9-4bd0-9b76-badc4fcf2082"), 0, false, "11D" },
                    { new Guid("fc37c1cd-bb91-452c-ba15-c6fde67addc4"), new Guid("af3a15f1-24bc-4b0e-8878-cf5ff936fe44"), 0, false, "7D" },
                    { new Guid("fc73f3b7-fc25-4d8c-adb6-ba820927dcee"), new Guid("25c022e9-b88b-4896-aac5-4eb0af2f3e66"), 0, false, "4D" },
                    { new Guid("fc851a83-02e6-4054-bdb7-f2683e55b36f"), new Guid("4c868a0a-6dc6-45e9-8eca-bdedd7f07f66"), 0, false, "6D" },
                    { new Guid("fd12ee1b-6de5-422a-a12f-4491f2aed939"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "7B" },
                    { new Guid("fd2751ec-43b2-480f-b205-9de4622c090a"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "7F" },
                    { new Guid("fdc132ce-065b-4374-8e15-9ad66b5158eb"), new Guid("3fe28a0e-2f3b-498f-a6f4-ef9b36708c1e"), 0, false, "5F" },
                    { new Guid("fdd217bf-0e42-4e09-960c-5a7838b3d739"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "9A" },
                    { new Guid("fe11ae22-a230-4dea-a262-71412110afc0"), new Guid("975368b0-3cf3-4789-9248-70c6d58ccc70"), 0, false, "5A" },
                    { new Guid("feb7220d-f355-4be5-a98c-632d3435e85e"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 0, false, "5D" },
                    { new Guid("ff5b179e-a6f5-4f23-af7e-aea481bbf53e"), new Guid("2e693be0-c464-40c2-b86d-b784f7d2f7a3"), 2, false, "4A" },
                    { new Guid("ffe1b1a1-211d-41be-970f-6edd60883f71"), new Guid("7c94ebe7-4c57-4873-a107-75468fcb056b"), 0, false, "8E" }
                });

            migrationBuilder.InsertData(
                table: "Conditions",
                columns: new[] { "ConditionId", "Category", "ConditionType", "ConditionValue", "DiscountId", "Property" },
                values: new object[,]
                {
                    { new Guid("3b43a81c-0be0-4e06-8632-75a194ed9ddd"), 0, 0, null, new Guid("72fd9fbf-ee02-4016-93da-a9ea628c1f42"), "Birthday" },
                    { new Guid("6d9125a6-b9d6-499b-961b-7b0f87d7d65d"), 5, 0, "Thursday", new Guid("d9d0bf82-fa9f-4b73-8ab9-c7077d951df2"), "self" },
                    { new Guid("f22570c8-d172-450e-8c37-815c5510824d"), 1, 0, "Africa", new Guid("d9d0bf82-fa9f-4b73-8ab9-c7077d951df2"), "Continent" }
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
