using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Globomantics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeUsersLazy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_AssigedToId",
                table: "Todo");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_Bug_AssigedToId",
                table: "Todo");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo");

            migrationBuilder.RenameColumn(
                name: "Bug_AssigedToId",
                table: "Todo",
                newName: "Bug_AssignedToId");

            migrationBuilder.RenameColumn(
                name: "AssigedToId",
                table: "Todo",
                newName: "AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Todo_Bug_AssigedToId",
                table: "Todo",
                newName: "IX_Todo_Bug_AssignedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Todo_AssigedToId",
                table: "Todo",
                newName: "IX_Todo_AssignedToId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Todo",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_AssignedToId",
                table: "Todo",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_Bug_AssignedToId",
                table: "Todo",
                column: "Bug_AssignedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_AssignedToId",
                table: "Todo");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_Bug_AssignedToId",
                table: "Todo");

            migrationBuilder.DropForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo");

            migrationBuilder.RenameColumn(
                name: "Bug_AssignedToId",
                table: "Todo",
                newName: "Bug_AssigedToId");

            migrationBuilder.RenameColumn(
                name: "AssignedToId",
                table: "Todo",
                newName: "AssigedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Todo_Bug_AssignedToId",
                table: "Todo",
                newName: "IX_Todo_Bug_AssigedToId");

            migrationBuilder.RenameIndex(
                name: "IX_Todo_AssignedToId",
                table: "Todo",
                newName: "IX_Todo_AssigedToId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedById",
                table: "Todo",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_AssigedToId",
                table: "Todo",
                column: "AssigedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_Bug_AssigedToId",
                table: "Todo",
                column: "Bug_AssigedToId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todo_Users_CreatedById",
                table: "Todo",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
