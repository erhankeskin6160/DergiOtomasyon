using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DergiOtomasyon.Migrations
{
    /// <inheritdoc />
    public partial class editmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MagazineId",
                table: "Categories",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MagazineId",
                table: "Categories");
        }
    }
}
