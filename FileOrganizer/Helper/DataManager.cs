using FileOrganizer.Enums;
using Runtime.Services.DefaultServices;
using Runtime.Services.Plumbing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Data;
using FileOrganizer.Dto;
using FileOrganizer.Models;

namespace FileOrganizer.Helper
{
    /// <summary>
    /// Managed die angelegten Extensions
    /// </summary>
    //public sealed class DataManager
    //{
    //    private static object _lock = new object();
    //    private static DataManager _instance;
    //    private static IDataPersistenceService _fileManager;

    //    private IEnumerable<string> _extensions;
    //    private IEnumerable<ExtensionGroup> _extensionGroups;
    //    private IEnumerable<ExtensionMappingItem> _extensionMappingItems;

    //    private DataManager()
    //    {

    //    }

    //    public static DataManager Default
    //    {
    //        get
    //        {
    //            lock(_lock)
    //            {
    //                if(_instance == null)
    //                {
    //                    _instance = new DataManager();
    //                    _fileManager = ServiceLocator.Default.GetService<IDataPersistenceService>();
    //                }
    //            }

    //            return _instance;
    //        }
    //    }

    //    public IEnumerable<string> Extensions
    //    {
    //        get
    //        {
    //            _extensions = new List<string>(_instance.LoadData<List<string>>(PathHelper.ExtensionsSavePath));
    //            return _extensions;
    //        }

    //        set
    //        {
    //            _extensions = value;
    //        }
    //    }

    //    public IEnumerable<ExtensionGroup> ExtensionGroups
    //    {
    //        get
    //        {
    //            _extensionGroups = new List<ExtensionGroup>(_instance.LoadData<List<ExtensionGroup>>(PathHelper.ExtensionGroupsSavePath));

    //            var model = new FileOrganizerModel();

    //            //var test = model.ExtensionGroups.ToList();

    //            //foreach (var extension in _extensionGroups)
    //            //{

    //            //    model.ExtensionGroups.Add(new ExtensionGroup
    //            //    {
    //            //        Name = extension.Name
    //            //    });
    //            //}

    //            //model.SaveChanges();

    //            return _extensionGroups;
    //        }

    //        set
    //        {
    //            _extensionGroups = value;
    //        }
    //    }

    //    public IEnumerable<ExtensionMappingItem> ExtensionMappingItems
    //    {
    //        get
    //        {
    //            _extensionMappingItems = new List<ExtensionMappingItem>(_instance.LoadData<List<ExtensionMappingItem>>(PathHelper.ExtensionAssignementsSavePath));
    //            return _extensionMappingItems;
    //        }

    //        set
    //        {
    //            _extensionMappingItems = value;
    //        }
    //    }

    //    public void ReloadExtensions()
    //    {
    //        Extensions = new List<string>(_instance.LoadData<List<string>>(PathHelper.ExtensionsSavePath));
    //    }

    //    public T LoadData<T>(string Path) where T : class, new()
    //    {
    //        var result = default(T);

    //        try
    //        {
    //            if (!File.Exists(Path))
    //            {
    //                result = new T();
    //            }
    //            else
    //            {
    //                result = _fileManager.Load(typeof(T), Path) as T;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //        }

    //        if (result == null)
    //        {
    //            result = new T();
    //        }

    //        return result;
    //    }

    //    public bool SaveExtensions(IEnumerable<string> extensions)
    //    {
    //        //Alle Extensions speichern
    //        //Geladene Extensions mit übergebenenen mergen
    //        List<string> allExtensions = new List<string>(Extensions);
    //        foreach (var extension in extensions)
    //        {
    //            if(!allExtensions.Select(x=>x.ToLower()).Contains(extension.ToLower()))
    //            {
    //                allExtensions.Add(extension);
    //            }
    //        }

    //        allExtensions.Sort();

    //        try
    //        {
    //            _fileManager.Save(allExtensions, PathHelper.ExtensionsSavePath);
    //        }
    //        catch(Exception ex)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public bool SaveExtensionMappingItems(IEnumerable<ExtensionMappingItemDto> extensionMappingItems)
    //    {
    //        try
    //        {
    //            _fileManager.Save(extensionMappingItems, PathHelper.ExtensionAssignementsSavePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public bool SaveExtensionGroups(IEnumerable<ExtensionGroup> extensionGroups)
    //    {
    //        try
    //        {
    //            _fileManager.Save(extensionGroups, PathHelper.ExtensionGroupsSavePath);
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //        return true;
    //    }
    //}
}
