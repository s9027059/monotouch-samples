using System;
using System.Collections.Generic;
using System.Drawing;

using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;

namespace SimpleCollectionView
{
    public class SimpleCollectionViewController : UICollectionViewController
    {
        static NSString animalCellId = new NSString ("AnimalCell");
        List<IAnimal> animals;

        public SimpleCollectionViewController (UICollectionViewLayout layout) : base (layout)
        {
            animals = new List<IAnimal> ();
            for (int i = 0; i < 20; i++) {
				animals.Add (i % 2 == 0 ? (IAnimal) new Monkey () : (IAnimal) new Tamarin ());
            }
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            CollectionView.RegisterClassForCell (typeof (AnimalCell), animalCellId);
        }

        public override int GetItemsCount (UICollectionView collectionView, int section)
        {
            return animals.Count;
        }

        public override UICollectionViewCell GetCell (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
            var animalCell = (AnimalCell) collectionView.DequeueReusableCell (animalCellId, indexPath);

            var animal = animals [indexPath.Row];
            animalCell.Image = animal.Image;

            return animalCell;
        }
    }

    public class AnimalCell : UICollectionViewCell
    {
        UIImageView imageView;

        [Export ("initWithFrame:")]
        public AnimalCell (System.Drawing.RectangleF frame) : base (frame)
        {
            BackgroundView = new UIView { BackgroundColor = UIColor.Orange };

            SelectedBackgroundView = new UIView { BackgroundColor = UIColor.Green };

            ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
            ContentView.Layer.BorderWidth = 2.0f;
            ContentView.BackgroundColor = UIColor.White;
            ContentView.Transform = CGAffineTransform.MakeScale (0.8f, 0.8f);

            imageView = new UIImageView (UIImage.FromBundle ("placeholder.png"));
            imageView.Center = ContentView.Center;
            imageView.Transform = CGAffineTransform.MakeScale (0.7f, 0.7f);

            ContentView.AddSubview (imageView);
        }

        public UIImage Image {
            set {
                imageView.Image = value;
            }
        }

		public override void ApplyLayoutAttributes (UICollectionViewLayoutAttributes layoutAttributes)
		{
			var attributes = layoutAttributes as CustomCollectionViewLayoutAttributes;
			if (attributes != null) {
				var data = attributes.Data;
				attributes.Center = new PointF (data.Center.X + data.Radius * attributes.Distance * (float) Math.Cos (2 * attributes.Row * Math.PI / data.CellCount),
				                                data.Center.Y + data.Radius * attributes.Distance * (float) Math.Sin (2 * attributes.Row * Math.PI / data.CellCount));
			}

			base.ApplyLayoutAttributes (layoutAttributes);
		}
    }
}
