using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileOrganizer.Data;
using FileOrganizer.Enums;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using Runtime.Extensions;
using Runtime.Messaging;
using Runtime.MVC;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;

namespace FileOrganizer.ViewModels
{
    public class AddExtensionAssignementViewModel : ModelBase
    {
        private ObservableCollection<ExtensionMappingItem> _mappingItems;
        private ExtensionMappingItem _selectedMappingItem;
        private ObservableCollection<string> _extensions;
        private IDataPersistenceService _persistenceService;

        public override void Initialize(params object[] parameter)
        {
            base.Initialize(parameter);

            //Gemappte Pfade laden
            MappingItems = PersistenceService.Load(typeof(ObservableCollection<ExtensionMappingItem>), PathHelper.ExtensionAssignementsSavePath) 
                as ObservableCollection<ExtensionMappingItem>;
        }

        public ObservableCollection<ExtensionMappingItem> MappingItems
        {
            get
            {
                return _mappingItems ?? (_mappingItems = new ObservableCollection<ExtensionMappingItem>()); 
            }

            set
            {
                _mappingItems = value;
                OnPropertyChanged(nameof(MappingItems));
            }
        }

        public IDataPersistenceService PersistenceService
        {
            get
            {
                return _persistenceService ??
                       (_persistenceService = ServiceLocator.Default.GetService<IDataPersistenceService>());
            }

            set { _persistenceService = value; }
        }

        public ExtensionMappingItem SelectedMappingItem
        {
            get
            {
                return _selectedMappingItem;
            }

            set
            {
                _selectedMappingItem = value;
                OnPropertyChanged(nameof(SelectedMappingItem));
            }
        }

        public ObservableCollection<string> Extensions
        {
            get
            {
                var dataModel = ContextManager.Context();
                return dataModel.Extensions.Select(x=>x.ExtensionName)
                    .ToList()
                    .OrderBy(x=>x)
                    .ToObservableCollection();
            }

            set
            {
                _extensions = value;
                OnPropertyChanged(nameof(Extensions));
            }
        }
    }
}
