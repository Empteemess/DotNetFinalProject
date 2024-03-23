using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addedSelledProductPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "WholeSelledProductPrice",
                table: "CartProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WholeSelledProductPrice",
                table: "CartProducts");
        }
    }
}
