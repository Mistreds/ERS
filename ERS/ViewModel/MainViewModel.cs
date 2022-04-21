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
    public class MainViewModel:ReactiveObject
    {
        private GuideViewModel guideViewModel;
        public static PictureViewModel pictureViewModel { get; private set; }   
        private ObservableCollection<UserControl> _controls { get; set; }

        
        [Reactive]
        public UserControl MainControl { get; set; }
        public MainViewModel()
        {
            guideViewModel = new GuideViewModel();
            pictureViewModel = new PictureViewModel();
            _controls = new ObservableCollection<UserControl> { new View.Welcome() , new View.Guide.MainGuide(guideViewModel), new View.Picture.PictureMain(pictureViewModel)};
            MainControl=_controls[0];
        }


        public ReactiveCommand<string, Unit> OpenPage => ReactiveCommand.Create<string>(OpenPageCommand);
        private void OpenPageCommand(string id)
        {
            MainControl=_controls[Convert.ToInt32(id)];
        }
    }
}
