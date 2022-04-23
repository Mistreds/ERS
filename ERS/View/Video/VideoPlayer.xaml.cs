using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ERS.View.Video
{
    /// <summary>
    /// Логика взаимодействия для VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private TimeSpan TotalTime;
        private bool _is_playing;
        public bool IsPlaying
        {
            get=>_is_playing;
            set
            {
                _is_playing= value;
                OnPropertyChanged();
            }
        }
        public VideoPlayer(Model.Video video)
        {
            ViewModel.MainViewModel.videoViewModel.IsPlaying=false;

            InitializeComponent();
            Media.MediaOpened+=Media_MediaOpened;
            Media.Source = new Uri(video.FilePath);
            slider.AddHandler(MouseLeftButtonUpEvent,
                       new MouseButtonEventHandler(timeSlider_MouseLeftButtonUp),
                       true);
            DataContext=ViewModel.MainViewModel.videoViewModel;
            
        }

        private void timeSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TotalTime.TotalSeconds > 0)
            {
                Media.Position =TimeSpan.FromSeconds(slider.Value);
            }
        }
       private DispatcherTimer timerVideoTime;
        private void Media_MediaOpened(object sender, RoutedEventArgs e)
        {
            slider.Maximum = Media.NaturalDuration.TimeSpan.TotalSeconds;
            TotalTime = Media.NaturalDuration.TimeSpan;

            // Create a timer that will update the counters and the time slider
             timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            timerVideoTime.Tick += new EventHandler(timer_Tick);
            timerVideoTime.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (Media.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                if (TotalTime.TotalSeconds > 0)
                {
                    // Updating time slider
                    slider.Value = Media.Position.TotalSeconds;
                }
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if(!ViewModel.MainViewModel.videoViewModel.IsPlaying)
            Media.Play();
            else
                Media.Pause();
            ViewModel.MainViewModel.videoViewModel.IsPlaying=!ViewModel.MainViewModel.videoViewModel.IsPlaying;
        }

       
        
        private void stop_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if(timerVideoTime!=null)
            timerVideoTime.Stop();
            IsPlaying=false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            BeginAnimation(OpacityProperty, Animation.StartOpacAnim());
           
        }
    }
}
