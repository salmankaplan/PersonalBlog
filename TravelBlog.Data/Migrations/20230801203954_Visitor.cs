using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravelBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class Visitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("40019f76-0e0d-47fa-9886-b5dbff75b370"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("fde2a814-446c-4217-9fa1-041dce561699"));

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IpAdress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleVisitors",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleVisitors", x => new { x.ArticleId, x.VisitorId });
                    table.ForeignKey(
                        name: "FK_ArticleVisitors_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleVisitors_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("55d0c696-1c10-4324-a319-d4175f937aa8"),
                column: "ConcurrencyStamp",
                value: "8629c61d-b4c3-4256-b4ad-ee271f7eab21");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("9e9d5a66-9b26-4484-ba31-6f51ee6f5bcb"),
                column: "ConcurrencyStamp",
                value: "a92a505a-fa15-4ed5-8a12-1690674dbb55");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("ea808ace-2727-4aef-b764-173b5a414630"),
                column: "ConcurrencyStamp",
                value: "d0d0a01e-8349-45bd-9b09-c8cfbcf9d090");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("391d592b-eed1-4e72-94ab-10b341d0739c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "817b1d20-ab1b-4b96-bf42-bd47c5c6d153", "AQAAAAEAACcQAAAAEBek3/UitU/zVex9tcqUmM3e8m/NO9ZTTm0/olQSwfl4Hy/IGuEAXxlqyQyhpWyZCw==", "45f1ea36-8cc2-4492-933c-772fb8aac645" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("6a7e0458-79d2-4e4d-a34c-609ce6ca1c00"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4513193f-1cbe-4a45-a5f8-e598c8506ac0", "AQAAAAEAACcQAAAAEMTqsmzsf9bSvcwvd6VOpjFaupWzvt6TGylboaoKM1EvBTb13mW8XeKhiNRhTxgYXQ==", "dbbb68cb-c916-4624-a6b1-c8b0f02678c5" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CreatedBy", "CreatedDate", "DeleteDate", "DeletedBy", "ImageId", "IsDeleted", "ModifiedBy", "ModifiedDate", "Title", "UserId", "ViewCount" },
                values: new object[,]
                {
                    { new Guid("d8ba1ff3-97a2-4319-bb35-b6ae6e300311"), new Guid("04aaca8b-5522-462f-ab6e-471717b04052"), "Visual Studio fames convallis velit vehicula neque commodo at ridiculus ultrices augue maecenas egestas eu a nulla phasellus viverra eleifend", "Admin Test", new DateTime(2023, 8, 1, 23, 39, 54, 108, DateTimeKind.Local).AddTicks(8652), null, null, new Guid("0c8f4329-3779-4848-959d-21a44658b6ee"), false, null, null, "Visual Studio Deneme Makalesi 1", new Guid("6a7e0458-79d2-4e4d-a34c-609ce6ca1c00"), 14 },
                    { new Guid("e09ccceb-bd22-4f5c-9f94-f571e77f454d"), new Guid("d200bb97-7952-4c15-bb5a-00463e3e1f0c"), "Aspnet Core fames convallis velit vehicula neque commodo at ridiculus ultrices augue maecenas egestas eu a nulla phasellus viverra eleifend", "Admin Test", new DateTime(2023, 8, 1, 23, 39, 54, 108, DateTimeKind.Local).AddTicks(8639), null, null, new Guid("3a119c03-576d-4c71-b180-5e67df74a69e"), false, null, null, "Aspnet Core Deneme Makalesi 1", new Guid("391d592b-eed1-4e72-94ab-10b341d0739c"), 14 }
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("04aaca8b-5522-462f-ab6e-471717b04052"),
                column: "CreatedDate",
                value: new DateTime(2023, 8, 1, 23, 39, 54, 109, DateTimeKind.Local).AddTicks(696));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d200bb97-7952-4c15-bb5a-00463e3e1f0c"),
                column: "CreatedDate",
                value: new DateTime(2023, 8, 1, 23, 39, 54, 109, DateTimeKind.Local).AddTicks(687));

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("0c8f4329-3779-4848-959d-21a44658b6ee"),
                column: "CreatedDate",
                value: new DateTime(2023, 8, 1, 23, 39, 54, 109, DateTimeKind.Local).AddTicks(834));

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("3a119c03-576d-4c71-b180-5e67df74a69e"),
                column: "CreatedDate",
                value: new DateTime(2023, 8, 1, 23, 39, 54, 109, DateTimeKind.Local).AddTicks(830));

            migrationBuilder.CreateIndex(
                name: "IX_ArticleVisitors_VisitorId",
                table: "ArticleVisitors",
                column: "VisitorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleVisitors");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("d8ba1ff3-97a2-4319-bb35-b6ae6e300311"));

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: new Guid("e09ccceb-bd22-4f5c-9f94-f571e77f454d"));

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("55d0c696-1c10-4324-a319-d4175f937aa8"),
                column: "ConcurrencyStamp",
                value: "025fbc7f-6f1b-46ea-9e71-7601162af4dd");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("9e9d5a66-9b26-4484-ba31-6f51ee6f5bcb"),
                column: "ConcurrencyStamp",
                value: "10245f7f-8a2f-4d1c-8c44-b953da55fb64");

            migrationBuilder.UpdateData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("ea808ace-2727-4aef-b764-173b5a414630"),
                column: "ConcurrencyStamp",
                value: "73b46c23-a768-46e1-b3be-95b850800c82");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("391d592b-eed1-4e72-94ab-10b341d0739c"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d6cba1fe-6f8e-4c2b-9d37-fdd50a536643", "AQAAAAEAACcQAAAAEOv9JPRe++u/mPKEJ7IUAfCnRyUQKch8VA2eXm0Vos9TeaigmML57/lZBtRZgQ8gcQ==", "6838bf08-64d0-43d8-a06d-6dc8b8392995" });

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("6a7e0458-79d2-4e4d-a34c-609ce6ca1c00"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54b4551b-54e8-4407-a02c-d09ad00ddece", "AQAAAAEAACcQAAAAEPKYrR6P4AC1BEIi5kug1lm0K/MNG4F0O7sTaw4kDOumyUXOW3sKqaa8q2GzWCmt8A==", "fd727ab9-2193-4a37-8f19-2e4e500047d2" });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "CategoryId", "Content", "CreatedBy", "CreatedDate", "DeleteDate", "DeletedBy", "ImageId", "IsDeleted", "ModifiedBy", "ModifiedDate", "Title", "UserId", "ViewCount" },
                values: new object[,]
                {
                    { new Guid("40019f76-0e0d-47fa-9886-b5dbff75b370"), new Guid("04aaca8b-5522-462f-ab6e-471717b04052"), "Visual Studio fames convallis velit vehicula neque commodo at ridiculus ultrices augue maecenas egestas eu a nulla phasellus viverra eleifend", "Admin Test", new DateTime(2023, 6, 6, 2, 6, 55, 739, DateTimeKind.Local).AddTicks(5281), null, null, new Guid("0c8f4329-3779-4848-959d-21a44658b6ee"), false, null, null, "Visual Studio Deneme Makalesi 1", new Guid("6a7e0458-79d2-4e4d-a34c-609ce6ca1c00"), 14 },
                    { new Guid("fde2a814-446c-4217-9fa1-041dce561699"), new Guid("d200bb97-7952-4c15-bb5a-00463e3e1f0c"), "Aspnet Core fames convallis velit vehicula neque commodo at ridiculus ultrices augue maecenas egestas eu a nulla phasellus viverra eleifend", "Admin Test", new DateTime(2023, 6, 6, 2, 6, 55, 739, DateTimeKind.Local).AddTicks(5259), null, null, new Guid("3a119c03-576d-4c71-b180-5e67df74a69e"), false, null, null, "Aspnet Core Deneme Makalesi 1", new Guid("391d592b-eed1-4e72-94ab-10b341d0739c"), 14 }
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("04aaca8b-5522-462f-ab6e-471717b04052"),
                column: "CreatedDate",
                value: new DateTime(2023, 6, 6, 2, 6, 55, 739, DateTimeKind.Local).AddTicks(5585));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d200bb97-7952-4c15-bb5a-00463e3e1f0c"),
                column: "CreatedDate",
                value: new DateTime(2023, 6, 6, 2, 6, 55, 739, DateTimeKind.Local).AddTicks(5576));

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("0c8f4329-3779-4848-959d-21a44658b6ee"),
                column: "CreatedDate",
                value: new DateTime(2023, 6, 6, 2, 6, 55, 739, DateTimeKind.Local).AddTicks(5785));

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: new Guid("3a119c03-576d-4c71-b180-5e67df74a69e"),
                column: "CreatedDate",
                value: new DateTime(2023, 6, 6, 2, 6, 55, 739, DateTimeKind.Local).AddTicks(5776));
        }
    }
}
