using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastTrackEServices.Migrations
{
    /// <inheritdoc />
    public partial class ChangedownedshoeFKshoerepairtonullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoeRepairId",
                table: "OwnedShoes");

            migrationBuilder.AlterColumn<int>(
                name: "shoeRepairId",
                table: "OwnedShoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoeRepairId",
                table: "OwnedShoes",
                column: "shoeRepairId",
                principalTable: "ShoeRepairs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoeRepairId",
                table: "OwnedShoes");

            migrationBuilder.AlterColumn<int>(
                name: "shoeRepairId",
                table: "OwnedShoes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoeRepairId",
                table: "OwnedShoes",
                column: "shoeRepairId",
                principalTable: "ShoeRepairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
