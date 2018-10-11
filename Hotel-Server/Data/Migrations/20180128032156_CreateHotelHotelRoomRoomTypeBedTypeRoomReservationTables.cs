using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HotelServer.Migrations
{
    public partial class CreateHotelHotelRoomRoomTypeBedTypeRoomReservationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BedTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BedTypes", x => new { x.Id, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Address = table.Column<string>(maxLength: 200, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => new { x.Id, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "HotelRooms",
                columns: table => new
                {
                    RoomNumber = table.Column<long>(nullable: false),
                    HotelId = table.Column<long>(nullable: false),
                    BedTypeId = table.Column<int>(nullable: false),
                    BedTypeId1 = table.Column<int>(nullable: true),
                    BedTypeName = table.Column<string>(nullable: true),
                    NightlyRate = table.Column<decimal>(nullable: false),
                    NumberOfBeds = table.Column<int>(nullable: false),
                    RoomTypeId = table.Column<int>(nullable: false),
                    RoomTypeId1 = table.Column<int>(nullable: true),
                    RoomTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRooms", x => new { x.RoomNumber, x.HotelId });
                    table.UniqueConstraint("AK_HotelRooms_BedTypeId_HotelId_RoomNumber_RoomTypeId", x => new { x.BedTypeId, x.HotelId, x.RoomNumber, x.RoomTypeId });
                    table.ForeignKey(
                        name: "FK_HotelRooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HotelRooms_BedTypes_BedTypeId1_BedTypeName",
                        columns: x => new { x.BedTypeId1, x.BedTypeName },
                        principalTable: "BedTypes",
                        principalColumns: new[] { "Id", "Name" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HotelRooms_RoomTypes_RoomTypeId1_RoomTypeName",
                        columns: x => new { x.RoomTypeId1, x.RoomTypeName },
                        principalTable: "RoomTypes",
                        principalColumns: new[] { "Id", "Name" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomReservations",
                columns: table => new
                {
                    ReservationId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EndDate = table.Column<DateTime>(nullable: false),
                    HotelId = table.Column<long>(nullable: false),
                    RoomNumber = table.Column<long>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomReservations", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_RoomReservations_HotelRooms_RoomNumber_HotelId",
                        columns: x => new { x.RoomNumber, x.HotelId },
                        principalTable: "HotelRooms",
                        principalColumns: new[] { "RoomNumber", "HotelId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_HotelId",
                table: "HotelRooms",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_BedTypeId1_BedTypeName",
                table: "HotelRooms",
                columns: new[] { "BedTypeId1", "BedTypeName" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelRooms_RoomTypeId1_RoomTypeName",
                table: "HotelRooms",
                columns: new[] { "RoomTypeId1", "RoomTypeName" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomReservations_RoomNumber_HotelId",
                table: "RoomReservations",
                columns: new[] { "RoomNumber", "HotelId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomReservations");

            migrationBuilder.DropTable(
                name: "HotelRooms");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "BedTypes");

            migrationBuilder.DropTable(
                name: "RoomTypes");
        }
    }
}
