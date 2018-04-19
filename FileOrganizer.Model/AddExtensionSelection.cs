using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;

namespace FileOrganizer.Model
{
    public class AddExtensionSelection : INotifyPropertyChanged
    {
        private ObservableCollection<ExtensionSelectionItem> _selectionItems = new ObservableCollection<ExtensionSelectionItem>();
        private bool _selectAll = true;

        public AddExtensionSelection(IEnumerable<string> extensions)
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
