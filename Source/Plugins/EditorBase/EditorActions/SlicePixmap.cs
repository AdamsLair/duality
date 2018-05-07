using System.Collections.Generic;
using System.Linq;
using Duality.Editor.Plugins.Base.Forms;
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
		public override int Priority
		{
			get { return base.Priority - 1; }
		}

		public override void Perform(IEnumerable<Pixmap> pixmaps)
		{
			// TODO: re-use existing forms
			PixmapSlicerForm slicingForm = new PixmapSlicerForm();
			slicingForm.TargetPixmap = pixmaps.First();
			slicingForm.Show(DualityEditorApp.MainForm.MainDockPanel);
		}

		public override bool CanPerformOn(IEnumerable<Pixmap> pixmaps)
		{
			return pixmaps.Count() == 1;
		}
	}
}
