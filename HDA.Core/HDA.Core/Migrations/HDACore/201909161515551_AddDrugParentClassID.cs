namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDrugParentClassID : DbMigration
    {
        public override void Up()
        {
            AddColumn("DrugClasses", "DrugParentClassID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("DrugClasses", "DrugParentClassID");
        }
    }
}
