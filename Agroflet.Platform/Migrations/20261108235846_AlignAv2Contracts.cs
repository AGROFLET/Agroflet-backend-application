using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Agroflet.Platform.Migrations
{
    /// <inheritdoc />
    public partial class AlignAv2Contracts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "event_id",
                table: "reported_positions",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "contact_messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    received_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_contact_messages", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "revoked_sessions",
                columns: table => new
                {
                    jti = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_revoked_sessions", x => x.jti);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_reported_positions_shipment_id_event_id",
                table: "reported_positions",
                columns: new[] { "shipment_id", "event_id" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "i_x_reported_positions_shipment_id",
                table: "reported_positions");

            migrationBuilder.CreateIndex(
                name: "i_x_revoked_sessions_expires_at",
                table: "revoked_sessions",
                column: "expires_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contact_messages");

            migrationBuilder.DropTable(
                name: "revoked_sessions");

            migrationBuilder.CreateIndex(
                name: "i_x_reported_positions_shipment_id",
                table: "reported_positions",
                column: "shipment_id");

            migrationBuilder.DropIndex(
                name: "i_x_reported_positions_shipment_id_event_id",
                table: "reported_positions");

            migrationBuilder.DropColumn(
                name: "event_id",
                table: "reported_positions");


        }
    }
}
