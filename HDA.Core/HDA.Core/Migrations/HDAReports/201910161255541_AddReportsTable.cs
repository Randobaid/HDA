namespace HDA.Core.Migrations.HDAReports
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reports",
                c => new
                    {
                        ReportID = c.Int(nullable: false, identity: true),
                        ReportCode = c.String(unicode: false),
                        ReportNameEn = c.String(unicode: false),
                        ReportNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ReportID);
            
        }
        
        public override void Down()
        {
            DropTable("Reports");
        }
    }
}
