namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditDrugTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("Drugs", "DrugGenericName", c => c.String(unicode: false));
            AddColumn("Drugs", "NormalAmountToOrder", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("Drugs", "NormalAmountToOrder");
            DropColumn("Drugs", "DrugGenericName");
        }
    }
}
