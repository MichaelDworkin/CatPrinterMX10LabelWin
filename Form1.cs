
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Bitmap
{
    public partial class Form1 : Form
    {
        private void ComboBoxFonts_DrawItem(object sender, DrawItemEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var fontFamily = (FontFamily)comboBox.Items[e.Index];
            var font = new Font(fontFamily, comboBox.Font.SizeInPoints);

            e.DrawBackground();
            e.Graphics.DrawString(font.Name, font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
        }
        public Form1()
        {
            InitializeComponent();
            ComboBoxFonts.DrawItem += ComboBoxFonts_DrawItem;
            ComboBoxFonts.DataSource = System.Drawing.FontFamily.Families.ToList();
        }
        
        System.Drawing.Bitmap flag;
        private void Form1_Load(object sender, EventArgs e)
        {
            //pictureBox1.Size = new Size(384, 384);
            flag = new System.Drawing.Bitmap(384, 192);
            Graphics flagGraphics = Graphics.FromImage(flag);

            flagGraphics.FillRectangle(Brushes.White, 0, 0, flagGraphics.VisibleClipBounds.Width, flagGraphics.VisibleClipBounds.Height);
            flagGraphics.TranslateTransform(32, 0);
            flagGraphics.RotateTransform(90);
            using (Font font1 = new Font("Times New Roman", 32, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                PointF pointF1 = new PointF(0, 0);
                flagGraphics.DrawString("Hello Welt", font1, Brushes.Black, pointF1);
            }
            flagGraphics.ResetTransform();
            pictureBox1.Image = BitmapTo1Bpp(flag); ;
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

            using (var client = new HttpClient())
            {
                var formData = new MultipartFormDataContent();

                // Add form fields
                formData.Add(new StringContent("John Doe"), "username");
                formData.Add(new StringContent("example@example.com"), "email");

                // Add file
                //var fileContent = new ByteArrayContent(File.ReadAllBytes(label1.Text));
                using (var stream = new MemoryStream())
                {
                    pictureBox1.Image.Save(stream, ImageFormat.Bmp);
                    var fileContent = new ByteArrayContent(stream.ToArray());
                    formData.Add(fileContent, "avatar", "file.jpg");
                }


                var response = client.PostAsync("http://catprinter.local/upload", formData);

                //response.Wait();

                Status.Text = response.Result.StatusCode.ToString();
                // Process the response as needed
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
