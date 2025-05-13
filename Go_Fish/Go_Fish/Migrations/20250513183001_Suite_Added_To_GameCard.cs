using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoFish.Migrations
{
    /// <inheritdoc />
    public partial class Suite_Added_To_GameCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Suite",
                table: "GameCards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suite",
                table: "GameCards");
        }
    }
}
