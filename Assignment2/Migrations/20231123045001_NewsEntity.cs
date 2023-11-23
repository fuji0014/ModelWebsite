using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment2.Migrations
{
    public partial class NewsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    SportClubId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.SportClubId);
                    table.ForeignKey(
                        name: "FK_News_SportClub_SportClubId",
                        column: x => x.SportClubId,
                        principalTable: "SportClub",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
