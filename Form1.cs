using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;               // Für .ToList()
using System.Windows.Forms;
using MX10ThermalPrinter;       // Dein Drucker-Treiber

namespace CatPrinter
{
    public partial class Form1 : Form
    {
        // --- Interne Bildrepräsentation (Druckbild) ---
        // Wird immer mit Breite 384 gerendert, Höhe variabel.
        Bitmap flag = new Bitmap(384, 384);

        // --- Druckerinstanz ---
        MX10Printer printer = new MX10Printer();

        // --- Ausrichtung (horizontal/vertikal) ---
        bool Vertical = false;

        // --- Position, an der der Text gezeichnet wird ---
        Point textLocation = new Point(0, 0);

        // --- Größe des aktuell gemessenen Textes (für Bounding/Heuristik) ---
        SizeF stringSize;

        // --- Dragging-Zustand für flüssiges Verschieben ---
        bool isDragging = false;
        Point dragStartMouse;

        // ============================================================
        // 1-Bit-Konvertierung (schnell mit LockBits anstelle von GetPixel)
        // KORREKTUR: Schwarze Schrift auf weißem Hintergrund
        // ============================================================
        public static Bitmap BitmapTo1Bpp(Bitmap src, byte threshold = 128)
        {
            // Ziel: 1-Bit-Index-Bitmap, schwarz/weiß
            int w = src.Width;
            int h = src.Height;

            // Erzeuge ein 1bpp-Bitmap
            Bitmap dst = new Bitmap(w, h, PixelFormat.Format1bppIndexed);

            // KORREKTUR: Palette für schwarze Schrift auf weißem Hintergrund setzen
            ColorPalette palette = dst.Palette;
            palette.Entries[0] = Color.White;  // Index 0 = Weiß (Hintergrund)
            palette.Entries[1] = Color.Black;  // Index 1 = Schwarz (Text)
            dst.Palette = palette;

            // Quelle sperren (lesen) – wir nehmen 32bpp für einfachen Zugriff
            Rectangle rect = new Rectangle(0, 0, w, h);
            Bitmap src32 = src.PixelFormat == PixelFormat.Format32bppArgb ? src : new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            if (!ReferenceEquals(src, src32))
            {
                using (Graphics g = Graphics.FromImage(src32))
                {
                    g.DrawImage(src, rect);
                }
            }

            BitmapData srcData = src32.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData dstData = dst.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed);

            try
            {
                unsafe
                {
                    byte* srcScan0 = (byte*)srcData.Scan0;
                    byte* dstScan0 = (byte*)dstData.Scan0;

                    for (int y = 0; y < h; y++)
                    {
                        byte* srcRow = srcScan0 + y * srcData.Stride;
                        byte* dstRow = dstScan0 + y * dstData.Stride;

                        byte mask = 0x80;
                        byte bVal = 0;

                        for (int x = 0; x < w; x++)
                        {
                            int idx = x * 4;
                            byte b = srcRow[idx + 0];
                            byte g = srcRow[idx + 1];
                            byte r = srcRow[idx + 2];

                            // Einfache Helligkeits-Bewertung (Luma-Approximation)
                            // Wertebereich 0..255 -> Schwelle (threshold)
                            int gray = (r * 299 + g * 587 + b * 114) / 1000;
                            bool isBlack = gray < threshold;  // KORREKTUR: < statt >=

                            if (isBlack)
                                bVal |= mask; // schwarzes Pixel setzen (1)

                            mask >>= 1;

                            if (mask == 0)
                            {
                                dstRow[x / 8] = bVal;
                                mask = 0x80;
                                bVal = 0;
                            }
                        }

                        // Restbits am Zeilenende schreiben
                        if (mask != 0x80)
                        {
                            dstRow[w / 8] = bVal;
                            bVal = 0;
                            mask = 0x80;
                        }
                    }
                }
            }
            finally
            {
                src32.UnlockBits(srcData);
                dst.UnlockBits(dstData);
                if (!ReferenceEquals(src, src32))
                    src32.Dispose();
            }

