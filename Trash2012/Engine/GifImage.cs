using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Interop;

namespace Trash2012.Engine
{
    public class GifImage : System.Windows.Controls.Image
    {
        private Bitmap _bitmap; // Local bitmap member to cache image resource
        private BitmapSource _bitmapSource;
        public delegate void FrameUpdatedEventHandler();


        /// <summary>
        /// Delete local bitmap resource
        /// Reference: http://msdn.microsoft.com/en-us/library/dd183539(VS.85).aspx
        /// </summary>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Override the OnInitialized method
        /// </summary>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Loaded += AnimatedGIFControl_Loaded;
            Unloaded += AnimatedGIFControl_Unloaded;
        }

        public int FrameCount { get; set; }

        public const int BeforeEndStep = 7;
        public const int EndStep = 11;

        public int CurrentFrame;
        public Action BeforeEndCallback { get; set; }
        public Action EndCallback { get; set; }

        public bool IsAnimationActive { get; private set; }

        public GifImage()
        {
            CurrentFrame = 0;
            FrameCount = 50;

            BeforeEndCallback = delegate() { };
            EndCallback = delegate() { };

            IsAnimationActive = false;

            _bitmap = null;
            Stretch = Stretch.UniformToFill;
        }

        /// <summary>
        /// Load the embedded image for the Image.Source
        /// </summary>
        void AnimatedGIFControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Get GIF image from Resources
            //if (Properties.Resources.ProgressIndicator != null)
            {
                //_bitmap = Properties.Resources.RightLeft_void2_;
                //Width = _bitmap.Width;
                //Height = _bitmap.Height;

                //_bitmapSource = GetBitmapSource();
                //Source = _bitmapSource;
            }
        }

        /// <summary>
        /// Close the FileStream to unlock the GIF file
        /// </summary>
        private void AnimatedGIFControl_Unloaded(object sender, RoutedEventArgs e)
        {
            StopAnimate();
        }

        /// <summary>
        /// Start animation
        /// </summary>
        public void StartAnimate(TruckAnimation animation)
        {
            if(IsAnimationActive)
                StopAnimate();

            switch (animation)
            {
                case TruckAnimation.Left2Right:
                    _bitmap = Properties.Resources.LeftRight_long_;
                    break;
                case TruckAnimation.Left2Bottom:
                    _bitmap = Properties.Resources.LeftBottom_long_;
                    break;
                case TruckAnimation.Top2Bottom:
                    _bitmap = Properties.Resources.TopBottom_long_;
                    break;
                case TruckAnimation.Top2Right:
                    _bitmap = Properties.Resources.TruckTopRight;
                    break;
                default:
                    Console.Error.WriteLine("Unhandled animation: " + animation);
                    _bitmap = Properties.Resources.TopBottom_long_; //FIXME Not the good one !!!
                    break;
            }

            _bitmapSource = GetBitmapSource();
            Source = _bitmapSource;
            CurrentFrame = 0;

            IsAnimationActive = true;
            LaunchAnimation();
        }

        private void LaunchAnimation()
        {
            if (_bitmap != null) ImageAnimator.Animate(_bitmap, OnFrameChanged);
        }

        /// <summary>
        /// Stop animation
        /// </summary>
        public void StopAnimate()
        {
            if (_bitmap != null)
            {
                IsAnimationActive = false;
                ImageAnimator.StopAnimate(_bitmap, OnFrameChanged);
            }
        }

        /// <summary>
        /// Event handler for the frame changed
        /// </summary>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Send,
                                   new FrameUpdatedEventHandler(FrameUpdatedCallback));
        }

        private void FrameUpdatedCallback()
        {
            ImageAnimator.UpdateFrames(_bitmap);

            CurrentFrame++;
            switch (CurrentFrame)
            {
                case BeforeEndStep:
                    BeforeEndCallback();
                    break;
                case EndStep:
                    EndCallback();
                    break;
            }

            if (_bitmapSource != null)
                _bitmapSource.Freeze();

            // Convert the bitmap to BitmapSource that can be display in WPF Visual Tree
            _bitmapSource = GetBitmapSource();
            Source = _bitmapSource;
            InvalidateVisual();
        }

        private BitmapSource GetBitmapSource()
        {
            IntPtr handle = IntPtr.Zero;
            try
            {
                handle = _bitmap.GetHbitmap();
                _bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                if (handle != IntPtr.Zero)
                    DeleteObject(handle);
            }

            return _bitmapSource;
        }
    }
}
