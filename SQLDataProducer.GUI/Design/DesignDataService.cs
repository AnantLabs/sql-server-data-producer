using System;
using SQLDataProducer.GUI.Model;
using SQLDataProducer.GUI.Model.DesignModels;

namespace SQLDataProducer.GUI.Design
{
    public class DesignDataService : IDataService
    {
        public void GetNewProjectModel(Action<ProjectModel, Exception> callback)
        {
            callback(ProjectModelDummyFactory.GetNewInstance(), null);
        }
    }
}