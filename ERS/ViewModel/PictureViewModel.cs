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

    public class PictureViewModel:ReactiveObject
    {
        [Reactive]
        public Model.AddPicture NewPicture { get; set; }
        [Reactive]
        public ObservableCollection<Model.GridActivity<Model.Picture>> PictureControl { get; set; }
        [Reactive]
        public int Scale { get; set; }
        [Reactive]
        public int ScaleGrid { get; set; }
        [Reactive]
        public Model.Picture SelectedPicture { get; set; }
        private OpenPageDelegate OpenPageDelegate { get; set; }
        private OpenPageId openPageId { get; set; }
        public PictureViewModel(OpenPageDelegate OpenPageDelegate, OpenPageId openPageId)
        {
            this.openPageId = openPageId;
            this.OpenPageDelegate = OpenPageDelegate;
            Scale=7;
            this.WhenAnyValue(p => p.Scale).Subscribe(_ => UpdateScaleGrid());
            GetPicture();
          
            
        }
        public ReactiveCommand<Unit, Unit> BackPage => ReactiveCommand.Create(() => { openPageId(2); });
        private void UpdateScaleGrid()
        {
            ScaleGrid=10-Scale;
        }
        public void GetPicture()
        {
            var picture = Model.Picture.GetPictures();
            PictureControl=new ObservableCollection<Model.GridActivity<Model.Picture>>();
            foreach (var pic in picture)
            {
                PictureControl.Add(new Model.GridActivity<Model.Picture>( pic, pic.Id, new View.Picture.PictureOnGrid()) );
            }
        }
        public void SelectPicture(Model.Picture picture)
        {
            SelectedPicture=picture;
            View.Picture.PictureOnScreen pictureOnScreen = new View.Picture.PictureOnScreen();
            pictureOnScreen.DataContext=this;
            OpenPageDelegate(pictureOnScreen);
        }
        public void Back()
        {
            openPageId(3);
        }
        public ReactiveCommand<Unit, Unit> OpenNewPicture => ReactiveCommand.Create(() => {
            View.Picture.AddPicture addPicture = new View.Picture.AddPicture();
            addPicture.DataContext=this;
            NewPicture=new Model.AddPicture();
            addPicture.Show();
        });
        public ReactiveCommand<Unit, Unit> AddNewPicture => ReactiveCommand.Create(() => {
            Model.Picture.AddPicture(NewPicture);
            GetPicture();
            NewPicture=new Model.AddPicture();  
        });
    }
}
