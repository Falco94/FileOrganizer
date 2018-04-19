using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileOrganizer.Data;
using FileOrganizer.Enums;
using FileOrganizer.Models;

namespace FileOrganizer.Helper
{
    public class FileCopier
    {
        public void Copy(String destPath, List<string> extensions)
        {
            var dataModel = ContextManager.Context();

            var mappingItems =
                dataModel.ExtensionMappingItems.ToList()
                .Where(x=> extensions
                .Select(y=>y.ToLower())
                .Contains(x.Extension.ExtensionName.ToLower()));

            var extensionMappingItems = mappingItems as ExtensionMappingItem[] ?? mappingItems.ToArray();
            if (extensionMappingItems.Any())
            {
                foreach (var mappingItem in extensionMappingItems)
                {
                    //Alle Dateien im Verzeichnis mit der entsprechenden Extension
                    var files = Directory.GetFiles(destPath)
                        .Where(x => System.IO.Path.GetExtension(x) == mappingItem.Extension.ExtensionName).ToList();

                    if (files.Count > 0)
                    {
                        var copier = new FileCopier();
                        //copier.Copy();
                        //foreach (var sourceFile in files)
                        //{
                        //    var destinationFile = Path.Combine(TestPaths.Ausgabeordner, Path.GetFileName(sourceFile));



                        //    FileSystem.CopyFile(sourceFile, destinationFile, UIOption.AllDialogs);
                        //}

                        //TODO: Prüfen ob Pfad existiert
                        copier.Copy(files.ToArray(), mappingItem.TargetPath);
                    }
                }
            }
        }

        public void Copy(String destPath)
        {
            var dataModel = ContextManager.Context();

            var mappingItems =
                dataModel.ExtensionMappingItems.ToList();

            if (mappingItems.Any())
            {
                foreach (var mappingItem in mappingItems)
                {
                    //Alle Dateien im Verzeichnis mit der entsprechenden Extension
                    var files = Directory.GetFiles(destPath)
                        .Where(x => String.Equals(System.IO.Path.GetExtension(x), mappingItem.Extension.ExtensionName, StringComparison.CurrentCultureIgnoreCase)).ToList();

                    if (files.Count > 0)
                    {
                        var copier = new FileCopier();
                        //copier.Copy();
                        //foreach (var sourceFile in files)
                        //{
                        //    var destinationFile = Path.Combine(TestPaths.Ausgabeordner, Path.GetFileName(sourceFile));



                        //    FileSystem.CopyFile(sourceFile, destinationFile, UIOption.AllDialogs);
                        //}

                        //TODO: Prüfen ob Pfad existiert
                        copier.Copy(files.ToArray(), mappingItem.TargetPath);
                    }
                }
            }
        }

        public void Copy(String[] source, String[] destFiles)
        {
            ShellLib.ShellFileOperation fo = new ShellLib.ShellFileOperation();
            
            fo.Operation = ShellLib.ShellFileOperation.FileOperations.FO_MOVE;
            fo.OwnerWindow = Process.GetCurrentProcess().MainWindowHandle;
            fo.SourceFiles = source;
            fo.DestFiles = destFiles;

            bool RetVal = fo.DoOperation();
            if (RetVal)
            {
                //TODO Fehlerbehandlung
            }

            //byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            //bool cancelFlag = false;

            //var maxBytes = SourceFiles.Sum(x => new FileInfo(x).Length);

            //foreach (var sourceFilePath in SourceFiles)
            //{
            //    var filename = Path.GetFileName(sourceFilePath);
            //    var destFilePath = Path.Combine(DestPath, filename);

            //    using (FileStream source = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            //    {
            //        using (FileStream dest = new FileStream(destFilePath, FileMode.CreateNew, FileAccess.Write))
            //        {
            //            long totalBytes = 0;
            //            int currentBlockSize = 0;

            //            while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
            //            {
            //                totalBytes += currentBlockSize;
            //                double persentage = (double)totalBytes* 100.0/ maxBytes;

            //                dest.Write(buffer, 0, currentBlockSize);

            //                cancelFlag = false;
            //                OnProgressChanged(persentage, ref cancelFlag);

            //                if (cancelFlag == true)
            //                {
            //                    // Delete dest file here
            //                    break;
            //                }
            //            }
            //        }
            //    }

            //}
            
        }

        public bool Copy(String[] source, String destPath)
        {
            ShellLib.ShellFileOperation fo = new ShellLib.ShellFileOperation();

            fo.Operation = ShellLib.ShellFileOperation.FileOperations.FO_MOVE;
            fo.OwnerWindow = Process.GetCurrentProcess().MainWindowHandle;
            fo.SourceFiles = source;

            var dest = new List<string>();

            foreach (var file in source)
            {
                var filename = Path.GetFileName(file);

                dest.Add(Path.Combine(destPath, filename));
            }

            fo.DestFiles = dest.ToArray();

            bool RetVal = fo.DoOperation();


            if (!RetVal)
            {
                //TODO Fehlerbehandlung
            }

            return RetVal;
        }
    }
}
