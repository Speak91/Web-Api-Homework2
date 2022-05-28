using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Migrations
{
    [Migration(0)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {

            Create.Table("agents")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Uri").AsString()
                .WithColumn("IsEnabled").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("agents");
        }
    }
}
