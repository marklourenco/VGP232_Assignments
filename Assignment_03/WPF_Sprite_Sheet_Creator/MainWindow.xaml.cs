using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Linq;
using TextureAtlasLib;

namespace WPF_Sprite_Sheet_Creator
{
    public partial class MainWindow : Window
    {
        // Holds the image paths displayed in the ListBox
        private ObservableCollection<string> imagePaths;
        private string currentProjectPath = null;
        private bool isDirty = false;
        public MainWindow()
        {
            InitializeComponent();

            // Initialize collection
            imagePaths = new ObservableCollection<string>();

            // Bind ListBox to collection
            lbImages.ItemsSource = imagePaths;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Select PNG Images",
                Filter = "PNG Images (*.png)|*.png",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string file in dialog.FileNames)
                {
                    if (!imagePaths.Contains(file))
                    {
                        imagePaths.Add(file);
                    }
                }
            }

            isDirty = true;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (lbImages.SelectedItem is string selectedImage)
            {
                imagePaths.Remove(selectedImage);
            }

            isDirty = true;
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder",
                ValidateNames = false
            };

            if (dialog.ShowDialog() == true)
            {
                string folderPath = Path.GetDirectoryName(dialog.FileName);
                tbOutputDir.Text = folderPath;
            }
        }


        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(tbColumns.Text, out int columns))
                {
                    MessageBox.Show("Columns must be a valid number.");
                    return;
                }

                // Create and configure spritesheet object
                Spritesheet sheet = new Spritesheet
                {
                    OutputDirectory = tbOutputDir.Text,
                    OutputFile = tbOutputFile.Text,
                    Columns = int.Parse(tbColumns.Text),
                    IncludeMetaData = chkMetaData.IsChecked == true,
                    InputPaths = imagePaths.ToList()
                };

                // Generate the sprite sheet
                sheet.Generate(overwrite: true);

                // Success message
                MessageBoxResult result = MessageBox.Show(
                    "Spritesheet generated successfully!\n\nWould you like to view the output folder?",
                    "Success",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);

                // Open output directory if user selects Yes
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start("explorer.exe", sheet.OutputDirectory);
                }
            }
            catch (Exception ex)
            {
                // Error handling
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "SpriteSheet Project (*.xml)|*.xml"
            };

            if (dialog.ShowDialog() == true)
            {
                currentProjectPath = dialog.FileName;
                SaveProject(currentProjectPath);
                tbProjectFile.Text = System.IO.Path.GetFileName(currentProjectPath);
                isDirty = false;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentProjectPath))
            {
                SaveProject(currentProjectPath);
                isDirty = false;
            }
        }

        private void SaveProject(string path)
        {
            try
            {
                SpriteSheetProject project = new SpriteSheetProject
                {
                    OutputDirectory = tbOutputDir.Text,
                    OutputFile = tbOutputFile.Text,
                    IncludeMetaData = chkMetaData.IsChecked == true,
                    ImagePaths = imagePaths.ToList()
                };

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SpriteSheetProject));
                    serializer.Serialize(stream, project);
                }

                SaveMenuItem.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "SpriteSheet Project (*.xml)|*.xml"
            };

            if (dialog.ShowDialog() == true)
            {
                LoadProject(dialog.FileName);
            }
        }

        private void LoadProject(string path)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SpriteSheetProject));

                    SpriteSheetProject project = (SpriteSheetProject)serializer.Deserialize(stream);

                    // Restore fields
                    tbOutputDir.Text = project.OutputDirectory;
                    tbOutputFile.Text = project.OutputFile;
                    chkMetaData.IsChecked = project.IncludeMetaData;

                    imagePaths.Clear();

                    List<string> missingFiles = new List<string>();

                    foreach (string img in project.ImagePaths)
                    {
                        if (File.Exists(img))
                        {
                            imagePaths.Add(img);
                        }
                        else
                        {
                            missingFiles.Add(img);
                        }
                    }

                    if (missingFiles.Count > 0)
                    {
                        MessageBox.Show(
                            "The following images were missing and removed:\n\n" +
                            string.Join("\n", missingFiles),
                            "Missing Files",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                    }

                    currentProjectPath = path;
                    tbProjectFile.Text = System.IO.Path.GetFileName(path);
                    SaveMenuItem.IsEnabled = true;
                    isDirty = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty)
            {
                var result = MessageBox.Show(
                    "Would you like to save the current project first?",
                    "Save Project",
                    MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e);
                }
            }

            // Clear everything
            tbOutputDir.Clear();
            tbOutputFile.Text = "SpriteSheet.png";
            tbColumns.Text = "1";
            chkMetaData.IsChecked = false;
            imagePaths.Clear();

            currentProjectPath = null;
            tbProjectFile.Text = "SpriteSheet.xml";
            SaveMenuItem.IsEnabled = false;
            isDirty = false;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty)
            {
                var result = MessageBox.Show(
                    "Would you like to save before exiting?",
                    "Exit",
                    MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }

                if (result == MessageBoxResult.Yes)
                {
                    Save_Click(sender, e);
                }
            }

            Application.Current.Shutdown();
        }
    }
}