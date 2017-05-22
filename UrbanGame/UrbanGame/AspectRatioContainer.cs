using System;
using Xamarin.Forms;

namespace UrbanGame
{
    public class AspectRatioContainer : ContentView
    {
        public double AspectRatio { get; set; }

        [Obsolete("Use OnMeasure")]
        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            return OnMeasure(widthConstraint, heightConstraint);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(widthConstraint, widthConstraint * AspectRatio));
        }
    }
}
