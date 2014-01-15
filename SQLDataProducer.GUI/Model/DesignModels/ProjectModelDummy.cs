using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLDataProducer.Entities.DatabaseEntities;
using SQLDataProducer.Entities.DatabaseEntities.Factories;

namespace SQLDataProducer.GUI.Model.DesignModels
{
    public sealed class ProjectModelDummyFactory
    {

        private static TableEntity CreateOrderTable()
        {
            var ordertable = new TableEntity("dbo", "Orders");
            ordertable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("OrderId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null));
            ordertable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("OrderDate", new ColumnDataTypeDefinition("datetime", false), false, 2, false, null, null));
            ordertable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("Quantity", new ColumnDataTypeDefinition("int", false), false, 3, false, null, null));
            ordertable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("TotalCost", new ColumnDataTypeDefinition("decimal(19,6)", false), false, 4, false, null, null));
            ordertable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), false, 5, true, null, new ForeignKeyEntity()));
            
            return ordertable;
        }

        private static TableEntity CreateCustomerTable()
        {
            var customerTable = new TableEntity("dbo", "Customer");
            var customerId = DatabaseEntityFactory.CreateColumnEntity("CustomerId", new ColumnDataTypeDefinition("int", false), true, 1, false, null, null);
            customerTable.AddColumn(customerId);
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("CustomerType", new ColumnDataTypeDefinition("int", false), false, 2, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("Name", new ColumnDataTypeDefinition("varchar", false), false, 3, false, null, null));
            customerTable.AddColumn(DatabaseEntityFactory.CreateColumnEntity("IsActive", new ColumnDataTypeDefinition("bit", false), false, 4, false, null, null));
            return customerTable;
        }

        
        public static ProjectModel GetNewInstance()
        {
            var model = new ProjectModel();

            var customerTable = CreateCustomerTable();
            model.Tables.Add(customerTable);

            var ordertable = CreateOrderTable();
            model.Tables.Add(ordertable);


            var customerNode = model.RootNode.AddChild(10, "Customer");
            customerNode.AddTable(CreateCustomerTable());
            customerNode.AddTable(CreateCustomerTable());

            var orderNode = customerNode.AddChild(3, "Orders").AddTable(CreateOrderTable()).AddTable(CreateOrderTable()).AddTable(CreateOrderTable());
            orderNode.AddChild(44, "Someting").AddTable(CreateCustomerTable());
            customerNode.AddChild(3, "Another order").AddTable(CreateOrderTable());
            
            return model;
        }
    }
}
