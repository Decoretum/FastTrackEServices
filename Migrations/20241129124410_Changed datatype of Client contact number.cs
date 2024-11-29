using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastTrackEServices.Migrations
{
    /// <inheritdoc />
    public partial class ChangeddatatypeofClientcontactnumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "contactNumber",
                table: "Clients",
                type: "varchar(11)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "contactNumber",
                table: "Clients",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