            return dst;
        }

        // ============================================================
        // Rendering: Bild (384 x variable Höhe) erzeugen
        // ============================================================
        private Bitmap RenderFlag()
        {
            // 1) Schriftart aus ComboBox holen
            var fontFamily = ComboBoxFonts.Items[ComboBoxFonts.SelectedIndex] as FontFamily;
            using var font1 = new Font(fontFamily!.Name,
                                       Convert.ToInt32(numericUpDown1.Value),
                                       FontStyle.Bold,
                                       GraphicsUnit.Pixel);

            // 2) Ein erster "Messdurchlauf", um die Textgröße zu kennen
            using (var tmp = new Bitmap(1, 1))
            using (var gTmp = Graphics.FromImage(tmp))
            {
                stringSize = gTmp.MeasureString(textBox1.Text, font1);
            }

            // 3) Benötigte Inhaltshöhe abschätzen
            //    - Horizontal: Höhe ≈ stringSize.Height
            //    - Vertikal (90°): die Textbreite wird zur Höhe
            int contentExtent = Vertical
                                ? (int)Math.Ceiling(stringSize.Width)
                                : (int)Math.Ceiling(stringSize.Height);

            // 4) Höhe berechnen: Start oben (y=0), Text wächst nach unten
            //    Wir erlauben Mindesthöhe 64, plus kleinen Puffer.
            int margin = 8;
            int height = Math.Max(64, textLocation.Y + contentExtent + margin);

            // 5) Neues Bitmap mit Breite 384 und berechneter Höhe
            var bmp = new Bitmap(384, height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                // Vertikale Ausrichtung: Zeichenfläche um 90° drehen
                if (Vertical)
                {
                    // 384 ist die feste Papierbreite -> nach Drehung als X-Offset nutzen
                    g.TranslateTransform(384, 0);
                    g.RotateTransform(90);
                }

                // Text zeichnen
                g.DrawString(textBox1.Text, font1, Brushes.Black, textLocation);

                // Transform zurücksetzen
                if (Vertical) g.ResetTransform();
            }

            // Rückgabe: unbeschnittenes Bild (wird gleich unten beschnitten)
            return bmp;
        }

        // ============================================================
        // Unten weiß wegschneiden (nur bedruckte Zeilen senden)
        // ============================================================
        private static Bitmap CropBottomWhite(Bitmap src)
        {
            int width = src.Width;
            int lastContentRow = -1;

            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            BitmapData data = src.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                unsafe
                {
                    for (int y = src.Height - 1; y >= 0; y--)
                    {
                        byte* row = (byte*)data.Scan0 + y * data.Stride;
                        bool allWhite = true;

                        for (int x = 0; x < width; x++)
                        {
                            int idx = x * 4;
                            byte b = row[idx + 0];
                            byte g = row[idx + 1];
                            byte r = row[idx + 2];

                            // absolut weiß?
                            if (!(r == 255 && g == 255 && b == 255))
                            {
                                allWhite = false;
                                break;
                            }
                        }

                        if (!allWhite)
                        {
                            lastContentRow = y;
                            break;
                        }
                    }
                }
            }
            finally
            {
                src.UnlockBits(data);
            }

            int newHeight = Math.Max(1, lastContentRow + 1);
            if (newHeight >= src.Height) return src; // nichts zu schneiden

