using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseDesk.Migrations
{
    /// <inheritdoc />
    public partial class Initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursesMaterials_Category_CourseCategoryId",
                table: "CoursesMaterials");

            migrationBuilder.RenameColumn(
                name: "CourseCategoryId",
                table: "CoursesMaterials",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursesMaterials_CourseCategoryId",
                table: "CoursesMaterials",
                newName: "IX_CoursesMaterials_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesMaterials_Category_CategoryId",
                table: "CoursesMaterials",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursesMaterials_Category_CategoryId",
                table: "CoursesMaterials");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "CoursesMaterials",
                newName: "CourseCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_CoursesMaterials_CategoryId",
                table: "CoursesMaterials",
                newName: "IX_CoursesMaterials_CourseCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesMaterials_Category_CourseCategoryId",
                table: "CoursesMaterials",
                column: "CourseCategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
