using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "PostCategories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PostCategories_UserId",
                table: "PostCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategories_Users_UserId",
                table: "PostCategories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategories_Users_UserId",
                table: "PostCategories");

            migrationBuilder.DropIndex(
                name: "IX_PostCategories_UserId",
                table: "PostCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostCategories");
        }
    }
}
