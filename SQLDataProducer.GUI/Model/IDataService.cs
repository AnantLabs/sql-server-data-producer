using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.GUI.Model
{
    public interface IDataService
    {
        void GetData(Action<DataItem, Exception> callback);

        void GetNewProjectModel(Action<ProjectModel, Exception> callback);
    }
}
