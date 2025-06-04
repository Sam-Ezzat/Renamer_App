using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RenamerApp.Models;
using RenamerApp.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace RenamerApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly FileSystemService _fileSystemService;

        [ObservableProperty]
        private string _selectedFolderPath;

        [ObservableProperty]
        private ObservableCollection<FileItem> _fileList;

        [ObservableProperty]
        private string _prefixText;

        public MainViewModel()
        {
            _fileSystemService = new FileSystemService();
            _fileList = new ObservableCollection<FileItem>();
            _selectedFolderPath = "No folder selected";
            _prefixText = "";
        }

        [RelayCommand]
        private void SelectFolder()
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                SelectedFolderPath = dialog.FolderName;
                LoadFiles(SelectedFolderPath);
            }
        }

        [RelayCommand(CanExecute = nameof(CanRenameFiles))]
        private void RenameFiles()
        {
            if (!Directory.Exists(SelectedFolderPath) || FileList == null || !FileList.Any() || string.IsNullOrEmpty(PrefixText))
            {
                return;
            }

            bool success = _fileSystemService.RenameFilesWithPrefix(SelectedFolderPath, FileList.ToList(), PrefixText);

            if (success)
            {
                LoadFiles(SelectedFolderPath);
            }
        }

        private bool CanRenameFiles()
        {
            return Directory.Exists(SelectedFolderPath) && FileList != null && FileList.Any() && !string.IsNullOrEmpty(PrefixText);
        }

        private void LoadFiles(string folderPath)
        {
            FileList.Clear();
            if (Directory.Exists(folderPath))
            {
                var files = _fileSystemService.GetFilesFromFolder(folderPath);
                foreach (var file in files)
                {
                    FileList.Add(file);
                }
            }
            RenameFilesCommand.NotifyCanExecuteChanged();
        }
    }
}