using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assignment2.Migrations
{
    public partial class CreateNewsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SportClubId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_SportClub_SportClubId",
                        column: x => x.SportClubId,
                        principalTable: "SportClub",
                        principalColumn: "Id");
                });
            
            migrationBuilder.CreateIndex(
                name: "IX_News_SportClubId",
                table: "News",
                column: "SportClubId"); */

            migrationBuilder.Sql(
                @"
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'News')
                BEGIN
                    CREATE TABLE News (
                        Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
                        SportClubId NVARCHAR(450),
                        FileName NVARCHAR(MAX),
                        Url NVARCHAR(MAX),
                        CONSTRAINT FK_News_SportClub FOREIGN KEY (SportClubId) REFERENCES SportClub(Id)
                    );

                    CREATE INDEX IX_News_SportClubId ON News(SportClubId);
                END
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /* migrationBuilder.DropTable(
                name: "News"); */
            migrationBuilder.Sql(
                @"
                IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'News')
                BEGIN
                    DROP INDEX IF EXISTS IX_News_SportClubId ON News;
                    DROP TABLE News;
                END
                ");
        }
    }
}
