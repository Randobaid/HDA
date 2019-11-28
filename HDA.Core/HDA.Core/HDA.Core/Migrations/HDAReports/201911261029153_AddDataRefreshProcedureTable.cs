namespace HDA.Core.Migrations.HDAReports
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataRefreshProcedureTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.DataRefreshProcedureStatus", "ProcedureDetail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DataRefreshProcedureStatus", "ProcedureDetail", c => c.String(unicode: false));
        }
    }
}
