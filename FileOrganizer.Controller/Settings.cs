using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BITS.UI.WPF.Core.Controllers;
using FileOrganizer.Helper;
using FileOrganizer.Models;
using Microsoft.Win32;

namespace FileOrganizer.Controller
{
    public class Settings : ContentController<Controller.Settings, View.Settings, Model.Settings>
    {
        private List<Setting> _settings;

        private Setting _autostart;
        private Setting _subfolders;

        public Settings(BITS.UI.WPF.Core.Controllers.Controller parent) : base(parent)
        {
            _settings = ContextManager.Context().Settings.ToList();
        }

        protected override async Task OnSetupAsync()
        {
            await base.OnSetupAsync();

            _autostart = _settings.FirstOrDefault(x => x.Name == "Autostart");

            if (_autostart == null)
            {
                _autostart = new Setting
                {
                    Name = "Autostart",
                    Value = "0"
                };

                _settings.Add(_autostart);

                ContextManager.Context().Settings.Add(_autostart);
                ContextManager.Context().SaveChanges();
            }

            _subfolders = _settings.FirstOrDefault(x => x.Name == "Subfolders");
            
            if (_subfolders == null)
            {
                _subfolders = new Setting
                {
                    Name = "Subfolders",
                    Value = "0"
                };

                _settings.Add(_subfolders);

                ContextManager.Context().Settings.Add(_subfolders);
                ContextManager.Context().SaveChanges();
            }

            this.View = new View.Settings();
            this.Model = new Model.Settings(_settings);

            //TODO: Change to binding
            this.View.SubfoldersCheckbox.IsChecked = _subfolders.Value == "-1";

            this.View.AutostartCheckbox.Click += (sender, args) => AutostartChecked(sender, args);
            this.View.SubfoldersCheckbox.Click += SubfoldersCheckboxOnClick;
        }

        private void SubfoldersCheckboxOnClick(object o, RoutedEventArgs routedEventArgs)
        {
            if (this.View.SubfoldersCheckbox.IsChecked == null)
                return;

            _subfolders.Value = this.View.SubfoldersCheckbox.IsChecked == true ? "-1" : "0";
            ContextManager.Context().SaveChanges();
        }

        private void AutostartChecked(object sender, RoutedEventArgs args)
        {
            //RegisterInStartup(this.View.AutostartCheckbox.IsChecked);

            RegisterInStartup(Model.Autostart.Value == "-1" ? true : false);
        }

        private void RegisterInStartup(bool? isChecked)
        {
            if (isChecked == null)
                return;

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (!isChecked.Value)
            {
                if (registryKey != null)
                {
                    var path = System.Reflection.Assembly.GetEntryAssembly().Location;
                    registryKey.SetValue("FileOrganizer", path);

                    _autostart.Value = "-1";
                }
            }
            else
            {
                if (registryKey != null)
                {
                    registryKey.DeleteValue("FileOrganizer");
                    _autostart.Value = "0";
                }
            }

            ContextManager.Context().SaveChanges();
        }

        public class Busy : IBusy
        {
            public bool IsBusy { get; set; }
        }
    }
}