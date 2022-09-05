using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LKDin.Server.DataAccess.Migrations
{
    public partial class CreateTriggerToUpdateVersionOnUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE TRIGGER UpdateUserVersion AFTER UPDATE ON Users BEGIN UPDATE Users SET Version = Version + 1 WHERE rowid = NEW.rowid; END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER UpdateUserVersion");
        }
    }
}
