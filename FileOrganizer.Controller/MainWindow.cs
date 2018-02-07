using BITS.UI.WPF.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileOrganizer.Data;
using FileOrganizer.Dto;

namespace FileOrganizer.Controller
{
    public class MainWindow : WindowController<MainWindow, View.MainWindow, Model.MainWindow>
    {
        public MainWindow(BITS.UI.WPF.Core.Controllers.Controller parent) : base(parent)
        {
        }

        protected override void OnInit()
        {
            base.OnInit();

            this.View = new View.MainWindow();
            this.Model = new Model.MainWindow();
            
            this.View.Show();

            this.ChannelIn<IBusy>().Subscribe(busy =>
            {
                this.Model.IsBusy = busy.IsBusy;
            });

            this.View.ExtensionsTab.Click += (s, e) =>
            {
                this.Model.IsBusy = true;
                IEnumerable<Models.Extension> extensions = null;

                Task.Run(() =>
                {
                    using (var dataModel = new FODataModel())
                    {
                        extensions = dataModel.Extensions.ToList();
                    }
                }).ContinueWith(async antecedent =>
                {
                    this.Model.IsBusy = false;
                    await this.SwitchByAsync(region => region.MainContent, new Extensions(this, extensions).Init());
                }, TaskScheduler.FromCurrentSynchronizationContext());

                //Delegate d = (Action)(() => { });

                //Dispatcher.CurrentDispatcher.BeginInvoke((Action)(async () =>
                //{
                //    await this.SwitchByAsync(region => region.MainContent, new Extensions(this).Init());
                //}));

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
                    using (var dataModel = new FODataModel())
                    {
                        extensions = dataModel.Extensions.ToList();
                        extensionGroups = dataModel.ExtensionGroups.ToList();

                        // for test:

                        var testItem = new Models.ExtensionGroup
                        {
                            Name = "MeinTest1"
                        };

                        testItem.Extensions.Add(extensions[0]);
                        testItem.Extensions.Add(extensions[1]);
                        testItem.Extensions.Add(extensions[2]);

                        extensionGroups.Add(testItem);


                        var testItem2 = new Models.ExtensionGroup
                        {
                            Name = "MeinTest2"
                        };

                        testItem2.Extensions.Add(extensions[3]);
                        testItem2.Extensions.Add(extensions[4]);
                        testItem2.Extensions.Add(extensions[5]);
                        
                        extensionGroups.Add(testItem2);
                    }

                }).ContinueWith(async antecedent =>
                {
                    this.Model.IsBusy = false;
                    await this.SwitchByAsync(region => region.MainContent, new Controller.ExtensionGroups(this, extensionGroups, extensions).Init());
                }, TaskScheduler.FromCurrentSynchronizationContext());
                
                this.Model.IsBusy = false;
            };

            this.View.FolderobservationTab.Click += (s, e) =>
            {
                this.Model.IsBusy = true;

                //IEnumerable<FileSystemWatcherDto> filewatchers = null;

                //Task.Run(() =>
                //{
                //    using (var dataModel = new FODataModel())
                //    {
                //        filewatchers = new FODataModel().FileSystemWatchers.ToList();
                //    }
                //}).ContinueWith(async antecedent =>
                //{
                //    this.Model.IsBusy = false;
                //    await this.SwitchByAsync(region => region.MainContent, new Controller.Filewatcher(this, filewatchers).Init());
                //}, TaskScheduler.FromCurrentSynchronizationContext());

            };

            this.View.DropDownTab.Click += async (s, e) =>
            {
                this.Model.IsBusy = true;
                await this.SwitchByAsync(region => region.MainContent, new DropExtension(this).Init());
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
    }

    public interface IBusy
    {
        bool IsBusy { get; set; }
    }


}
