using System;
using Xamarin.Forms;

namespace UrbanGame
{
    public class AspectRatioContainer : ContentView
    {
        [Obsolete("Use OnMeasure")]
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * AspectRatio));
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * AspectRatio));
        }

        public double AspectRatio { get; set; }
    }
}
