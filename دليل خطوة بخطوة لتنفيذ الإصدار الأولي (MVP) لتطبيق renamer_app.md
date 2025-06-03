# دليل خطوة بخطوة لتنفيذ الإصدار الأولي (MVP) لتطبيق renamer_app

أهلاً بك في مرحلة التنفيذ! بناءً على طلبك، سأقوم بتقسيم عملية بناء الـ MVP لتطبيق `renamer_app` باستخدام C# و WPF ونمط MVVM إلى خطوات واضحة ومفصلة، مع تقديم اقتراحات للأسماء وشرح للتفاصيل وتلميحات للنقاط التي قد تحتاج إلى تركيز إضافي.

**تذكير بهدف الـ MVP:**

*   السماح للمستخدم باختيار مجلد.
*   عرض قائمة بالملفات الموجودة في هذا المجلد (اسم الملف الحالي فقط مبدئياً).
*   السماح للمستخدم بإدخال نص ليكون بادئة (Prefix) للاسم الجديد.
*   وجود زر لتنفيذ عملية إعادة التسمية (سنقوم بإعادة تسمية *كل* الملفات المعروضة في هذه المرحلة لتبسيط الـ MVP).
*   إضافة البادئة المدخلة إلى بداية اسم كل ملف.

**هيكل المشروع المقترح (نمط MVVM):**

سنقوم بتنظيم المشروع في المجلدات التالية لتعزيز فصل الاهتمامات:

*   `Views`: تحتوي على ملفات XAML الخاصة بواجهات المستخدم (مثل `MainWindow.xaml`).
*   `ViewModels`: تحتوي على الأصناف (Classes) التي تعمل كوسيط بين الـ View والـ Model (مثل `MainViewModel.cs`).
*   `Models`: تحتوي على أصناف تمثل بيانات التطبيق (مثل `FileItem.cs` لتمثيل ملف واحد).
*   `Services`: تحتوي على أصناف مسؤولة عن منطق العمل الخارجي مثل التفاعل مع نظام الملفات (مثل `FileSystemService.cs`).
*   `Commands`: (اختياري) يمكن وضع أصناف الأوامر المساعدة هنا (مثل `RelayCommand.cs`).

---

**الخطوة 1: إعداد المشروع وهيكله**

1.  **إنشاء المشروع:**
    *   افتح Visual Studio.
    *   اختر "Create a new project".
    *   ابحث عن "WPF App (.NET)" وتأكد من اختيار قالب C#.
    *   اختر اسماً للمشروع (مثل `RenamerApp`) وموقعاً له.
    *   اختر أحدث إصدار .NET مدعوم (مثل .NET 8.0 أو .NET 6.0).
    *   انقر "Create".

2.  **إنشاء مجلدات MVVM:**
    *   في نافذة Solution Explorer، انقر بزر الماوس الأيمن على اسم المشروع (`RenamerApp`).
    *   اختر `Add` -> `New Folder`.
    *   أنشئ المجلدات التالية: `Views`, `ViewModels`, `Models`, `Services`, `Commands`.

3.  **نقل MainWindow.xaml:**
    *   اسحب ملف `MainWindow.xaml` من المجلد الرئيسي للمشروع وأفلته داخل مجلد `Views`.
    *   **تلميح:** قد تحتاج إلى تعديل ملف `App.xaml` لتحديث مسار `StartupUri` ليشير إلى الموقع الجديد: `StartupUri="Views/MainWindow.xaml"`.
    *   قد تحتاج أيضاً لتعديل الـ namespace في ملف `MainWindow.xaml` و `MainWindow.xaml.cs` ليعكس وجوده داخل مجلد `Views` (مثلاً `RenamerApp.Views`).

---

**الخطوة 2: تصميم واجهة المستخدم الأساسية (View - MainWindow.xaml)**

سنقوم بتصميم واجهة بسيطة تحتوي على العناصر المطلوبة للـ MVP.

