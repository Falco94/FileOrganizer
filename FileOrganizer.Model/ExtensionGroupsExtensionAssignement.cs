using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FileOrganizer.Model
{
    public class ExtensionGroupsExtensionAssignement : INotifyPropertyChanged
    {
        private ObservableCollection<GroupExtensionItem> _extensions;
        private ExtensionGroup _group;

        public ExtensionGroupsExtensionAssignement(ExtensionGroup group, IEnumerable<Extension> extensions)
        {
            if (extensions == null)
                return;

            if (group == null)
                return;

            Group = group;

            Extensions = new ObservableCollection<GroupExtensionItem>(extensions.Select(x=> new GroupExtensionItem
            {
                ExtensionName = x.ExtensionName,
                Id = x.Id,
                // Ist die Extension bereits angewählt?
               IsSelected = group.Extensions.Select(y=>y.Id).Contains(x.Id)
            }));
        }

        public ObservableCollection<GroupExtensionItem> Extensions
        {
            get { return _extensions; }
            set
            {
                _extensions = value; 
                OnPropertyChanged();
            }
        }

        public ExtensionGroup Group
        {
            get { return _group; }
            set { _group = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Mapping für flache Hierarchie im XAML (IsSelected Property)
    /// </summary>
    public class GroupExtensionItem : Extension
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value; 
                OnPropertyChanged();
            }
        }
    }

}
