using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DergiOtomasyon.Migrations
{
    /// <inheritdoc />
    public partial class editmodel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_MagazinesInfo_MagazineInfoId",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Borrowings_MagazineInfoId",
                table: "Borrowings");

            migrationBuilder.AddColumn<int>(
                name: "BorrowingId",
                table: "MagazinesInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MagazinesInfo_BorrowingId",
                table: "MagazinesInfo",
                column: "BorrowingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MagazinesInfo_Borrowings_BorrowingId",
                table: "MagazinesInfo",
                column: "BorrowingId",
                principalTable: "Borrowings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MagazinesInfo_Borrowings_BorrowingId",
                table: "MagazinesInfo");

            migrationBuilder.DropIndex(
                name: "IX_MagazinesInfo_BorrowingId",
                table: "MagazinesInfo");

            migrationBuilder.DropColumn(
                name: "BorrowingId",
                table: "MagazinesInfo");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_MagazineInfoId",
                table: "Borrowings",
                column: "MagazineInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_MagazinesInfo_MagazineInfoId",
                table: "Borrowings",
                column: "MagazineInfoId",
                principalTable: "MagazinesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
