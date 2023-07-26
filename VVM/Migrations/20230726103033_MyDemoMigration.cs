using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VVM.Migrations
{
    /// <inheritdoc />
    public partial class MyDemoMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "Drinks",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Drinks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Drinks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "Drinks",
                newName: "Count");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Drinks",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Drinks",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Drinks",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Drinks",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Drinks",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Drinks",
                newName: "count");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Drinks",
                newName: "id");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                table: "Drinks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