1.  **فتح `Views/MainWindow.xaml`:**
2.  **تعديل كود XAML:** استبدل محتوى `<Grid>` الافتراضي بما يشبه التالي (هذا مجرد هيكل أساسي، يمكنك تحسين التصميم لاحقاً):

```xml
<Window x:Class="RenamerApp.Views.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:RenamerApp.Views"
        xmlns:vm="clr-namespace:RenamerApp.ViewModels" 
        mc:Ignorable="d"
        Title="Renamer App MVP" Height="450" Width="600">

    <!-- تعيين DataContext للـ ViewModel -->
    <Window.DataContext>
        <vm:MainViewModel/>
    </Window.DataContext>

    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/> <!-- صف لاختيار المجلد -->
            <RowDefinition Height="*"/>    <!-- صف لعرض قائمة الملفات -->
            <RowDefinition Height="Auto"/> <!-- صف لإدخال البادئة -->
            <RowDefinition Height="Auto"/> <!-- صف لزر إعادة التسمية -->
        </Grid.RowDefinitions>

        <!-- منطقة اختيار المجلد -->
        <StackPanel Grid.Row="0" Orientation="Horizontal">
            <Button Content="Select Folder..." 
                    Command="{Binding SelectFolderCommand}" 
                    Padding="5" Margin="0,0,10,0"/>
            <TextBlock Text="{Binding SelectedFolderPath, FallbackValue=\'No folder selected\', TargetNullValue=\'No folder selected\'}" 
                       VerticalAlignment="Center"/>
        </StackPanel>

        <!-- قائمة الملفات -->
        <ListView Grid.Row="1" Margin="0,10,0,10" ItemsSource="{Binding FileList}">
            <ListView.View>
                <GridView>
                    <GridViewColumn Header="Current Name" DisplayMemberBinding="{Binding CurrentName}"/>
                    <!-- لاحقاً يمكن إضافة عمود للاسم الجديد للمعاينة -->
                </GridView>
            </ListView.View>
        </ListView>

        <!-- منطقة إدخال البادئة -->
        <StackPanel Grid.Row="2" Orientation="Horizontal">
            <Label Content="Prefix:" VerticalAlignment="Center" Margin="0,0,5,0"/>
            <TextBox Text="{Binding PrefixText, UpdateSourceTrigger=PropertyChanged}" 
                     Width="200" VerticalAlignment="Center"/>
        </StackPanel>

        <!-- زر إعادة التسمية -->
        <Button Grid.Row="3" Content="Rename Files" 
                Command="{Binding RenameFilesCommand}" 
                Padding="5" Margin="0,10,0,0" HorizontalAlignment="Left"/>

    </Grid>
</Window>
```

*   **شرح:**
    *   `xmlns:vm="clr-namespace:RenamerApp.ViewModels"`: نُعرف اختصاراً للوصول إلى الـ ViewModels.
    *   `<Window.DataContext>`: نربط الـ DataContext الخاص بالنافذة بمثيل من `MainViewModel` (سنقوم بإنشائه في الخطوة التالية). هذا هو أساس ربط البيانات في MVVM.
    *   `Command="{Binding SelectFolderCommand}"`: نربط زر اختيار المجلد بـ `ICommand` في الـ ViewModel.
    *   `Text="{Binding SelectedFolderPath...}"`: نربط النص المعروض بـ Property في الـ ViewModel لعرض المسار المختار.
    *   `ItemsSource="{Binding FileList}"`: نربط مصدر بيانات `ListView` بـ Collection في الـ ViewModel.
    *   `DisplayMemberBinding="{Binding CurrentName}"`: نعرض خاصية `CurrentName` من كل عنصر في `FileList`.
    *   `Text="{Binding PrefixText...}"`: نربط مربع النص بخاصية في الـ ViewModel لتلقي البادئة.
    *   `UpdateSourceTrigger=PropertyChanged`: تجعل الـ ViewModel يتحدث بقيمة النص فوراً عند كل تغيير (مفيد للمعاينة لاحقاً).

