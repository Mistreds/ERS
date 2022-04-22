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
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ERS.Model
{
    public class Picture:ReactiveObject
    {
        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public string FileName { get; set; }
        [Reactive]
        public byte[] FileByte { get; set; }
        [Reactive]
        [NotMapped]
        public BitmapImage Image { get; set; }
        public static bool AddPicture(Picture picture)
        {
            using (var db = new ConnectDB())
            {
                if (picture.FileByte==null)
                {

                    System.Windows.Forms.MessageBox.Show("Не выбрано изображение","Ошибка");
                    return false;
                }
                    
                db.Picture.Add(picture);
                db.SaveChanges();
                return true;
            }
        }
        public static void DeletePicture(Picture picture)
        {
            if (picture==null) return;
            var dialogResult = System.Windows.MessageBox.Show($"Вы действительно хотите удалить {picture.Name}", "Внимание", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using (var db = new ConnectDB())
                {
                    db.Picture.Remove(picture);
                    db.SaveChanges();
                }
            }
        }
        public static ObservableCollection<Picture> GetPictures()
        {
            using (var db = new ConnectDB())
            {
                return new ObservableCollection<Picture>(db.Picture.Select(p => new Picture(p.Id, p.Name, p.FileName, p.FileByte)));
            }
        }
        public ReactiveCommand<Unit, Unit> SaveFile => ReactiveCommand.Create(() => {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = $"Файл {Path.GetExtension(FileName)} | *.{Path.GetExtension(FileName)} ";

            if (saveFileDialog1.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog1.FileName, FileByte);

            }
        });
       
    
        public Picture()
        {

        }
        public Picture(int id, string name, string file_name, byte[] file)
        {
            this.Id = id;
            this.Name=name;
            this.FileName = file_name;
            this.FileByte=file;
            if(file!=null)
            this.Image=ToImage(file);
        }
        private BitmapImage ToImage(byte[] array)//Делаем из потока байтов картинку
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.Rotation = Rotation.Rotate0;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat; 
                    image.EndInit();
                    return image;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
    public class AddPicture:Picture
    {
        public ReactiveCommand<Unit, Unit> OpenPicture => ReactiveCommand.Create(() => {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Файлы изображений|*.png;*.jpeg;*.jpg;*.bmp";
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            var image = System.Drawing.Image.FromFile(dialog.FileName);
            FileByte  =ImageToByte2(image);
            FileName = dialog.FileName;
            FileName=Path.GetFileName(FileName);
        });
        public AddPicture()
        {
          
        }
       private byte[] ImageToByte2(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}
