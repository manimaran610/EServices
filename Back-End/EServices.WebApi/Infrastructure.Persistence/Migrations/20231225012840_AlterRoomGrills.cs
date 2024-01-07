using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterRoomGrills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Effective",
                table: "RoomGrills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Penetration",
                table: "RoomGrills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "RoomGrills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "RoomGrills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UpStreamConcat",
                table: "RoomGrills",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Effective",
                table: "RoomGrills");

            migrationBuilder.DropColumn(
                name: "Penetration",
                table: "RoomGrills");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "RoomGrills");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "RoomGrills");

            migrationBuilder.DropColumn(
                name: "UpStreamConcat",
                table: "RoomGrills");
        }
    }
}
