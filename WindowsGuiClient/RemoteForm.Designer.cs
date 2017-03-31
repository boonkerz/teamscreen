namespace WindowsGuiClient
{
    partial class RemoteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.drawingArea1 = new WindowsGuiClient.Controls.DrawingArea();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnFileTransfer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawingArea1
            // 
            this.drawingArea1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.drawingArea1.Location = new System.Drawing.Point(0, 35);
            this.drawingArea1.Name = "drawingArea1";
            this.drawingArea1.Size = new System.Drawing.Size(654, 235);
            this.drawingArea1.TabIndex = 0;
            this.drawingArea1.Text = "drawingArea1";
            this.drawingArea1.Click += new System.EventHandler(this.drawingArea1_Click);
            this.drawingArea1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.drawingArea1_KeyDown);
            this.drawingArea1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.drawingArea1_KeyPress);
            this.drawingArea1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.drawingArea1_KeyUp);
            this.drawingArea1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseClick);
            this.drawingArea1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseDoubleClick);
            this.drawingArea1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseDown);
            this.drawingArea1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseMove_1);
            this.drawingArea1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnFileTransfer);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 29);
            this.panel1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(12, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnFileTransfer
            // 
            this.btnFileTransfer.Location = new System.Drawing.Point(275, 3);
            this.btnFileTransfer.Name = "btnFileTransfer";
            this.btnFileTransfer.Size = new System.Drawing.Size(75, 23);
            this.btnFileTransfer.TabIndex = 1;
            this.btnFileTransfer.Text = "File Transfer";
            this.btnFileTransfer.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(541, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Rx 120kb Tx 300kb";
            // 
            // RemoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 270);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.drawingArea1);
            this.Name = "RemoteForm";
            this.Text = "RemoteForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteForm_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DrawingArea drawingArea1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFileTransfer;
        private System.Windows.Forms.Button btnClose;
    }
}