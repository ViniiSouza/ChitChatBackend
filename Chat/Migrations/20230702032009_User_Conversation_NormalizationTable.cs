using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Migrations
{
    public partial class User_Conversation_NormalizationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Conversation_ConversationId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ConversationId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "User");

            migrationBuilder.CreateTable(
                name: "User_Conversation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Conversation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Conversation_Conversation_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Conversation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_Conversation_ConversationId",
                table: "User_Conversation",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Conversation_UserId",
                table: "User_Conversation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User_Conversation");

            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ConversationId",
                table: "User",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Conversation_ConversationId",
                table: "User",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "Id");
        }
    }
}
