   M S B A M L   `   `   `  ����F  BRenamer_App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:RenamerApp.ViewsRenamerApp.Views  5   	   =#clr-namespace:RenamerApp.ViewModelsRenamerApp.ViewModels  5   	   !    RenamerApp.Views.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:RenamerApp.Views     5   	   2vm#clr-namespace:RenamerApp.ViewModels     5   	     � Title$  Renamer App MVP��5	   	   $	��450��6!   $	��600��6.    � DataContext 5      )   #RenamerApp.ViewModels.MainViewModel  5   
   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� $��Select Folder...��6   $��5q�5      $��0,0,10,0q�6!   ��5      �� *SelectFolderCommand+5   !   �� 5      $��Center=�5      ��5      �� *SelectedFolderPath+ �� FallbackValue$ No folder selected�� �� TargetNullValue$ No folder selected��5      5      �� 5   
   $��1��6   $��	0,10,0,10q�6    ��63   �� *
FileList+ �� View 5       �� 5!      .P�5"      �� $��Current Name��6%    �� DisplayMemberBinding 6;   �� *CurrentName+5#      5%      �� 5'   
   $��2��6   $ 
Horizontal=�6"   .+�5(      �� $��Prefix:��6   $��Center=�6&   $��0,0,5,0q�6A   �� 5)      $	��200��5*      $��Center=�6"   ��5)      �� *
PrefixText+ �� UpdateSourceTrigger$ PropertyChanged=�5*   "   5+      �� 5-   
   $��3��6   $��Rename Files��6   $��5q�5/      $��0,10,0,0q�6   $
��Left=�6/   ��5.      �� *RenameFilesCommand+5/   /   50      51      