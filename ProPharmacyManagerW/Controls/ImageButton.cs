using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProPharmacyManagerW.Controls
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(ImageButton), new PropertyMetadata(double.NaN));
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(ImageButton), new PropertyMetadata(double.NaN));
        public static readonly DependencyProperty HoverImageProperty = DependencyProperty.Register("ImageHover", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty PressedImageProperty = DependencyProperty.Register("ImagePressed", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty DisabledImageProperty = DependencyProperty.Register("ImageDisabled", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty BorderBrushHoverProperty = DependencyProperty.Register("BorderBrHover", typeof(SolidColorBrush), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty BorderBrushPressedProperty = DependencyProperty.Register("BorderBrPressed", typeof(SolidColorBrush), typeof(ImageButton), new PropertyMetadata(null));
        public static readonly DependencyProperty BorderBrushDisabledProperty = DependencyProperty.Register("BorderBrDisabled", typeof(SolidColorBrush), typeof(ImageButton), new PropertyMetadata(null));

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public ImageSource ImageHover
        {
            get { return (ImageSource)GetValue(HoverImageProperty); }
            set { SetValue(HoverImageProperty, value); }
        }

        public ImageSource ImagePressed
        {
            get { return (ImageSource)GetValue(PressedImageProperty); }
            set { SetValue(PressedImageProperty, value); }
        }

        public ImageSource ImageDisabled
        {
            get { return (ImageSource)GetValue(DisabledImageProperty); }
            set { SetValue(DisabledImageProperty, value); }
        }

        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public new SolidColorBrush BorderBrush
        {
            get { return (SolidColorBrush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public SolidColorBrush BorderBrHover
        {
            get { return (SolidColorBrush)GetValue(BorderBrushHoverProperty); }
            set { SetValue(BorderBrushHoverProperty, value); }
        }

        public SolidColorBrush BorderBrPressed
        {
            get { return (SolidColorBrush)GetValue(BorderBrushPressedProperty); }
            set { SetValue(BorderBrushPressedProperty, value); }
        }

        public SolidColorBrush BorderBrDisabled
        {
            get { return (SolidColorBrush)GetValue(BorderBrushDisabledProperty); }
            set { SetValue(BorderBrushDisabledProperty, value); }
        }
    }
}