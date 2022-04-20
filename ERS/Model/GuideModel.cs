using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERS.Model
{
    public class Guide:ReactiveObject
    {
        [Reactive]
        public int Id { get; set; } 

        [Reactive]
        public string Name { get; set; }
        [Reactive]
        public int MainGuideId { get; set; }
        [Reactive]
        public Guide MainGuide {get;set;}
        [Reactive]
        public ObservableCollection<Guide> Guides { get; set; }

        public static ObservableCollection<Guide> GetGuides()
        {
            using (var db = new ConnectDB())
            {
                return new ObservableCollection<Guide>(db.Guide.Where(p=>p.MainGuideId==1 && p.Id!=1).Select(p=>new Guide(p.Id, p.Name,p.MainGuideId)));
            }
            
        }
        public Guide()
        {


        }
        public Guide(int id, string name, int guide_id)
        {
            this.Id=id;
            this.Name=name;
            this.MainGuideId=guide_id;
            using (var db = new ConnectDB())
            {
                Guides=new ObservableCollection<Guide>(db.Guide.Where(p => p.MainGuideId==id).Select(p => new Guide(p.Id, p.Name, p.MainGuideId)));
            }
        }

    }
}