            var cropped = new Bitmap(width, newHeight, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(cropped))
            {
                g.DrawImage(src,
                    new Rectangle(0, 0, width, newHeight),
                    new Rectangle(0, 0, width, newHeight),
                    GraphicsUnit.Pixel);
            }
            return cropped;
        }

        // ============================================================
        // Anzeige-Aktualisierung (Rendern + Zuschneiden + Vorschau 1bpp)
        // KORREKTUR: Hintergrund des Panels hellgrau setzen
        // ============================================================
        public void FlagRefresh()
        {
            var rendered = RenderFlag();               // variable Höhe
            var cropped = CropBottomWhite(rendered);   // unten weiß weg
            flag = cropped;                            // Druckbild aktualisieren

            // Vorschau in 1bpp (schnell & sparsam)
            pictureBox1.Image = BitmapTo1Bpp(flag);

            // KORREKTUR: Panel-Hintergrund hellgrau setzen
            canvasPanel.BackColor = Color.LightGray;

            pictureBox1.Invalidate();                  // neu zeichnen
        }

        // ============================================================
        // Drucker-Verbindungslogik (wie gehabt, mit Status)
        // ============================================================
        private async void ConnectToPrinter()
        {
            Status.Text = "Verbinde mit Drucker...";
            button2.Enabled = false;
            buttonConnect.Enabled = false;
            try
            {
                // Versuche zuerst mit gespeicherter MAC zu verbinden
                string? savedMac = CatPrinterLabel.Settings.Default.PrinterMac;
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

                    // MAC speichern
                    if (!string.IsNullOrEmpty(printer.MacAddress))
                    {
                        CatPrinterLabel.Settings.Default.PrinterMac = printer.MacAddress;
                        CatPrinterLabel.Settings.Default.Save();
                    }

                    // Standard-Energie setzen
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

        // ============================================================
        // Drucken: flag (zugeschnitten, variable Höhe) senden
        // ============================================================
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
                // Hinweis: FlagRefresh wurde bereits bei Positions-/Textänderungen gerufen.
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

        // ============================================================
        // Papier-Vorschub (TrackBar)
        // ============================================================
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

        // ============================================================
        // ComboBox Owner-Draw: saubere Anzeige nur mit Fontnamen
        // ============================================================
        private void ComboBoxFonts_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var comboBox = (System.Windows.Forms.ComboBox)sender;
            FontFamily? fontFamily = comboBox.Items[e.Index] as FontFamily;
            using var font = new Font(fontFamily!, comboBox.Font.SizeInPoints);

            // Hintergrund zeichnen (Highlight bei Auswahl)
            e.DrawBackground();
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var bg = isSelected ? SystemBrushes.Highlight : SystemBrushes.Window;
            var fg = isSelected ? Brushes.White : Brushes.Black;

            e.Graphics.FillRectangle(bg, e.Bounds);
            e.Graphics.DrawString(font.Name, font, fg, e.Bounds.X + 4, e.Bounds.Y + 2);
            e.DrawFocusRectangle();
        }

        // ============================================================
        // Konstruktor: Initialisierung
        // ============================================================
        public Form1()
        {
            InitializeComponent();

            // Flackern reduzieren
            this.DoubleBuffered = true;
        }

        // ============================================================
        // Form-Lebenszyklus & Settings laden
        // KORREKTUR: Alle Initialisierungen nach InitializeComponent in Load
        // ============================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            // Drucker-Button deaktivieren bis Verbindung besteht
            button2.Enabled = false;

            // ComboBox-Eigenschaften & Datenquelle
            ComboBoxFonts.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBoxFonts.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBoxFonts.DisplayMember = "Name";
            var families = System.Drawing.FontFamily.Families.ToList();
            ComboBoxFonts.DataSource = families;
            ComboBoxFonts.DrawItem += ComboBoxFonts_DrawItem!;

            // --- letzte Auswahl laden und ANWENDEN ---
            var savedName = CatPrinterLabel.Settings.Default.LastFontFamily;
            int idx = !string.IsNullOrEmpty(savedName)
                      ? families.FindIndex(f => f.Name == savedName)
                      : 0;
            ComboBoxFonts.SelectedIndex = (idx >= 0) ? idx : 0;

            var savedSize = CatPrinterLabel.Settings.Default.LastFontSize;
            if (savedSize > 0 && savedSize >= numericUpDown1.Minimum && savedSize <= numericUpDown1.Maximum)
                numericUpDown1.Value = savedSize;

            checkBox1.Checked = CatPrinterLabel.Settings.Default.LastVertical;
            Vertical = checkBox1.Checked;

            // Text aus Settings laden
            var savedText = CatPrinterLabel.Settings.Default.LastText;
            if (!string.IsNullOrEmpty(savedText))
            {
                textBox1.Text = savedText;
            }

            // Position aus Settings laden
            var savedX = CatPrinterLabel.Settings.Default.LastTextX;
            var savedY = CatPrinterLabel.Settings.Default.LastTextY;
            textLocation = new Point(savedX, savedY);

            // Anfangsrender
            FlagRefresh();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Drucker trennen
            if (printer.IsConnected)
            {
                printer.Disconnect();
            }

            // KORREKTUR: Alle Settings beim Beenden speichern
            var fontFamily = ComboBoxFonts.Items[ComboBoxFonts.SelectedIndex] as FontFamily;
            if (fontFamily != null)
            {
                CatPrinterLabel.Settings.Default.LastFontFamily = fontFamily.Name;
            }
            CatPrinterLabel.Settings.Default.LastFontSize = (int)numericUpDown1.Value;
            CatPrinterLabel.Settings.Default.LastVertical = Vertical;
            CatPrinterLabel.Settings.Default.LastText = textBox1.Text;
            CatPrinterLabel.Settings.Default.LastTextX = textLocation.X;
            CatPrinterLabel.Settings.Default.LastTextY = textLocation.Y;

            // Einmal speichern
            CatPrinterLabel.Settings.Default.Save();
        }

        // ============================================================
        // UI-Events (Buttons/Controls)
        // ============================================================
        private void button1_Click(object sender, EventArgs e)
        {
            // Bild speichern (Vorschau ist 1bpp) – alternativ 'flag' (farbig)
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Bitmap (*.bmp)|*.bmp";
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
                DisconnectFromPrinter();
            else
                ConnectToPrinter();
        }

        private void ComboBoxFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            var fontFamily = ComboBoxFonts.Items[ComboBoxFonts.SelectedIndex] as FontFamily;
            if (fontFamily != null)
            {
                Status.Text = fontFamily.Name;
                FlagRefresh();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // aktuell nicht benutzt (Rendering erfolgt in FlagRefresh)
            // Könnte für zusätzliche Overlays (Hilfslinien) genutzt werden.
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

        // KORREKTUR: Text nicht mehr bei jeder Änderung speichern
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

        // ============================================================
        // Maus-Interaktion: flüssiges Ziehen der Textposition
        // KORREKTUR: Position speichern bei Änderung
        // ============================================================
        private void UpdateTextLocationFromMouse(Point mouse)
        {
            // Abbildung der Mausposition auf Textposition
            // Horizontal: (X,Y) direkt
            // Vertikal: deine bestehende Logik (90° Drehung)
            if (Vertical)
            {
                textLocation.Y = 384 - mouse.X;
                textLocation.X = mouse.Y;
            }
            else
            {
                textLocation = mouse;
            }

            // Position wird beim Schließen gespeichert, nicht bei jeder Bewegung
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!e.Button.HasFlag(MouseButtons.Left)) return;

            isDragging = true;
            dragStartMouse = e.Location;
            UpdateTextLocationFromMouse(e.Location);
            FlagRefresh();
            pictureBox1.Capture = true; // auch außerhalb weiter Ereignisse bekommen
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // 1) Dragging: Textposition in Echtzeit aktualisieren
            if (isDragging)
            {
                UpdateTextLocationFromMouse(e.Location);
                FlagRefresh();
                return;
            }

            // 2) Optionales Cursor-Feedback (nur horizontal genau):
            //    Zeige Handcursor, wenn Maus über dem Text liegt.
            if (!Vertical)
            {
                var rect = new RectangleF(textLocation.X, textLocation.Y, stringSize.Width, stringSize.Height);
                pictureBox1.Cursor = rect.Contains(e.Location)
                                     ? Cursors.Hand
                                     : Cursors.Default;
            }
            else
            {
                // Für vertikale Drehung ist die exakte Bounding-Berechnung komplexer.
                // Wir belassen den Standardcursor, um Verwirrung zu vermeiden.
                pictureBox1.Cursor = Cursors.SizeAll;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!isDragging) return;
            isDragging = false;
            pictureBox1.Capture = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}