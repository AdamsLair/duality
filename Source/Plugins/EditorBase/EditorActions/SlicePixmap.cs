using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Duality.Editor.Plugins.Base.Forms.PixmapSlicer;
using Duality.Editor.Plugins.Base.Properties;
using Duality.Resources;

namespace Duality.Editor.Plugins.Base.EditorActions
{
	public class SlicePixmap : EditorAction<Pixmap>
	{
		public override string Name
		{
			get { return EditorBaseRes.ActionName_SlicePixmap; }
		}
		public override string Description
		{
			get { return EditorBaseRes.ActionDesc_SlicePixmap; }
		}
		public override Image Icon
		{
			get { return EditorBaseRes.IconPixmapSlicer; }
		}
		public override int Priority
		{
			get { return base.Priority - 1; }
		}

		public override void Perform(IEnumerable<Pixmap> pixmaps)
		{
			PixmapSlicerForm slicingForm = DualityEditorApp.GetPlugin<EditorBasePlugin>().RequestPixmapSlicerForm();
			slicingForm.TargetPixmap = pixmaps.First();
			slicingForm.Show();
		}

		public override bool CanPerformOn(IEnumerable<Pixmap> pixmaps)
		{
			return pixmaps.Count() == 1;
		}
	}
}