---

**الخطوة 3: إنشاء الـ Model الأساسي**

سنحتاج إلى صنف بسيط لتمثيل كل ملف في القائمة.

1.  **إنشاء `FileItem.cs`:**
    *   انقر بزر الماوس الأيمن على مجلد `Models`.
    *   اختر `Add` -> `Class...`.
    *   سمّ الملف `FileItem.cs`.
    *   أضف الخصائص التالية:

```csharp
namespace RenamerApp.Models
{
    public class FileItem
    {
        // المسار الكامل للملف
        public string FullPath { get; set; }

        // اسم الملف الحالي (مع الامتداد)
        public string CurrentName { get; set; }

        // يمكن إضافة خصائص أخرى لاحقاً مثل الاسم الجديد، الحجم، التاريخ، إلخ.

        public FileItem(string fullPath, string currentName)
        {
            FullPath = fullPath;
            CurrentName = currentName;
        }
    }
}
```

---

**الخطوة 4: إنشاء ViewModel الرئيسي (`MainViewModel.cs`)**

هذا هو "العقل" الخاص بالواجهة. سيتعامل مع الأوامر ويحتفظ بالبيانات التي تعرضها الواجهة.

1.  **إنشاء `MainViewModel.cs`:**
    *   انقر بزر الماوس الأيمن على مجلد `ViewModels`.
    *   اختر `Add` -> `Class...`.
    *   سمّ الملف `MainViewModel.cs`.

2.  **تحقيق `INotifyPropertyChanged`:**
    *   هذه الواجهة ضرورية لإعلام الـ View بأي تغييرات تحدث في خصائص الـ ViewModel.
    *   **تلميح:** يمكنك كتابة التنفيذ يدوياً، أو استخدام مكتبة مساعدة مثل `CommunityToolkit.Mvvm` (NuGet package) التي توفر `ObservableObject` لتبسيط هذه العملية بشكل كبير. سنفترض هنا استخدام `ObservableObject` من `CommunityToolkit.Mvvm` للسهولة.
    *   قم بتثبيت الحزمة: انقر بزر الماوس الأيمن على المشروع -> `Manage NuGet Packages...` -> ابحث عن `CommunityToolkit.Mvvm` وقم بتثبيته.
    *   اجعل الصنف يرث من `ObservableObject`:

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RenamerApp.Models; // لاستخدام FileItem
using RenamerApp.Services; // لاستخدام FileSystemService (سننشئه لاحقاً)
using System.Collections.ObjectModel; // لاستخدام ObservableCollection
using System.IO; // لاستخدام Path
using System.Linq; // لاستخدام LINQ
// قد تحتاج لإضافة using لـ FolderBrowserDialog أو ما يعادله
using Microsoft.Win32; // كمثال لاستخدام OpenFolderDialog في .NET Core+

