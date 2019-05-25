namespace KnowledgeMVCSite.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<KnowledgeMVCSite.Models.KnowledgeModel>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "KnowledgeMVCSite.Models.KnowledgeModel";
        }

        protected override void Seed(KnowledgeMVCSite.Models.KnowledgeModel context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
