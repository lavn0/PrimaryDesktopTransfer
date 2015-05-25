using System;
using System.Linq;
using System.Windows.Forms;

namespace PrimaryDesktopTransfer
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void 終了するToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void notifyIcon_Click(object sender, EventArgs e)
		{
			this.contextMenuStrip.Items.Clear();
			var endMenu = new ToolStripMenuItem("終了する");
			endMenu.Click += this.終了するToolStripMenuItem_Click;
			this.contextMenuStrip.Items.Add(endMenu);

			for (var i = 0; i < Screen.AllScreens.Length; i++)
			{
				var menu = new ToolStripMenuItem(string.Format("Screen{0}に表示", i));
				if (Screen.AllScreens[i] == Screen.PrimaryScreen)
				{
					menu.Enabled = false;
					menu.Text += " (メインディスプレイ)";
				}

				menu.Tag = i;
				menu.Click += this.Screenに表示ToolStripMenuItem_Click;
				this.contextMenuStrip.Items.Add(menu);
			}

			this.contextMenuStrip.Show(Cursor.Position);
		}

		private void Screenに表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var strip = sender as ToolStripMenuItem;
			int screenIndex = (int)strip.Tag;

			var subdisplay = Screen.AllScreens.ElementAt(screenIndex);
			this.Left = subdisplay.Bounds.X;
			this.Top = subdisplay.Bounds.Y;
			this.WindowState = FormWindowState.Normal;
			this.FormBorderStyle = FormBorderStyle.None;
			this.WindowState = FormWindowState.Maximized;
		}
	}
}
