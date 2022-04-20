using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERS.ViewModel
{
    public class GuideViewModel:ReactiveObject
    {
        [Reactive]
        public ObservableCollection<Model.Guide> Guides { get; set; }
        public GuideViewModel()
        {
            Guides=Model.Guide.GetGuides();
           foreach(var guide in Guides)
            {
                Console.WriteLine(guide.Guides.Count);
            }
        }
    }
}
