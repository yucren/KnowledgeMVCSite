namespace KnowledgeMVCSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Discusses", new[] { "Context" });
            AlterColumn("dbo.Discusses", "Context", c => c.String(nullable: false, maxLength: 900));
            CreateIndex("dbo.Discusses", "Context");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Discusses", new[] { "Context" });
            AlterColumn("dbo.Discusses", "Context", c => c.String(nullable: false, maxLength: 1000));
            CreateIndex("dbo.Discusses", "Context");
        }
    }
}
