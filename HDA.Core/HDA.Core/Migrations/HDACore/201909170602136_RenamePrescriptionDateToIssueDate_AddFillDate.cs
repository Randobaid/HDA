namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamePrescriptionDateToIssueDate_AddFillDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("Prescriptions", "IssueDate", c => c.DateTime(nullable: false, precision: 0));
            AddColumn("Prescriptions", "FillDate", c => c.DateTime(precision: 0));
            DropColumn("Prescriptions", "PrescriptionDate");
        }
        
        public override void Down()
        {
            AddColumn("Prescriptions", "PrescriptionDate", c => c.DateTime(nullable: false, precision: 0));
            DropColumn("Prescriptions", "FillDate");
            DropColumn("Prescriptions", "IssueDate");
        }
    }
}
