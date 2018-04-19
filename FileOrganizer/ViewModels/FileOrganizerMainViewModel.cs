using FileOrganizer.Enums;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.Test;
using Runtime.MVC;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FileOrganizer.Data;

namespace FileOrganizer.ViewModels
{
    public class FileOrganizerMainViewModel : ModelBase
    {
        private ObservableCollection<ExtensionMappingItem> _mappingItems = new ObservableCollection<ExtensionMappingItem>();
        private ObservableCollection<string> _extensions = new ObservableCollection<string>();
        private string _selectedExtension;

        public ObservableCollection<ExtensionMappingItem> MappingItems
        {
            get
            {
                return _mappingItems;
            }

            set
            {
                _mappingItems = value;
                OnPropertyChanged(nameof(this.MappingItems));
            }
        }

        public ObservableCollection<string> Extensions
        {
            get
            {
                return _extensions;
            }

            set
            {
                _extensions = value;
                OnPropertyChanged(nameof(this.Extensions));
            }
        }

        public string SelectedExtension
        {
            get
            {
                return _selectedExtension;
            }

            set
            {
                _selectedExtension = value;
                OnPropertyChanged(nameof(this.SelectedExtension));
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            var dataModel = ContextManager.Context();

            Extensions = new ObservableCollection<string>(dataModel.Extensions.Select(x=>x.ExtensionName).ToList());
            MappingItems = new ObservableCollection<ExtensionMappingItem>(dataModel.ExtensionMappingItems.ToList());

            //LoadSavedExtensions();

            //TestMethod();
        }

        public bool LoadSavedExtensions()
        {
            var loader = ServiceLocator.Default.GetService<IDataPersistenceService>();

            var extensions = loader.Load(typeof(List<string>), PathHelper.ExtensionsSavePath) as List<string>;

            if (extensions == null)
            {
                Extensions = new ObservableCollection<string>();
            }
            else
            {
                Extensions = new ObservableCollection<string>(extensions);
            }
            
            return true;
        }

        private void TestMethod()
        {
            
        }
    }
}
