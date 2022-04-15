using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlphaCinema.Infrastructure.Migrations
{
    public partial class RemovedIsUsedFromVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Vouchers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Vouchers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
