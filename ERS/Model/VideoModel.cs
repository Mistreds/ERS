using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ERS.Model
{

    public class VideoFile : ReactiveObject
    {
        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public byte[] FileByte { get; set; }
    }

   public class Video:ReactiveObject
    {
        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public string FileName { get; set; }
        [Reactive]
        public int VideoFileId { get; set; }
        [Reactive]
        public VideoFile VideoFile { get; set; }
        [Reactive]
        public byte[] ImagePreview { get; set; }
        [Reactive]
        [NotMapped]
        public BitmapImage Image { get; set; }
        [Reactive]
        [NotMapped]
        public string FilePath { get; set; }
        public static bool AddVideo(Video video)
        {
            using (var db= new ConnectDB())
            {
                if(video.VideoFile.FileByte==null)
                {

                    System.Windows.MessageBox.Show("Ошибка, не выбран файл","Ошибка");
                    return false;
                }
                try
                {
                   
                    db.Video.Add(video);
                    db.SaveChanges(); Console.WriteLine(video.VideoFile.FileByte.Count());
                    db.VideoFile.FromSqlInterpolated($"update VideoFile set FileByte={video.VideoFile.FileByte} where  id = {video.VideoFile.Id}");
                    
                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }
                return true;
            }
        }
        public static ObservableCollection<Video> GetVideo()
        {
            using (var db = new ConnectDB())
            {
                return new ObservableCollection<Video>(db.Video.Select(p => new Video(p.Id, p.Name, p.FileName, p.ImagePreview,p.VideoFileId)));
            }
        }
        public static Video GetVideoFile(Video video)
        {
            using (var db = new ConnectDB())
            {
                var vid= db.Video.Where(p=>p.Id==video.VideoFileId).Include(p=>p.VideoFile).FirstOrDefault();
                var tmpFile = Path.GetTempPath(); ;

                var tmpFileStream = File.OpenWrite(tmpFile+"\\"+vid.FileName);
                tmpFileStream.Write(vid.VideoFile.FileByte, 0, vid.VideoFile.FileByte.Length);
                tmpFileStream.Close();
                vid.FilePath=tmpFile+"\\"+vid.FileName;
                return vid;
            }
        }
        public Video() { }
        public Video(int id, string name, string file_name, byte[] file, int videp_id)
        {
            this.Id = id;
            this.Name=name;
            this.FileName = file_name;
            this.VideoFileId = videp_id;
            if(file!=null)
                this.Image=ToImage(file);
        }
        public static void DeleteVideo(Video Video)
        {
            if (Video==null) return;
            var dialogResult = System.Windows.MessageBox.Show($"Вы действительно хотите удалить {Video.Name}", "Внимание", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                using (var db = new ConnectDB())
                {
                    db.Video.Remove(Video);
                    db.VideoFile.RemoveRange(db.VideoFile.Where(p => p.Id==Video.VideoFileId));
                    db.SaveChanges();
                }
            }
        }
        public ReactiveCommand<Unit, Unit> SaveFile => ReactiveCommand.Create(() => {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = $"Файл {Path.GetExtension(FileName)} | *.{Path.GetExtension(FileName)} ";

            if (saveFileDialog1.ShowDialog() == true)
            {
                using (var db = new ConnectDB())
                {
                    File.WriteAllBytes(saveFileDialog1.FileName, db.VideoFile.Where(p => p.Id==VideoFileId).Select(p => p.FileByte).FirstOrDefault());

                }
                    

            }
        });
        protected BitmapImage ToImage(byte[] array)//Делаем из потока байтов картинку
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
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
    public class AddVideo:Video
    {
        public AddVideo()
        {

        }
        public ReactiveCommand<Unit, Unit> OpenVideo => ReactiveCommand.Create(() => {
            VideoFile=new VideoFile();
            var dialog = new OpenFileDialog();
            dialog.Filter = "Файлы avi|*.avi|Файлы mp4|*.mp4";
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            VideoFile.FileByte  = File.ReadAllBytes(dialog.FileName);
            FileName = dialog.FileName;
            FileName=Path.GetFileName(FileName);
            System.Windows.Media.MediaPlayer mediaPlayer = new System.Windows.Media.MediaPlayer();
            mediaPlayer.MediaOpened += new EventHandler(HandleMediaPlayerMediaOpened);
            mediaPlayer.ScrubbingEnabled = true;
          

            
            mediaPlayer.Open(new Uri(dialog.FileName));
          mediaPlayer.Position=TimeSpan.FromSeconds(1);

          
        });

        private void HandleMediaPlayerMediaOpened(object sender, EventArgs e)
        {
            MediaPlayer mediaPlayer = sender as MediaPlayer;
            var framePixels = new uint[mediaPlayer.NaturalVideoWidth * mediaPlayer.NaturalVideoHeight];
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawVideo(mediaPlayer, new Rect(0, 0, mediaPlayer.NaturalVideoWidth, mediaPlayer.NaturalVideoHeight));
            drawingContext.Close();
            double dpiX = 1 / 200;
            double dpiY = 1 / 200;
            RenderTargetBitmap bmp = new RenderTargetBitmap( mediaPlayer.NaturalVideoWidth, mediaPlayer.NaturalVideoHeight, dpiX, dpiY, PixelFormats.Pbgra32);
            Console.WriteLine(bmp);
            bmp.Render(drawingVisual);
            bmp.CopyPixels(framePixels, mediaPlayer.NaturalVideoWidth * 4, 0);
            BitmapImage image = new BitmapImage();
            image.UriSource=mediaPlayer.Source;
            var bitmapEncoder = new JpegBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bmp));
            BitmapSource bitmapSource = bmp;
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                ImagePreview=ImageToByte2(System.Drawing.Image.FromStream(stream));
            }
            Image=ToImage(ImagePreview);
            System.Windows.MessageBox.Show("Загрузка видео завершена", "Внимание");
        }

        private byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        public static byte[] ImageToByte2(System.Drawing.Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
