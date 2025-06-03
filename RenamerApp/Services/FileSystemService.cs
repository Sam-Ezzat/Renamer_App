using RenamerApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RenamerApp.Services
{
    public class FileSystemService
    {
        public IEnumerable<FileItem> GetFilesFromFolder(string folderPath)
        {
            try
            {
                var files = Directory.GetFiles(folderPath);
                return files.Select(fullPath => new FileItem(fullPath, Path.GetFileName(fullPath)));
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting files: {ex.Message}");
                return Enumerable.Empty<FileItem>();
            }
        }

        public bool RenameFilesWithPrefix(string folderPath, List<FileItem> filesToRename, string prefix)
        {
            bool allSucceeded = true;
            foreach (var fileItem in filesToRename)
            {
                try
                {
                    string currentFileName = fileItem.CurrentName;
                    string newFileName = prefix + currentFileName;
                    string newFullPath = Path.Combine(folderPath, newFileName);

                    if (File.Exists(newFullPath))
                    { 
                        System.Diagnostics.Debug.WriteLine($"Skipping rename for {currentFileName}: Target file {newFileName} already exists.");
                        allSucceeded = false;
                        continue;
                    }

                    File.Move(fileItem.FullPath, newFullPath);
                    System.Diagnostics.Debug.WriteLine($"Renamed {currentFileName} to {newFileName}");
                }
                catch (System.Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error renaming file {fileItem.CurrentName}: {ex.Message}");
                    allSucceeded = false;
                }
            }
            return allSucceeded;
        }
    }
}