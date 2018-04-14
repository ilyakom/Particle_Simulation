using System.Windows.Forms;
using Diploma.NumericalMethods;

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
			this.PartitionWidthTextBox = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.particlesCountTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.PartitionsCountTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.TimeStepTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.AveragingTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.simulateTimer = new System.Windows.Forms.Timer(this.components);
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.parzenWindowWidthTextBox = new System.Windows.Forms.TextBox();
			this.parzenMethod = new System.Windows.Forms.RadioButton();
			this.wholeWayAveraging = new System.Windows.Forms.RadioButton();
			this.everyStepAveraging = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.sweepStepTextBox = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.sweepDimensionTextBox = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.CurrentStepTextBox = new System.Windows.Forms.TextBox();
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
			this.graph1d.Location = new System.Drawing.Point(16, 15);
			this.graph1d.Margin = new System.Windows.Forms.Padding(4);
			this.graph1d.Name = "graph1d";
			this.graph1d.Size = new System.Drawing.Size(1172, 593);
			this.graph1d.StencilBits = ((byte)(0));
			this.graph1d.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(1206, 567);
			this.button1.Margin = new System.Windows.Forms.Padding(4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 41);
			this.button1.TabIndex = 1;
			this.button1.Text = "Simulate";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.PartitionWidthTextBox);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.particlesCountTextBox);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.PartitionsCountTextBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(1472, 15);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(263, 201);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Метод частиц:";
			// 
			// PartitionWidthTextBox
			// 
			this.PartitionWidthTextBox.Location = new System.Drawing.Point(13, 153);
			this.PartitionWidthTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.PartitionWidthTextBox.Name = "PartitionWidthTextBox";
			this.PartitionWidthTextBox.ReadOnly = true;
			this.PartitionWidthTextBox.Size = new System.Drawing.Size(223, 24);
			this.PartitionWidthTextBox.TabIndex = 7;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(9, 131);
			this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(150, 18);
			this.label8.TabIndex = 6;
			this.label8.Text = "Ширина разбиения : ";
			// 
			// particlesCountTextBox
			// 
			this.particlesCountTextBox.Enabled = false;
			this.particlesCountTextBox.Location = new System.Drawing.Point(13, 103);
			this.particlesCountTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.particlesCountTextBox.Name = "particlesCountTextBox";
			this.particlesCountTextBox.ReadOnly = true;
			this.particlesCountTextBox.Size = new System.Drawing.Size(223, 24);
			this.particlesCountTextBox.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 81);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(156, 18);
			this.label2.TabIndex = 2;
			this.label2.Text = "Колличество частиц:";
			// 
			// PartitionsCountTextBox
			// 
			this.PartitionsCountTextBox.Location = new System.Drawing.Point(13, 50);
			this.PartitionsCountTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.PartitionsCountTextBox.Name = "PartitionsCountTextBox";
			this.PartitionsCountTextBox.Size = new System.Drawing.Size(223, 24);
			this.PartitionsCountTextBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 27);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(182, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "Колличество разбиений:";
			// 
			// TimeStepTextBox
			// 
			this.TimeStepTextBox.Location = new System.Drawing.Point(1206, 485);
			this.TimeStepTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.TimeStepTextBox.Name = "TimeStepTextBox";
			this.TimeStepTextBox.ReadOnly = true;
			this.TimeStepTextBox.Size = new System.Drawing.Size(223, 22);
			this.TimeStepTextBox.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(1203, 464);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(124, 17);
			this.label3.TabIndex = 4;
			this.label3.Text = "Шаг по времени : ";
			// 
			// AveragingTextBox
			// 
			this.AveragingTextBox.Location = new System.Drawing.Point(12, 60);
			this.AveragingTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.AveragingTextBox.Name = "AveragingTextBox";
			this.AveragingTextBox.Size = new System.Drawing.Size(224, 22);
			this.AveragingTextBox.TabIndex = 7;
			this.AveragingTextBox.Text = "1";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 41);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(178, 17);
			this.label4.TabIndex = 6;
			this.label4.Text = "Колличество испытаний :";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(1314, 567);
			this.button2.Margin = new System.Windows.Forms.Padding(4);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(100, 41);
			this.button2.TabIndex = 3;
			this.button2.Text = "Stop";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2_Click);
			// 
			// simulateTimer
			// 
			this.simulateTimer.Tick += new System.EventHandler(this.SimulateTimer_Tick);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.parzenWindowWidthTextBox);
			this.groupBox2.Controls.Add(this.parzenMethod);
			this.groupBox2.Controls.Add(this.wholeWayAveraging);
			this.groupBox2.Controls.Add(this.everyStepAveraging);
			this.groupBox2.Controls.Add(this.AveragingTextBox);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Location = new System.Drawing.Point(1472, 230);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox2.Size = new System.Drawing.Size(263, 251);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Восстановление плотности распределения";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(9, 185);
			this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(102, 17);
			this.label7.TabIndex = 11;
			this.label7.Text = "Ширина окна :";
			// 
			// parzenWindowWidthTextBox
			// 
			this.parzenWindowWidthTextBox.Location = new System.Drawing.Point(12, 206);
			this.parzenWindowWidthTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.parzenWindowWidthTextBox.Name = "parzenWindowWidthTextBox";
			this.parzenWindowWidthTextBox.ReadOnly = true;
			this.parzenWindowWidthTextBox.Size = new System.Drawing.Size(224, 22);
			this.parzenWindowWidthTextBox.TabIndex = 6;
			// 
			// parzenMethod
			// 
			this.parzenMethod.AutoSize = true;
			this.parzenMethod.Location = new System.Drawing.Point(12, 150);
			this.parzenMethod.Margin = new System.Windows.Forms.Padding(4);
			this.parzenMethod.Name = "parzenMethod";
			this.parzenMethod.Size = new System.Drawing.Size(223, 21);
			this.parzenMethod.TabIndex = 10;
			this.parzenMethod.TabStop = true;
			this.parzenMethod.Text = "Метод Парзена-Розенблатта";
			this.parzenMethod.UseVisualStyleBackColor = true;
			// 
			// wholeWayAveraging
			// 
			this.wholeWayAveraging.AutoSize = true;
			this.wholeWayAveraging.Location = new System.Drawing.Point(12, 122);
			this.wholeWayAveraging.Margin = new System.Windows.Forms.Padding(4);
			this.wholeWayAveraging.Name = "wholeWayAveraging";
			this.wholeWayAveraging.Size = new System.Drawing.Size(223, 21);
			this.wholeWayAveraging.TabIndex = 9;
			this.wholeWayAveraging.Text = "Усреднение по полному пути";
			this.wholeWayAveraging.UseVisualStyleBackColor = true;
			// 
			// everyStepAveraging
			// 
			this.everyStepAveraging.AutoSize = true;
			this.everyStepAveraging.Checked = true;
			this.everyStepAveraging.Location = new System.Drawing.Point(12, 94);
			this.everyStepAveraging.Margin = new System.Windows.Forms.Padding(4);
			this.everyStepAveraging.Name = "everyStepAveraging";
			this.everyStepAveraging.Size = new System.Drawing.Size(218, 21);
			this.everyStepAveraging.TabIndex = 8;
			this.everyStepAveraging.TabStop = true;
			this.everyStepAveraging.Text = "Усреднение на каждом шаге";
			this.everyStepAveraging.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.sweepStepTextBox);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.sweepDimensionTextBox);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Location = new System.Drawing.Point(1196, 15);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox3.Size = new System.Drawing.Size(263, 142);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Метод прогонки";
			// 
			// sweepStepTextBox
			// 
			this.sweepStepTextBox.Location = new System.Drawing.Point(8, 98);
			this.sweepStepTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.sweepStepTextBox.Name = "sweepStepTextBox";
			this.sweepStepTextBox.ReadOnly = true;
			this.sweepStepTextBox.Size = new System.Drawing.Size(228, 22);
			this.sweepStepTextBox.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(13, 79);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(85, 17);
			this.label6.TabIndex = 2;
			this.label6.Text = "Шаг сетки : ";
			// 
			// sweepDimensionTextBox
			// 
			this.sweepDimensionTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.sweepDimensionTextBox.Location = new System.Drawing.Point(8, 44);
			this.sweepDimensionTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.sweepDimensionTextBox.Name = "sweepDimensionTextBox";
			this.sweepDimensionTextBox.Size = new System.Drawing.Size(228, 22);
			this.sweepDimensionTextBox.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 25);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(143, 17);
			this.label5.TabIndex = 0;
			this.label5.Text = "Размерность сетки :";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(1203, 511);
			this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(106, 17);
			this.label9.TabIndex = 6;
			this.label9.Text = "Текущий шаг : ";
			// 
			// CurrentStepTextBox
			// 
			this.CurrentStepTextBox.Location = new System.Drawing.Point(1206, 532);
			this.CurrentStepTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.CurrentStepTextBox.Name = "CurrentStepTextBox";
			this.CurrentStepTextBox.ReadOnly = true;
			this.CurrentStepTextBox.Size = new System.Drawing.Size(223, 22);
			this.CurrentStepTextBox.TabIndex = 7;
			// 
			// StartForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(1748, 619);
			this.Controls.Add(this.CurrentStepTextBox);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.TimeStepTextBox);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.graph1d);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "StartForm";
			this.Text = "One Dimensional Particle Method";
			this.Load += new System.EventHandler(this.StartForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl graph1d;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox particlesCountTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PartitionsCountTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TimeStepTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer simulateTimer;
        private System.Windows.Forms.TextBox AveragingTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton parzenMethod;
        private System.Windows.Forms.RadioButton wholeWayAveraging;
        private System.Windows.Forms.RadioButton everyStepAveraging;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox sweepStepTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox sweepDimensionTextBox;
        private System.Windows.Forms.Label label5;
		private Label label7;
		private TextBox parzenWindowWidthTextBox;
		private TextBox PartitionWidthTextBox;
		private Label label8;
		private Label label9;
		private TextBox CurrentStepTextBox;
	}
}

