using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Agroflet.Platform.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    city = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    region = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    kind = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_locations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    first_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    last_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    user_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    company_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    preferred_language = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false),
                    token_version = table.Column<int>(type: "int", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_users", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "drivers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dispatcher_id = table.Column<int>(type: "int", nullable: false),
                    first_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    last_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    dni = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    license_number = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    license_category = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_drivers", x => x.id);
                    table.ForeignKey(
                        name: "f_k_drivers__user_dispatcher_id",
                        column: x => x.dispatcher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "password_reset_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    token_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    used = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_password_reset_requests", x => x.id);
                    table.ForeignKey(
                        name: "f_k_password_reset_requests__user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    dispatcher_id = table.Column<int>(type: "int", nullable: false),
                    plate = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    brand = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    model = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    capacity_tons = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    body_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vehicles", x => x.id);
                    table.ForeignKey(
                        name: "f_k_vehicles__user_dispatcher_id",
                        column: x => x.dispatcher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shipments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    code = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    dispatcher_id = table.Column<int>(type: "int", nullable: false),
                    buyer_id = table.Column<int>(type: "int", nullable: false),
                    cargo_type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    cargo_description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    weight_tons = table.Column<decimal>(type: "decimal(12,3)", precision: 12, scale: 3, nullable: false),
                    origin_id = table.Column<int>(type: "int", nullable: false),
                    destination_id = table.Column<int>(type: "int", nullable: false),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    driver_id = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    planned_departure_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estimated_arrival_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    actual_departure_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    delivered_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    cancellation_reason = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_shipments", x => x.id);
                    table.ForeignKey(
                        name: "f_k_shipments_drivers_driver_id",
                        column: x => x.driver_id,
                        principalTable: "drivers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_shipments_locations_destination_id",
                        column: x => x.destination_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_shipments_locations_origin_id",
                        column: x => x.origin_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_shipments_users_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_shipments_users_dispatcher_id",
                        column: x => x.dispatcher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_shipments_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "incidents",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    estimated_delay_minutes = table.Column<int>(type: "int", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    reported_by = table.Column<int>(type: "int", nullable: false),
                    idempotency_key = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_incidents", x => x.id);
                    table.ForeignKey(
                        name: "f_k_incidents__shipment_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_incidents_users_reported_by",
                        column: x => x.reported_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    recipient_id = table.Column<int>(type: "int", nullable: false),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    shipment_code = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    event_key = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    payload_json = table.Column<string>(type: "varchar(4000)", maxLength: 4000, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    read_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_notifications", x => x.id);
                    table.ForeignKey(
                        name: "f_k_notifications__shipment_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_notifications_users_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reported_positions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    latitude = table.Column<double>(type: "double", nullable: false),
                    longitude = table.Column<double>(type: "double", nullable: false),
                    reported_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    recorded_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    source = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    recorded_by = table.Column<int>(type: "int", nullable: false),
                    is_demo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    note = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_reported_positions", x => x.id);
                    table.ForeignKey(
                        name: "f_k_reported_positions_shipments_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_reported_positions_users_recorded_by",
                        column: x => x.recorded_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "status_changes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    shipment_id = table.Column<int>(type: "int", nullable: false),
                    previous_status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    new_status = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    changed_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_status_changes", x => x.id);
                    table.ForeignKey(
                        name: "f_k_status_changes_shipments_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "f_k_status_changes_users_changed_by",
                        column: x => x.changed_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "locations",
                columns: new[] { "id", "city", "kind", "latitude", "longitude", "name", "region", "version" },
                values: new object[,]
                {
                    { 1, "Huancayo", "origin", -12.055999999999999, -75.215999999999994, "Centro de Acopio El Tambo", "Junín", 0 },
                    { 2, "Huánuco", "origin", -9.9306000000000001, -76.242199999999997, "Acopio Amarilis", "Huánuco", 0 },
                    { 3, "Ica", "origin", -14.093999999999999, -75.704999999999998, "Fundo Los Aquijes", "Ica", 0 },
                    { 4, "Trujillo", "origin", -8.0891000000000002, -78.959999999999994, "Acopio Laredo", "La Libertad", 0 },
                    { 5, "Cañete", "origin", -13.077, -76.387, "Acopio San Vicente", "Lima Provincias", 0 },
                    { 6, "Huaral", "origin", -11.494999999999999, -77.206999999999994, "Acopio Valle de Huaral", "Lima Provincias", 0 },
                    { 7, "Santa Anita", "destination", -12.055999999999999, -76.968999999999994, "Gran Mercado Mayorista de Lima", "Lima Metropolitana", 0 },
                    { 8, "La Victoria", "destination", -12.068, -77.019000000000005, "Mercado Mayorista N.° 2 de Frutas", "Lima Metropolitana", 0 },
                    { 9, "Lurín", "destination", -12.273999999999999, -76.870000000000005, "Centro de Distribución Lurín", "Lima Metropolitana", 0 },
                    { 10, "Santa Anita", "destination", -12.048, -76.962000000000003, "Mercado de Productores de Santa Anita", "Lima Metropolitana", 0 }
                });

            migrationBuilder.CreateIndex(
                name: "i_x_drivers_dispatcher_id_dni",
                table: "drivers",
                columns: new[] { "dispatcher_id", "dni" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_drivers_dispatcher_id_license_number",
                table: "drivers",
                columns: new[] { "dispatcher_id", "license_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_reported_by",
                table: "incidents",
                column: "reported_by");

            migrationBuilder.CreateIndex(
                name: "i_x_incidents_shipment_id_idempotency_key",
                table: "incidents",
                columns: new[] { "shipment_id", "idempotency_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_notifications_recipient_id_event_key",
                table: "notifications",
                columns: new[] { "recipient_id", "event_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_notifications_shipment_id",
                table: "notifications",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_password_reset_requests_token_hash",
                table: "password_reset_requests",
                column: "token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_password_reset_requests_user_id",
                table: "password_reset_requests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_reported_positions_recorded_by",
                table: "reported_positions",
                column: "recorded_by");

            migrationBuilder.CreateIndex(
                name: "i_x_reported_positions_shipment_id",
                table: "reported_positions",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_shipments_buyer_id",
                table: "shipments",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "i_x_shipments_code",
                table: "shipments",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_shipments_destination_id",
                table: "shipments",
                column: "destination_id");

            migrationBuilder.CreateIndex(
                name: "i_x_shipments_dispatcher_id",
                table: "shipments",
                column: "dispatcher_id");

            migrationBuilder.CreateIndex(
                name: "i_x_shipments_driver_id",
                table: "shipments",
                column: "driver_id");

            migrationBuilder.CreateIndex(
                name: "i_x_shipments_origin_id",
                table: "shipments",
                column: "origin_id");

            migrationBuilder.CreateIndex(
                name: "i_x_shipments_vehicle_id",
                table: "shipments",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "i_x_status_changes_changed_by",
                table: "status_changes",
                column: "changed_by");

            migrationBuilder.CreateIndex(
                name: "i_x_status_changes_shipment_id",
                table: "status_changes",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "i_x_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_dispatcher_id_plate",
                table: "vehicles",
                columns: new[] { "dispatcher_id", "plate" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "incidents");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "password_reset_requests");

            migrationBuilder.DropTable(
                name: "reported_positions");

            migrationBuilder.DropTable(
                name: "status_changes");

            migrationBuilder.DropTable(
                name: "shipments");

            migrationBuilder.DropTable(
                name: "drivers");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "vehicles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
