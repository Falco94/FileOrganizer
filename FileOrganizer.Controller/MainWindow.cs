using BITS.UI.WPF.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Effects;
using FileOrganizer.IService;
using MahApps.Metro.Controls.Dialogs;
using PayPal.Api;
using Application = System.Windows.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace FileOrganizer.Controller
{
    public class MainWindow : WindowController<MainWindow, View.MainWindow, Model.MainWindow>, Core.UI.IDialogAccessor, IProvideExtensions
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

            var blur = new BlurEffect();
            this.View.DockPanel.Effect = blur;
            this.View.MainContent.Effect = blur;

            this.Model.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "IsBusy")
                {
                    if (this.Model.IsBusy)
                    {
                        blur.Radius = 4;
                    }
                    else
                    {
                        blur.Radius = 0;
                    }
                }
            };

            DialogHandler.DialogRoot = this.View;

            CreateNotificationArea();

            ContextManager.Init();

            //var dialog = new DialogController(this, this.View.DialogContent).Init();
            //TODO: neccessary?
            //DialogManager.DialogAccessor = dialog;
            //DialogController = dialog;

            this.View.Show();

            InitializeFileSystemWatchersAsync();

            this.View.ExtensionsTab.Click += (s, e) =>
            {
                ExtensionsTabClick();
            };

            this.View.ExtensiongroupsTab.Click += (s, e) =>
            {
                ExtensiongroupsTabClick();
            };

            this.View.FolderobservationTab.Click += (s, e) =>
            {
                FolderobservationTabClick();
            };

            this.View.DropDownTab.Click += (s, e) =>
            {
                DropDownTabClick();
            };

            this.View.MappingTab.Click += (s, e) =>
            {
                SafeExecutor.ExecuteFn(MappingTabClick, "MainWindow.MappingTab.Click");
            };

            this.View.LogsTab.Click += async (s, e) =>
            {
                await LogsTabClick();
            };

            this.View.Donate.Click += (s, e) =>
            {
                DonateTabClick();
            };
        }

        private void DonateTabClick()
        {
            this.Model.IsBusy = true;

            var controller = new Donate(this).Init();

            RunAsync(async () =>
            {
            }, async () =>
            {
                this.Model.IsBusy = false;
                await this.SwitchByAsync(region => region.MainContent, controller);
            });
        }

        private async Task LogsTabClick()
        {
            this.Model.IsBusy = true;
            await this.SwitchByAsync(region => region.MainContent, new Logs(this).Init(), (region, view) =>
            {
                region.Children.Clear();
                region.Children.Add(view);
            },
                (region, view) => region.Children.Clear());

            this.Model.IsBusy = false;
        }

        private void MappingTabClick()
        {
            this.Model.IsBusy = true;

            IEnumerable<ExtensionMappingItem> extensionMappings = null;
            IEnumerable<Extension> extensions = null;
            IEnumerable<ExtensionGroup> extensionGroups = null;

            Task.Run(() =>
            {
                var context = ContextManager.Context();

                extensionMappings = context.ExtensionMappingItems.ToList();
                extensions = GetExtensions();
                extensionGroups = context.ExtensionGroups.ToList();

            }).ContinueWith(async antecedent =>
            {
                this.Model.IsBusy = false;

                await this.SwitchByAsync(region => region.MainContent,
                    new ExtensionMapping(this, extensionMappings, extensions, extensionGroups).Init(), (region, view) =>
                    {
                        region.Children.Clear();
                        region.Children.Add(view);
                    },
                    (region, view) => region.Children.Clear());

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ExtensionsTabClick()
        {
            this.Model.IsBusy = true;
            IEnumerable<Models.Extension> extensions = null;

            Task.Run(() =>
            {
                var context = ContextManager.Context();
                extensions = GetExtensions();

            }).ContinueWith(async antecedent =>
            {
                this.Model.IsBusy = false;

                await this.SwitchByAsync(region => region.MainContent,
                    new Extensions(this, DialogCoordinator.Instance, extensions, this).Init(), (region, view) =>
                    {
                        region.Children.Clear();
                        region.Children.Add(view);
                    },
                    (region, view) => region.Children.Clear());

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task DropDownTabClick()
        {
            this.Model.IsBusy = true;

                await this.SwitchByAsync(region => region.MainContent, new DropExtension(this).Init(), (region, view) =>
                    {
                        region.Children.Clear();
                        region.Children.Add(view);
                    },
                    (region, view) => region.Children.Clear());

                this.Model.IsBusy = false;
        }

        private void FolderobservationTabClick()
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
        }

        private void ExtensiongroupsTabClick()
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
                        .Init(), (region, view) =>
                        {
                            region.Children.Clear();
                            region.Children.Add(view);
                        },
                (region, view) => region.Children.Clear());

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void CreateNotificationArea()
        {
            var contextMenu = new System.Windows.Forms.ContextMenu();

            var menuItem = new MenuItem("Exit");
            menuItem.Click += (s, e) =>
            {
                this.View.Show();
                this.View.WindowState = WindowState.Normal;
                this.View.Close(); 
            };

            contextMenu.MenuItems.Add(menuItem);

            menuItem = new MenuItem("Open");
            menuItem.Click += (s, e) =>
            {
                this.View.Show();
                this.View.WindowState = WindowState.Normal;
            };

            contextMenu.MenuItems.Add(menuItem);

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

            //Get version
            System.Reflection.Assembly executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fieVersionInfo = FileVersionInfo.GetVersionInfo(executingAssembly.Location);
            var version = fieVersionInfo.FileVersion;

            ni.Text = "FileOrganizer " + version;
            ni.ContextMenu = contextMenu;

            try
            {
                var uri = new Uri("/FileOrganizer.View;component/Resources/Main-64.png", UriKind.Relative);
                var resource = Application.GetResourceStream(uri)?.Stream;
                if (resource != null)
                {
                    var image = Bitmap.FromStream(resource);
                    ni.Icon = System.Drawing.Icon.FromHandle(((Bitmap)image).GetHicon());
                }

                ni.Visible = true;
            }
            catch (Exception ex)
            {
            }

            ni.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    this.View.Show();
                    this.View.WindowState = WindowState.Normal;
                };
        }

        private void InitializeFileSystemWatchersAsync()
        {
            this.Model.IsBusy = true;

            string wrongPathWatchers = String.Empty;

            Task.Run(() =>
            {
                wrongPathWatchers = InitializeFileSystemWatchers();
            }).ContinueWith(async antecedent =>
            {
                this.Model.IsBusy = false;

                if (!String.IsNullOrWhiteSpace(wrongPathWatchers))
                {
                    await DialogHandler.DialogRoot.ShowMessageAsync("Warning", wrongPathWatchers);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private string InitializeFileSystemWatchers()
        {

            var context = ContextManager.Context();

            var watchers = context.FileSystemWatchers.Where(x=>x.Active).ToList();

            string wrongPathWatchers = String.Empty;

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
                else
                {
                    wrongPathWatchers +=
                        $"Path \"{fileSystemWatcherDto.Path}\" of filewatcher does not exist. The watcher was deactivated.";
                    fileSystemWatcherDto.Active = false;
                    context.SaveChanges();
                }
            }

            return wrongPathWatchers;
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

        public IEnumerable<Extension> GetExtensions()
        {
            var context = ContextManager.Context();

            return context.Extensions.ToList();
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
