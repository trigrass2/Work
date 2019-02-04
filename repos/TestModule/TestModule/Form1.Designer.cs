namespace TestModule
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelDurState = new System.Windows.Forms.Label();
            this.labelRepState = new System.Windows.Forms.Label();
            this.labelLogState = new System.Windows.Forms.Label();
            this.labelBSUErrorsState = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelLeitState = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.textBox14 = new System.Windows.Forms.TextBox();
            this.textBox15 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.labelLeitError = new System.Windows.Forms.Label();
            this.labelLeitErrors = new System.Windows.Forms.Label();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.labelBsuProd = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelHmiCran = new System.Windows.Forms.Label();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.textBox19 = new System.Windows.Forms.TextBox();
            this.textBox20 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Location = new System.Drawing.Point(24, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button2.Location = new System.Drawing.Point(155, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(24, 200);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(334, 20);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(24, 242);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(334, 20);
            this.textBox2.TabIndex = 3;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(24, 279);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(334, 20);
            this.textBox3.TabIndex = 4;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(24, 356);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(334, 20);
            this.textBox4.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Модуль для AWM_Dur :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Модуль для AWM_Rep :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Модуль для AWM_Hmimesh_Log :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(308, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Модуль для BSU_Errors :";
            // 
            // labelDurState
            // 
            this.labelDurState.AutoSize = true;
            this.labelDurState.Location = new System.Drawing.Point(152, 69);
            this.labelDurState.Name = "labelDurState";
            this.labelDurState.Size = new System.Drawing.Size(58, 13);
            this.labelDurState.TabIndex = 10;
            this.labelDurState.Text = "выключен";
            // 
            // labelRepState
            // 
            this.labelRepState.AutoSize = true;
            this.labelRepState.Location = new System.Drawing.Point(155, 101);
            this.labelRepState.Name = "labelRepState";
            this.labelRepState.Size = new System.Drawing.Size(58, 13);
            this.labelRepState.TabIndex = 11;
            this.labelRepState.Text = "выключен";
            // 
            // labelLogState
            // 
            this.labelLogState.AutoSize = true;
            this.labelLogState.Location = new System.Drawing.Point(202, 130);
            this.labelLogState.Name = "labelLogState";
            this.labelLogState.Size = new System.Drawing.Size(58, 13);
            this.labelLogState.TabIndex = 12;
            this.labelLogState.Text = "выключен";
            // 
            // labelBSUErrorsState
            // 
            this.labelBSUErrorsState.AutoSize = true;
            this.labelBSUErrorsState.Location = new System.Drawing.Point(444, 69);
            this.labelBSUErrorsState.Name = "labelBSUErrorsState";
            this.labelBSUErrorsState.Size = new System.Drawing.Size(58, 13);
            this.labelBSUErrorsState.TabIndex = 13;
            this.labelBSUErrorsState.Text = "выключен";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(24, 431);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(334, 20);
            this.textBox5.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(308, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Модуль для  Leit_PU:";
            // 
            // labelLeitState
            // 
            this.labelLeitState.AutoSize = true;
            this.labelLeitState.Location = new System.Drawing.Point(444, 104);
            this.labelLeitState.Name = "labelLeitState";
            this.labelLeitState.Size = new System.Drawing.Size(58, 13);
            this.labelLeitState.TabIndex = 17;
            this.labelLeitState.Text = "выключен";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(381, 356);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(334, 20);
            this.textBox7.TabIndex = 21;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(381, 279);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(334, 20);
            this.textBox8.TabIndex = 20;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(381, 242);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(334, 20);
            this.textBox9.TabIndex = 19;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(381, 200);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(334, 20);
            this.textBox10.TabIndex = 18;
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(736, 356);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(334, 20);
            this.textBox12.TabIndex = 26;
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(736, 279);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(334, 20);
            this.textBox13.TabIndex = 25;
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point(736, 242);
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new System.Drawing.Size(334, 20);
            this.textBox14.TabIndex = 24;
            // 
            // textBox15
            // 
            this.textBox15.Location = new System.Drawing.Point(736, 200);
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new System.Drawing.Size(334, 20);
            this.textBox15.TabIndex = 23;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(736, 396);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(334, 20);
            this.textBox6.TabIndex = 29;
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(381, 396);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(334, 20);
            this.textBox11.TabIndex = 28;
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(24, 396);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(334, 20);
            this.textBox16.TabIndex = 27;
            // 
            // label6
            // 
            this.labelLeitError.AutoSize = true;
            this.labelLeitError.Location = new System.Drawing.Point(444, 133);
            this.labelLeitError.Name = "label6";
            this.labelLeitError.Size = new System.Drawing.Size(58, 13);
            this.labelLeitError.TabIndex = 31;
            this.labelLeitError.Text = "выключен";
            // 
            // labelLeitErrors
            // 
            this.labelLeitErrors.AutoSize = true;
            this.labelLeitErrors.Location = new System.Drawing.Point(308, 133);
            this.labelLeitErrors.Name = "labelLeitErrors";
            this.labelLeitErrors.Size = new System.Drawing.Size(125, 13);
            this.labelLeitErrors.TabIndex = 30;
            this.labelLeitErrors.Text = "Модуль для  Leit_Errors:";
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(24, 474);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(334, 20);
            this.textBox17.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(308, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(162, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Модуль для  BSU_Productions :";
            // 
            // label8
            // 
            this.labelBsuProd.AutoSize = true;
            this.labelBsuProd.Location = new System.Drawing.Point(476, 159);
            this.labelBsuProd.Name = "label8";
            this.labelBsuProd.Size = new System.Drawing.Size(58, 13);
            this.labelBsuProd.TabIndex = 34;
            this.labelBsuProd.Text = "выключен";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 159);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(172, 13);
            this.label9.TabIndex = 35;
            this.label9.Text = "Модуль для AWM_HmiCran_Log :";
            // 
            // label10
            // 
            this.labelHmiCran.AutoSize = true;
            this.labelHmiCran.Location = new System.Drawing.Point(202, 159);
            this.labelHmiCran.Name = "label10";
            this.labelHmiCran.Size = new System.Drawing.Size(58, 13);
            this.labelHmiCran.TabIndex = 36;
            this.labelHmiCran.Text = "выключен";
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(736, 316);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(334, 20);
            this.textBox18.TabIndex = 39;
            // 
            // textBox19
            // 
            this.textBox19.Location = new System.Drawing.Point(381, 316);
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new System.Drawing.Size(334, 20);
            this.textBox19.TabIndex = 38;
            // 
            // textBox20
            // 
            this.textBox20.Location = new System.Drawing.Point(24, 316);
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new System.Drawing.Size(334, 20);
            this.textBox20.TabIndex = 37;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1102, 554);
            this.Controls.Add(this.textBox18);
            this.Controls.Add(this.textBox19);
            this.Controls.Add(this.textBox20);
            this.Controls.Add(this.labelHmiCran);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelBsuProd);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox17);
            this.Controls.Add(this.labelLeitError);
            this.Controls.Add(this.labelLeitErrors);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.textBox16);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.textBox14);
            this.Controls.Add(this.textBox15);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.labelLeitState);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.labelBSUErrorsState);
            this.Controls.Add(this.labelLogState);
            this.Controls.Add(this.labelRepState);
            this.Controls.Add(this.labelDurState);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Module_Zavod_NF";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelDurState;
        private System.Windows.Forms.Label labelRepState;
        private System.Windows.Forms.Label labelLogState;
        private System.Windows.Forms.Label labelBSUErrorsState;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelLeitState;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.TextBox textBox15;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Label labelLeitError;
        private System.Windows.Forms.Label labelLeitErrors;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelBsuProd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelHmiCran;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.TextBox textBox19;
        private System.Windows.Forms.TextBox textBox20;
    }
}

