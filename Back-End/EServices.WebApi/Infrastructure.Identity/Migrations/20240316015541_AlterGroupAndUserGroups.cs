using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AlterGroupAndUserGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                schema: "Identity",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                schema: "Identity",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "Groups",
                schema: "Identity",
                newName: "Group",
                newSchema: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Group",
                schema: "Identity",
                table: "Group",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Group_GroupId",
                schema: "Identity",
                table: "UserGroups",
                column: "GroupId",
                principalSchema: "Identity",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Group_GroupId",
                schema: "Identity",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Group",
                schema: "Identity",
                table: "Group");

            migrationBuilder.RenameTable(
                name: "Group",
                schema: "Identity",
                newName: "Groups",
                newSchema: "Identity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                schema: "Identity",
                table: "Groups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                schema: "Identity",
                table: "UserGroups",
                column: "GroupId",
                principalSchema: "Identity",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
