using System;

namespace SQLDataProducer.GUI.Model
{
    public class DataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }


        public void GetNewProjectModel(Action<ProjectModel, Exception> callback)
        {
            callback(SQLDataProducer.GUI.Model.DesignModels.ProjectModelDummy.Instance, null);
        }
    }
}