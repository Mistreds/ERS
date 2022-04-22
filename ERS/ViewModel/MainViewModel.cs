using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ERS.ViewModel
{
    public delegate void OpenPageDelegate(UserControl userControl);
    public delegate void OpenPageId(int id);
    public class MainViewModel:ReactiveObject
    {
        private GuideViewModel guideViewModel;
        public static PictureViewModel pictureViewModel { get; private set; }   
        public static VideoViewModel videoViewModel { get; private set; }
        private DocumentViewModel documentViewModel;
        private ObservableCollection<UserControl> _controls { get; set; }

        
        [Reactive]
        public UserControl MainControl { get; set; }
        private OpenPageDelegate OpenPageDelegate { get; set; }
        private OpenPageId openPageId { get; set; }
        public MainViewModel()
        {
            guideViewModel = new GuideViewModel();
            OpenPageDelegate=OpenPageCommandUser;
            openPageId= OpenPageCommand;
            pictureViewModel = new PictureViewModel(OpenPageDelegate, openPageId);
            videoViewModel = new VideoViewModel(OpenPageDelegate,openPageId);
            documentViewModel=new DocumentViewModel();
            _controls = new ObservableCollection<UserControl> { new View.Welcome() , new View.Guide.MainGuide(guideViewModel), new View.Picture.PictureMain(pictureViewModel), new View.Video.VideoMain(videoViewModel),new View.Document.MainDocument(documentViewModel)};
            MainControl=_controls[0];
        }


        public ReactiveCommand<string, Unit> OpenPage => ReactiveCommand.Create<string>(OpenPageCommand);
        private void OpenPageCommand(string id)
        {
            MainControl=_controls[Convert.ToInt32(id)];
        }
        private void OpenPageCommand(int id)
        {
            MainControl=_controls[id];
        }
        private void OpenPageCommandUser(UserControl userControl)
        {
            MainControl=userControl;
        }
    }
}
