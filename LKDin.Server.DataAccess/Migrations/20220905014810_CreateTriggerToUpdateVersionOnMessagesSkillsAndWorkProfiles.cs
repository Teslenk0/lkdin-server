using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LKDin.Server.DataAccess.Migrations
{
    public partial class CreateTriggerToUpdateVersionOnMessagesSkillsAndWorkProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE TRIGGER UpdateSkillsVersion AFTER UPDATE ON Skills BEGIN UPDATE Skills SET Version = Version + 1 WHERE rowid = NEW.rowid; END;");

            migrationBuilder.Sql("CREATE TRIGGER UpdateWorkProfilesVersion AFTER UPDATE ON WorkProfiles BEGIN UPDATE WorkProfiles SET Version = Version + 1 WHERE rowid = NEW.rowid; END;");

            migrationBuilder.Sql("CREATE TRIGGER UpdateChatMessagesVersion AFTER UPDATE ON ChatMessages BEGIN UPDATE ChatMessages SET Version = Version + 1 WHERE rowid = NEW.rowid; END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER UpdateSkillsVersion");

            migrationBuilder.Sql("DROP TRIGGER UpdateWorkProfilesVersion");

            migrationBuilder.Sql("DROP TRIGGER UpdateChatMessagesVersion");
        }
    }
}
