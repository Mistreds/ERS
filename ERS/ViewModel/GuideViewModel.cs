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
    public class GuideViewModel : ReactiveObject
    {
        [Reactive]
        public ObservableCollection<Model.Guide> Guides { get; set; }
        [Reactive]
        public ObservableCollection<Model.Guide> GuidCombo { get; set; }
        [Reactive]
        public Model.Guide SelectedGuide { get; set; }
        [Reactive]
        public Model.AddGuide NewGuide { get; set; }
        public GuideViewModel()
        {
            SelectAllGuide();
        }
        private void SelectAllGuide()
        {
            Guides=Model.Guide.GetGuides();
            GuidCombo=Model.Guide.GetGuidesForCombo();
        }
        public ReactiveCommand<Model.Guide, Unit> SelectGuide => ReactiveCommand.Create<Model.Guide>(SelectGuideCommand);
        private void SelectGuideCommand(Model.Guide guide)
        { if (guide==null)
                return;
            SelectedGuide=guide;
        }
        public ReactiveCommand<Unit, Unit> AddNewGuide => ReactiveCommand.Create(() => { View.Guide.AddGuide addGuide = new View.Guide.AddGuide();
            addGuide.DataContext=this;
            NewGuide=new Model.AddGuide();
            addGuide.Show();
        });
        public ReactiveCommand<Unit, Unit> AddNewGuideCommand => ReactiveCommand.Create(() => {
            Model.Guide.AddGuides(NewGuide);
            NewGuide=new Model.AddGuide();
            SelectAllGuide();
        });
        public ReactiveCommand<Model.Guide, Unit> DeleteGuid => ReactiveCommand.Create<Model.Guide>(DeleteGuidCommand);
        private void DeleteGuidCommand(Model.Guide guid)
        {
            Model.Guide.DeleteGuide(guid);
            SelectAllGuide();

        }
        

    }
}
