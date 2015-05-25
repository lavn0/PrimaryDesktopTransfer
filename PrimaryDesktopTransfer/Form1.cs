using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PrimaryDesktopTransfer
{
	public partial class Form1 : Form
	{
		private const int IntervalMilliSeconds = 2000;

		public Form1()
		{
			this.InitializeComponent();
			this.backgroundWorker.RunWorkerAsync();
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

		private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			while (!this.backgroundWorker.CancellationPending)
			{
				this.backgroundWorker.ReportProgress(0);
				Thread.Sleep(IntervalMilliSeconds);
				Debug.WriteLine(DateTime.Now.TimeOfDay + " while loop.");
			}
		}

		private void backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
		{
			var rect = Screen.PrimaryScreen.Bounds;

			// Imageを使いまわさないとメモリ不足に陥り、GDI+ が不明なエラーメッセージの例外を発生させるので注意
			this.pictureBox1.Image = this.pictureBox1.Image ?? new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppPArgb);

			using (var g = Graphics.FromImage(this.pictureBox1.Image))
			{
				g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size, CopyPixelOperation.SourceCopy);
			}
		}
	}
}
