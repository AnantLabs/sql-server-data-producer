using GalaSoft.MvvmLight;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.GUI.Model;
using System.Linq;
using SQLDataProducer.Entities.DatabaseEntities;

namespace SQLDataProducer.GUI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ProjectViewModel : ViewModelBase
    {

        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="Project" /> property's name.
        /// </summary>
        public const string ModelPropertyName = "Model";
        private ProjectModel _model;

        /// <summary>
        /// Sets and gets the MyProperty property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ProjectModel Model
        {
            get
            {
                return _model;
            }
            set
            {
                if (_model == value)
                {
                    return;
                }

                RaisePropertyChanging(ModelPropertyName);
                _model = value;
                RaisePropertyChanged(ModelPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedExecutionNode" /> property's name.
        /// </summary>
        public const string SelectedExecutionNodePropertyName = "SelectedExecutionNode";

        private ExecutionNode _selectedExecutionNode = null;

        /// <summary>
        /// Sets and gets the SelectedExecutionNode property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ExecutionNode SelectedExecutionNode
        {
            get
            {
                return _selectedExecutionNode;
            }

            set
            {
                if (_selectedExecutionNode == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedExecutionNodePropertyName);
                _selectedExecutionNode = value;
                RaisePropertyChanged(SelectedExecutionNodePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedTable" /> property's name.
        /// </summary>
        public const string SelectedTablePropertyName = "SelectedTable";

        private TableEntity _selectedTable = null;

        /// <summary>
        /// Sets and gets the SelectedTable property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public TableEntity SelectedTable
        {
            get
            {
                return _selectedTable;
            }

            set
            {
                if (_selectedTable == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedTablePropertyName);
                _selectedTable = value;
                RaisePropertyChanged(SelectedTablePropertyName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the ProjectViewModel class.
        /// </summary>
        public ProjectViewModel(IDataService dataservice)
        {
            _dataService = dataservice;
            _dataService.GetNewProjectModel((model, error) =>
            {
                if (error != null)
                {
                    // Report error here
                    return;
                }

                Model = model ;
                SelectedExecutionNode = Model.RootNode.Children.First();
            });
        }
    }
}