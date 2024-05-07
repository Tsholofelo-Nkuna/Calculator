using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Calculator.SQLServer.DAL.Migrations
{
    public partial class RemoveMemoryRecallTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoryRecall");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemoryRecall",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Inactive = table.Column<bool>(type: "bit", nullable: false),
                    MRValue = table.Column<double>(type: "float", nullable: false),
                    MasterId = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryRecall", x => x.Id);
                });
        }
    }
}
