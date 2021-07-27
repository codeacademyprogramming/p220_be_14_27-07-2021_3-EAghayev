using Microsoft.EntityFrameworkCore.Migrations;

namespace Pustok.Migrations
{
    public partial class SettingsTableCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupportPhone = table.Column<string>(maxLength: 25, nullable: true),
                    ContactPhone = table.Column<string>(maxLength: 25, nullable: true),
                    Address = table.Column<string>(maxLength: 250, nullable: true),
                    HeaderLogo = table.Column<string>(maxLength: 100, nullable: true),
                    FooterLogo = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    PromotionTitle = table.Column<string>(maxLength: 150, nullable: true),
                    PromotionSubTitle = table.Column<string>(maxLength: 250, nullable: true),
                    PromotionBtnText = table.Column<string>(maxLength: 50, nullable: true),
                    PromotionBtnUrl = table.Column<string>(maxLength: 250, nullable: true),
                    PromotionBgImage = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
