using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokensTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "dynamic_field_types",
                keyColumn: "id",
                keyValue: new Guid("64ad5fd6-6119-42ca-88dd-827aad8d736c"));

            migrationBuilder.DeleteData(
                table: "dynamic_field_types",
                keyColumn: "id",
                keyValue: new Guid("9172d875-63c0-4038-b9b5-6e2476db75d5"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("52963336-bf52-45d9-bd0a-85e4e5dfb569"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("8bdc1577-7d17-4f19-993a-1e8f58c1cf80"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("ceae89a6-83ee-4f9f-8da2-a6f740e26415"));

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    expiry_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.user_id);
                });

            migrationBuilder.InsertData(
                table: "dynamic_field_types",
                columns: new[] { "id", "title" },
                values: new object[,]
                {
                    { new Guid("6df25cbc-83c9-4051-b626-0bc570902a1f"), "string" },
                    { new Guid("81771ba7-081d-47f4-a7a7-9a413dec2f66"), "number" }
                });

            migrationBuilder.InsertData(
                table: "users_roles",
                columns: new[] { "id", "can_edit_others_events", "is_admin", "title" },
                values: new object[,]
                {
                    { new Guid("072b55e0-9c08-4e8c-9479-ab7b57106d63"), true, true, "Admin" },
                    { new Guid("5ec4c9b6-caca-49bf-ad38-84a6de8fb3f0"), false, false, "User" },
                    { new Guid("ecc36465-ef74-4f69-8905-0d8d43637468"), true, false, "Moderator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DeleteData(
                table: "dynamic_field_types",
                keyColumn: "id",
                keyValue: new Guid("6df25cbc-83c9-4051-b626-0bc570902a1f"));

            migrationBuilder.DeleteData(
                table: "dynamic_field_types",
                keyColumn: "id",
                keyValue: new Guid("81771ba7-081d-47f4-a7a7-9a413dec2f66"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("072b55e0-9c08-4e8c-9479-ab7b57106d63"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("5ec4c9b6-caca-49bf-ad38-84a6de8fb3f0"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("ecc36465-ef74-4f69-8905-0d8d43637468"));

            migrationBuilder.InsertData(
                table: "dynamic_field_types",
                columns: new[] { "id", "title" },
                values: new object[,]
                {
                    { new Guid("64ad5fd6-6119-42ca-88dd-827aad8d736c"), "string" },
                    { new Guid("9172d875-63c0-4038-b9b5-6e2476db75d5"), "number" }
                });

            migrationBuilder.InsertData(
                table: "users_roles",
                columns: new[] { "id", "can_edit_others_events", "is_admin", "title" },
                values: new object[,]
                {
                    { new Guid("52963336-bf52-45d9-bd0a-85e4e5dfb569"), false, false, "User" },
                    { new Guid("8bdc1577-7d17-4f19-993a-1e8f58c1cf80"), true, true, "Admin" },
                    { new Guid("ceae89a6-83ee-4f9f-8da2-a6f740e26415"), true, false, "Moderator" }
                });
        }
    }
}
