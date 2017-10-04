using BITS.UI.WPF.Core.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
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

            this.View.Tile1.Click += (s, e) =>
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

            this.View.Tile2.Click += async (s, e) =>
            {
                this.Model.IsBusy = true;
                await this.SwitchByAsync(region => region.MainContent, new ExtensionGroups(this).Init());
                this.Model.IsBusy = false;
            };

            this.View.Tile3.Click += (s, e) =>
            {
                this.Model.IsBusy = true;

                IEnumerable<FileSystemWatcherDto> filewatchers = null;

                //Task.Run(() =>
                //{
                //    using (var dataModel = new FODataModel())
                //    {
                //        filewatchers = new FODataModel().FileSystemWatchers.ToList();
                //    }
                //}).ContinueWith(async antecedent =>
                //{
                //    this.Model.IsBusy = false;
                //    await this.SwitchByAsync(region => region.MainContent, new Filewatcher(this, filewatchers).Init());
                //}, TaskScheduler.FromCurrentSynchronizationContext());
                
            };

            this.View.TileExtensionDrop.Click += async (s, e) =>
            {
                this.Model.IsBusy = true;
                await this.SwitchByAsync(region => region.MainContent, new DropExtension(this).Init());
                this.Model.IsBusy = false;
            };

            this.View.TileExtensionMapping.Click += (s, e) =>
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
