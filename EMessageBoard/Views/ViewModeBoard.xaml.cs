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
using System.Windows.Threading;

namespace EMessageBoard.Views
{
    /// <summary>
    /// Interaction logic for ViewModeBoard.xaml
    /// </summary>
    public partial class ViewModeBoard : UserControl
    {
        private string[]                    images;
        private static double imgWidth      = 300;
        private static double imgHeight     = 421;
        private static double imgSpeed      = 0.2;
        private static double imgBounce     = .1;	//bounce bounce effect		     
        private static double imgDistance   = 270;
        private static double imgOpacity    = 0.10;
        private static bool AlreadySwiped   = false;
        //private static double imgMax = 1;
        private double x_Position           = 90;
        private double y_Position           = 120;
        private double initial              = 0;
        private double currentSlide         = 0;
        private double toSlide              = 0;
        private List<Image> imgList         = new List<Image>();
        private static int fps              = 48;
        private DispatcherTimer mytimer     = new DispatcherTimer();
        protected TouchPoint TouchStart;
        protected TouchPoint TouchSwipe;


        public ViewModeBoard()
        {
            InitializeComponent();
            //images = RTAceo.Lib.FileDirectoryHelper.GetFileNames("_thumbnails", "*.png");
            images = EMessageBoard.Helpers.FileDirectoryHelpers.GetFileNames("images", "*.png");
            addImages();
        }

        void thumbnailsCanvas_TouchDown(object sender, TouchEventArgs e)
        {
            TouchStart = e.GetTouchPoint(this);
            AlreadySwiped = false;
        }

        void thumbnailsCanvas_TouchMove(object sender, TouchEventArgs e)
        {
            if (!AlreadySwiped)
            {
                var Touch = e.GetTouchPoint(this);
                if (TouchStart != null && Touch.Position.X > (TouchStart.Position.X + 100))
                {
                    moveIndexPrev(-1);
                    AlreadySwiped = true;
                }

                if (TouchStart != null && Touch.Position.X < (TouchStart.Position.X - 100))
                {
                    moveIndexNext(1);
                    AlreadySwiped = true;
                }
            }
            e.Handled = true;
        }

        private void carouselwindow_Loaded(object sender, RoutedEventArgs e)
        {
            Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < imgList.Count; i++)
            {
                Image image = imgList[i];
                post_Image(image, i);
            }
            if (initial == imgList.Count)
                initial = 0;
            toSlide = (initial - currentSlide) * imgSpeed + toSlide * imgBounce;
            currentSlide += toSlide;
        }

        private void addImages()
        {
            for (int i = 0; i < images.Length; i++)
            {
                string url = images[i];
                Image image = new Image();
                Uri uri = new Uri(AppDomain.CurrentDomain.BaseDirectory + "images\\" + url, UriKind.RelativeOrAbsolute);
                image.Source = new BitmapImage(uri);
                image.Width = 510;
                image.TouchUp += image_TouchUp;

                thumbnailsCanvas.Children.Add(image);
                post_Image(image, i);
                imgList.Add(image);
            }
        }

        /// <summary>
        /// On preview images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int margin  = 50;
        int limit   = 0;
        void image_TouchUp(object sender, TouchEventArgs e)
        {
            if (!AlreadySwiped && (limit >= 0))
            {
                ImagePreview imgControl = new ImagePreview();
                imgControl.UpdateSource((e.Source as Image).Source);
                imgControl.Margin = new Thickness(margin);
                imgControl.CloseBtn.TouchDown += CloseBtn_TouchDown;
                //imgControl.TouchDown += imgControl_TouchDown;
                rootGrid.Children.Add(imgControl);
                margin += 50;
                limit -= 1;
            }
        }

        void CloseBtn_TouchDown(object sender, TouchEventArgs e)
        {
            Button btn = e.Source as Button;
            Grid g = btn.Parent as Grid;
            ImagePreview imgControl = g.Parent as ImagePreview;
            rootGrid.Children.Remove(imgControl);
            margin -= 50;
            limit += 1;
        }

        private void post_Image(Image image, int index)
        {
            double diffFactor = index - currentSlide;

            ScaleTransform scaleTransform = new ScaleTransform();
            scaleTransform.ScaleX = 0.5;
            scaleTransform.ScaleY = 0.5;

            //scaleTransform.ScaleX = imgMax - Math.Abs(diffFactor) * imgOpacity;
            //scaleTransform.ScaleY = imgMax - Math.Abs(diffFactor) * imgOpacity;

            image.RenderTransform = scaleTransform;

            double left = x_Position - (imgWidth * scaleTransform.ScaleX) / 2 + diffFactor * imgDistance;
            double top = y_Position - (imgHeight * scaleTransform.ScaleY) / 2;
            image.Opacity = 1 - Math.Abs(diffFactor) * imgOpacity;

            image.SetValue(Canvas.LeftProperty, left);
            image.SetValue(Canvas.TopProperty, top);

            image.SetValue(Canvas.ZIndexProperty, (int)Math.Abs(scaleTransform.ScaleX * 100));
        }

        public void Start()
        {
            mytimer = new DispatcherTimer();
            mytimer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / fps);
            mytimer.Tick += new EventHandler(timer_Tick);
            mytimer.Start();
        }

        private void moveIndexPrev(int value)
        {
            initial = initial + value;
            initial = Math.Max(0, initial);
            initial = Math.Min(imgList.Count - 1, initial);
        }

        private void moveIndexNext(int value)
        {
            initial = initial + value;
            initial = Math.Max(0, initial);
            initial = Math.Min(imgList.Count - 1, initial);
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            moveIndexPrev(-1);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            moveIndexNext(1);
        }

        private void touchPrev(object sender, TouchEventArgs e)
        {
            moveIndexPrev(-1);
        }

        private void touchNext(object sender, TouchEventArgs e)
        {
            moveIndexNext(1);
        }

        private void writeModeImg_TouchUp(object sender, TouchEventArgs e)
        {
            MainWindow main = App.Current.MainWindow as MainWindow;
            main.canvasContent.Children.Clear();
            main.canvasContent.Children.Add(new WriteModeBoard());
        }

    }
}
