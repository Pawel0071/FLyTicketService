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
                    RegistrationNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aircrafts", x => x.AircraftId);
                });

            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    AirportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AirportName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IATA = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ICAO = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Altitude = table.Column<double>(type: "float", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DST = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.AirportId);
                });

            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    AirlineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IATA = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    AirlineName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.AirlineId);
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

            migrationBuilder.InsertData(
                table: "Aircrafts",
                columns: new[] { "AircraftId", "Model", "RegistrationNumber" },
                values: new object[,]
                {
                    { new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), "Boeing 737-MAX", "SP-LAA" },
                    { new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), "Airbus A320Neo", "SP-LBG" }
                });

            migrationBuilder.InsertData(
                table: "Airports",
                columns: new[] { "AirportId", "AirportName", "Altitude", "City", "Country", "DST", "IATA", "ICAO", "Latitude", "Longitude", "Timezone" },
                values: new object[,]
                {
                    { new Guid("052b64a4-4c43-4ad6-a261-527bd1d9763e"), "Dallas/Fort Worth International Airport", 607.0, "Dallas-Fort Worth", "United States", "A", "DFW", "KDFW", 32.896799999999999, -97.037999999999997, "America/Chicago" },
                    { new Guid("0dfa908f-fb37-4cfc-8942-b4ca4374457d"), "John Paul II International Airport Kraków–Balice", 791.0, "Kraków", "Poland", "E", "KRK", "EPKK", 50.0777, 19.784800000000001, "Europe/Warsaw" },
                    { new Guid("0e0edb20-c0e7-4b09-ab07-a0536d2137df"), "San Francisco International Airport", 13.0, "San Francisco", "United States", "A", "SFO", "KSFO", 37.6188, -122.375, "America/Los_Angeles" },
                    { new Guid("12ed44b8-9c4b-479a-a555-2b2b5aa27475"), "Adolfo Suárez Madrid–Barajas Airport", 1998.0, "Madrid", "Spain", "E", "MAD", "LEMD", 40.471899999999998, -3.5626000000000002, "Europe/Madrid" },
                    { new Guid("156b135a-364c-467e-89e1-6a019bde6c72"), "Charles de Gaulle Airport", 392.0, "Paris", "France", "E", "CDG", "LFPG", 49.009700000000002, 2.5478999999999998, "Europe/Paris" },
                    { new Guid("1a2f3a19-8196-4e19-a1ee-567af0d2e8f6"), "Los Angeles International Airport", 125.0, "Los Angeles", "United States", "A", "LAX", "KLAX", 33.942500000000003, -118.4081, "America/Los_Angeles" },
                    { new Guid("22585730-68ca-4ff4-aee6-100ba0244c06"), "Seattle-Tacoma International Airport", 433.0, "Seattle", "United States", "A", "SEA", "KSEA", 47.450200000000002, -122.30880000000001, "America/Los_Angeles" },
                    { new Guid("2c65d764-4b4f-4af2-b555-9d8c2503eaab"), "O'Hare International Airport", 672.0, "Chicago", "United States", "A", "ORD", "KORD", 41.9786, -87.904799999999994, "America/Chicago" },
                    { new Guid("30683ffa-c6f5-475e-b45f-ef645d3025fd"), "Munich Airport", 1487.0, "Munich", "Germany", "E", "MUC", "EDDM", 48.3538, 11.786099999999999, "Europe/Berlin" },
                    { new Guid("3874ed80-24b8-4a14-9427-af38c0245a40"), "Heathrow Airport", 83.0, "London", "United Kingdom", "E", "LHR", "EGLL", 51.470599999999997, -0.46189999999999998, "Europe/London" },
                    { new Guid("53016a72-5a18-4d6b-9dd7-45033ae5b56d"), "Charles de Gaulle Airport", 392.0, "Paris", "France", "E", "CDG", "LFPG", 49.009700000000002, 2.5478999999999998, "Europe/Paris" },
                    { new Guid("55c74bf0-41f4-4b89-9abf-cd1101aaa1b3"), "Warsaw Chopin Airport", 362.0, "Warsaw", "Poland", "E", "WAW", "EPWA", 52.165700000000001, 20.967099999999999, "Europe/Warsaw" },
                    { new Guid("5678ca70-aecc-4d7b-97e4-d8a4f2b21afa"), "O'Hare International Airport", 672.0, "Chicago", "United States", "A", "ORD", "KORD", 41.9786, -87.904799999999994, "America/Chicago" },
                    { new Guid("595b9d28-9b8e-4c5a-b7cc-de2b7ef864a0"), "Heathrow Airport", 83.0, "London", "United Kingdom", "E", "LHR", "EGLL", 51.470599999999997, -0.46189999999999998, "Europe/London" },
                    { new Guid("59b8349e-3070-4e8d-86d5-e88f7a612b9e"), "John F. Kennedy International Airport", 13.0, "New York", "United States", "A", "JFK", "KJFK", 40.641300000000001, -73.778099999999995, "America/New_York" },
                    { new Guid("62a70b00-5b93-476a-9f53-949de2cc7779"), "Barcelona–El Prat Airport", 12.0, "Barcelona", "Spain", "E", "BCN", "LEBL", 41.297400000000003, 2.0832999999999999, "Europe/Madrid" },
                    { new Guid("62e8885c-3b95-4a2b-a0ef-f83256622823"), "Beijing Capital International Airport", 116.0, "Beijing", "China", "N", "PEK", "ZBAA", 40.080100000000002, 116.58459999999999, "Asia/Shanghai" },
                    { new Guid("670934e4-a1f3-4109-b31f-f23586b7fd70"), "John F. Kennedy International Airport", 13.0, "New York", "United States", "A", "JFK", "KJFK", 40.641300000000001, -73.778099999999995, "America/New_York" },
                    { new Guid("6ac9b3ac-8fc6-4aea-b900-032b630acfc4"), "Frankfurt Airport", 364.0, "Frankfurt", "Germany", "E", "FRA", "EDDF", 50.0379, 8.5622000000000007, "Europe/Berlin" },
                    { new Guid("6ef2424e-94b7-47af-9bfa-25a49c921bd0"), "Amsterdam Airport Schiphol", -11.0, "Amsterdam", "Netherlands", "E", "AMS", "EHAM", 52.308599999999998, 4.7638999999999996, "Europe/Amsterdam" },
                    { new Guid("7b7010d9-17bb-4a4d-b9e9-aa200aae7ee9"), "San Francisco International Airport", 13.0, "San Francisco", "United States", "A", "SFO", "KSFO", 37.6188, -122.375, "America/Los_Angeles" },
                    { new Guid("7e9e97bb-5960-4d8b-b22b-922d8d91e8b2"), "Amsterdam Airport Schiphol", -11.0, "Amsterdam", "Netherlands", "E", "AMS", "EHAM", 52.308599999999998, 4.7638999999999996, "Europe/Amsterdam" },
                    { new Guid("84039810-bab3-47d9-a8e5-ac0d53d9c48b"), "Miami International Airport", 8.0, "Miami", "United States", "A", "MIA", "KMIA", 25.7959, -80.287000000000006, "America/New_York" },
                    { new Guid("85bc5ecb-b957-41a8-b9bd-720b10494ad1"), "Dallas/Fort Worth International Airport", 607.0, "Dallas-Fort Worth", "United States", "A", "DFW", "KDFW", 32.896799999999999, -97.037999999999997, "America/Chicago" },
                    { new Guid("86baeade-ee30-40fd-bb2b-7b9008a73287"), "Barcelona–El Prat Airport", 12.0, "Barcelona", "Spain", "E", "BCN", "LEBL", 41.297400000000003, 2.0832999999999999, "Europe/Madrid" },
                    { new Guid("87ed1053-d4c3-47cb-a0bc-dcc15a8a7146"), "Denver International Airport", 5431.0, "Denver", "United States", "A", "DEN", "KDEN", 39.861699999999999, -104.67310000000001, "America/Denver" },
                    { new Guid("93dce287-8e28-4084-8fc2-b80b0f6f0263"), "Hartsfield-Jackson Atlanta International Airport", 1026.0, "Atlanta", "United States", "A", "ATL", "KATL", 33.636699999999998, -84.428100000000001, "America/New_York" },
                    { new Guid("a00687bf-90db-43c1-a810-39d3752d4d38"), "Hartsfield-Jackson Atlanta International Airport", 1026.0, "Atlanta", "United States", "A", "ATL", "KATL", 33.636699999999998, -84.428100000000001, "America/New_York" },
                    { new Guid("b7ee7109-861d-4016-a5c3-6df7d1faa983"), "Adolfo Suárez Madrid–Barajas Airport", 1998.0, "Madrid", "Spain", "E", "MAD", "LEMD", 40.471899999999998, -3.5626000000000002, "Europe/Madrid" },
                    { new Guid("b904dc06-aab7-475d-b7d8-3485bcd9541b"), "Zurich Airport", 1416.0, "Zurich", "Switzerland", "E", "ZRH", "LSZH", 47.464700000000001, 8.5492000000000008, "Europe/Zurich" },
                    { new Guid("bac8f71b-5f45-4774-99d6-e7ccac92f879"), "Miami International Airport", 8.0, "Miami", "United States", "A", "MIA", "KMIA", 25.7959, -80.287000000000006, "America/New_York" },
                    { new Guid("c4169483-a26d-4784-9518-07a46c8da8e4"), "Leonardo da Vinci–Fiumicino Airport", 13.0, "Rome", "Italy", "E", "FCO", "LIRF", 41.8003, 12.238899999999999, "Europe/Rome" },
                    { new Guid("c7924d51-ecaf-4c5a-8c71-1e12c6bf4768"), "Munich Airport", 1487.0, "Munich", "Germany", "E", "MUC", "EDDM", 48.3538, 11.786099999999999, "Europe/Berlin" },
                    { new Guid("c8d49b0f-c357-4a49-9a9e-69894af29d9c"), "Denver International Airport", 5431.0, "Denver", "United States", "A", "DEN", "KDEN", 39.861699999999999, -104.67310000000001, "America/Denver" },
                    { new Guid("d24a0bb9-a1e4-48ce-b4e2-0bc42c4dd297"), "Leonardo da Vinci–Fiumicino Airport", 13.0, "Rome", "Italy", "E", "FCO", "LIRF", 41.8003, 12.238899999999999, "Europe/Rome" },
                    { new Guid("d27dae84-d8c2-4ea4-a217-ea51d62988e4"), "Los Angeles International Airport", 125.0, "Los Angeles", "United States", "A", "LAX", "KLAX", 33.942500000000003, -118.4081, "America/Los_Angeles" },
                    { new Guid("d48bced6-df68-4d8c-bab5-12ff5b2ea1aa"), "Beijing Capital International Airport", 116.0, "Beijing", "China", "N", "PEK", "ZBAA", 40.080100000000002, 116.58459999999999, "Asia/Shanghai" },
                    { new Guid("dd3bab2b-a2ec-4f86-9373-dc2578c2ca51"), "Seattle-Tacoma International Airport", 433.0, "Seattle", "United States", "A", "SEA", "KSEA", 47.450200000000002, -122.30880000000001, "America/Los_Angeles" },
                    { new Guid("edfbe350-1c7e-42b6-b891-ed4de257625a"), "Istanbul Airport", 325.0, "Istanbul", "Turkey", "E", "IST", "LTFM", 41.275300000000001, 28.751899999999999, "Europe/Istanbul" },
                    { new Guid("f79d4663-9649-4c7c-af47-4b1fd73692b6"), "Frankfurt Airport", 364.0, "Frankfurt", "Germany", "E", "FRA", "EDDF", 50.0379, 8.5622000000000007, "Europe/Berlin" },
                    { new Guid("ffd6d433-d834-4192-ab2c-cc42b4da5460"), "Istanbul Airport", 325.0, "Istanbul", "Turkey", "E", "IST", "LTFM", 41.275300000000001, 28.751899999999999, "Europe/Istanbul" }
                });

            migrationBuilder.InsertData(
                table: "Operators",
                columns: new[] { "AirlineId", "AirlineName", "Country", "IATA" },
                values: new object[,]
                {
                    { new Guid("5c0e4717-d08e-4fc2-ac4a-02967b09942e"), "Emirates", "UAE", "EK" },
                    { new Guid("8f1e36b9-9a5c-4cac-b1d0-9bf654084b58"), "Delta Airlines", "USA", "DL" },
                    { new Guid("b0a64a5e-71f6-4296-9b05-0da62da662e8"), "LOT", "Poland", "LO" },
                    { new Guid("e29dbd49-8574-4e78-8983-d5bcd9f2f741"), "American Airlines", "USA", "AA" },
                    { new Guid("e9d8196e-ad35-4bbf-97dc-6c1d013dc184"), "Air India", "India", "AI" },
                    { new Guid("f5abe099-90d7-4731-b4c7-0910da1b1db7"), "Qatar Airways", "Qatar", "QR" }
                });

            migrationBuilder.InsertData(
                table: "AircraftSeats",
                columns: new[] { "AircraftSeatId", "AircraftId", "Class", "OutOfService", "SeatNumber" },
                values: new object[,]
                {
                    { new Guid("02088211-b948-4244-b1a9-541300c38094"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "1E" },
                    { new Guid("06ca6b1a-dc74-485f-ad4c-f4862b828e37"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "2D" },
                    { new Guid("0a37cefd-cf6d-4815-aacd-cbb9eb88e7da"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "2A" },
                    { new Guid("0c72a93a-129c-45b6-8d21-5384a16500d3"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "1A" },
                    { new Guid("1733efdf-2341-488b-b14e-506c8b5155ff"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "1E" },
                    { new Guid("20e80335-01cb-4dfb-bfb9-52e05978f20d"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "3C" },
                    { new Guid("2a599a38-7038-4106-8ea9-e3b11a8f1065"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "1B" },
                    { new Guid("2cbd9108-ed3c-4ade-ac55-4884df8fdb7b"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "1D" },
                    { new Guid("46c8fdf1-d07d-4318-bb1c-3472b8c8e29a"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "1D" },
                    { new Guid("508ba879-8d09-44c4-b762-d86ddb802705"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "2A" },
                    { new Guid("57744294-340c-46b5-b32a-98b0ba30dd87"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "1C" },
                    { new Guid("59efca74-baa3-431f-8151-23935799170c"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "2B" },
                    { new Guid("6c5b7673-9484-4b0a-b8ca-f29f79806c15"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "3E" },
                    { new Guid("70d18860-ae6f-4f07-8207-5da4e8c4f47b"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "3B" },
                    { new Guid("74f1eea2-2be9-489f-9c24-ea568ed7b8d7"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "3A" },
                    { new Guid("8102b7fb-55e9-4dab-bc04-cc44f8d590c1"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "3D" },
                    { new Guid("8174c953-f7d9-4062-9192-a5e4c83b0a03"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "1C" },
                    { new Guid("899ffa5c-9128-4c62-9e3b-5199be696bb2"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "2C" },
                    { new Guid("8c545eee-aed9-4f3d-bc7e-d1575a830892"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "1B" },
                    { new Guid("9c29259b-873c-409c-a40b-4c72ecc7f50b"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "2E" },
                    { new Guid("abe2fa3d-e5a0-45eb-afdc-a13e6ad0c87f"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "2B" },
                    { new Guid("cae6e272-8db3-4044-a57b-38fa5248d19b"), new Guid("4d50d09f-2fd0-492c-9e3d-05fc1831677e"), 1, false, "2E" },
                    { new Guid("f671ba9c-63bf-42ed-973f-06acb365eeef"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "2D" },
                    { new Guid("f9b7266c-7bfb-4c3a-b5be-3908ba1a79a0"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "1A" },
                    { new Guid("ff6a2999-c9a4-4f95-91d6-d22a48b5e8e5"), new Guid("eadb857d-e038-4ff3-8680-8426e8a5ec4b"), 1, false, "2C" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AircraftSeats_AircraftId",
                table: "AircraftSeats",
                column: "AircraftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AircraftSeats");

            migrationBuilder.DropTable(
                name: "Airports");

            migrationBuilder.DropTable(
                name: "Operators");

            migrationBuilder.DropTable(
                name: "Aircrafts");
        }
    }
}
