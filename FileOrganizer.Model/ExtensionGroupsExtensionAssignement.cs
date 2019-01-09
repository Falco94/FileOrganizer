using FileOrganizer.Model.Annotations;
using FileOrganizer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Runtime.Utility;

namespace FileOrganizer.Model
{
    public class ExtensionGroupsExtensionAssignement : INotifyPropertyChanged
    {
        private AsyncObservableCollection<GroupExtensionItem> _extensions;
        private ExtensionGroup _group;
        private bool _isBusy;

        public ExtensionGroupsExtensionAssignement(ExtensionGroup group, IEnumerable<Extension> extensions)
        {
            if (extensions == null)
                return;

            if (group == null)
                return;

            Group = group;

            Extensions = new AsyncObservableCollection<GroupExtensionItem>(extensions.Select(x=> new GroupExtensionItem
            {
                ExtensionName = x.ExtensionName,
                ExtensionId = x.ExtensionId,
                // Ist die Extension bereits angewählt?
               IsSelected = group.Extensions.Select(y=>y.ExtensionId).Contains(x.ExtensionId)
            }));

            foreach (var groupExtensionItem in Extensions)
            {
                if (groupExtensionItem.IsSelected)
                {
                    groupExtensionItem.ExtensionGroup = group;
                    groupExtensionItem.CurrentExtensionGroupId = group.ExtensionGroupId;
                }
            }
        }

        public AsyncObservableCollection<GroupExtensionItem> Extensions
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

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value; 
                OnPropertyChanged();
            }
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
