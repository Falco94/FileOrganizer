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
using Runtime.Extensions;

namespace FileOrganizer.ViewModels
{
    public class AddNewExtensionsViewModel : ModelBase
    {
        private ObservableCollection<string> _extensions = new ObservableCollection<string>();
        private ObservableCollection<ExtensionGroup> _extensionGroups = new ObservableCollection<ExtensionGroup>();
        private string _selectedExtension;

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

        public ObservableCollection<ExtensionGroup> ExtensionGroups
        {
            get
            {
                return _extensionGroups;
            }

            set
            {
                _extensionGroups = value;
            }
        }

        public override void Initialize(object[] args)
        {
            base.Initialize(args);

            ReloadExtensions();
            ReloadExtensionGroups();
        }

        public void ReloadExtensions()
        {
            using (var dataModel = new FileOrganizerDataModel())
            {
                Extensions = dataModel.Extensions.Select(x => x.ExtensionName)
                    .ToList()
                    .OrderBy(x => x)
                    .ToObservableCollection();
            }
        }

        public void ReloadExtensionGroups()
        {
            using (var dataModel = new FileOrganizerDataModel())
            {
                ExtensionGroups = dataModel.ExtensionGroups
                    .ToList()
                    .ToObservableCollection(); 
            }
        }
    }
}
