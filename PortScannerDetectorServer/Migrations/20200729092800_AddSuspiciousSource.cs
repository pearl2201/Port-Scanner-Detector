using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PortScannerDetectorServer.Migrations
{
    public partial class AddSuspiciousSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TcpPackets");

            migrationBuilder.DropColumn(
                name: "Suspicious",
                table: "Addresses");

            migrationBuilder.CreateTable(
                name: "SuspiciousSources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ip = table.Column<string>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuspiciousSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuspiciousSources_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuspiciousSources_AddressId",
                table: "SuspiciousSources",
                column: "AddressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuspiciousSources");

            migrationBuilder.AddColumn<bool>(
                name: "Suspicious",
                table: "Addresses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TcpPackets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AddressId = table.Column<int>(type: "integer", nullable: true),
                    DstIp = table.Column<string>(type: "text", nullable: false),
                    DstPort = table.Column<int>(type: "integer", nullable: false),
                    SrcIp = table.Column<string>(type: "text", nullable: false),
                    SrcPort = table.Column<int>(type: "integer", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TcpPackets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TcpPackets_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TcpPackets_AddressId",
                table: "TcpPackets",
                column: "AddressId");
        }
    }
}
