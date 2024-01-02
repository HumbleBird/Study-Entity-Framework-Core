using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMO_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class ItemGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Itemgrade",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Itemgrade",
                table: "Item");
        }
    }
}
