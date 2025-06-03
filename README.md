# Renamer App

## Overview
Renamer App is a simple WPF application designed to help users rename multiple files by adding a specified prefix to their names. The application follows the MVVM (Model-View-ViewModel) design pattern, ensuring a clean separation of concerns and maintainability.

## Project Structure
The project is organized into several folders, each serving a specific purpose:

- **Views**: Contains the XAML files that define the user interface.
  - `MainWindow.xaml`: The main window of the application.
  
- **ViewModels**: Contains the ViewModel classes that handle the application's logic and data binding.
  - `MainViewModel.cs`: The ViewModel for the main window.

- **Models**: Contains the data models used in the application.
  - `FileItem.cs`: Represents a file with properties for its full path and current name.

- **Services**: Contains classes that handle external operations, such as file system interactions.
  - `FileSystemService.cs`: Provides methods for retrieving files and renaming them.

- **Commands**: Contains command implementations that can be bound to UI elements.
  - `RelayCommand.cs`: Implements the ICommand interface for command binding.

- **App.xaml**: Defines application-level resources and settings.

- **App.xaml.cs**: Contains the application entry point and initialization logic.

- **README.md**: Documentation for the project, including setup instructions and usage.

## Setup Instructions
1. Clone the repository or download the project files.
2. Open the project in your preferred IDE.
3. Restore any necessary NuGet packages.
4. Build the project to ensure all dependencies are resolved.
5. Run the application.

## Usage
1. Launch the application.
2. Click on the "Select Folder..." button to choose a directory containing the files you want to rename.
3. Enter a prefix in the provided text box.
4. Click the "Rename Files" button to apply the prefix to all files in the selected directory.

## Contributing
Contributions are welcome! Please feel free to submit issues or pull requests for any enhancements or bug fixes.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.