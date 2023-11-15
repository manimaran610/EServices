using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RoomandGrillsTableCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignACPH = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoOfGrills = table.Column<int>(type: "int", nullable: false),
                    RoomVolume = table.Column<int>(type: "int", nullable: false),
                    TotalAirFlowCFM = table.Column<int>(type: "int", nullable: false),
                    AirChangesPerHour = table.Column<int>(type: "int", nullable: false),
                    CustomerDetailId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_CustomerDetails_CustomerDetailId",
                        column: x => x.CustomerDetailId,
                        principalTable: "CustomerDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomGrills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilterAreaSqft = table.Column<int>(type: "int", nullable: false),
                    AirVelocityReadingInFPMO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvgVelocityFPM = table.Column<int>(type: "int", nullable: false),
                    AirFlowCFM = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomGrills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomGrills_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomGrills_RoomId",
                table: "RoomGrills",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CustomerDetailId",
                table: "Rooms",
                column: "CustomerDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomGrills");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
