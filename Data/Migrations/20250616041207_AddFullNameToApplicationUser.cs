using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITAssetManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ActivityLogs");

            migrationBuilder.RenameColumn(
                name: "ActivityLogID",
                table: "ActivityLogs",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "AssetID",
                table: "ActivityLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AssignedTo",
                table: "Assets",
                column: "AssignedTo");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Employees_AssignedTo",
                table: "Assets",
                column: "AssignedTo",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Employees_AssignedTo",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_AssignedTo",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "AssetID",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "ActivityLogs");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ActivityLogs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActivityLogs",
                newName: "ActivityLogID");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ActivityLogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ActivityLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
