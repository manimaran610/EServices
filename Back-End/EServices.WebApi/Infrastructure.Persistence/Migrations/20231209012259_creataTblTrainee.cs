using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class creataTblTrainee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TraineeId",
                table: "CustomerDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Trainees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerDetails_TraineeId",
                table: "CustomerDetails",
                column: "TraineeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerDetails_Trainees_TraineeId",
                table: "CustomerDetails",
                column: "TraineeId",
                principalTable: "Trainees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerDetails_Trainees_TraineeId",
                table: "CustomerDetails");

            migrationBuilder.DropTable(
                name: "Trainees");

            migrationBuilder.DropIndex(
                name: "IX_CustomerDetails_TraineeId",
                table: "CustomerDetails");

            migrationBuilder.DropColumn(
                name: "TraineeId",
                table: "CustomerDetails");
        }
    }
}
