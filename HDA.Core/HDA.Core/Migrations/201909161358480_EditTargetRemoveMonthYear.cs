namespace HDA.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditTargetRemoveMonthYear : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Targets", "EffectiveDate", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("Targets", "Year");
            DropColumn("Targets", "Month");
        }
        
        public override void Down()
        {
            AddColumn("Targets", "Month", c => c.Int());
            AddColumn("Targets", "Year", c => c.Int());
            AlterColumn("Targets", "EffectiveDate", c => c.DateTime(precision: 0));
        }
    }
}
