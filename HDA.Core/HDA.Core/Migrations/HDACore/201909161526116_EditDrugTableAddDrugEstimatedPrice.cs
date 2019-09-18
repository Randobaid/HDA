namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDrugTableAddDrugEstimatedPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Drugs", "DrugEstimatedPrice", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Drugs", "DrugEstimatedPrice");
        }
    }
}
