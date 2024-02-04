using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AlterRoomGrillv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpStreamConcaLtr",
                table: "RoomGrills",
                newName: "UpStreamConcatLtr");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpStreamConcatLtr",
                table: "RoomGrills",
                newName: "UpStreamConcaLtr");
        }
    }
}
