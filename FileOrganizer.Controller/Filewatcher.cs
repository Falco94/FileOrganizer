using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Data;
using FileOrganizer.Dto;

namespace FileOrganizer.Controller
{
    public class Filewatcher : ContentController<Controller.Filewatcher, View.Filewatcher, Model.Filewatcher>
    {
        private IEnumerable<Model.FileSystemWatcherDto> _fileSystemWatchers;

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            this.View = new View.Filewatcher();
            this.Model = new Model.Filewatcher().Init(_fileSystemWatchers);
        }

        public Filewatcher(BITS.UI.WPF.Core.Controllers.Controller parent, IEnumerable<Model.FileSystemWatcherDto> fileSystemWatcher) : base(parent)
        {
            _fileSystemWatchers = fileSystemWatcher;
        }
    }
}
