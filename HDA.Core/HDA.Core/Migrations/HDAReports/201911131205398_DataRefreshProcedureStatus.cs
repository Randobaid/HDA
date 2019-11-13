namespace HDA.Core.Migrations.HDAReports
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataRefreshProcedureStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataRefreshProcedureStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcedureName = c.String(unicode: false),
                        ProcedureDetail = c.String(unicode: false),
                        ProcedureStartime = c.DateTime(nullable: false, precision: 0),
                        ProcedureEndDate = c.DateTime(nullable: false, precision: 0),
                        ProcedureStatus = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("DataRefreshProcedureStatus");
        }
    }
}
