using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projekat.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchasedAt",
                table: "Purchases",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Purchases",
                newName: "PurchasedAt");
        }
    }
}
