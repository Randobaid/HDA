namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refills : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "PrescriptionRefills",
                c => new
                    {
                        PrescriptionRefillID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        PrescriptionID = c.Int(nullable: false),
                        DrugID = c.Int(nullable: false),
                        RefillSequence = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        DaysSupply = c.Int(nullable: false),
                        FillDate = c.DateTime(nullable: false, precision: 0),
                        PharmacyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PrescriptionRefillID)
                .ForeignKey("Drugs", t => t.DrugID, cascadeDelete: true)
                .ForeignKey("Pharmacies", t => t.PharmacyID, cascadeDelete: true)
                .ForeignKey("Prescriptions", t => t.PrescriptionID, cascadeDelete: true)
                .Index(t => t.PrescriptionID)
                .Index(t => t.DrugID)
                .Index(t => t.PharmacyID);
            
            AddColumn("PrescriptionTotals", "TotalRefillQuantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("PrescriptionRefills", "PrescriptionID", "Prescriptions");
            DropForeignKey("PrescriptionRefills", "PharmacyID", "Pharmacies");
            DropForeignKey("PrescriptionRefills", "DrugID", "Drugs");
            DropIndex("PrescriptionRefills", new[] { "PharmacyID" });
            DropIndex("PrescriptionRefills", new[] { "DrugID" });
            DropIndex("PrescriptionRefills", new[] { "PrescriptionID" });
            DropColumn("PrescriptionTotals", "TotalRefillQuantity");
            DropTable("PrescriptionRefills");
        }
    }
}
