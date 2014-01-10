using GalaSoft.MvvmLight;
using SQLDataProducer.GUI.Model;

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
            });
        }
    }
}