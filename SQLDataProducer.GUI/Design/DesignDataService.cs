using System;
using SQLDataProducer.GUI.Model;
using SQLDataProducer.GUI.Model.DesignModels;

namespace SQLDataProducer.GUI.Design
{
    public class DesignDataService : IDataService
    {
        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem("Welcome to MVVM Light [design]");
            callback(item, null);
        }

        public void GetNewProjectModel(Action<ProjectModel, Exception> callback)
        {
            callback(ProjectModelDummy.Instance, null);
        }
    }
}