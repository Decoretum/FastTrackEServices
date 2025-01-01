using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastTrackEServices.Migrations
{
    /// <inheritdoc />
    public partial class Revampedtableentityproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoeRepairId",
                table: "OwnedShoes");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_Shoes_shoeId",
                table: "OwnedShoes");

            migrationBuilder.DropTable(
                name: "ShoeColors");

            migrationBuilder.DropTable(
                name: "ShoeOrders");

            migrationBuilder.DropTable(
                name: "Shoes");

            migrationBuilder.RenameColumn(
                name: "shoeRepairId",
                table: "OwnedShoes",
                newName: "shoewareRepairId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoes_shoeRepairId",
                table: "OwnedShoes",
                newName: "IX_OwnedShoes_shoewareRepairId");

            migrationBuilder.CreateTable(
                name: "Shoewares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    brand = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoewares", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShoewareColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shoeId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoewareColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoewareColors_Shoewares_shoeId",
                        column: x => x.shoeId,
                        principalTable: "Shoewares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShoewareOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shoewareId = table.Column<int>(type: "int", nullable: false),
                    orderCartId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    shoeColor = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoewareOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoewareOrders_OrderCarts_orderCartId",
                        column: x => x.orderCartId,
                        principalTable: "OrderCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoewareOrders_Shoewares_shoewareId",
                        column: x => x.shoewareId,
                        principalTable: "Shoewares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ShoewareColors_shoeId",
                table: "ShoewareColors",
                column: "shoeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoewareOrders_orderCartId",
                table: "ShoewareOrders",
                column: "orderCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoewareOrders_shoewareId",
                table: "ShoewareOrders",
                column: "shoewareId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoewareRepairId",
                table: "OwnedShoes");

            migrationBuilder.DropForeignKey(
                name: "FK_OwnedShoes_Shoewares_shoeId",
                table: "OwnedShoes");

            migrationBuilder.DropTable(
                name: "ShoewareColors");

            migrationBuilder.DropTable(
                name: "ShoewareOrders");

            migrationBuilder.DropTable(
                name: "Shoewares");

            migrationBuilder.RenameColumn(
                name: "shoewareRepairId",
                table: "OwnedShoes",
                newName: "shoeRepairId");

            migrationBuilder.RenameIndex(
                name: "IX_OwnedShoes_shoewareRepairId",
                table: "OwnedShoes",
                newName: "IX_OwnedShoes_shoeRepairId");

            migrationBuilder.CreateTable(
                name: "Shoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    brand = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    stock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShoeColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shoeId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoeColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoeColors_Shoes_shoeId",
                        column: x => x.shoeId,
                        principalTable: "Shoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ShoeOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    orderCartId = table.Column<int>(type: "int", nullable: false),
                    shoeId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    shoeColor = table.Column<string>(type: "varchar(50)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoeOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoeOrders_OrderCarts_orderCartId",
                        column: x => x.orderCartId,
                        principalTable: "OrderCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoeOrders_Shoes_shoeId",
                        column: x => x.shoeId,
                        principalTable: "Shoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeColors_shoeId",
                table: "ShoeColors",
                column: "shoeId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeOrders_orderCartId",
                table: "ShoeOrders",
                column: "orderCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeOrders_shoeId",
                table: "ShoeOrders",
                column: "shoeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoes_ShoeRepairs_shoeRepairId",
                table: "OwnedShoes",
                column: "shoeRepairId",
                principalTable: "ShoeRepairs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OwnedShoes_Shoes_shoeId",
                table: "OwnedShoes",
                column: "shoeId",
                principalTable: "Shoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
