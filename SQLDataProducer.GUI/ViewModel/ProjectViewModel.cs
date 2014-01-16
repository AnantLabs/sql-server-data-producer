using GalaSoft.MvvmLight;
using SQLDataProducer.Entities.ExecutionEntities;
using SQLDataProducer.GUI.Model;
using System.Linq;
using SQLDataProducer.Entities.DatabaseEntities;
using GalaSoft.MvvmLight.Command;

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

                if (value == null)
                    return;

                if (value.Tables.Count() > 0)
                {
                    SelectedTable = value.Tables.First();
                    SelectedColumn = SelectedTable.Columns.FirstOrDefault();
                }
                else
                {
                    SelectedTable = null;
                    SelectedColumn = null;
                }
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
        /// The <see cref="SelectedColumn" /> property's name.
        /// </summary>
        public const string SelectedColumnPropertyName = "SelectedColumn";

        private ColumnEntity _selectedColumn = null;

        /// <summary>
        /// Sets and gets the SelectedColumn property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ColumnEntity SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }

            set
            {
                if (_selectedColumn == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedColumnPropertyName);
                _selectedColumn = value;
                RaisePropertyChanged(SelectedColumnPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedAvailableTable" /> property's name.
        /// </summary>
        public const string SelectedAvailableTablePropertyName = "SelectedAvailableTable";

        private TableEntity _selectedAvailableTable = null;

        /// <summary>
        /// Sets and gets the SelectedAvailableTable property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public TableEntity SelectedAvailableTable
        {
            get
            {
                return _selectedAvailableTable;
            }

            set
            {
                if (_selectedAvailableTable == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedAvailableTablePropertyName);
                _selectedAvailableTable = value;
                RaisePropertyChanged(SelectedAvailableTablePropertyName);
            }
        }

        private RelayCommand<TableEntity> _addTableCommand;
        /// <summary>
        /// Gets the AddTableCommand.
        /// </summary>
        public RelayCommand<TableEntity> AddTableToSelectedExecutionNodeCommand
        {
            get
            {
                return _addTableCommand
                    ?? (_addTableCommand = new RelayCommand<TableEntity>(_commandHandler.AddTableToNode));
            }
        }

        private RelayCommand<TableEntity> _removeTableCommand;

        /// <summary>
        /// Gets the RemoveTableCommand.
        /// </summary>
        public RelayCommand<TableEntity> RemoveTableFromSelectedNodeCommand
        {
            get
            {
                return _removeTableCommand
                    ?? (_removeTableCommand = new RelayCommand<TableEntity>(_commandHandler.RemoveTableFromSelectedNode));
            }
        }

        private RelayCommand<TableEntity> _moveSelectedTableUpCommand;

        /// <summary>
        /// Gets the MoveSelectedTableUpCommand.
        /// </summary>
        public RelayCommand<TableEntity> MoveTableUpCommand
        {
            get
            {
                return _moveSelectedTableUpCommand
                    ?? (_moveSelectedTableUpCommand = new RelayCommand<TableEntity>(_commandHandler.MoveTableUp));
            }
        }

        private RelayCommand<TableEntity> _moveSelectedTableDownCommand;

        /// <summary>
        /// Gets the MoveTableDownCommand.
        /// </summary>
        public RelayCommand<TableEntity> MoveTableDownCommand
        {
            get
            {
                return _moveSelectedTableDownCommand
                    ?? (_moveSelectedTableDownCommand = new RelayCommand<TableEntity>(_commandHandler.MoveTableDown));
            }
        }

        private RelayCommand<ExecutionNode> _addChildNodeCommand;

        /// <summary>
        /// Gets the AddChildNodeCommand.
        /// </summary>
        public RelayCommand<ExecutionNode> AddChildNodeCommand
        {
            get
            {
                return _addChildNodeCommand
                    ?? (_addChildNodeCommand = new RelayCommand<ExecutionNode>(_commandHandler.AddChildNode));
            }
        }

        private RelayCommand<ExecutionNode> _addParentNodeCommand;

        /// <summary>
        /// Gets the AddParentNodeCommand.
        /// </summary>
        public RelayCommand<ExecutionNode> AddParentNodeCommand
        {
            get
            {
                return _addParentNodeCommand
                    ?? (_addParentNodeCommand = new RelayCommand<ExecutionNode>(_commandHandler.AddParentNode));
            }
        }

        private RelayCommand<ExecutionNode> _mergeNodeWithParentNode;

        /// <summary>
        /// Gets the MergeNodeWithParentNodeCommand.
        /// </summary>
        public RelayCommand<ExecutionNode> MergeNodeWithParentNodeCommand
        {
            get
            {
                return _mergeNodeWithParentNode
                    ?? (_mergeNodeWithParentNode = new RelayCommand<ExecutionNode>(_commandHandler.MergeNodeWithParentNode));
            }
        }

        private RelayCommand<ExecutionNode> _removeNodeCommand;

        /// <summary>
        /// Gets the RemoveNodeCommand.
        /// </summary>
        public RelayCommand<ExecutionNode> RemoveNodeCommand
        {
            get
            {
                return _removeNodeCommand
                    ?? (_removeNodeCommand = new RelayCommand<ExecutionNode>(_commandHandler.RemoveNode));
            }
        }

        /// <summary>
        /// Handler of all commands in this viewmodel
        /// </summary>
        private ProjectViewModelCommandHandler _commandHandler;

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
                if (Model.RootNode.Children.Count() > 0)
                {
                    SelectedExecutionNode = Model.RootNode.Children.FirstOrDefault();
                   
                }
                
            });

            _commandHandler = new ProjectViewModelCommandHandler(this);
        }
    }
}