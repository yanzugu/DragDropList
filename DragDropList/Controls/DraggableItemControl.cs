using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DragDropList.Controls
{
    internal class DraggableItemControl : DraggableControlBase
    {
        static DraggableItemControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DraggableItemControl), new FrameworkPropertyMetadata(typeof(DraggableItemControl)));
        }

        public DraggableItemControl(string title) : base(title)
        {
            AllowDrop = true;
        }
    }
}
