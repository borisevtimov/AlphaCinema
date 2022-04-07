using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlphaCinema.Infrastructure.Data.Migrations
{
    public partial class AddedBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Cards",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Cards");
        }
    }
}
