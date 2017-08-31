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
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.lblTransfered = new System.Windows.Forms.Label();
            this.btnFileTransfer = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // drawingArea1
            // 
            this.drawingArea1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.panel1.Controls.Add(this.btnSaveAs);
            this.panel1.Controls.Add(this.lblTransfered);
            this.panel1.Controls.Add(this.btnFileTransfer);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(654, 29);
            this.panel1.TabIndex = 1;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(134, 3);
            this.btnSaveAs.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(58, 23);
            this.btnSaveAs.TabIndex = 3;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // lblTransfered
            // 
            this.lblTransfered.AutoSize = true;
            this.lblTransfered.Location = new System.Drawing.Point(345, 8);
            this.lblTransfered.Name = "lblTransfered";
            this.lblTransfered.Size = new System.Drawing.Size(53, 13);
            this.lblTransfered.TabIndex = 2;
            this.lblTransfered.Text = "Rx 120kb";
            // 
            // btnFileTransfer
            // 
            this.btnFileTransfer.Location = new System.Drawing.Point(264, 3);
            this.btnFileTransfer.Name = "btnFileTransfer";
            this.btnFileTransfer.Size = new System.Drawing.Size(75, 23);
            this.btnFileTransfer.TabIndex = 1;
            this.btnFileTransfer.Text = "File Transfer";
            this.btnFileTransfer.UseVisualStyleBackColor = true;
            this.btnFileTransfer.Click += new System.EventHandler(this.btnFileTransfer_Click);
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
        private System.Windows.Forms.Label lblTransfered;
        private System.Windows.Forms.Button btnFileTransfer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSaveAs;
    }
}