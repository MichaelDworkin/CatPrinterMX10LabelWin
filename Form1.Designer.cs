namespace CatPrinter
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            Status = new Label();
            numericUpDown1 = new NumericUpDown();
            ComboBoxFonts = new ComboBox();
            checkBox1 = new CheckBox();
            textBox1 = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(384, 384);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            // 
            // button1
            // 
            button1.Location = new Point(408, 32);
            button1.Name = "button1";
            button1.Size = new Size(198, 53);
            button1.TabIndex = 1;
            button1.Text = "Speichern";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(408, 102);
            button2.Name = "button2";
            button2.Size = new Size(198, 49);
            button2.TabIndex = 2;
            button2.Text = "Print";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(483, 9);
            label1.Name = "label1";
            label1.Size = new Size(82, 20);
            label1.TabIndex = 3;
            label1.Text = "Dateiname";
            // 
            // Status
            // 
            Status.BorderStyle = BorderStyle.Fixed3D;
            Status.Dock = DockStyle.Bottom;
            Status.Location = new Point(0, 408);
            Status.Name = "Status";
            Status.RightToLeft = RightToLeft.No;
            Status.Size = new Size(1028, 22);
            Status.TabIndex = 4;
            Status.Text = "Status";
            // 
            // numericUpDown1
            // 
            numericUpDown1.AllowDrop = true;
            numericUpDown1.Location = new Point(629, 202);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(209, 27);
            numericUpDown1.TabIndex = 5;
            numericUpDown1.Value = new decimal(new int[] { 24, 0, 0, 0 });
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // ComboBoxFonts
            // 
            ComboBoxFonts.DrawMode = DrawMode.OwnerDrawFixed;
            ComboBoxFonts.FormattingEnabled = true;
            ComboBoxFonts.Location = new Point(629, 157);
            ComboBoxFonts.Name = "ComboBoxFonts";
            ComboBoxFonts.Size = new Size(209, 28);
            ComboBoxFonts.TabIndex = 6;
            ComboBoxFonts.Text = "Schriftart";
            ComboBoxFonts.SelectedIndexChanged += ComboBoxFonts_SelectedIndexChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(629, 249);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(80, 24);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "Vertikal";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(629, 32);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(209, 53);
            textBox1.TabIndex = 8;
            textBox1.Text = "Irgendwas 1234567890";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1028, 430);
            Controls.Add(textBox1);
            Controls.Add(checkBox1);
            Controls.Add(ComboBoxFonts);
            Controls.Add(numericUpDown1);
            Controls.Add(Status);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private Button button2;
        private Label label1;
        private Label Status;
        private NumericUpDown numericUpDown1;
        private ComboBox ComboBoxFonts;
        private CheckBox checkBox1;
        private TextBox textBox1;
    }
}
