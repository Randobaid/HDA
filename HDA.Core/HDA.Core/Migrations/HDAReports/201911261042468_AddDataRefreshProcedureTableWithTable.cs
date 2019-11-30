namespace HDA.Core.Migrations.HDAReports
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataRefreshProcedureTableWithTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataRefreshProcedures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcedureName = c.String(unicode: false),
                        ProcedureDetail = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DataRefreshProcedures");
        }
    }
}
