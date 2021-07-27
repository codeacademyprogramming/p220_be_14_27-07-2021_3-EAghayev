using Microsoft.EntityFrameworkCore.Migrations;

namespace Pustok.Migrations
{
    public partial class SlidersTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Subtitle = table.Column<string>(maxLength: 200, nullable: true),
                    Image = table.Column<string>(maxLength: 100, nullable: false),
                    RedirectUrl = table.Column<string>(maxLength: 250, nullable: true),
                    Order = table.Column<int>(nullable: false),
                    BtnPrice = table.Column<string>(maxLength: 10, nullable: true),
                    BtnText = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sliders");
        }
    }
}
