namespace RenamerApp.Models
{
    public class FileItem
    {
        // Full path of the file
        public string FullPath { get; set; }

        // Current name of the file (including extension)
        public string CurrentName { get; set; }

        public FileItem(string fullPath, string currentName)
        {
            FullPath = fullPath;
            CurrentName = currentName;
        }
    }
}