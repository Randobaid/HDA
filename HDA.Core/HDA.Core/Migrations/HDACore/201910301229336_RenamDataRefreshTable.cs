namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamDataRefreshTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataRefreshProcedureStatus",
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
            
            DropTable("dbo.DataRefreshProcedures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DataRefreshProcedures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcedureName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.DataRefreshProcedureStatus");
        }
    }
}
