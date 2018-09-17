namespace TeamScreenClientPortable
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
            this.button1 = new System.Windows.Forms.Button();
            this.drawingArea1 = new TeamScreenClientPortable.Controls.DrawingArea();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(797, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Quit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // drawingArea1
            // 
            this.drawingArea1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.drawingArea1.Location = new System.Drawing.Point(12, 41);
            this.drawingArea1.Name = "drawingArea1";
            this.drawingArea1.Size = new System.Drawing.Size(860, 480);
            this.drawingArea1.TabIndex = 0;
            this.drawingArea1.Text = "drawingArea1";
            this.drawingArea1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.drawingArea1_KeyDown);
            this.drawingArea1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.drawingArea1_KeyUp);
            this.drawingArea1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseDoubleClick);
            this.drawingArea1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseDown);
            this.drawingArea1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseMove);
            this.drawingArea1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawingArea1_MouseUp);
            // 
            // RemoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 533);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.drawingArea1);
            this.Name = "RemoteForm";
            this.Text = "Remote View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DrawingArea drawingArea1;
        private System.Windows.Forms.Button button1;
    }
}