using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ERS.Model
{
    public class DocFile:ReactiveObject
    {
        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public byte[] FileByte { get; set; }
    }
    public class Document:ReactiveObject
    {
        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public string FileName { get; set; }
        [Reactive]
        public int DocFileId { get; set; }
        [Reactive]
        public DocFile DocFile { get; set; }
        public static void AddDocument(Document doc)
        {
            using (var db=new ConnectDB())
            {
                db.Document.Add(doc);
                db.SaveChanges();
            }
        }
        public static ObservableCollection<Document> GetDocuments(string Find)
        {
            using (var db = new ConnectDB())
            {
              return new ObservableCollection<Document>(db.Document.FromSqlInterpolated($"select * from document where Name like lower({"%"+Find.ToLower()+"%"}) ").ToList());
            }
        }
        public static void DeleteDocument(Document Document)
        {
            if (Document==null) return;
            var dialogResult = System.Windows.MessageBox.Show($"Вы действительно хотите удалить {Document.Name}", "Внимание", MessageBoxButton.YesNo);
            if (dialogResult == System.Windows.MessageBoxResult.Yes)
            {
                using (var db = new ConnectDB())
                {
                    db.Document.Remove(Document);
                    db.DocFile.RemoveRange(db.DocFile.Where(p => p.Id==Document.DocFileId));
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
                    File.WriteAllBytes(saveFileDialog1.FileName, db.DocFile.Where(p => p.Id==DocFileId).Select(p => p.FileByte).FirstOrDefault());

                }
            }
        });
        public ReactiveCommand<Unit, Unit> OpenFile => ReactiveCommand.Create(() => {
            Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            using (var db = new ConnectDB())
            {
                DocFile=db.DocFile.Where(p => p.Id==DocFileId).FirstOrDefault();
            }
            var tmpFile = Path.GetTempFileName();
            var tmpFileStream = File.OpenWrite(tmpFile);
            tmpFileStream.Write(DocFile.FileByte, 0, DocFile.FileByte.Length);
            tmpFileStream.Close();
            var document = application.Documents.Open(tmpFile,
                                       ReadOnly: false,
                                       ConfirmConversions: false,
                                       NoEncodingDialog: true);
            application.Visible = true;

        });
    }
    public class  AddDocument:Document
    {
        public AddDocument()
        {

        }
        public ReactiveCommand<Unit, Unit> OpenDocument => ReactiveCommand.Create(() =>
        {
            var dialog = new OpenFileDialog();
            DocFile=new DocFile();
            dialog.Filter = "Файлы Microsoft Word|*.doc;*.docx";
            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
                return;
            DocFile.FileByte = File.ReadAllBytes(dialog.FileName);
            FileName = dialog.FileName;
            FileName=Path.GetFileName(FileName);
        });

        }
}
