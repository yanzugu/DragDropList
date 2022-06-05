using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace DragDropList.Controls
{
    internal class DraggableControlBase : Control, INotifyPropertyChanged
    {
        public Rectangle ReorderLine { get; set; }

        public DraggableControlBase(string title)
        {
            Title = title;
            MouseMove += DraggableControlBase_MouseMove;
            MouseUp += DraggableControlBase_MouseUp;
            Drop += DraggableControlBase_Drop;
            DragOver += DraggableControlBase_DragOver;
            DragLeave += DraggableControlBase_DragLeave;
        }

        public override void OnApplyTemplate()
        {
            ReorderLine = Template.FindName("PART_ReorderLine", this) as Rectangle;

            if (ReorderLine is not null)
            {
                ReorderLine.Visibility = Visibility.Collapsed;
            }

            base.OnApplyTemplate();
        }

        protected virtual void DraggableControlBase_DragLeave(object sender, DragEventArgs e)
        {
            var position = PointToScreen(new Point(0, 0));
            var mousePosition = Utilities.Utility.GetMousePosition();

            if (this is DraggableListControl ||
                mousePosition.X <= position.X ||
                mousePosition.Y <= position.Y ||
                mousePosition.X >= position.X + ActualWidth ||
                mousePosition.Y >= position.Y + ActualHeight)
            {
                HideReorderLine();
            }
        }

        protected virtual void DraggableControlBase_DragOver(object sender, DragEventArgs e)
        {
            var centerPosY = PointToScreen(new Point(0, 0)).Y + ActualHeight / 2d;
            var mousePosY = Utilities.Utility.GetMousePosition().Y;

            if (ReorderLine is not null)
            {
                // button
                if (mousePosY >= centerPosY)
                {
                    ReorderLine.VerticalAlignment = VerticalAlignment.Bottom;
                }
                else // top
                {
                    ReorderLine.VerticalAlignment = VerticalAlignment.Top;
                }
                ReorderLine.Visibility = Visibility.Visible;
            }

            e.Handled = true;
        }

        protected virtual void DraggableControlBase_Drop(object sender, DragEventArgs e)
        {
            HideReorderLine();
            var source = e.Data.GetData(DataFormats.Serializable) as DraggableControlBase;

            if (source is not null)
            {
                source.MoveTo(this);
            }

            e.Handled = true;
        }

        protected void HideReorderLine()
        {
            if (ReorderLine is not null)
            {
                Task.Run(() =>
                {
                    SpinWait.SpinUntil(() => false, 5);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ReorderLine.Visibility = Visibility.Collapsed;
                    });
                });
            }
        }

        protected virtual void DraggableControlBase_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        protected virtual void DraggableControlBase_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed &&
                sender is DraggableControlBase draggableBase)
            {
                DragDrop.DoDragDrop(draggableBase, new DataObject(DataFormats.Serializable, draggableBase), DragDropEffects.Move);
            }
        }

        public DraggableListControl? ParentControl { get; set; }
        public ObservableCollection<DraggableControlBase> ParentList { get; set; }
        public string Title { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    internal static class Extension
    {
        internal static void MoveTo(this DraggableControlBase source, DraggableControlBase target)
        {
            if (target == source) return;

            int insertIndex = target.ParentList.IndexOf(target);

            if (target.ReorderLine is not null)
            {
                if (source.ParentList == target.ParentList)
                {
                    int sourceIndex = source.ParentList.IndexOf(source);

                    if (sourceIndex < insertIndex)
                        --insertIndex;
                }

                if (target.ReorderLine.VerticalAlignment == VerticalAlignment.Bottom)
                    ++insertIndex;
            }

            source.ParentList.Remove(source);
            source.ParentList = target.ParentList;

            target.ParentList.Insert(insertIndex, source);

            if (target is DraggableListControl)
            {
                source.ParentControl = (DraggableListControl)target;
            }
        }
    }
}
