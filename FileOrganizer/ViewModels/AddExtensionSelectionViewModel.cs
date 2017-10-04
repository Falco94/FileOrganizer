using FileOrganizer.Helper;
using FileOrganizer.Models;
using FileOrganizer.Test;
using Runtime.MVC;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileOrganizer.ViewModels
{
    public class AddExtensionSelectionViewModel : ModelBase
    {
        private ObservableCollection<ExtensionSelectionItem> _selectionItems = new ObservableCollection<ExtensionSelectionItem>();
        private bool _selectAll = true;

        public AddExtensionSelectionViewModel(IEnumerable<string> extensions)
        {
            foreach (var extension in extensions)
            {
                SelectionItems.Add(new ExtensionSelectionItem()
                {
                    Extension = extension,
                    IsSelected = true
                });
            }
        }

        public ObservableCollection<ExtensionSelectionItem> SelectionItems
        {
            get
            {
                return _selectionItems;
            }

            set
            {
                _selectionItems = value;
                OnPropertyChanged(nameof(this.SelectionItems));
            }
        }

        public bool SelectAll
        {
            get
            {
                return _selectAll;
            }

            set
            {
                _selectAll = value;

                foreach (var selectionItem in _selectionItems)
                {
                    selectionItem.IsSelected = _selectAll;
                }

                OnPropertyChanged(nameof(this.SelectAll));
            }
        }
    }
}
