using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class addrolestodatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2df119cc-93aa-44b8-a807-1340597469ac", null, "User", "USER" },
                    { "ceffc352-505f-4c31-abdd-aa2bfbe82e9c", null, "Editor", "EDITOR" },
                    { "f6b9143d-5b98-4f64-bc09-9d874b6854cf", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2df119cc-93aa-44b8-a807-1340597469ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ceffc352-505f-4c31-abdd-aa2bfbe82e9c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6b9143d-5b98-4f64-bc09-9d874b6854cf");
        }
    }
}
