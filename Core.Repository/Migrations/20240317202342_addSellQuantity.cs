using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addSellQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellQuantity",
                table: "CartProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellQuantity",
                table: "CartProducts");
        }
    }
}
