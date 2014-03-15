namespace Diploma
{
    partial class StartForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.graph1d = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.button1 = new System.Windows.Forms.Button();
            this.DrawTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.simulateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // graph1d
            // 
            this.graph1d.AccumBits = ((byte)(0));
            this.graph1d.AutoCheckErrors = false;
            this.graph1d.AutoFinish = false;
            this.graph1d.AutoMakeCurrent = true;
            this.graph1d.AutoSwapBuffers = true;
            this.graph1d.BackColor = System.Drawing.Color.Black;
            this.graph1d.ColorBits = ((byte)(32));
            this.graph1d.DepthBits = ((byte)(16));
            this.graph1d.Location = new System.Drawing.Point(12, 12);
            this.graph1d.Name = "graph1d";
            this.graph1d.Size = new System.Drawing.Size(906, 440);
            this.graph1d.StencilBits = ((byte)(0));
            this.graph1d.TabIndex = 0;
            this.graph1d.MouseMove += new System.Windows.Forms.MouseEventHandler(this.graph1d_MouseMove);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(924, 244);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "Simulate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DrawTimer
            // 
            this.DrawTimer.Interval = 30;
            this.DrawTimer.Tick += new System.EventHandler(this.PointInGrap_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(925, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 208);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры метода:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(9, 171);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(169, 21);
            this.textBox4.TabIndex = 7;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Усреднение :";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(10, 128);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(172, 21);
            this.textBox3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Шаг по времени : ";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(10, 84);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(172, 21);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Колличество частиц:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 41);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(172, 21);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Колличество разбиений:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1047, 244);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 33);
            this.button2.TabIndex = 3;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // simulateTimer
            // 
            this.simulateTimer.Interval = 60;
            this.simulateTimer.Tick += new System.EventHandler(this.simulateTimer_Tick);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 458);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.graph1d);
            this.Name = "StartForm";
            this.Text = "Particle Method 1-D";
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl graph1d;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer DrawTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer simulateTimer;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label4;

    }
}

