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
        [Reactive]
        public byte[] ByteFile { get; set; }
        [NotMapped]
        [Reactive]
        public System.Windows.Controls.UserControl Control { get; set; }
        public static ObservableCollection<Guide> GetGuides()
        {
            using (var db = new ConnectDB())
            {
                return new ObservableCollection<Guide>(db.Guide.Where(p=>p.MainGuideId==1 && p.Id!=1).Select(p=>new Guide(p.Id, p.Name,p.MainGuideId, p.Url, p.ByteFile)));
            }
            
        }
        public static ObservableCollection<Guide> GetGuidesFromFind(string Find)
        {
            using (var db = new ConnectDB())
            {

                return new ObservableCollection<Guide>(db.Guide.FromSqlInterpolated($"SELECT * FROM Guide where id!=1 and Name like lower({"%"+Find.ToLower()+"%"}) ").Select(p => new Guide(p.Id, p.Name, p.MainGuideId, p.Url, p.ByteFile)));
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
        public Guide()
        {


        }
        public Guide(int id, string name, int guide_id,string Url, byte[] bytes)
        {
            this.Id=id;
            this.Name=name;
            this.MainGuideId=guide_id;
            ByteFile= bytes;
            this.Url=Url;
            if(bytes!=null)
            { 
                var tmpFile = Path.GetTempPath(); ;

                var tmpFileStream = File.OpenWrite(tmpFile+"\\"+Url);
                tmpFileStream.Write(bytes, 0, bytes.Length);
                tmpFileStream.Close();
                Url=tmpFile+"\\"+Url;
            }
            
            this.Control=new View.Guide.WebPage(Url);
            using (var db = new ConnectDB())
            {
                Guides=new ObservableCollection<Guide>(db.Guide.Where(p => p.MainGuideId==id).Select(p => new Guide(p.Id, p.Name, p.MainGuideId, p.Url, p.ByteFile)));
            }
        }

    }
    public class AddGuide:Guide
    {
        [Reactive]
        public string FileName { get; set; }
        private bool _is_byte;
        public bool IsByte
        {
            get => _is_byte;
            set
            {
                FileName=null;
               
                ByteFile=null;
                this.RaiseAndSetIfChanged(ref _is_byte, value); 
            }
        }

        public ReactiveCommand<Unit, Unit> OpenFile => ReactiveCommand.Create(() => {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Файлы html|*.html";
            var result = dialog.ShowDialog();

            if (result != DialogResult.OK)
                return;
            ByteFile = File.ReadAllBytes(dialog.FileName);
            FileName = dialog.FileName;
            Url=Path.GetFileName(FileName);

        });
        public AddGuide()
        {
            MainGuideId=1;
            Name="Меню";
        }
        
    }
}
