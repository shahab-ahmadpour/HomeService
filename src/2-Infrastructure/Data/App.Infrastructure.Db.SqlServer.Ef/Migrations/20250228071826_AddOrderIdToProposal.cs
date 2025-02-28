using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Infrastructure.Db.SqlServer.Ef.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIdToProposal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Proposals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3dc51e8b-7ca8-4738-a1b1-86d1a7aab177", new DateTime(2025, 2, 28, 7, 18, 25, 488, DateTimeKind.Utc).AddTicks(7771), "AQAAAAIAAYagAAAAEDw+MTrzIYPTPlKj+OMGz3MMTjAtuAccrOeIVZSxSI0lRdpcBnx4Ki+btJdCHpjlbQ==", "9fc31881-dca6-492c-922c-dfd707727388" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3e4ddbdd-3c5c-4e1a-941a-12f7cd012201", new DateTime(2025, 2, 28, 7, 18, 25, 545, DateTimeKind.Utc).AddTicks(3779), "AQAAAAIAAYagAAAAEI/p3D//Q7NUD8LvSfpxmuBBfyywST6O1VoiWR3iqvUQ3G88cezz6hHqY3tAfvCC8A==", "8613464b-e30a-400a-aeac-c1a3a8eb277c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9948f5bf-98e2-4295-b074-c7580fcc9d73", new DateTime(2025, 2, 28, 7, 18, 25, 601, DateTimeKind.Utc).AddTicks(4536), "AQAAAAIAAYagAAAAEPfGdviAcaITsXJnlQT2PcnDZswMHEptfh04xaOclWrFJ6OsRvpMhwYqZOI+Nyl8eQ==", "e028a20c-38fb-4ec6-b777-cd1f4dfdcb16" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26fcbff9-365b-4b56-8202-074fc5c99245", new DateTime(2025, 2, 28, 7, 18, 25, 657, DateTimeKind.Utc).AddTicks(8385), "AQAAAAIAAYagAAAAEAkfVAAWaqBcPG9v+SRcwd5sAx+6XoDv5h+Fz3dpOMmsMgQUSV7867Flz8NcH0VFPg==", "cbd7e1a2-b995-4717-8728-0c3aea9642e4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4df45e40-ac5f-4101-a698-273ea5a81b84", new DateTime(2025, 2, 28, 7, 18, 25, 714, DateTimeKind.Utc).AddTicks(4846), "AQAAAAIAAYagAAAAEJkSodK+fZ0/C4JLvetbhNCQkf9F8H0VlMzDTQ2TmM168J7DN3Eqoa3xze3N2N4d1Q==", "aed21c49-9e13-471f-b262-18f59bdbf6b0" });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 7, 18, 25, 487, DateTimeKind.Utc).AddTicks(6697));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 7, 18, 25, 487, DateTimeKind.Utc).AddTicks(6702));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 7, 18, 25, 487, DateTimeKind.Utc).AddTicks(6704));

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExecutionDate", "OrderId", "ResponseTime" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6157), new DateTime(2025, 3, 5, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6153), 1, new DateTime(2025, 3, 1, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6156) });

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ExecutionDate", "OrderId", "ResponseTime" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6160), new DateTime(2025, 3, 3, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6159), 2, new DateTime(2025, 3, 1, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6160) });

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ExecutionDate", "OrderId", "ResponseTime" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6163), new DateTime(2025, 3, 7, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6162), 3, new DateTime(2025, 3, 2, 7, 18, 25, 486, DateTimeKind.Utc).AddTicks(6163) });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Deadline", "ExecutionDate" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1442), new DateTime(2025, 3, 5, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1436), new DateTime(2025, 3, 3, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1442) });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Deadline", "ExecutionDate" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1446), new DateTime(2025, 3, 7, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1445), new DateTime(2025, 3, 5, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1446) });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Deadline", "ExecutionDate" },
                values: new object[] { new DateTime(2025, 2, 28, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1449), new DateTime(2025, 3, 10, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1448), new DateTime(2025, 3, 8, 7, 18, 25, 484, DateTimeKind.Utc).AddTicks(1449) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 7, 18, 25, 488, DateTimeKind.Utc).AddTicks(800));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 7, 18, 25, 488, DateTimeKind.Utc).AddTicks(803));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 7, 18, 25, 488, DateTimeKind.Utc).AddTicks(804));

            migrationBuilder.CreateIndex(
                name: "IX_Proposals_OrderId",
                table: "Proposals",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Orders_OrderId",
                table: "Proposals",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Orders_OrderId",
                table: "Proposals");

            migrationBuilder.DropIndex(
                name: "IX_Proposals_OrderId",
                table: "Proposals");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Proposals");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4a0539d5-4327-49b3-a44c-73f88748625e", new DateTime(2025, 2, 28, 5, 57, 8, 581, DateTimeKind.Utc).AddTicks(6143), "AQAAAAIAAYagAAAAENkab+AKAcEsKbc8ec5rCQq+0OjcCs4PD6hBiDi1oqHJBx9s4L7xOiZBl2sIW2ddng==", "192824e7-3868-41ea-9322-5bc5ffa55c2f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "27fef48f-bf7d-4d15-9e83-2a944045bf58", new DateTime(2025, 2, 28, 5, 57, 8, 641, DateTimeKind.Utc).AddTicks(1939), "AQAAAAIAAYagAAAAEIoJ9Y1haGg1ATrA15FrNJmMVcK63dvyZtRvcIv63UQRvnrw4cXuxq1MrFbDuY3YGg==", "35c61bd4-1ae0-422c-91b9-b3f93249da8d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b80c8df1-cb8a-4029-9c81-e710e4e1a6be", new DateTime(2025, 2, 28, 5, 57, 8, 698, DateTimeKind.Utc).AddTicks(2559), "AQAAAAIAAYagAAAAEN9NrYuI/u+9Eycpc1rVdiJq62+9M5jMufoJvrqoXE6WnlssHTvT4FNNLskcNfGmZA==", "6b2c9c2e-9fe8-4fad-8c32-ca2bd85b931b" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8c4f3451-aa9d-495e-86d9-e034a3bdf30c", new DateTime(2025, 2, 28, 5, 57, 8, 754, DateTimeKind.Utc).AddTicks(8096), "AQAAAAIAAYagAAAAEIwTKzITbObVjVJKZrUuvHPeESkOoGw4FIXbnsUrAC12FdQvPiN/Y3efUsMo4p0ypw==", "b739b8dd-70cf-4329-a483-10cdc1623b32" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "914c7dd0-dbda-434b-8c02-844acc9822ce", new DateTime(2025, 2, 28, 5, 57, 8, 811, DateTimeKind.Utc).AddTicks(4726), "AQAAAAIAAYagAAAAENFg9qIB2kKt2qwWUzPObRqe05JPv5rUG7+p+P66ynWVySXchP/r0GPkqbbHxdP6aA==", "64e3a09d-b3fd-47f5-b8e0-572b2a43d611" });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 5, 57, 8, 580, DateTimeKind.Utc).AddTicks(5477));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 5, 57, 8, 580, DateTimeKind.Utc).AddTicks(5481));

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 5, 57, 8, 580, DateTimeKind.Utc).AddTicks(5483));

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ExecutionDate", "ResponseTime" },
                values: new object[] { new DateTime(2025, 2, 28, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5657), new DateTime(2025, 3, 5, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5655), new DateTime(2025, 3, 1, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5657) });

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ExecutionDate", "ResponseTime" },
                values: new object[] { new DateTime(2025, 2, 28, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5660), new DateTime(2025, 3, 3, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5659), new DateTime(2025, 3, 1, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5660) });

            migrationBuilder.UpdateData(
                table: "Proposals",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ExecutionDate", "ResponseTime" },
                values: new object[] { new DateTime(2025, 2, 28, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5663), new DateTime(2025, 3, 7, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5662), new DateTime(2025, 3, 2, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(5662) });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Deadline", "ExecutionDate" },
                values: new object[] { new DateTime(2025, 2, 28, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1831), new DateTime(2025, 3, 5, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1824), new DateTime(2025, 3, 3, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1830) });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Deadline", "ExecutionDate" },
                values: new object[] { new DateTime(2025, 2, 28, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1834), new DateTime(2025, 3, 7, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1833), new DateTime(2025, 3, 5, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1833) });

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Deadline", "ExecutionDate" },
                values: new object[] { new DateTime(2025, 2, 28, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1836), new DateTime(2025, 3, 10, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1835), new DateTime(2025, 3, 8, 5, 57, 8, 579, DateTimeKind.Utc).AddTicks(1836) });

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 5, 57, 8, 580, DateTimeKind.Utc).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 5, 57, 8, 580, DateTimeKind.Utc).AddTicks(9640));

            migrationBuilder.UpdateData(
                table: "Reviews",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 2, 28, 5, 57, 8, 580, DateTimeKind.Utc).AddTicks(9642));
        }
    }
}
