namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RefreshProcedures : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataRefreshProcedures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcedureName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DataRefreshProcedures");
        }
    }
}
