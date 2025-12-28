
namespace CatPrinter
{
    partial class Form1
    {
        /// <summary>
        /// Vom Designer benötigtes Feld.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">true, wenn verwaltete Ressourcen entsorgt werden sollen; andernfalls false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung – 
        /// der Inhalt dieser Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            canvasPanel = new System.Windows.Forms.Panel();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            button1 = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            buttonConnect = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            Status = new System.Windows.Forms.Label();
            numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ComboBoxFonts = new System.Windows.Forms.ComboBox();
            checkBox1 = new System.Windows.Forms.CheckBox();
            textBox1 = new System.Windows.Forms.TextBox();
            trackBar1 = new System.Windows.Forms.TrackBar();
            timer1 = new System.Windows.Forms.Timer(components);
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();

            // --- Panel mit AutoScroll (für Endlospapier) ---
            // Das Panel enthält die PictureBox. Wenn das Bild höher wird,
            // erscheint automatisch eine Scrollbar.
            canvasPanel.Location = new System.Drawing.Point(12, 12);
            canvasPanel.Name = "canvasPanel";
            canvasPanel.Size = new System.Drawing.Size(384, 384);
            canvasPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            canvasPanel.AutoScroll = true;

            // --- PictureBox in AutoSize ---
            // Die PictureBox passt sich automatisch der Bildgröße an.
            // Durch das Panel mit AutoScroll bleibt die Ansicht bedienbar.
            pictureBox1.Location = new System.Drawing.Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(384, 384); // Startgröße
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;   // NEU: flüssiges Ziehen
            pictureBox1.MouseUp += pictureBox1_MouseUp;       // NEU: Ende des Ziehens
            pictureBox1.Cursor = System.Windows.Forms.Cursors.SizeAll;

            // PictureBox ins Panel einsetzen
            canvasPanel.Controls.Add(pictureBox1);

            // --- Button "Speichern" ---
            button1.Location = new System.Drawing.Point(408, 62);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(198, 40);
            button1.TabIndex = 1;
            button1.Text = "Speichern";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;

            // --- Button "Print" ---
            button2.Location = new System.Drawing.Point(408, 108);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(198, 40);
            button2.TabIndex = 2;
            button2.Text = "Print";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;

            // --- Button "Verbinden/Trennen" ---
            buttonConnect.Location = new System.Drawing.Point(408, 12);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new System.Drawing.Size(198, 44);
            buttonConnect.TabIndex = 3;
            buttonConnect.Text = "Verbinden";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;

            // --- Label Dateiname ---
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(612, 72);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(82, 20);
            label1.TabIndex = 4;
            label1.Text = "Dateiname";

            // --- Statuszeile unten ---
            Status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Status.Dock = System.Windows.Forms.DockStyle.Bottom;
            Status.Location = new System.Drawing.Point(0, 408);
            Status.Name = "Status";
            Status.RightToLeft = System.Windows.Forms.RightToLeft.No;
            Status.Size = new System.Drawing.Size(849, 22);
            Status.TabIndex = 5;
            Status.Text = "Nicht verbunden";

            // --- Schriftgröße (NumericUpDown) ---
            numericUpDown1.AllowDrop = true;
            numericUpDown1.Location = new System.Drawing.Point(629, 120);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new System.Drawing.Size(209, 27);
            numericUpDown1.TabIndex = 6;
            numericUpDown1.Value = new decimal(new int[] { 24, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;

            // --- Schriftart-ComboBox ---
            ComboBoxFonts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            ComboBoxFonts.FormattingEnabled = true;
            ComboBoxFonts.Location = new System.Drawing.Point(629, 12);
            ComboBoxFonts.Name = "ComboBoxFonts";
            ComboBoxFonts.Size = new System.Drawing.Size(209, 28);
            ComboBoxFonts.TabIndex = 7;
            ComboBoxFonts.Text = "Schriftart";
            ComboBoxFonts.SelectedIndexChanged += ComboBoxFonts_SelectedIndexChanged;
            // DrawItem-Ereignis wird im Konstruktor gebunden (siehe Form1.cs)

            // --- Vertikal-CheckBox ---
            checkBox1.AutoSize = true;
            checkBox1.Location = new System.Drawing.Point(629, 153);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(80, 24);
            checkBox1.TabIndex = 8;
            checkBox1.Text = "Vertikal";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;

            // --- Textfeld für Inhalt ---
            textBox1.Location = new System.Drawing.Point(408, 177);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(239, 219);
            textBox1.TabIndex = 9;
            textBox1.Text = "Irgendwas 1234567890";
            textBox1.TextChanged += textBox1_TextChanged;

            // --- Vorschub-TrackBar ---
            trackBar1.AllowDrop = true;
            trackBar1.Location = new System.Drawing.Point(677, 177);
            trackBar1.Maximum = 200;
            trackBar1.Minimum = -200;
            trackBar1.Name = "trackBar1";
            trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
            trackBar1.RightToLeftLayout = true;
            trackBar1.Size = new System.Drawing.Size(56, 219);
            trackBar1.TabIndex = 10;
            trackBar1.TickFrequency = 10;
            trackBar1.Scroll += trackBar1_Scroll;
            trackBar1.MouseUp += trackBar1_MouseUp;

            // --- Timer für TrackBar-Reset ---
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;

            // --- Label "Vorschub" ---
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(747, 183);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(69, 20);
            label2.TabIndex = 11;
            label2.Text = "Vorschub";

            // --- Anzeige des aktuellen Vorschub-Werts ---
            label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            label3.Location = new System.Drawing.Point(747, 276);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(74, 24);
            label3.TabIndex = 12;
            label3.Text = "0";
            label3.TextAlign = System.Drawing.ContentAlignment.TopRight;

            // --- Form ---
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(849, 430);

            // Reihenfolge: zuerst Panel (mit PictureBox), dann restliche Controls
            Controls.Add(canvasPanel);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(trackBar1);
            Controls.Add(textBox1);
            Controls.Add(checkBox1);
            Controls.Add(ComboBoxFonts);
            Controls.Add(numericUpDown1);
            Controls.Add(Status);
            Controls.Add(label1);
            Controls.Add(buttonConnect);
            Controls.Add(button2);
            Controls.Add(button1);

            Name = "Form1";
            Text = "Cat Printer (Bluetooth)";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;

            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        // --- Deklaration der Steuerelemente ---
        private System.Windows.Forms.Panel canvasPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox ComboBoxFonts;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
