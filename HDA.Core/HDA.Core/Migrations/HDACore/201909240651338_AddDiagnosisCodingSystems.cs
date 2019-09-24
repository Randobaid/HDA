namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDiagnosisCodingSystems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DiagnosisCodingSystems",
                c => new
                    {
                        DiagnosisCodingSystemID = c.Int(nullable: false, identity: true),
                        CodingSystemName = c.String(unicode: false),
                        CodingSystemVersion = c.String(unicode: false),
                        CodingSystemEffectiveDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.DiagnosisCodingSystemID);
            
        }
        
        public override void Down()
        {
            DropTable("DiagnosisCodingSystems");
        }
    }
}
