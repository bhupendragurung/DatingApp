using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e01"), "7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e01", "Member", "MEMBER" },
                    { new Guid("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e02"), "7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e02", "Admin", "ADMIN" },
                    { new Guid("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e03"), "7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e03", "Moderator", "MODERATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e01"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e02"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7d3f9a4e-2b1c-4e8a-9f6d-1a2b3c4d5e03"));
        }
    }
}
