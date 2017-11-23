using System;
using System.Diagnostics;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;

namespace LabelPartProperties
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				LabelPartProperties.Run();
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.InnerException + ex.Message + ex.StackTrace);
			}
		}

		public static class LabelPartProperties
		{
			public static void Run()
			{
				var pickedObjects = new Picker().PickObjects(Picker.PickObjectsEnum.PICK_N_PARTS);
				if (pickedObjects.GetSize() < 1) return;

				while (pickedObjects.MoveNext())
				{
					var part = pickedObjects.Current as Part;

					if(part == null)
						continue;

					var origin = part.GetCoordinateSystem().Origin;
					var axisX = part.GetCoordinateSystem().AxisX;
					var normal = axisX.GetNormal();
					var length = axisX.GetLength();

					double height = 0;
					part.GetReportProperty("HEIGHT", ref height);

					string topLevel = string.Empty;
					part.GetReportProperty("TOP_LEVEL", ref topLevel);

					var midPoint = new Point(
						origin.X + normal.X * length /2,
						origin.Y + normal.Y * length / 2,
						origin.Z + normal.Z * length / 2
						);

					var markPoint = new Point(midPoint);
					markPoint.Z += height / 2 + 250;

					var profilePoint = new Point(markPoint);
					profilePoint.Z -= 100;

					var levelPoint = new Point(markPoint);
					levelPoint.Z -= 200;

					var partMark = part.GetPartMark();

					var drawer = new GraphicsDrawer();
					var color = new Color(1, 1, 1);

					drawer.DrawText(markPoint, partMark, color);
					drawer.DrawText(profilePoint, part.Profile.ProfileString, color);
					drawer.DrawText(levelPoint, topLevel, color);

					drawer.DrawLineSegment(midPoint, markPoint, color);
				}
			}
		}
	}
}
