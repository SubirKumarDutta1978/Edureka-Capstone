namespace Edureka.Capstone.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            lblTitle = new Label();
            pnlContent = new Panel();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            pnlHeader.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(31, 78, 121);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1000, 60);
            pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(15, 0, 0, 0);
            lblTitle.Size = new Size(1000, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Edureka Capstone";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlContent  (Step 2: upload and question controls go here)
            // 
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 60);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(10);
            pnlContent.Size = new Size(1000, 518);
            pnlContent.TabIndex = 1;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip.Location = new Point(0, 578);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1000, 22);
            statusStrip.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 17);
            lblStatus.Text = "Ready";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(pnlContent);
            Controls.Add(statusStrip);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edureka Capstone";
            Load += MainForm_Load;
            pnlHeader.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private Panel pnlContent;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
    }
}
