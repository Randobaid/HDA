namespace HDA.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDrugIDToPrescriptionTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("PrescriptionTotals", "DrugId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("PrescriptionTotals", "DrugId");
        }
    }
}
