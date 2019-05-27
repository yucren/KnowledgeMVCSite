namespace KnowledgeMVCSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_category2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Knowledges", "category_Id", "dbo.Categories");
            DropIndex("dbo.Knowledges", new[] { "category_Id" });
            AlterColumn("dbo.Knowledges", "category_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Knowledges", "category_Id");
            AddForeignKey("dbo.Knowledges", "category_Id", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Knowledges", "category_Id", "dbo.Categories");
            DropIndex("dbo.Knowledges", new[] { "category_Id" });
            AlterColumn("dbo.Knowledges", "category_Id", c => c.Int());
            CreateIndex("dbo.Knowledges", "category_Id");
            AddForeignKey("dbo.Knowledges", "category_Id", "dbo.Categories", "Id");
        }
    }
}
