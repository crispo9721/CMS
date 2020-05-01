﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class removeoncascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Medias_ImageId",
                table: "Articles");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Medias_ImageId",
                table: "Articles",
                column: "ImageId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Medias_ImageId",
                table: "Articles");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Medias_ImageId",
                table: "Articles",
                column: "ImageId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}