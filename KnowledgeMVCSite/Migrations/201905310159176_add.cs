namespace KnowledgeMVCSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Praises",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        KnowledgeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Knowledges", t => t.KnowledgeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.KnowledgeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Praises", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Praises", "KnowledgeId", "dbo.Knowledges");
            DropIndex("dbo.Praises", new[] { "KnowledgeId" });
            DropIndex("dbo.Praises", new[] { "UserId" });
            DropTable("dbo.Praises");
        }
    }
}
