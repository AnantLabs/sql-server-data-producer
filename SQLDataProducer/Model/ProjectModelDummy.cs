using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;

namespace SQLDataProducer.Model
{
    public class ProjectModelDummy
    {
        static ProjectModelDummy()
        {
            model = new ProjectModel();
            var customerTable = new TableEntity("dbo", "Customer");
            var customerId = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            customerTable.AddColumn(customerId);
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerType", new ColumnDataTypeDefinition("int", false), false, 2, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("Name", new ColumnDataTypeDefinition("varchar", false), false, 3, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("IsActive", new ColumnDataTypeDefinition("bit", false), false, 4, false, null, null));

            model.Tables.Add(customerTable);
        }

        private static ProjectModel model;
        public static ProjectModel Instance
        {
            get
            {
                return model;
            }
        }
    }
}
