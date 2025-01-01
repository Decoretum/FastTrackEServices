using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastTrackEServices.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    clientId = table.Column<int>(type: "int", nullable: false),
                    cart_name = table.Column<string>(type: "varchar(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dateRegistered = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    dateConfirmed = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCarts_Clients_clientId",
                        column: x => x.clientId,
                        principalTable: "Clients",
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
                    shoeId = table.Column<int>(type: "int", nullable: false),
                    orderCartId = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_OrderCarts_clientId",
                table: "OrderCarts",
                column: "clientId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeOrders_orderCartId",
                table: "ShoeOrders",
                column: "orderCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeOrders_shoeId",
                table: "ShoeOrders",
                column: "shoeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoeOrders");

            migrationBuilder.DropTable(
                name: "OrderCarts");
        }
    }
}
