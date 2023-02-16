namespace TeltonikaGPS
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Map = new System.Windows.Forms.TabPage();
            this.Protocol = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoxPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxIP = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.Map.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(585, 416);
            this.listBox1.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Map);
            this.tabControl1.Controls.Add(this.Protocol);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(201, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(599, 450);
            this.tabControl1.TabIndex = 12;
            // 
            // Map
            // 
            this.Map.Controls.Add(this.listBox1);
            this.Map.Location = new System.Drawing.Point(4, 24);
            this.Map.Name = "Map";
            this.Map.Padding = new System.Windows.Forms.Padding(3);
            this.Map.Size = new System.Drawing.Size(591, 422);
            this.Map.TabIndex = 0;
            this.Map.Text = "Map";
            this.Map.UseVisualStyleBackColor = true;
            this.Map.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // Protocol
            // 
            this.Protocol.Location = new System.Drawing.Point(4, 24);
            this.Protocol.Name = "Protocol";
            this.Protocol.Padding = new System.Windows.Forms.Padding(3);
            this.Protocol.Size = new System.Drawing.Size(591, 422);
            this.Protocol.TabIndex = 1;
            this.Protocol.Text = "Protocol";
            this.Protocol.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtBoxPort);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtBoxIP);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(195, 450);
            this.panel1.TabIndex = 8;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(25, 365);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(151, 23);
            this.button4.TabIndex = 20;
            this.button4.Text = "CLEAR LOG";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(25, 408);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(151, 23);
            this.button3.TabIndex = 19;
            this.button3.Text = "CLEAR LOG FILE";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 224);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "STOP";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Stop_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 182);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "START";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Start_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(25, 127);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 15);
            this.lblStatus.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "Port";
            // 
            // txtBoxPort
            // 
            this.txtBoxPort.Location = new System.Drawing.Point(25, 89);
            this.txtBoxPort.Name = "txtBoxPort";
            this.txtBoxPort.Size = new System.Drawing.Size(151, 23);
            this.txtBoxPort.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 15);
            this.label1.TabIndex = 13;
            this.label1.Text = "Ip";
            // 
            // txtBoxIP
            // 
            this.txtBoxIP.Location = new System.Drawing.Point(25, 37);
            this.txtBoxIP.Name = "txtBoxIP";
            this.txtBoxIP.Size = new System.Drawing.Size(151, 23);
            this.txtBoxIP.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "TeltonikaGPS";
            this.tabControl1.ResumeLayout(false);
            this.Map.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblOnOff;
        private ListBox listBox1;
        private TabControl tabControl1;
        private TabPage Map;
        private TabPage Protocol;
        private Panel panel1;
        private Button button4;
        private Button button3;
        private Button button2;
        private Button button1;
        private Label lblStatus;
        private Label label2;
        private TextBox txtBoxPort;
        private Label label1;
        private TextBox txtBoxIP;
    }
}