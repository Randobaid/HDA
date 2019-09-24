namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiagnosisCodes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DiagnosisCodes",
                c => new
                    {
                        DiagnosisCodeID = c.Int(nullable: false, identity: true),
                        Code = c.String(unicode: false),
                        DiagnosisCodingSystemID = c.Int(nullable: false),
                        DiagnosisNameEn = c.String(unicode: false),
                        DiagnosisNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DiagnosisCodeID)
                .ForeignKey("DiagnosisCodingSystems", t => t.DiagnosisCodingSystemID, cascadeDelete: true)
                .Index(t => t.DiagnosisCodingSystemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DiagnosisCodes", "DiagnosisCodingSystemID", "DiagnosisCodingSystems");
            DropIndex("DiagnosisCodes", new[] { "DiagnosisCodingSystemID" });
            DropTable("DiagnosisCodes");
        }
    }
}
