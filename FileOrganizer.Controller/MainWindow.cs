using BITS.UI.WPF.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BITS.UI.WPF.Core;
using BITS.UI.WPF.Core.Models;
using FileOrganizer.Data;
using FileOrganizer.Helper;
using Runtime.View;
using FileOrganizer.Controller.Helper;
using FileOrganizer.Enums;
using FileOrganizer.Models;

namespace FileOrganizer.Controller
{
    public class MainWindow : WindowController<MainWindow, View.MainWindow, Model.MainWindow>
    {
        private List<FileSystemWatcher> _fileSystemWatchers = new List<FileSystemWatcher>();

        public MainWindow(BITS.UI.WPF.Core.Controllers.Controller parent) : base(parent)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            this.View = new View.MainWindow();
            this.Model = new Model.MainWindow();

            ContextManager.Init();

            DialogManager.DialogAccessor = new DialogController(this, this.View.DialogContent).Init();

            InitializeFileSystemWatchers();

            this.View.Show();

            this.View.ExtensionsTab.Click += (s, e) =>
            {
                this.Model.IsBusy = true;
                IEnumerable<Models.Extension> extensions = null;

                Task.Run(() =>
                {
                    var context = ContextManager.Context();

                    extensions = context.Extensions.ToList();

                }).ContinueWith(async antecedent =>
                {
                    this.Model.IsBusy = false;

                    await this.SwitchByAsync(region => region.MainContent, 
                        new Extensions(this, extensions).Init(), (region, view) =>
                        {
                            region.Children.Clear();
                            region.Children.Add(view);
                        },
                        (region, view) => region.Children.Clear());

                }, TaskScheduler.FromCurrentSynchronizationContext());
            };

            this.View.ExtensiongroupsTab.Click += (s, e) =>
            {
                this.Model.IsBusy = true;
                
                //TODO: Fill Extension and Extension Groups
                //await this.SwitchByAsync(region => region.MainContent, new Controller.ExtensionGroups(this, new List<Models.ExtensionGroup>() ,new List<Models.Extension>()).Init());

                List<Models.Extension> extensions = null;
                List<Models.ExtensionGroup> extensionGroups = null;

                Task.Run(() =>
                {
                    var context = ContextManager.Context();

                    extensions = context.Extensions
                        .Include(x => x.ExtensionGroup).ToList();

                    extensionGroups = context.ExtensionGroups.ToList();

                }).ContinueWith(async antecedent =>
                {
                    this.Model.IsBusy = false;

                    await this.SwitchByAsync(region => region.MainContent, 
                        new Controller.ExtensionGroups(this, extensionGroups, extensions)
                            .Init(),(region, view) =>
                    {
                        region.Children.Clear();
                        region.Children.Add(view);
                    }, 
                    (region, view) => region.Children.Clear());

                }, TaskScheduler.FromCurrentSynchronizationContext());
            };

            this.View.FolderobservationTab.Click += (s, e) =>
            {
                this.Model.IsBusy = true;

                IEnumerable<FileSystemWatcherDto> filewatchers = null;

                Task.Run(() =>
                {
                    var context = ContextManager.Context();
                    filewatchers = context.FileSystemWatchers.ToList();

                }).ContinueWith(async antecedent =>
                {
                    this.Model.IsBusy = false;
                    await this.SwitchByAsync(region => region.MainContent, new Controller.Filewatcher(this, filewatchers).Init());
                }, TaskScheduler.FromCurrentSynchronizationContext());

            };

            this.View.DropDownTab.Click += async (s, e) =>
            {
                this.Model.IsBusy = true;
                await this.SwitchByAsync(region => region.MainContent, new DropExtension(this).Init(), (region, view) =>
                    {
                        region.Children.Clear();
                        region.Children.Add(view);
                    },
                    (region, view) => region.Children.Clear());

                this.Model.IsBusy = false;
            };

            this.View.MappingTab.Click += (s, e) =>
            {
                var controller = new ExtensionMapping(this).Init();

                RunAsync(async () =>
                {
                    await controller.LoadData();
                }, async () => {

                   await this.SwitchByAsync(region => region.MainContent, controller);
                });
            };

            this.View.LogsTab.Click += async (s, e) =>
            {
                this.Model.IsBusy = true;
                await this.SwitchByAsync(region => region.MainContent, new Logs(this).Init(), (region, view) =>
                    {
                        region.Children.Clear();
                        region.Children.Add(view);
                    },
                    (region, view) => region.Children.Clear());

                this.Model.IsBusy = false;
            };
        }

        private void InitializeFileSystemWatchers()
        {

            var context = ContextManager.Context();

            var watchers = context.FileSystemWatchers.Where(x=>x.Active).ToList();

            foreach (var fileSystemWatcherDto in watchers)
            {
                if (Directory.Exists(fileSystemWatcherDto.Path))
                {
                    var watcher = new FileSystemWatcher(fileSystemWatcherDto.Path);

                    watcher.IncludeSubdirectories = false;

                    watcher.Created += Watcher_Created;
                    watcher.Renamed += Watcher_Renamed;

                    //TODO:
                    //watcher.Error += Watcher_Error

                    watcher.EnableRaisingEvents = true;

                    _fileSystemWatchers.Add(watcher);
                }
            }
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Watcher_Copy(Path.GetDirectoryName(e.FullPath));
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Watcher_Copy(Path.GetDirectoryName(e.FullPath));
        }

        private void Watcher_Copy(string directory)
        {
            Dictionary<string, List<string>> sourceTargetDictionary = new Dictionary<string, List<string>>();

            var copier = new FileCopier();

            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                var destinationPath = ExtensionMappingManager.LookUpExtensionMapping(Path.GetExtension(file));

                if(String.IsNullOrWhiteSpace(destinationPath))
                    continue;

                if (sourceTargetDictionary.ContainsKey(destinationPath))
                {
                    sourceTargetDictionary[destinationPath].Add(file);
                }
                else
                {
                    var list = new List<string>(new string[] {file});
                    sourceTargetDictionary.Add(destinationPath, list);
                }
            }

            var context = ContextManager.Context();

            foreach (var sourceTargetPair in sourceTargetDictionary)
            {
                if (copier.Copy(sourceTargetPair.Value.ToArray(), sourceTargetPair.Key))
                {
                    var date = DateTime.Now;

                    foreach (var fileFullPath in sourceTargetPair.Value)
                    {
                        context.LogEntries.Add(new LogEntry
                        {
                            File = Path.GetFileName(fileFullPath),
                            From = Path.GetDirectoryName(fileFullPath),
                            To = sourceTargetPair.Key,
                            DateTime = date,
                            Action = FileAction.Moved
                        });
                    }

                    context.SaveChanges();
                }
            }

            
        }

        private void RunAsync(Action asyncFn, Action continueFn)
        {
            this.Model.IsBusy = true;
            Task.Run(() =>
            {
                asyncFn();
            }).ContinueWith(antecedent =>
            {
                this.Model.IsBusy = false;
                continueFn();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public DialogController DialogController { get; set; }

        public List<FileSystemWatcher> FileSystemWatchers
        {
            get { return _fileSystemWatchers; }
            set { _fileSystemWatchers = value; }
        }


    }

    public interface IBusy
    {
        bool IsBusy { get; set; }
    }
}
