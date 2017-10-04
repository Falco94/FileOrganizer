using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using Runtime.Messaging;
using Runtime.MVC;

namespace FileOrganizer.ViewModels
{
    public class AddExtensionGroupViewModel : ModelBase, IDataErrorInfo
    {
        private ObservableCollection<string> _extensions = new ObservableCollection<string>();
        private ObservableCollection<ExtensionSelectionItem> _selectedExtensions = new ObservableCollection<ExtensionSelectionItem>();
        private List<ExtensionGroup> _extensionGroups;
        private string _selectedExtension;
        private string _filter;
        private string _name;

        private bool _nameIsValid;

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

        public string Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
                OnPropertyChanged(nameof(SelectedExtensions));
            }
        }

        public ObservableCollection<ExtensionSelectionItem> SelectedExtensions
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(_filter))
                {
                    return new ObservableCollection<ExtensionSelectionItem>(_selectedExtensions.Where(x => x.Extension.Contains(_filter)));
                }
                return _selectedExtensions;
            }

            set
            {
                _selectedExtensions = value;
            }
        }

        public ObservableCollection<ExtensionSelectionItem> SelectedExtensionsSave
        {
            get
            {
                return _selectedExtensions;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Error
        {
            get { return null; }
        }

        public List<ExtensionGroup> ExtensionGroups
        {
            get
            {
                if (_extensionGroups == null)
                {
                    var dataModel = new FileOrganizerDataModel();
                    _extensionGroups = new List<ExtensionGroup>(dataModel.ExtensionGroups.ToList());
                }

                return _extensionGroups;
            }

            set
            {
                _extensionGroups = value;
            }
        }

        public bool NameIsValid
        {
            get
            {
                return GetValidationErrors(nameof(Name)) == null;
            }

            set
            {
                _nameIsValid = value;
            }
        }

        public string this[string propertyName]
        {
            get { return GetValidationErrors(propertyName); }
        }

        private string GetValidationErrors(string propertyName)
        {
            switch (propertyName)
            {
                case "Name":
                    return ValidateNameProperty();
                default:
                    return string.Empty;
            }
        }

        private readonly string[] ValidatedProperties =
        {
            nameof(Name)
        };

        private string ValidateNameProperty()
        {
            if (String.IsNullOrWhiteSpace(_name))
            {
                return "Bitte geben Sie einen Namen an";
            }

            if (ExtensionGroups?.FirstOrDefault(x => x.Name == _name) != null)
            {
                return "Der Name ist bereits vergeben";
            }

            return null;
        }

        public override void Initialize(object[] args)
        {
            base.Initialize(args);

            ReloadExtensions();
            ReloadExtensionSelectionItems();

            if (args != null)
            {
                var extensionGroup = args[0] as ExtensionGroup;
                if (extensionGroup != null)
                {
                    Name = extensionGroup.Name;
                    foreach (var extension in extensionGroup.Extensions)
                    {
                        var item = SelectedExtensions.FirstOrDefault(
                            /*x => String.Compare(x.Extension, extension, StringComparison.CurrentCultureIgnoreCase) == 0*/);

                        if (item != null)
                        {
                            item.IsSelected = true;
                        }
                    }
                }
            }
        }

        public void ReloadExtensions()
        {
            var dataModel = new FileOrganizerDataModel();
            Extensions = new ObservableCollection<string>(dataModel.Extensions.Select(x=>x.ExtensionName));
        }

        public void ReloadExtensionSelectionItems()
        {
            SelectedExtensions = new ObservableCollection<ExtensionSelectionItem>(Extensions.Select(x => new ExtensionSelectionItem
            {
                Extension = x,
                IsSelected = false
            }));
        }
    }
}
