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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EMessageBoard.Views
{
    /// <summary>
    /// Interaction logic for ImagePreview.xaml
    /// </summary>
    public partial class ImagePreview : UserControl
    {
        private DispatcherTimer idleTimer = new DispatcherTimer();

        public ImagePreview()
        {
            InitializeComponent();
            this.Loaded += Image_Loaded;
        }

        void Image_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard FlipIn = this.FindResource("FlipIn") as Storyboard;
            Storyboard.SetTarget(FlipIn, mainGrid);
            FlipIn.Completed += FlipIn_Completed;
            FlipIn.Begin();
        }

        void FlipIn_Completed(object sender, EventArgs e)
        {
            mainGrid.RenderTransform = FindResource("InitialMatrixTransform") as MatrixTransform;
        }

        public void UpdateSource(ImageSource imgSrc)
        {
            MainImage.Source = imgSrc;
            MainImage.TouchDown += MainImage_TouchDown;
            MainImage.TouchLeave += MainImage_TouchLeave;
            IdleTime();
        }

        void MainImage_TouchLeave(object sender, TouchEventArgs e)
        {
            IdleTime();
        }

        void MainImage_TouchDown(object sender, TouchEventArgs e)
        {
            CloseBtn.Visibility = Visibility.Visible;
            IdleTime(false);
        }

        #region IDLE TIME
        private void IdleTime(bool isStart = true)
        {
            idleTimer.Tick += idleTimer_Tick;
            idleTimer.Interval = new TimeSpan(00, 00, 05);
            if (isStart) idleTimer.Start(); else idleTimer.Stop();
        }

        void idleTimer_Tick(object sender, EventArgs e)
        {
            CloseBtn.Visibility = Visibility.Hidden;
            idleTimer.Stop();
        }
        #endregion

        #region Manipulation functions
        void UserControl_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = this;
            e.Handled = true;
        }

        void UserControl_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            try
            {
                // Get the Rectangle and its RenderTransform matrix.
                Grid rectToMove = e.OriginalSource as Grid;
                Matrix rectsMatrix = ((MatrixTransform)rectToMove.RenderTransform).Matrix;

                // Rotate the Rectangle.
                rectsMatrix.RotateAt(e.DeltaManipulation.Rotation,
                                     e.ManipulationOrigin.X,
                                     e.ManipulationOrigin.Y);

                // Resize the Rectangle.  Keep it square 
                // so use only the X value of Scale.
                rectsMatrix.ScaleAt(e.DeltaManipulation.Scale.X,
                                    e.DeltaManipulation.Scale.X,
                                    e.ManipulationOrigin.X,
                                    e.ManipulationOrigin.Y);

                // Move the Rectangle.
                rectsMatrix.Translate(e.DeltaManipulation.Translation.X,
                                      e.DeltaManipulation.Translation.Y);

                // Apply the changes to the Rectangle.
                rectToMove.RenderTransform = new MatrixTransform(rectsMatrix);

                //For CloseButton Only

                Rect containingRect =
                    new Rect(((FrameworkElement)e.ManipulationContainer).RenderSize);

                Rect shapeBounds =
                    rectToMove.RenderTransform.TransformBounds(
                        new Rect(rectToMove.RenderSize));

                // Check if the rectangle is completely in the window.
                // If it is not and intertia is occuring, stop the manipulation.
                if (e.IsInertial && !containingRect.Contains(shapeBounds))
                {
                    e.Complete();
                }
            }
            catch (Exception error)
            { 
                //throw error exception
            }

            e.Handled = true;
        }

        void UserControl_InertiaStarting(object sender, ManipulationInertiaStartingEventArgs e)
        {

            // Decrease the velocity of the Rectangle's movement by 
            // 10 inches per second every second.
            // (10 inches * 96 pixels per inch / 1000ms^2)
            e.TranslationBehavior.DesiredDeceleration = 10.0 * 96.0 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's resizing by 
            // 0.1 inches per second every second.
            // (0.1 inches * 96 pixels per inch / (1000ms^2)
            e.ExpansionBehavior.DesiredDeceleration = 0.1 * 96 / (1000.0 * 1000.0);

            // Decrease the velocity of the Rectangle's rotation rate by 
            // 2 rotations per second every second.
            // (2 * 360 degrees / (1000ms^2)
            e.RotationBehavior.DesiredDeceleration = 720 / (1000.0 * 1000.0);

            e.Handled = true;
        }
        #endregion
    }
}