namespace RenamerApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        // --- خدمات --- 
        private readonly FileSystemService _fileSystemService; // خدمة التعامل مع الملفات (سننشئها)

        // --- خصائص مرتبطة بالواجهة --- 

        [ObservableProperty]
        private string _selectedFolderPath;

        [ObservableProperty]
        private ObservableCollection<FileItem> _fileList;

        [ObservableProperty]
        private string _prefixText;

        // --- Constructor --- 
        public MainViewModel()
        {
            _fileSystemService = new FileSystemService(); // إنشاء مثيل من الخدمة
            _fileList = new ObservableCollection<FileItem>();
            _selectedFolderPath = "No folder selected";
            _prefixText = "";
        }

        // --- أوامر --- 

        [RelayCommand]
        private void SelectFolder()
        {
            // **تلميح:** استخدام FolderBrowserDialog التقليدي قد يتطلب إضافة مرجعية 
            // لـ System.Windows.Forms. الأفضل استخدام بدائل أحدث إذا أمكن.
            // مثال باستخدام OpenFolderDialog (يتطلب .NET Core 3.0+ أو .NET 5+)
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

        [RelayCommand(CanExecute = nameof(CanRenameFiles))] // ربط إمكانية التنفيذ بدالة
        private void RenameFiles()
        {
            if (!Directory.Exists(SelectedFolderPath) || FileList == null || !FileList.Any() || string.IsNullOrEmpty(PrefixText))
            {
                // يمكنك إضافة رسالة خطأ للمستخدم هنا
                return;
            }

            // **تلميح:** يجب إضافة معالجة أخطاء أفضل هنا (try-catch)
            bool success = _fileSystemService.RenameFilesWithPrefix(SelectedFolderPath, FileList.ToList(), PrefixText);

            if (success)
            {
                // إعادة تحميل قائمة الملفات بعد إعادة التسمية الناجحة
                LoadFiles(SelectedFolderPath);
                // يمكنك إضافة رسالة نجاح للمستخدم
            }
            else
            { 
                // يمكنك إضافة رسالة فشل للمستخدم
            }
        }

        // دالة لتحديد ما إذا كان يمكن تنفيذ أمر RenameFiles
        private bool CanRenameFiles()
        {
            // يمكن تنفيذ الأمر فقط إذا كان هناك مجلد محدد، وقائمة ملفات غير فارغة، وبادئة مدخلة
            return Directory.Exists(SelectedFolderPath) && FileList != null && FileList.Any() && !string.IsNullOrEmpty(PrefixText);
        }


        // --- دوال مساعدة --- 

        private void LoadFiles(string folderPath)
        {
            FileList.Clear(); // مسح القائمة القديمة
            if (Directory.Exists(folderPath))
            {
                var files = _fileSystemService.GetFilesFromFolder(folderPath);
                foreach (var file in files)
                {
                    FileList.Add(file);
                }
            }
            // إعلام الواجهة بأن حالة CanExecute قد تغيرت لزر Rename
            RenameFilesCommand.NotifyCanExecuteChanged(); 
        }
    }
}
```

*   **شرح:**
    *   `ObservableObject`: يوفر تنفيذ `INotifyPropertyChanged`.
    *   `[ObservableProperty]`: سمة (Attribute) من `CommunityToolkit.Mvvm` تقوم تلقائياً بإنشاء خاصية قابلة للربط (Property) مع آلية الإشعار بالتغيير.
    *   `ObservableCollection<FileItem>`: نوع Collection يُعلم الـ UI تلقائياً عند إضافة أو إزالة عناصر، مما يجعله مثالياً لربط القوائم.
    *   `[RelayCommand]`: سمة أخرى من `CommunityToolkit.Mvvm` تنشئ `ICommand` يمكن ربطه بالأزرار في XAML. تقوم بتغليف الدوال (مثل `SelectFolder`, `RenameFiles`).
    *   `CanExecute`: خاصية في `ICommand` تحدد ما إذا كان الزر المرتبط مفعلاً أم لا. ربطناها بدالة `CanRenameFiles`.
    *   `NotifyCanExecuteChanged()`: يجب استدعاؤها عندما يتغير الشرط الذي تعتمد عليه `CanExecute` (مثل اختيار مجلد أو إدخال بادئة).
    *   `_fileSystemService`: سنقوم بحقن (Inject) أو إنشاء مثيل من خدمة نظام الملفات للقيام بالعمل الفعلي.
    *   **تلميح مهم:** في تطبيقات أكبر، يُفضل استخدام حقن الاعتماديات (Dependency Injection) لتوفير الخدمات للـ ViewModel بدلاً من إنشائها مباشرة (`new FileSystemService()`).

---

**الخطوة 5: إنشاء خدمة نظام الملفات (`FileSystemService.cs`)**

هذا الصنف سيعزل منطق التعامل المباشر مع نظام الملفات عن الـ ViewModel.

1.  **إنشاء `FileSystemService.cs`:**
    *   انقر بزر الماوس الأيمن على مجلد `Services`.
    *   اختر `Add` -> `Class...`.
    *   سمّ الملف `FileSystemService.cs`.

2.  **إضافة الدوال:**

```csharp
using RenamerApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RenamerApp.Services
{
    public class FileSystemService
    {
        // دالة لجلب قائمة الملفات من مجلد معين
        public IEnumerable<FileItem> GetFilesFromFolder(string folderPath)
        {
            // **تلميح:** يجب إضافة معالجة أخطاء هنا (try-catch) للتعامل مع 
            // مشاكل مثل عدم وجود المجلد أو نقص الصلاحيات.
            try
            {
                // الحصول على الملفات فقط (وليس المجلدات الفرعية في هذه المرحلة)
                var files = Directory.GetFiles(folderPath);
                return files.Select(fullPath => new FileItem(fullPath, Path.GetFileName(fullPath)));
            }
            catch (System.Exception ex)
            {
                // يمكنك تسجيل الخطأ هنا (Logging)
                System.Diagnostics.Debug.WriteLine($"Error getting files: {ex.Message}");
                return Enumerable.Empty<FileItem>(); // إرجاع قائمة فارغة عند حدوث خطأ
            }
        }

        // دالة لإعادة تسمية الملفات بإضافة بادئة
        public bool RenameFilesWithPrefix(string folderPath, List<FileItem> filesToRename, string prefix)
        {
            // **تلميح:** هذه دالة حساسة وتحتاج إلى معالجة أخطاء دقيقة.
            // يجب التحقق من صلاحيات الكتابة، ومن أن الاسم الجديد صالح، 
            // ومن عدم وجود تعارض في الأسماء (ملفين يحصلان على نفس الاسم الجديد).
            // الـ MVP سيبسط هذا، لكن في التطبيق الحقيقي، هذا الجزء معقد.
            bool allSucceeded = true;
            foreach (var fileItem in filesToRename)
            {
                try
                {
                    string currentFileName = fileItem.CurrentName;
                    string newFileName = prefix + currentFileName;
                    string newFullPath = Path.Combine(folderPath, newFileName);

                    // التحقق مما إذا كان الملف بالاسم الجديد موجوداً بالفعل (تبسيط للـ MVP)
                    if (File.Exists(newFullPath))
                    { 
                        System.Diagnostics.Debug.WriteLine($"Skipping rename for {currentFileName}: Target file {newFileName} already exists.");
                        allSucceeded = false; // اعتبار العملية غير ناجحة بالكامل إذا تم تخطي ملف
                        continue; // الانتقال للملف التالي
                    }

                    // تنفيذ إعادة التسمية
                    File.Move(fileItem.FullPath, newFullPath);
                    System.Diagnostics.Debug.WriteLine($"Renamed {currentFileName} to {newFileName}");
                }
                catch (System.Exception ex)
                {
                    // تسجيل الخطأ
                    System.Diagnostics.Debug.WriteLine($"Error renaming file {fileItem.CurrentName}: {ex.Message}");
                    allSucceeded = false; // اعتبار العملية غير ناجحة بالكامل إذا فشل ملف واحد
                    // يمكنك اختيار الاستمرار مع الملفات الأخرى أو التوقف
                }
            }
            return allSucceeded;
        }
    }
}
```

*   **شرح:**
    *   `GetFilesFromFolder`: تستخدم `Directory.GetFiles` للحصول على مسارات الملفات وتحولها إلى قائمة `FileItem`.
    *   `RenameFilesWithPrefix`: تمر على قائمة الملفات، تنشئ الاسم الجديد، وتستخدم `File.Move` لإعادة التسمية. تتضمن معالجة أخطاء أساسية جداً وتحققاً بسيطاً من وجود الملف الهدف.
    *   **تلميحات مهمة:**
        *   **معالجة الأخطاء:** الكود أعلاه يتضمن `try-catch` أساسي. في تطبيق حقيقي، ستحتاج إلى معالجة أكثر تفصيلاً لأنواع مختلفة من الأخطاء (IOExceptions, SecurityExceptions, UnauthorizedAccessException) وربما تقديم تغذية راجعة أفضل للمستخدم.
        *   **عمليات غير متزامنة (Async):** عمليات الملفات يمكن أن تكون بطيئة، خاصة مع عدد كبير من الملفات. يُفضل جعل هذه الدوال غير متزامنة (`async Task`) واستخدام نظائرها غير المتزامنة من `System.IO` لتجنب تجميد واجهة المستخدم.
        *   **التعارض:** التحقق من وجود الملف الهدف (`File.Exists`) هو تبسيط. قد تحتاج إلى استراتيجية أكثر تعقيداً للتعامل مع التعارضات (مثل إضافة رقم للاسم الجديد).

---

**الخطوة 6: التشغيل والاختبار الأولي**

1.  **بناء المشروع (Build):** اضغط `Ctrl+Shift+B` أو اختر `Build` -> `Build Solution`.
2.  **تشغيل المشروع (Run):** اضغط `F5` أو انقر على زر التشغيل الأخضر.
3.  **الاختبار:**
    *   انقر زر "Select Folder..." واختر مجلداً يحتوي على بعض الملفات (يفضل نسخ بعض الملفات إلى مجلد اختبار حتى لا تعدل ملفاتك الأصلية).
    *   تأكد من عرض أسماء الملفات في القائمة.
    *   أدخل نصاً في مربع "Prefix:".
    *   لاحظ أن زر "Rename Files" يجب أن يصبح مفعلاً الآن.
    *   انقر زر "Rename Files".
    *   تحقق من مجلد الاختبار لترى ما إذا تمت إعادة تسمية الملفات بإضافة البادئة.
    *   تحقق من تحديث قائمة الملفات في التطبيق لتعكس الأسماء الجديدة.

---

**ملخص وتلميحات إضافية:**

*   **MVVM:** لقد أنشأنا بنية MVVM أساسية حيث الـ View (XAML) معزول عن المنطق، والـ ViewModel يدير حالة الواجهة والأوامر، والـ Service يتعامل مع نظام الملفات.
*   **ربط البيانات (Data Binding):** استخدمنا ربط البيانات لربط عناصر الواجهة بخصائص وأوامر الـ ViewModel.
*   **الأوامر (Commands):** استخدمنا `RelayCommand` لتغليف منطق الأزرار.
*   **`INotifyPropertyChanged` / `ObservableObject`:** ضروري لتحديث الواجهة تلقائياً عند تغير البيانات في الـ ViewModel.
*   **`ObservableCollection<T>`:** ضروري لتحديث القوائم في الواجهة تلقائياً.
*   **التحسينات المستقبلية (خارج نطاق الـ MVP الأولي):**
    *   اختيار ملفات محددة لإعادة التسمية بدلاً من الكل.
    *   إضافة عمود "New Name" للمعاينة قبل التنفيذ.
    *   إضافة المزيد من قواعد إعادة التسمية (لاحقة، ترقيم، تاريخ، استبدال، تغيير حالة الأحرف).
    *   السماح بدمج قواعد متعددة.
    *   معالجة أخطاء أكثر قوة وتفصيلاً مع تغذية راجعة للمستخدم.
    *   جعل عمليات الملفات غير متزامنة (Async/Await).
    *   تحسين تصميم الواجهة (UI/UX).
    *   كتابة اختبارات الوحدة (Unit Tests) للـ ViewModel والـ Service.
    *   استخدام حقن الاعتماديات (Dependency Injection).

هذا الدليل يوفر لك نقطة انطلاق قوية لبناء الـ MVP. تذكر أن تبني خطوة بخطوة، تختبر بشكل متكرر، ولا تتردد في البحث عن تفاصيل إضافية حول WPF، MVVM، و `System.IO` عند الحاجة. بالتوفيق!
