using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace prog
{
    public sealed class FileWorker
    {
        public FileInfo CurrentFile;
        public FileWorker()
        {

        }
        private static FileWorker _fileWorker;
        public static FileWorker Instance
        {
            get
            {
                if (_fileWorker == null)
                {
                    _fileWorker = new FileWorker();
                }
                return _fileWorker;
            }
        }

        public void SaveCurrentFile(ObservableCollection<Student> data)
        {
            switch (Path.GetExtension(CurrentFile.FullName))
            {
                case ".csv":
                    ExportAsCsv(data);
                    break;
                case ".json":
                    ExportAsJson(data);
                    break;
                default:
                    break;
            }
        }

        public ObservableCollection<Student> NewFile()
        {
            CurrentFile = null;
            return new ObservableCollection<Student>();
        }

        public ObservableCollection<Student> OpenFile(string fileName)
        {
            SetCurrentFile(fileName);

            ObservableCollection<Student> students = null;

            switch (Path.GetExtension(fileName))
            {
                case ".csv":
                    students = ImportAsCsv();
                    break;
                case ".json":
                    students = ImportAsJson();
                    break;
                default:
                    break;
            }

            return students;
        }

        public ObservableCollection<Student> ImportAsCsv()
        {
            var lines = File.ReadAllLines(CurrentFile.FullName);

            ObservableCollection<Student> students = new ObservableCollection<Student>();
            foreach (string l in lines)
            {
                var splited = l.Split(',');
                if (string.IsNullOrEmpty(splited[0]) || splited[0] == "ID")
                {
                    continue;
                }

                Student newStudent = new Student
                {
                    ID = int.Parse(splited[0]),
                    LastName = splited[1],
                    FirstName = splited[2],
                    SchoolNumber = int.Parse(splited[3]),
                    Math = int.Parse(splited[4]),
                    Geometry = int.Parse(splited[5])
                };

                students.Add(newStudent);
            }

            return students;
        }

        public ObservableCollection<Student> ImportAsJson()
        {
            string allText = File.ReadAllText(CurrentFile.FullName);

            ObservableCollection<Student> students = JsonConvert.DeserializeObject<ObservableCollection<Student>>(allText);

            return students;
        }

        public void ExportAsCsv(ObservableCollection<Student> students)
        {
            using (StreamWriter fileStream = new StreamWriter(CurrentFile.FullName, false))
            {                
                PropertyInfo[] props = typeof(Student).GetProperties();
                string propString = String.Join(',', props.Select(p => p.Name));
                fileStream.WriteLine(propString);

                foreach (var item in students)
                {
                    string itemValues = String.Join(',', props.Select(p => p.GetValue(item)));
                    fileStream.WriteLine(itemValues);
                }
            }
        }

        public void SetCurrentFile(string fileName)
        {
            CurrentFile = new FileInfo(fileName);
        }

        public void ExportAsJson(ObservableCollection<Student> students)
        {
            string json = JsonConvert.SerializeObject(students);
            using (StreamWriter sw = new StreamWriter(CurrentFile.FullName, false))
            {
                sw.Write(json);
            }
        }
    }
}
