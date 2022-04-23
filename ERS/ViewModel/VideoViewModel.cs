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
    public class VideoViewModel : ReactiveObject
    {
        [Reactive]
        public ObservableCollection<Model.GridActivity<Model.Video>> VideoControl { get; set; }
        private OpenPageDelegate OpenPageDelegate { get; set; }
        private OpenPageId openPageId { get; set; }
        [Reactive]
        public Model.AddVideo NewVideo { get; set; }
        [Reactive]
        public int Scale { get; set; }
        [Reactive]
        public int ScaleGrid { get; set; }
        [Reactive]
        public bool IsPlaying { get; set; } 
        public VideoViewModel(OpenPageDelegate OpenPageDelegate, OpenPageId openPageId)
        {
            this.openPageId = openPageId;
            this.OpenPageDelegate = OpenPageDelegate;
            Scale=7;
            this.WhenAnyValue(p => p.Scale).Subscribe(_ => UpdateScaleGrid());
            GetVideo();
        }
        public ReactiveCommand<Unit, Unit> OpenNewVideo => ReactiveCommand.Create(() => {
            View.Video.AddVideo addvideo = new View.Video.AddVideo();
            addvideo.DataContext=this;
            NewVideo=new Model.AddVideo();
            addvideo.Show();
        });
        private void UpdateScaleGrid()
        {
            ScaleGrid=10-Scale;
        }
        public void SelectVideo(Model.Video video)
        {
            var vid =Model.Video.GetVideoFile(video);
            View.Video.VideoPlayer videoPlayer = new View.Video.VideoPlayer(vid);
            videoPlayer.DataContext=this;
            OpenPageDelegate(videoPlayer);
        }
        public void GetVideo()
        {
            var video = Model.Video.GetVideo();
            VideoControl=new ObservableCollection<Model.GridActivity<Model.Video>>();
            foreach (var vid in video)
            {
                VideoControl.Add(new Model.GridActivity<Model.Video>(vid, vid.Id, new View.Video.VideoOnGrid()));
            }
        }
        public ReactiveCommand<Unit, Unit> AddNewVideo => ReactiveCommand.Create(() => {
            if (!Model.Video.AddVideo(NewVideo))
                return;
            GetVideo();
            NewVideo =new Model.AddVideo();
        });
    }
}
