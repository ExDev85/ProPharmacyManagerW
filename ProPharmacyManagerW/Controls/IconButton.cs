// <copyright>
//      This work is licensed under the
//      Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License.
//      To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/4.0/.
// </copyright>
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProPharmacyManagerW.Controls
{
    class IconButton : Button
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(IconButton), new PropertyMetadata(null));
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(IconButton), new PropertyMetadata(double.NaN));
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(IconButton), new PropertyMetadata(double.NaN));
        public static readonly DependencyProperty HoverColorProperty = DependencyProperty.Register("ColorHover", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(Brushes.LightGray));
        public static readonly DependencyProperty PressedColorProperty = DependencyProperty.Register("ColorPressed", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(Brushes.Gray));
        public static readonly DependencyProperty DisabledColorProperty = DependencyProperty.Register("ColorDisabled", typeof(SolidColorBrush), typeof(IconButton), new PropertyMetadata(Brushes.Gray));

        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
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

        public SolidColorBrush ColorHover
        {
            get { return (SolidColorBrush)GetValue(HoverColorProperty); }
            set { SetValue(HoverColorProperty, value); }
        }

        public SolidColorBrush ColorPressed
        {
            get { return (SolidColorBrush)GetValue(PressedColorProperty); }
            set { SetValue(PressedColorProperty, value); }
        }

        public SolidColorBrush ColorDisabled
        {
            get { return (SolidColorBrush)GetValue(DisabledColorProperty); }
            set { SetValue(DisabledColorProperty, value); }
        }
    }
}
