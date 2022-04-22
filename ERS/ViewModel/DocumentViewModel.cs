using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ERS.ViewModel
{
    public class DocumentViewModel:ReactiveObject
    {
        [Reactive]
        public Model.AddDocument NewDocument { get; set; }
        [Reactive] 
        public ObservableCollection<Model.Document>  Documents { get; set; }    
        [Reactive]
        public string Find { get; set; }
        public DocumentViewModel()
        {
            Find="";
            Documents =Model.Document.GetDocuments("");
        }
        public ReactiveCommand<Unit, Unit> OpenNewDocument => ReactiveCommand.Create(() => {
            View.Document.AddDocument adddoc = new View.Document.AddDocument();
            adddoc.DataContext=this;
            NewDocument=new Model.AddDocument();
            adddoc.Show();
        });
        public ReactiveCommand<Unit, Unit> AddNewDocument => ReactiveCommand.Create(() => {
            Model.Document.AddDocument(NewDocument);
            Documents =Model.Document.GetDocuments(Find);
            NewDocument=new Model.AddDocument();

        });
        public ReactiveCommand<string, Unit> FindDocument => ReactiveCommand.Create<string>(FindGuideCommand);
        private void FindGuideCommand(string find)
        {
            if (string.IsNullOrEmpty(find))
                Documents =Model.Document.GetDocuments("");
            else
                Documents =Model.Document.GetDocuments(find);
        }
        public ReactiveCommand<Model.Document, Unit> DeleteDocument => ReactiveCommand.Create<Model.Document>(DeleteDocumentCommand);
        private void DeleteDocumentCommand(Model.Document doc)
        {
            if (doc == null)
                return;
            Model.Document.DeleteDocument(doc);
            Documents =Model.Document.GetDocuments(Find);
        }
    }
}
