using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace ERS.Model
{
    public class Guide:ReactiveObject
    {
        [Reactive]
        public int Id { get; set; } 

        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public int MainGuideId { get; set; }
        [Reactive]
        public Guide MainGuide {get;set;}
        [Reactive]
        public ObservableCollection<Guide> Guides { get; set; }
        [Reactive]
        public string Url { get; set; }
        private bool _is_web;
        public bool IsWeb
        {
            get => _is_web;
            set
            {
                FileName=null;
                this.RaiseAndSetIfChanged(ref _is_web, value);
            }
        }
        [NotMapped]
        [Reactive]
        public string FileName { get; set; }
        [NotMapped]
        [Reactive]
        public System.Windows.Controls.UserControl Control { get; set; }
        [Reactive]
        public string DirectoryFiles { get; set; }
        public static ObservableCollection<Guide> GetGuides()
        {
            using (var db = new ConnectDB())
            {
                return new ObservableCollection<Guide>(db.Guide.Where(p=>p.MainGuideId==1 && p.Id!=1).Select(p=>new Guide(p.Id, p.Name,p.MainGuideId, p.Url, p.IsWeb)));
            }
            
        }
        public static ObservableCollection<Guide> GetGuidesFromFind(string Find)
        {
            using (var db = new ConnectDB())
            {

                return new ObservableCollection<Guide>(db.Guide.FromSqlInterpolated($"SELECT * FROM Guide where id!=1 and Name like lower({"%"+Find.ToLower()+"%"}) ").Select(p => new Guide(p.Id, p.Name, p.MainGuideId, p.Url, p.IsWeb)));
                //Обычно я использую для запросов чисто linq но тут как показала практика такой запрос работает эффективнее
            }

        }
        public static ObservableCollection<Guide> GetGuidesForCombo()
        {
            using (var db = new ConnectDB())
            {
               
                return new ObservableCollection<Guide>(db.Guide);
            }

        }
        public static bool AddGuides(Guide new_guide)
        {
            using (var db = new ConnectDB())
            {

                if (new_guide.Url==null )
                {
                    System.Windows.MessageBox.Show("Выберите html файл или введите url сайта","Ошибка");
                    return false;
                }
                if(!new_guide.IsWeb)
                {
                    if (File.Exists(new_guide.Url))
                        File.Delete(new_guide.Url);
                    File.Copy(new_guide.FileName, new_guide.Url);
                    if (Directory.Exists(Directory.GetCurrentDirectory()+"\\HTML"+new_guide.DirectoryFiles.Replace(Directory.GetParent(new_guide.DirectoryFiles).ToString(), "")))
                    {
                        Directory.Delete(Directory.GetCurrentDirectory()+"\\HTML"+new_guide.DirectoryFiles.Replace(Directory.GetParent(new_guide.DirectoryFiles).ToString(), ""),true);
                    }
                    CopyDirectory(new_guide.DirectoryFiles, Directory.GetCurrentDirectory()+"\\HTML"+new_guide.DirectoryFiles.Replace(Directory.GetParent(new_guide.DirectoryFiles).ToString(), ""),true);
                    new_guide.DirectoryFiles=Directory.GetCurrentDirectory()+"\\HTML"+new_guide.DirectoryFiles.Replace(Directory.GetParent(new_guide.DirectoryFiles).ToString(), "");
                }
                
                db.Add(new_guide);
                db.SaveChanges();
                return true;
            }
        }
        public static void DeleteGuide(Guide del_guide)
        {
            if(del_guide==null) return;
            var dialogResult = System.Windows.MessageBox.Show($"Вы действительно хотите удалить {del_guide.Name}", "Внимание", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using(var db=new ConnectDB())
                {
                    db.Guide.Remove(del_guide);
                    db.SaveChanges();
                }
            }
        }
      private  static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
        public Guide()
        {


        }
        public Guide(int id, string name, int guide_id,string Url, bool is_web)
        {
            this.Id=id;
            this.Name=name;
            this.MainGuideId=guide_id;
            
            this.Url=Url;
            if (!is_web)
                this.Url=Directory.GetCurrentDirectory()+"\\"+Url;
            Console.WriteLine(this.Url);
            this.Control=new View.Guide.WebPage(this.Url);
            using (var db = new ConnectDB())
            {
                Guides=new ObservableCollection<Guide>(db.Guide.Where(p => p.MainGuideId==id).Select(p => new Guide(p.Id, p.Name, p.MainGuideId, p.Url, p.IsWeb)));
            }
        }

    }
    public class AddGuide:Guide
    {
        

        public ReactiveCommand<Unit, Unit> OpenFile => ReactiveCommand.Create(() => {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Файлы html|*.html;*.htm";
            var result = dialog.ShowDialog();

            if (result != DialogResult.OK)
                return;
 
            string curr_dic= Directory.GetCurrentDirectory()+"\\HTML";
            if(!Directory.Exists(curr_dic))
            {
                Directory.CreateDirectory(curr_dic);
            }
            
            Console.WriteLine(curr_dic);
            FileName = dialog.FileName;
            Url="HTML\\"+Path.GetFileName(FileName);
            
            if(Directory.Exists(dialog.FileName.Replace(Path.GetExtension(dialog.FileName), "")+"_files"))
            {
                DirectoryFiles=dialog.FileName.Replace(Path.GetExtension(dialog.FileName), "")+"_files";

                
            }
            else
            {
                DirectoryFiles=null;
                System.Windows.MessageBox.Show("Выберите директорию с файлами HTML страницы","Внимание");
            }
          



        });
        public ReactiveCommand<Unit, Unit> OpenDirectory => ReactiveCommand.Create(() => {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirectoryFiles=fbd.SelectedPath;
            }
        });
        public AddGuide()
        {
            MainGuideId=1;
            Name="Меню";
            DirectoryFiles="";
        }
        
    }
}
