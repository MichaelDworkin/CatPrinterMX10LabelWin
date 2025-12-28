using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using MX10ThermalPrinter;

namespace CatPrinter
{
    public partial class Form1 : Form
    {
        Bitmap flag = new Bitmap(384, 384);
        MX10Printer printer = new MX10Printer();

        bool Vertical = false;
        Point textLocation = new Point(0, 0);
        SizeF stringSize;

        public static Bitmap BitmapTo1Bpp(Bitmap img)
        {
            int w = img.Width;
            int h = img.Height;
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format1bppIndexed);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format1bppIndexed);
            byte[] scan = new byte[(w + 7) / 8];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (x % 8 == 0) scan[x / 8] = 0;
                    Color c = img.GetPixel(x, y);
                    if (c.GetBrightness() >= 0.5) scan[x / 8] |= (byte)(0x80 >> (x % 8));
                }
                System.Runtime.InteropServices.Marshal.Copy(scan, 0, (IntPtr)((long)data.Scan0 + data.Stride * y), scan.Length);
            }
            bmp.UnlockBits(data);
            return bmp;
        }

        public void FlagRefresh()
        {
            Graphics flagGraphics = Graphics.FromImage(flag);
            flagGraphics.FillRectangle(Brushes.White, 0, 0, flagGraphics.VisibleClipBounds.Width, flagGraphics.VisibleClipBounds.Height);

            var fontFamily = ComboBoxFonts.Items[ComboBoxFonts.SelectedIndex] as FontFamily;
            Font font1 = new Font(fontFamily!.Name.ToString(), Convert.ToInt32(numericUpDown1.Value), FontStyle.Bold, GraphicsUnit.Pixel);
            if (Vertical) flagGraphics.TranslateTransform(384, 0);
            if (Vertical) flagGraphics.RotateTransform(90);

            flagGraphics.DrawString(textBox1.Text, font1, Brushes.Black, textLocation);
            if (Vertical) flagGraphics.ResetTransform();
            stringSize = flagGraphics.MeasureString(textBox1.Text, font1);
            pictureBox1.Image = BitmapTo1Bpp(flag);
        }

        private async void ConnectToPrinter()
        {
            Status.Text = "Verbinde mit Drucker...";
            button2.Enabled = false;
            buttonConnect.Enabled = false;

            try
            {
                // Versuche zuerst mit gespeicherter MAC zu verbinden
                string? savedMac = Properties.Settings.Default.PrinterMac;
                bool connected = false;

                if (!string.IsNullOrEmpty(savedMac))
                {
                    connected = await printer.ConnectByMacAsync(savedMac);
                }

                // Falls nicht erfolgreich, suche nach Drucker
                if (!connected)
                {
                    connected = await printer.ScanAndConnectAsync("MX10", 10);
                }

                if (connected)
                {
                    Status.Text = $"Verbunden mit {printer.DeviceName} ({printer.MacAddress})";
                    button2.Enabled = true;
                    buttonConnect.Text = "Trennen";
                    
                    // Speichere MAC-Adresse
                    if (!string.IsNullOrEmpty(printer.MacAddress))
                    {
                        Properties.Settings.Default.PrinterMac = printer.MacAddress;
                        Properties.Settings.Default.Save();
                    }

                    // Setze Standard-Energie
                    await printer.SetEnergyAsync(10000);
                }
                else
                {
                    Status.Text = "Verbindung fehlgeschlagen";
                    buttonConnect.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Status.Text = $"Fehler: {ex.Message}";
                buttonConnect.Enabled = true;
            }
        }

        private void DisconnectFromPrinter()
        {
            printer.Disconnect();
            Status.Text = "Getrennt";
            button2.Enabled = false;
            buttonConnect.Text = "Verbinden";
            buttonConnect.Enabled = true;
        }

        private async void PrintImage()
        {
            if (!printer.IsConnected)
            {
                Status.Text = "Nicht verbunden!";
                return;
            }

            Status.Text = "Drucke...";
            button2.Enabled = false;

            try
            {
                await printer.PrintGraphicsAsync(flag);
                Status.Text = "Druck abgeschlossen";
            }
            catch (Exception ex)
            {
                Status.Text = $"Druckfehler: {ex.Message}";
            }
            finally
            {
                button2.Enabled = true;
            }
        }

        private async void FeedPaper(int lines)
        {
            if (!printer.IsConnected)
            {
                Status.Text = "Nicht verbunden!";
                return;
            }

            try
            {
                await printer.FeedAsync(lines);
                Status.Text = $"Vorschub: {lines} Zeilen";
            }
            catch (Exception ex)
            {
                Status.Text = $"Vorschub-Fehler: {ex.Message}";
            }
        }

        private void ComboBoxFonts_DrawItem(object sender, DrawItemEventArgs e)
        {
            var comboBox = (System.Windows.Forms.ComboBox)sender;
            FontFamily? fontFamily = comboBox.Items[e.Index] as FontFamily;
            var font = new Font(fontFamily!, comboBox.Font.SizeInPoints);

            e.DrawBackground();
            e.Graphics.DrawString(font.Name, font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
        }

        public Form1()
        {
            InitializeComponent();
            ComboBoxFonts.DrawItem += ComboBoxFonts_DrawItem!;
            ComboBoxFonts.DataSource = System.Drawing.FontFamily.Families.ToList();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (printer.IsConnected)
            {
                printer.Disconnect();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Data Files (*.bmp)|*.bmp";
            dialog.DefaultExt = "bmp";
            dialog.AddExtension = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(dialog.FileName, ImageFormat.Bmp);
                label1.Text = dialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PrintImage();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (printer.IsConnected)
            {
                DisconnectFromPrinter();
            }
            else
            {
                ConnectToPrinter();
            }
        }

        private void ComboBoxFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fontFamily = ComboBoxFonts.Items[ComboBoxFonts.SelectedIndex] as FontFamily;
            if (fontFamily != null)
            {
                Status.Text = fontFamily.Name.ToString();
                FlagRefresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Vertical = checkBox1.Checked;
            FlagRefresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FlagRefresh();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Left)) return;

            if (Vertical)
            {
                textLocation.Y = 384 - e.Location.X;
                textLocation.X = e.Location.Y;
            }
            else
            {
                textLocation.X = e.Location.X;
                textLocation.Y = e.Location.Y;
            }

            FlagRefresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FlagRefresh();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label3.Text = trackBar1.Value.ToString();
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            Status.Text = trackBar1.Value.ToString();
            FeedPaper(trackBar1.Value);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            trackBar1.Value = 0;
            label3.Text = trackBar1.Value.ToString();
            timer1.Enabled = false;
        }
    }
}