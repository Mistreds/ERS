using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ERS.Model
{
    public class GridActivity<T> : ReactiveObject
    {
        [Reactive]
        public T Data { get; set; }
        [Reactive]
        public int Id { get; set; }
        [Reactive]
        public UserControl Control { get; set; }

        public GridActivity(T Data, int id, UserControl control)
        {
            this.Data = Data;
            this.Id = id;
            this.Control = control;
            control.DataContext=Data;
        }

    }
}
