using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EMessageBoard.Views
{
    /// <summary>
    /// Interaction logic for WriteModeBoard.xaml
    /// </summary>
    public partial class WriteModeBoard : UserControl
    {
        public WriteModeBoard()
        {
            InitializeComponent();
            EMessageBoard.Helpers.DateTimeHelper.timeUpdater(signTimeLbl);
        }

        /// <summary>
        /// This is Prototype  -> Convert this to MVVM Binding
        /// </summary>
        bool isDrawingLine                                              = true;
        bool isColorPickerLeft                                          = true;
        System.Windows.Media.Animation.DoubleAnimation moveRight;
        System.Windows.Media.Animation.DoubleAnimation fadeIn           = new System.Windows.Media.Animation.DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.3)));
        System.Windows.Media.Animation.DoubleAnimation fadeOut          = new System.Windows.Media.Animation.DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.3)));
        //private static List<System.Windows.Ink.Stroke> _undoStack       = new List<System.Windows.Ink.Stroke>();
        //private static List<System.Windows.Ink.Stroke> _redoStack       = new List<System.Windows.Ink.Stroke>();
        Dictionary<System.Windows.Ink.Stroke, bool> _undoStack          = new Dictionary<System.Windows.Ink.Stroke, bool>();
        Dictionary<System.Windows.Ink.Stroke, bool> _redoStack          = new Dictionary<System.Windows.Ink.Stroke, bool>();

        private static List<EMessageBoard.Models.UndoStack> _undoStackModel = new List<EMessageBoard.Models.UndoStack>();
        private static List<EMessageBoard.Models.RedoStack> _redoStackModel = new List<EMessageBoard.Models.RedoStack>();

        #region TOOLBOX CONTROLS
        private void DrawLine_TouchDown(object sender, TouchEventArgs e)
        {
            if (!isDrawingLine)
            {
                isDrawingLine = true;
                ink.EditingMode = InkCanvasEditingMode.Ink;
            }
            MoveActiveTool(Convert.ToInt16((e.Source as Image).Tag));
            AnimateToolOptions(false, false);
        }

        private void Eraser_TouchDown(object sender, TouchEventArgs e)
        {
            if (isDrawingLine)
            {
                isDrawingLine = false;
                ink.EditingMode = InkCanvasEditingMode.EraseByStroke;
            }
            MoveActiveTool(Convert.ToInt16((e.Source as Image).Tag));
            AnimateToolOptions(false, false);
        }

        private void selectedColorBtn_Click(object sender, RoutedEventArgs e)
        {
            AnimateToolOptions(true, false);
            MoveActiveTool(Convert.ToInt16((e.Source as Button).Tag));
        }

        /// <summary>
        /// Set isColorActive if Coloroption open,
        /// Set isThicnessActive if thicness option is open
        /// </summary>
        /// <param name="isColorActive"></param>
        /// <param name="isThicnessActive"></param>
        private void AnimateToolOptions(bool isColorActive, bool isThicnessActive)
        {
            if (!isColorActive && !isThicnessActive)
            {
                System.Windows.Media.Animation.DoubleAnimation moveLeft = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(colorPickerBtn), 10, new Duration(TimeSpan.FromSeconds(0.3)));
                moveLeft.Completed += moveLeft_Completed;
                colorPickerBtn.BeginAnimation(Canvas.LeftProperty, moveLeft);
                isColorPickerLeft = true;

                System.Windows.Media.Animation.DoubleAnimation moveLeft2 = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(StrokePickerBorder), 6, new Duration(TimeSpan.FromSeconds(0.3)));
                StrokePickerBorder.BeginAnimation(Canvas.LeftProperty, moveLeft2);
            }
            else
            {
                if (isColorActive)
                {
                    if (isColorPickerLeft)
                    {
                        colorPickerBtn.Opacity = 0.7;
                        moveRight = new System.Windows.Media.Animation.DoubleAnimation(50, 90, new Duration(TimeSpan.FromSeconds(0.3)));
                        colorPickerBtn.BeginAnimation(Canvas.LeftProperty, moveRight);
                        isColorPickerLeft = false;
                    }
                    else
                    {
                        System.Windows.Media.Animation.DoubleAnimation moveLeft = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(colorPickerBtn), 10, new Duration(TimeSpan.FromSeconds(0.3)));
                        moveLeft.Completed += moveLeft_Completed;
                        colorPickerBtn.BeginAnimation(Canvas.LeftProperty, moveLeft);
                        isColorPickerLeft = true;
                    }
                    System.Windows.Media.Animation.DoubleAnimation moveLeft2 = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(StrokePickerBorder), 6, new Duration(TimeSpan.FromSeconds(0.3)));
                    StrokePickerBorder.BeginAnimation(Canvas.LeftProperty, moveLeft2);
                }
                if (isThicnessActive)
                {
                    if (Canvas.GetLeft(StrokePickerBorder) == 90)
                    {
                        System.Windows.Media.Animation.DoubleAnimation moveLeft = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(StrokePickerBorder), 6, new Duration(TimeSpan.FromSeconds(0.3)));
                        StrokePickerBorder.BeginAnimation(Canvas.LeftProperty, moveLeft);
                    }
                    else 
                    {
                        System.Windows.Media.Animation.DoubleAnimation moveLeft = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(StrokePickerBorder), 90, new Duration(TimeSpan.FromSeconds(0.3)));
                        StrokePickerBorder.BeginAnimation(Canvas.LeftProperty, moveLeft);
                    }
                    System.Windows.Media.Animation.DoubleAnimation moveLeft2 = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(colorPickerBtn), 10, new Duration(TimeSpan.FromSeconds(0.3)));
                    moveLeft2.Completed += moveLeft_Completed;
                    colorPickerBtn.BeginAnimation(Canvas.LeftProperty, moveLeft2);
                    isColorPickerLeft = true;
                }
            }
        }
        void moveLeft_Completed(object sender, EventArgs e)
        {
            colorPickerBtn.Opacity = 0;
        }

        private void Canvas_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            ink.DefaultDrawingAttributes.Color = (Color)ColorConverter.ConvertFromString(btn.Name);
            selectedColorBtn.Background = btn.Background;
            AnimateToolOptions(true, false);
            MoveActiveTool(Convert.ToInt16(DrawLine.Tag));
            isDrawingLine = true;
            ink.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void strokeSize_TouchDown(object sender, TouchEventArgs e)
        {
            MoveActiveTool(Convert.ToInt16((e.Source as Image).Tag));
            AnimateToolOptions(false, true);
        }
        private void StackPanel_TouchUp(object sender, TouchEventArgs e)
        {
            Image img = e.Source as Image;
            switch (img.Name)
            {
                case "thictness_1":
                    SetThicknessSize(1, img);
                    break;
                case "thictness_5":
                    SetThicknessSize(5, img);
                    break;
                case "thictness_10":
                    SetThicknessSize(10, img);
                    break;
            }
        }
        private void SetThicknessSize(int i, Image img)
        {
            ink.DefaultDrawingAttributes.Width = i;
            ink.DefaultDrawingAttributes.Height = i;
            strokeSize.Source = img.Source;
            AnimateToolOptions(false, false);
            MoveActiveTool(Convert.ToInt16(DrawLine.Tag));
            isDrawingLine = true;
            ink.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void UndoImg_TouchDown(object sender, TouchEventArgs e)
        {
            AnimateToolOptions(false, false);
            MoveActiveTool(Convert.ToInt16((e.Source as Image).Tag));
            UndoRedoCommand(true);
        }

        private void RedoImg_TouchDown(object sender, TouchEventArgs e)
        {
            AnimateToolOptions(false, false);
            MoveActiveTool(Convert.ToInt16((e.Source as Image).Tag));
            UndoRedoCommand(false);
        }
        private void UndoRedoCommand(bool isUndo)
        {
            try
            {
                if (isUndo)
                {
                    Models.RedoStack redo = new Models.RedoStack();
                    redo.isErase = _undoStackModel.Last().isErase;
                    redo.stroke = _undoStackModel.Last().stroke;
                    _redoStackModel.Add(redo);
                    if (!_undoStackModel.Last().isErase)
                        ink.Strokes.Remove(_undoStackModel.Last().stroke);
                    else
                        ink.Strokes.Add(_undoStackModel.Last().stroke);
                    _undoStackModel.Remove(_undoStackModel.Last());
                }
                else
                {
                    Models.UndoStack undo = new Models.UndoStack();
                    undo.isErase = _redoStackModel.Last().isErase;
                    undo.stroke = _redoStackModel.Last().stroke;
                    _undoStackModel.Add(undo);
                    if (_undoStackModel.Last().isErase)
                        ink.Strokes.Remove(_undoStackModel.Last().stroke);
                    else
                        ink.Strokes.Add(_undoStackModel.Last().stroke);
                    _redoStackModel.Remove(_redoStackModel.Last());
                }
            }
            catch (Exception e)
            { 
                //Throw error message
            }
        }
        
        #region ActiveToolAnimation
        private void MoveActiveTool(int i)
        {
            System.Windows.Media.Animation.DoubleAnimation MoveUpAndDown = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetTop(ActiveToolBg), i, new Duration(TimeSpan.FromSeconds(0.2)));
            ActiveToolBg.BeginAnimation(Canvas.TopProperty, MoveUpAndDown);
        }
        #endregion

        #endregion

        private void ink_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
          // _undoStack.Add(e.Stroke, true);
           Models.UndoStack undo = new Models.UndoStack();
           undo.isErase = false;
           undo.stroke = e.Stroke;
           _undoStackModel.Add(undo);
        }

        private void ink_StrokeErasing(object sender, InkCanvasStrokeErasingEventArgs e)
        {
           // _undoStack.Add(e.Stroke, true);
            Models.UndoStack undo = new Models.UndoStack();
            undo.isErase = true;
            undo.stroke = e.Stroke;
            _undoStackModel.Add(undo);
        }

        bool isOpenToolBox = true;
        private void PanelImg_TouchUp(object sender, TouchEventArgs e)
        {
            if (isOpenToolBox)
            {
                closePanelImg.Visibility = Visibility.Hidden;
                showPanelImg.Visibility = Visibility.Visible;
                StrokePickerBorder.Visibility = Visibility.Hidden;
                colorPickerBtn.Visibility = Visibility.Hidden;
                System.Windows.Media.Animation.DoubleAnimation moveLeftPanel = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(toolBarCanvas), -100, new Duration(TimeSpan.FromSeconds(0.2)));
                toolBarCanvas.BeginAnimation(Canvas.LeftProperty, moveLeftPanel);
                isOpenToolBox = false; 
            }
            else
            {
                showPanelImg.Visibility = Visibility.Hidden;
                closePanelImg.Visibility = Visibility.Visible;
                System.Windows.Media.Animation.DoubleAnimation moveLeftPanel = new System.Windows.Media.Animation.DoubleAnimation(Canvas.GetLeft(toolBarCanvas), 0, new Duration(TimeSpan.FromSeconds(0.2)));
                moveLeftPanel.Completed += moveLeftPanel_Completed;
                toolBarCanvas.BeginAnimation(Canvas.LeftProperty, moveLeftPanel);
                isOpenToolBox = true;
            }
        }

        void moveLeftPanel_Completed(object sender, EventArgs e)
        {
            StrokePickerBorder.Visibility = Visibility.Visible;
            colorPickerBtn.Visibility = Visibility.Visible;
        }

        private void submitSignature_TouchUp(object sender, TouchEventArgs e)
        {
            if (ink.Strokes.Count() >= 1)
            {
                signatureCanvas.Visibility = Visibility.Visible;
                string[] files = System.IO.Directory.GetFiles(@"images\", "*");
                EMessageBoard.Converters.ControlToImage.SaveCanvas(this, this.ink, 96, @"images\" + (files.Length + 1) + ".png");
                signatureCanvas.Visibility = Visibility.Hidden;

                canvasSuccessOverlay.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Show ViewMode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewSignature_TouchUp(object sender, TouchEventArgs e)
        {
            createNewSignature();
            MainWindow main = App.Current.MainWindow as MainWindow;
            main.canvasContent.Children.Clear();
            main.canvasContent.Children.Add(new ViewModeBoard());
        }

        private void createNewSignature()
        {
            _undoStackModel.Clear();
            _redoStackModel.Clear();
            ink.Strokes.Clear();
        }

        private void createSignatureBtn_TouchUp(object sender, TouchEventArgs e)
        {
            createNewSignature();
            canvasSuccessOverlay.Visibility = Visibility.Hidden;
        }

    }
}
