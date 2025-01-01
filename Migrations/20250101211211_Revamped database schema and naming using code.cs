using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastTrackEServices.Migrations
{
    /// <inheritdoc />
    public partial class Revampeddatabaseschemaandnamingusingcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_Clients_clientId",
                table: "OwnedShoes");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoewareRepairId",
                table: "OwnedShoes");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_Shoewares_shoeId",
                table: "OwnedShoes");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoeRepairs_Clients_clientId",
                table: "ShoeRepairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoeRepairs",
                table: "ShoeRepairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedShoes",
                table: "OwnedShoes");

            migrationBuilder.RenameTable(
                name: "ShoeRepairs",
                newName: "ShoewareRepairs");

            migrationBuilder.RenameTable(
                name: "OwnedShoes",
                newName: "OwnedShoewares");

            migrationBuilder.RenameIndex(
                name: "IX_ShoeRepairs_clientId",
                table: "ShoewareRepairs",
                newName: "IX_ShoewareRepairs_clientId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoes_shoewareRepairId",
                table: "OwnedShoewares",
                newName: "IX_OwnedShoewares_shoewareRepairId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoes_shoeId",
                table: "OwnedShoewares",
                newName: "IX_OwnedShoewares_shoeId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoes_clientId",
                table: "OwnedShoewares",
                newName: "IX_OwnedShoewares_clientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoewareRepairs",
                table: "ShoewareRepairs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedShoewares",
                table: "OwnedShoewares",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoewares_Clients_clientId",
                table: "OwnedShoewares",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoewares_ShoewareRepairs_shoewareRepairId",
                table: "OwnedShoewares",
                column: "shoewareRepairId",
                principalTable: "ShoewareRepairs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoewares_Shoewares_shoeId",
                table: "OwnedShoewares",
                column: "shoeId",
                principalTable: "Shoewares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoewareRepairs_Clients_clientId",
                table: "ShoewareRepairs",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoewares_Clients_clientId",
                table: "OwnedShoewares");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoewares_ShoewareRepairs_shoewareRepairId",
                table: "OwnedShoewares");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoewares_Shoewares_shoeId",
                table: "OwnedShoewares");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoewareRepairs_Clients_clientId",
                table: "ShoewareRepairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoewareRepairs",
                table: "ShoewareRepairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OwnedShoewares",
                table: "OwnedShoewares");

            migrationBuilder.RenameTable(
                name: "ShoewareRepairs",
                newName: "ShoeRepairs");

            migrationBuilder.RenameTable(
                name: "OwnedShoewares",
                newName: "OwnedShoes");

            migrationBuilder.RenameIndex(
                name: "IX_ShoewareRepairs_clientId",
                table: "ShoeRepairs",
                newName: "IX_ShoeRepairs_clientId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoewares_shoewareRepairId",
                table: "OwnedShoes",
                newName: "IX_OwnedShoes_shoewareRepairId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoewares_shoeId",
                table: "OwnedShoes",
                newName: "IX_OwnedShoes_shoeId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoewares_clientId",
                table: "OwnedShoes",
                newName: "IX_OwnedShoes_clientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoeRepairs",
                table: "ShoeRepairs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OwnedShoes",
                table: "OwnedShoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoes_Clients_clientId",
                table: "OwnedShoes",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoewareRepairId",
                table: "OwnedShoes",
                column: "shoewareRepairId",
                principalTable: "ShoeRepairs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoes_Shoewares_shoeId",
                table: "OwnedShoes",
                column: "shoeId",
                principalTable: "Shoewares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoeRepairs_Clients_clientId",
                table: "ShoeRepairs",
                column: "clientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
