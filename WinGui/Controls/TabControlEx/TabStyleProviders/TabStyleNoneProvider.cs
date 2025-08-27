namespace CustomControls 
{
	using System;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Windows.Forms;
	
	[System.ComponentModel.ToolboxItem(false)]
	public class TabStyleNoneProvider : TabStyleProvider
	{
		public TabStyleNoneProvider(TabControlEx tabControl) : base(tabControl){
		}
		
		public override void AddTabBorder(System.Drawing.Drawing2D.GraphicsPath path, System.Drawing.Rectangle tabBounds){
		}
	}
}
