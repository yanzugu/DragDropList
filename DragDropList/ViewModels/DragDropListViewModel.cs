using DragDropList.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragDropList.ViewModels
{
    internal class DragDropListViewModel : INotifyPropertyChanged
    {
        public DragDropListViewModel()
        {
            DraggableControls = new();

            AddNewDraggable(new DraggableItemControl("A"));
            AddNewDraggable(new DraggableItemControl("B"));
            AddNewDraggable(new DraggableItemControl("C"));

            var a = new DraggableListControl("D");
            var b = new DraggableListControl("E");
            var c = new DraggableListControl("F");

            a.Add(new DraggableItemControl("D-2"));
            a.Add(new DraggableItemControl("D-3"));

            b.Add(new DraggableItemControl("E-1"));
            b.Add(new DraggableItemControl("E-2"));
            b.Add(new DraggableItemControl("E-3"));

            c.Add(new DraggableItemControl("F-1"));
            c.Add(new DraggableItemControl("F-2"));
            c.Add(new DraggableItemControl("F-2"));
            c.Add(new DraggableItemControl("F-4"));

            a.Add(b);

            AddNewDraggable(a);
            AddNewDraggable(c);
        }

        public DraggableControlBase AddNewDraggable(DraggableControlBase draggableControl)
        {
            draggableControl.ParentList = DraggableControls;
            DraggableControls.Add(draggableControl);
            return draggableControl;
        }

        public ObservableCollection<DraggableControlBase> DraggableControls { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
