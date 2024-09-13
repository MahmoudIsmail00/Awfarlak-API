using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class removesubId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubCategoryId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubCategoryId1",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId1",
                table: "Products",
                column: "SubCategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId1",
                table: "Products",
                column: "SubCategoryId1",
                principalTable: "SubCategories",
                principalColumn: "Id");
        }
    }
}
