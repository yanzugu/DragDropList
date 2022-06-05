using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DragDropList.Controls
{
    internal class DraggableListControl : DraggableControlBase
    {
        TextBlock expandButton;
        ListBox listBox;

        static DraggableListControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DraggableListControl), new FrameworkPropertyMetadata(typeof(DraggableListControl)));
        }

        public DraggableListControl(string title) : base(title)
        {
            ItemsSource = new();
            IsExpanded = true;
        }

        public void Add(DraggableControlBase item)
        {
            item.ParentList = ItemsSource;
            item.ParentControl = this;
            ItemsSource.Add(item);
        }

        protected override void DraggableControlBase_DragLeave(object sender, DragEventArgs e)
        {
            if (expandButton is not null)
            {
                var position = expandButton.PointToScreen(new Point(0, 0));
                var mousePosition = Utilities.Utility.GetMousePosition();

                if (mousePosition.X <= position.X ||
                    mousePosition.Y <= position.Y ||
                    mousePosition.X >= position.X + expandButton.Width ||
                    mousePosition.Y >= position.Y + expandButton.ActualHeight)
                {
                    HideReorderLine();
                }
            }
        }

        public override void OnApplyTemplate()
        {
            expandButton = Template.FindName("PART_ExpandButton", this) as TextBlock;
            listBox = Template.FindName("PART_List", this) as ListBox;

            if (expandButton is not null)
            {
                expandButton.AllowDrop = true;
                expandButton.MouseUp += ExpandButton_MouseUp;
            }

            if (listBox is not null)
            {
                listBox.AllowDrop = true;
                listBox.Drop += ListBox_Drop;
            }

            base.OnApplyTemplate();
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            HideReorderLine();
            var data = e.Data.GetData(DataFormats.Serializable) as DraggableControlBase;
            if (data is not null && data.ParentControl != this)
            {
                data.ParentList.Remove(data);
                ItemsSource.Add(data);
                data.ParentControl = this;
                data.ParentList = ItemsSource;
            }
            e.Handled = true;
        }

        private void ExpandButton_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IsExpanded = !IsExpanded;
        }

        public bool IsExpanded { get; set; }


        public ObservableCollection<DraggableControlBase> ItemsSource
        {
            get { return (ObservableCollection<DraggableControlBase>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<DraggableControlBase>), typeof(DraggableListControl), new PropertyMetadata(null));


    }
}
