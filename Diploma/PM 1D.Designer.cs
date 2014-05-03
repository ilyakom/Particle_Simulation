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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.simulateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.parzenMethod = new System.Windows.Forms.RadioButton();
            this.wholeWayAveraging = new System.Windows.Forms.RadioButton();
            this.everyStepAveraging = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sweepStepTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.sweepDementionTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            this.graph1d.Size = new System.Drawing.Size(906, 482);
            this.graph1d.StencilBits = ((byte)(0));
            this.graph1d.TabIndex = 0;
            this.graph1d.MouseMove += new System.Windows.Forms.MouseEventHandler(this.graph1d_MouseMove);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(925, 461);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 1;
            this.button1.Text = "Simulate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(925, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 156);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры метода:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(10, 128);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(168, 21);
            this.textBox3.TabIndex = 5;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
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
            this.textBox2.Size = new System.Drawing.Size(168, 21);
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
            this.textBox1.Size = new System.Drawing.Size(168, 21);
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
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(9, 49);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(169, 20);
            this.textBox4.TabIndex = 7;
            this.textBox4.Text = "1";
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Колличество испытаний :";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1047, 461);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 33);
            this.button2.TabIndex = 3;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // simulateTimer
            // 
            this.simulateTimer.Tick += new System.EventHandler(this.simulateTimer_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.parzenMethod);
            this.groupBox2.Controls.Add(this.wholeWayAveraging);
            this.groupBox2.Controls.Add(this.everyStepAveraging);
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(925, 175);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(197, 155);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Восстановление плотности распределения";
            // 
            // parzenMethod
            // 
            this.parzenMethod.AutoSize = true;
            this.parzenMethod.Location = new System.Drawing.Point(9, 122);
            this.parzenMethod.Name = "parzenMethod";
            this.parzenMethod.Size = new System.Drawing.Size(172, 17);
            this.parzenMethod.TabIndex = 10;
            this.parzenMethod.TabStop = true;
            this.parzenMethod.Text = "Метод Парзена-Розенблатта";
            this.parzenMethod.UseVisualStyleBackColor = true;
            // 
            // wholeWayAveraging
            // 
            this.wholeWayAveraging.AutoSize = true;
            this.wholeWayAveraging.Location = new System.Drawing.Point(9, 99);
            this.wholeWayAveraging.Name = "wholeWayAveraging";
            this.wholeWayAveraging.Size = new System.Drawing.Size(173, 17);
            this.wholeWayAveraging.TabIndex = 9;
            this.wholeWayAveraging.Text = "Усреднение по полному пути";
            this.wholeWayAveraging.UseVisualStyleBackColor = true;
            // 
            // everyStepAveraging
            // 
            this.everyStepAveraging.AutoSize = true;
            this.everyStepAveraging.Checked = true;
            this.everyStepAveraging.Location = new System.Drawing.Point(9, 76);
            this.everyStepAveraging.Name = "everyStepAveraging";
            this.everyStepAveraging.Size = new System.Drawing.Size(173, 17);
            this.everyStepAveraging.TabIndex = 8;
            this.everyStepAveraging.TabStop = true;
            this.everyStepAveraging.Text = "Усреднение на каждом шаге";
            this.everyStepAveraging.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.sweepStepTextBox);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.sweepDementionTextBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(925, 337);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 115);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Метод прогонки";
            // 
            // sweepStepTextBox
            // 
            this.sweepStepTextBox.Location = new System.Drawing.Point(6, 80);
            this.sweepStepTextBox.Name = "sweepStepTextBox";
            this.sweepStepTextBox.ReadOnly = true;
            this.sweepStepTextBox.Size = new System.Drawing.Size(172, 20);
            this.sweepStepTextBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Шаг сетки : ";
            // 
            // sweepDementionTextBox
            // 
            this.sweepDementionTextBox.Location = new System.Drawing.Point(6, 36);
            this.sweepDementionTextBox.Name = "sweepDementionTextBox";
            this.sweepDementionTextBox.Size = new System.Drawing.Size(172, 20);
            this.sweepDementionTextBox.TabIndex = 1;
            this.sweepDementionTextBox.TextChanged += new System.EventHandler(this.sweepDemention_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Размерность сетки :";
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1127, 652);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.graph1d);
            this.Name = "StartForm";
            this.Text = "Particle Method 1-D";
            this.Load += new System.EventHandler(this.StartForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl graph1d;
        private System.Windows.Forms.Button button1;
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton parzenMethod;
        private System.Windows.Forms.RadioButton wholeWayAveraging;
        private System.Windows.Forms.RadioButton everyStepAveraging;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox sweepStepTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox sweepDementionTextBox;
        private System.Windows.Forms.Label label5;

    }
}

