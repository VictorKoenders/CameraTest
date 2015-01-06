using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using MODI;

namespace CameraTest
{
	public partial class Form1 : Form
	{
		private readonly LiveJob _job;
		private LiveDeviceSource _deviceSource;
		private bool _isRunning = true;

		private readonly Thread _imageThread;

		public Form1()
		{
			InitializeComponent();
			_job = new LiveJob();

			foreach (EncoderDevice edv in EncoderDevices.FindDevices(EncoderDeviceType.Video))
			{
				lstVideoDevices.Items.Add(edv.Name);
			}

			lstVideoDevices.SelectedIndex = 0;

			lstVideoDevices_SelectedIndexChanged(lstVideoDevices, new EventArgs());

			_imageThread = new Thread(CheckImage);
			_imageThread.Start();
		}

		delegate Bitmap GetPanelBitmapCallback(Panel panel);

		private Bitmap GetPanelBitmap(Panel panel)
		{
			// InvokeRequired required compares the thread ID of the 
			// calling thread to the thread ID of the creating thread. 
			// If these threads are different, it returns true. 
			if (panel.InvokeRequired)
			{
				GetPanelBitmapCallback d = GetPanelBitmap;
				try
				{
					return Invoke(d, panel) as Bitmap;
				}
				catch
				{
					return null;
				}
			}
			Rectangle rectangle = panel1.Bounds;
			Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				Point source = panel1.PointToScreen(new Point(panel1.ClientRectangle.X, panel1.ClientRectangle.Y));
				g.CopyFromScreen(source, Point.Empty, rectangle.Size);
			}
			return bitmap;
		}

		private static readonly Regex WhiteSpaceRegex = new Regex("[\t\n\r ]", RegexOptions.Compiled);

		private void CheckImage()
		{
			if (!Directory.Exists("Images"))
			{
				Directory.CreateDirectory("Images");
			}
			while (_isRunning)
			{
				Thread.Sleep(1);
				using (Bitmap bitmap = GetPanelBitmap(panel1))
				{
					if (bitmap == null)
					{
						continue;
					}

					Guid guid = Guid.NewGuid();
					string image = "Images/" + guid + ".png";

					bitmap.Save(image, ImageFormat.Png);

					try
					{
						Document modiDocument = new Document();
						modiDocument.Create(image);
						modiDocument.OCR(MiLANGUAGES.miLANG_ENGLISH);
						MODI.Image modiImage = (modiDocument.Images[0] as MODI.Image);
						if (modiImage != null)
						{
							string extractedText = modiImage.Layout.Text;

							Console.WriteLine(extractedText.Replace("\n", "\\n"));
						}
						modiDocument.Close();
					}
					catch (Exception ex)
					{
					}

				}

			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			_isRunning = false;
			Thread.Sleep(500);
			_imageThread.Interrupt();
			_imageThread.Join();
		}

		private void lstVideoDevices_SelectedIndexChanged(object sender, EventArgs e)
		{
			EncoderDevice video = EncoderDevices.FindDevices(EncoderDeviceType.Video).FirstOrDefault(d => d.Name == lstVideoDevices.SelectedItem as string);

			if (_deviceSource != null)
			{
				_job.RemoveDeviceSource(_deviceSource);
				_deviceSource = null;
			}

			if (video == null)
			{
				return;
			}
			// Create a new device source. We use the first audio and video devices on the system
			_deviceSource = _job.AddDeviceSource(video, null);

			// Sets preview window to winform panel hosted by xaml window
			_deviceSource.PreviewWindow = new PreviewWindow(new HandleRef(panel1, panel1.Handle));

			// Make this source the active one
			_job.ActivateSource(_deviceSource);
		}
	}
}
