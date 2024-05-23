
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CatPrinter
{
    public partial class Form1 : Form
    {

        Bitmap flag = new Bitmap(384, 384);
        HttpClient httpClient = new HttpClient();

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
                Marshal.Copy(scan, 0, (IntPtr)((long)data.Scan0 + data.Stride * y), scan.Length);
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

        static async void PostAsync(Bitmap Bild, Label statusLabel, HttpClient client)
        {

            var formData = new MultipartFormDataContent();
            // Add form fields
            //formData.Add(new StringContent("John Doe"), "username");
            //formData.Add(new StringContent("example@example.com"), "email");
            // Add file
            //var fileContent = new ByteArrayContent(File.ReadAllBytes(label1.Text));
            using (var stream = new MemoryStream())
            {
                Bild.Save(stream, ImageFormat.Bmp);
                var fileContent = new ByteArrayContent(stream.ToArray());
                formData.Add(fileContent, "avatar", "file.jpg");
            }
            string httpResponseBody = "";
            try
            {
                var response =  await client.PostAsync("http://catprinter.local/upload", formData);
                response.EnsureSuccessStatusCode();
                httpResponseBody = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
            }
            //response.Wait();

            statusLabel.Text = httpResponseBody;

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
            
            // var bmp = new Bitmap(100, 100, PixelFormat.Format1bppIndexed);
            //pictureBox1.Image = bmp;
            /*
            bool Vertical=false;
            //pictureBox1.Size = new Size(384, 384);
            flag = new Bitmap(384, 192);
            Graphics flagGraphics = Graphics.FromImage(flag);

            flagGraphics.FillRectangle(Brushes.White, 0, 0, flagGraphics.VisibleClipBounds.Width, flagGraphics.VisibleClipBounds.Height);
            if (Vertical) flagGraphics.TranslateTransform(32, 0);
            if (Vertical) flagGraphics.RotateTransform(90);
            var fontFamily = (FontFamily)ComboBoxFonts.Items[ComboBoxFonts.SelectedIndex];
            using (Font font1 = new Font(fontFamily.Name.ToString(), 32, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                PointF pointF1 = new PointF(0, 0);
                flagGraphics.DrawString("Hello Welt", font1, Brushes.Black, pointF1);
            }
            if (Vertical) flagGraphics.ResetTransform();
            pictureBox1.Image = BitmapTo1Bpp(flag); 
            */


            // pictureBox1.Image = flag;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Data Files (*.bmp)|*.bmp";
            dialog.DefaultExt = "bmp";
            dialog.AddExtension = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                //flag = BitmapTo1Bpp(flag);
                pictureBox1.Image.Save(dialog.FileName, ImageFormat.Bmp);
                label1.Text = dialog.FileName;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PostAsync(flag, Status, httpClient);
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
                textLocation.X =  e.Location.X;
                textLocation.Y = e.Location.Y;
                //textLocation.Y = textLocation.Y - (int)(stringSize.Height) / 2;
                //textLocation.X = textLocation.X - (int)(stringSize.Width) / 2;
            }
            
            FlagRefresh();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FlagRefresh();
        }

   

   
    }
}
