using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Input;
using System.Text;

namespace prog
{
    /// <summary>
    /// Логика взаимодействия для Task2.xaml
    /// </summary>
    public partial class Task2 : Page
    {
        public ObservableCollection<Student> Students = new ObservableCollection<Student>();
        private string _allowedExtensions = "Файл разметки (*.csv)|*.csv|JSON файл *(.json)|*.json";
        public Task2()
        {
            InitializeComponent();
            StudentsGrid.ItemsSource = Students;
        }

        private void AddNewStudent_Click(object sender, RoutedEventArgs e)
        {
            int lastId = Students.Max(t => t.ID);
            Student student = new Student();
            student.ID = lastId += 1;
            Students.Add(student);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = _allowedExtensions;

            Nullable<bool> result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                FileWorker.Instance.SetCurrentFile(saveFileDialog.FileName);
                FileWorker.Instance.SaveCurrentFile(Students);
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = _allowedExtensions;

            if (fileDialog.ShowDialog() == true && fileDialog.FileName != "")
            {
                Students = FileWorker.Instance.OpenFile(fileDialog.FileName);
                StudentsGrid.ItemsSource = Students;
                SavedChangesForCurrentFile();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (FileWorker.Instance.CurrentFile != null)
            {
                FileWorker.Instance.SaveCurrentFile(Students);
            }
            else
            {
                ExportButton_Click(sender, e);
            }
            SavedChangesForCurrentFile();
        }

        private void UnsavedChangesForCurrentFile()
        {
            if (FileWorker.Instance.CurrentFile != null)
            {
                CurrentFile.Content = $"*{FileWorker.Instance.CurrentFile.FullName}";
            }
            else
            {
                CurrentFile.Content += $"*{CurrentFile.Content}";
            }

        }

        private void SavedChangesForCurrentFile()
        {
            CurrentFile.Content = FileWorker.Instance.CurrentFile.FullName;
        }

        private void NoCurrentFile()
        {
            CurrentFile.Content = "[Новый файл]";
        }

        private void NewFileButton_Click(object sender, RoutedEventArgs e)
        {
            Students = FileWorker.Instance.NewFile();
            StudentsGrid.ItemsSource = Students;
            NoCurrentFile();
        }

        private void StudentsGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            UnsavedChangesForCurrentFile();
        }

        private void DeleteSelectedStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedValue is Student)
            {
                Student student = (Student)StudentsGrid.SelectedValue;
                Students.Remove(student);
            }
            else
            {
                MessageBox.Show("Ошибка", "Студент не выбран!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TopStudentButton_Click(object sender, RoutedEventArgs e)
        {
            var maxes = from st in Students
                        group st by st.SchoolNumber into g
                        select new { SchoolNumber = g.Key, Max = g.Max(o => o.Geometry + o.Math) };
            var students = from st in Students
                           join maxRow in maxes on st.SchoolNumber equals maxRow.SchoolNumber
                           where (st.Geometry + st.Math) == maxRow.Max && st.SchoolNumber == maxRow.SchoolNumber
                           select st;

            // Students.Clear();
            Students = new ObservableCollection<Student>(students);
            StudentsGrid.ItemsSource = Students;
        }
    }
}
