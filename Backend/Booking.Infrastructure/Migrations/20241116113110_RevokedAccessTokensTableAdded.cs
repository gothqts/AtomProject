using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RevokedAccessTokensTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "banner_image_filepath",
                table: "users_events",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "event_signup_entries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "fio",
                table: "event_signup_entries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "event_signup_entries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "revoked_access_tokens",
                columns: table => new
                {
                    jti = table.Column<Guid>(type: "uuid", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_revoked_access_tokens", x => x.jti);
                });

            migrationBuilder.InsertData(
                table: "dynamic_field_types",
                columns: new[] { "id", "title" },
                values: new object[,]
                {
                    { new Guid("006e9c43-991a-49cf-9edb-a70e98bbea9a"), "number" },
                    { new Guid("b9155151-2245-458c-a86b-4aca9a085fb5"), "string" }
                });

            migrationBuilder.InsertData(
                table: "users_roles",
                columns: new[] { "id", "can_edit_others_events", "is_admin", "title" },
                values: new object[,]
                {
                    { new Guid("016f026f-0eb2-4d6e-8bee-0e279e5fc246"), true, false, "Moderator" },
                    { new Guid("82d06a87-b83d-4c91-bef2-64db4b7e77e8"), true, true, "Admin" },
                    { new Guid("b21a4227-ce10-4e66-a34d-0b1158dee441"), false, false, "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "revoked_access_tokens");

            migrationBuilder.DeleteData(
                table: "dynamic_field_types",
                keyColumn: "id",
                keyValue: new Guid("006e9c43-991a-49cf-9edb-a70e98bbea9a"));

            migrationBuilder.DeleteData(
                table: "dynamic_field_types",
                keyColumn: "id",
                keyValue: new Guid("b9155151-2245-458c-a86b-4aca9a085fb5"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("016f026f-0eb2-4d6e-8bee-0e279e5fc246"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("82d06a87-b83d-4c91-bef2-64db4b7e77e8"));

            migrationBuilder.DeleteData(
                table: "users_roles",
                keyColumn: "id",
                keyValue: new Guid("b21a4227-ce10-4e66-a34d-0b1158dee441"));

            migrationBuilder.AlterColumn<string>(
                name: "banner_image_filepath",
                table: "users_events",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "event_signup_entries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fio",
                table: "event_signup_entries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "event_signup_entries",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

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
    }
}
