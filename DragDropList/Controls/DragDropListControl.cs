using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DragDropList.Controls
{
    internal class DragDropListControl : Control, INotifyPropertyChanged
    {
        static DragDropListControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragDropListControl), new FrameworkPropertyMetadata(typeof(DragDropListControl)));
        }

        public DragDropListControl()
        {
            AllowDrop = true;
            Drop += DraggableItemControl_Drop;
            DragOver += DraggableItemControl_DragOver;
        }

        private void DraggableItemControl_DragOver(object sender, DragEventArgs e)
        {
        }

        private void DraggableItemControl_Drop(object sender, DragEventArgs e)
        {
        }

        private ListBox listBox;

        public ObservableCollection<DraggableControlBase> ItemsSource
        {
            get { return (ObservableCollection<DraggableControlBase>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<DraggableControlBase>), typeof(DragDropListControl), new PropertyMetadata(null));

        public override void OnApplyTemplate()
        {
            listBox = Template.FindName("PART_DragDropList", this) as ListBox;
            base.OnApplyTemplate();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
