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
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            lblDocSummary = new ToolStripStatusLabel();
            splitMain = new SplitContainer();
            grpDocuments = new GroupBox();
            lvDocuments = new ListView();
            colName = new ColumnHeader();
            colType = new ColumnHeader();
            colSize = new ColumnHeader();
            lblDropHint = new Label();
            flpDocButtons = new FlowLayoutPanel();
            btnAddFiles = new Button();
            btnRemoveFile = new Button();
            btnClearFiles = new Button();
            grpConversation = new GroupBox();
            rtbConversation = new RichTextBox();
            pnlInput = new Panel();
            txtPrompt = new Edureka.Capstone.Forms.Controls.PromptRichTextBox();
            flpAttachments = new FlowLayoutPanel();
            lblPromptCaption = new Label();
            pnlInputButtons = new Panel();
            lblHint = new Label();
            flpSendButtons = new FlowLayoutPanel();
            btnSend = new Button();
            btnClearChat = new Button();
            pnlHeader.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            grpDocuments.SuspendLayout();
            flpDocButtons.SuspendLayout();
            grpConversation.SuspendLayout();
            pnlInput.SuspendLayout();
            pnlInputButtons.SuspendLayout();
            flpSendButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(31, 78, 121);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1100, 60);
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
            lblTitle.Size = new Size(1100, 60);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Edureka Capstone";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus, lblDocSummary });
            statusStrip.Location = new Point(0, 698);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1100, 22);
            statusStrip.TabIndex = 2;
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1000, 17);
            lblStatus.Spring = true;
            lblStatus.Text = "Ready";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDocSummary
            // 
            lblDocSummary.Name = "lblDocSummary";
            lblDocSummary.Size = new Size(85, 17);
            lblDocSummary.Text = "No documents";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 60);
            splitMain.Name = "splitMain";
            splitMain.Panel1.Controls.Add(grpDocuments);
            splitMain.Panel1.Padding = new Padding(10, 10, 5, 10);
            splitMain.Panel1MinSize = 300;
            splitMain.Panel2.Controls.Add(grpConversation);
            splitMain.Panel2.Padding = new Padding(5, 10, 10, 10);
            splitMain.Panel2MinSize = 450;
            splitMain.Size = new Size(1100, 638);
            splitMain.SplitterDistance = 360;
            splitMain.TabIndex = 1;
            // 
            // grpDocuments
            // 
            grpDocuments.AllowDrop = true;
            grpDocuments.Controls.Add(lvDocuments);
            grpDocuments.Controls.Add(lblDropHint);
            grpDocuments.Controls.Add(flpDocButtons);
            grpDocuments.Dock = DockStyle.Fill;
            grpDocuments.Location = new Point(10, 10);
            grpDocuments.Name = "grpDocuments";
            grpDocuments.Padding = new Padding(8);
            grpDocuments.Size = new Size(345, 618);
            grpDocuments.TabIndex = 0;
            grpDocuments.TabStop = false;
            grpDocuments.Text = "Documents";
            grpDocuments.DragDrop += Documents_DragDrop;
            grpDocuments.DragEnter += Documents_DragEnter;
            // 
            // lvDocuments
            // 
            lvDocuments.AllowDrop = true;
            lvDocuments.Columns.AddRange(new ColumnHeader[] { colName, colType, colSize });
            lvDocuments.Dock = DockStyle.Fill;
            lvDocuments.FullRowSelect = true;
            lvDocuments.HideSelection = false;
            lvDocuments.Location = new Point(8, 66);
            lvDocuments.Name = "lvDocuments";
            lvDocuments.ShowItemToolTips = true;
            lvDocuments.Size = new Size(329, 506);
            lvDocuments.TabIndex = 1;
            lvDocuments.UseCompatibleStateImageBehavior = false;
            lvDocuments.View = View.Details;
            lvDocuments.DragDrop += Documents_DragDrop;
            lvDocuments.DragEnter += Documents_DragEnter;
            lvDocuments.SelectedIndexChanged += lvDocuments_SelectedIndexChanged;
            lvDocuments.KeyDown += lvDocuments_KeyDown;
            // 
            // colName
            // 
            colName.Text = "Name";
            colName.Width = 180;
            // 
            // colType
            // 
            colType.Text = "Type";
            colType.Width = 60;
            // 
            // colSize
            // 
            colSize.Text = "Size";
            colSize.Width = 80;
            // 
            // lblDropHint
            // 
            lblDropHint.Dock = DockStyle.Bottom;
            lblDropHint.ForeColor = Color.Gray;
            lblDropHint.Location = new Point(8, 572);
            lblDropHint.Name = "lblDropHint";
            lblDropHint.Padding = new Padding(0, 6, 0, 0);
            lblDropHint.Size = new Size(329, 38);
            lblDropHint.TabIndex = 2;
            lblDropHint.Text = "Drag and drop files here";
            // 
            // flpDocButtons
            // 
            flpDocButtons.Controls.Add(btnAddFiles);
            flpDocButtons.Controls.Add(btnRemoveFile);
            flpDocButtons.Controls.Add(btnClearFiles);
            flpDocButtons.Dock = DockStyle.Top;
            flpDocButtons.Location = new Point(8, 24);
            flpDocButtons.Name = "flpDocButtons";
            flpDocButtons.Size = new Size(329, 42);
            flpDocButtons.TabIndex = 0;
            // 
            // btnAddFiles
            // 
            btnAddFiles.Location = new Point(3, 3);
            btnAddFiles.Name = "btnAddFiles";
            btnAddFiles.Size = new Size(105, 32);
            btnAddFiles.TabIndex = 0;
            btnAddFiles.Text = "Add files...";
            btnAddFiles.UseVisualStyleBackColor = true;
            btnAddFiles.Click += btnAddFiles_Click;
            // 
            // btnRemoveFile
            // 
            btnRemoveFile.Enabled = false;
            btnRemoveFile.Location = new Point(114, 3);
            btnRemoveFile.Name = "btnRemoveFile";
            btnRemoveFile.Size = new Size(95, 32);
            btnRemoveFile.TabIndex = 1;
            btnRemoveFile.Text = "Remove";
            btnRemoveFile.UseVisualStyleBackColor = true;
            btnRemoveFile.Click += btnRemoveFile_Click;
            // 
            // btnClearFiles
            // 
            btnClearFiles.Enabled = false;
            btnClearFiles.Location = new Point(215, 3);
            btnClearFiles.Name = "btnClearFiles";
            btnClearFiles.Size = new Size(95, 32);
            btnClearFiles.TabIndex = 2;
            btnClearFiles.Text = "Clear all";
            btnClearFiles.UseVisualStyleBackColor = true;
            btnClearFiles.Click += btnClearFiles_Click;
            // 
            // grpConversation
            // 
            grpConversation.Controls.Add(rtbConversation);
            grpConversation.Controls.Add(pnlInput);
            grpConversation.Dock = DockStyle.Fill;
            grpConversation.Location = new Point(5, 10);
            grpConversation.Name = "grpConversation";
            grpConversation.Padding = new Padding(8);
            grpConversation.Size = new Size(716, 618);
            grpConversation.TabIndex = 0;
            grpConversation.TabStop = false;
            grpConversation.Text = "Conversation";
            // 
            // rtbConversation
            // 
            rtbConversation.BackColor = Color.White;
            rtbConversation.BorderStyle = BorderStyle.None;
            rtbConversation.Dock = DockStyle.Fill;
            rtbConversation.HideSelection = false;
            rtbConversation.Location = new Point(8, 24);
            rtbConversation.Name = "rtbConversation";
            rtbConversation.ReadOnly = true;
            rtbConversation.Size = new Size(700, 316);
            rtbConversation.TabIndex = 0;
            rtbConversation.Text = "";
            // 
            // pnlInput
            // 
            pnlInput.Controls.Add(txtPrompt);
            pnlInput.Controls.Add(flpAttachments);
            pnlInput.Controls.Add(lblPromptCaption);
            pnlInput.Controls.Add(pnlInputButtons);
            pnlInput.Dock = DockStyle.Bottom;
            pnlInput.Location = new Point(8, 340);
            pnlInput.Name = "pnlInput";
            pnlInput.Size = new Size(700, 270);
            pnlInput.TabIndex = 1;
            // 
            // txtPrompt
            // 
            txtPrompt.BorderStyle = BorderStyle.FixedSingle;
            txtPrompt.Dock = DockStyle.Fill;
            txtPrompt.Font = new Font("Segoe UI", 10F);
            txtPrompt.Location = new Point(0, 106);
            txtPrompt.Name = "txtPrompt";
            txtPrompt.Size = new Size(700, 122);
            txtPrompt.TabIndex = 0;
            txtPrompt.Text = "";
            txtPrompt.TextChanged += txtPrompt_TextChanged;
            txtPrompt.KeyDown += txtPrompt_KeyDown;
            // 
            // flpAttachments
            // 
            flpAttachments.Dock = DockStyle.Top;
            flpAttachments.Location = new Point(0, 22);
            flpAttachments.Name = "flpAttachments";
            flpAttachments.Padding = new Padding(0, 2, 0, 2);
            flpAttachments.Size = new Size(700, 84);
            flpAttachments.TabIndex = 3;
            flpAttachments.Visible = false;
            flpAttachments.WrapContents = false;
            flpAttachments.AutoScroll = true;
            // 
            // lblPromptCaption
            // 
            lblPromptCaption.Dock = DockStyle.Top;
            lblPromptCaption.Location = new Point(0, 0);
            lblPromptCaption.Name = "lblPromptCaption";
            lblPromptCaption.Size = new Size(700, 22);
            lblPromptCaption.TabIndex = 2;
            lblPromptCaption.Text = "Ask a question about your documents:";
            lblPromptCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlInputButtons
            // 
            pnlInputButtons.Controls.Add(lblHint);
            pnlInputButtons.Controls.Add(flpSendButtons);
            pnlInputButtons.Dock = DockStyle.Bottom;
            pnlInputButtons.Location = new Point(0, 228);
            pnlInputButtons.Name = "pnlInputButtons";
            pnlInputButtons.Size = new Size(700, 42);
            pnlInputButtons.TabIndex = 1;
            // 
            // lblHint
            // 
            lblHint.Dock = DockStyle.Fill;
            lblHint.ForeColor = Color.Gray;
            lblHint.Location = new Point(0, 0);
            lblHint.Name = "lblHint";
            lblHint.Size = new Size(460, 42);
            lblHint.TabIndex = 0;
            lblHint.Text = "Enter = new line  |  Ctrl+Enter = send  |  Ctrl+V pastes text or images";
            lblHint.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flpSendButtons
            // 
            flpSendButtons.AutoSize = true;
            flpSendButtons.Controls.Add(btnSend);
            flpSendButtons.Controls.Add(btnClearChat);
            flpSendButtons.Dock = DockStyle.Right;
            flpSendButtons.FlowDirection = FlowDirection.RightToLeft;
            flpSendButtons.Location = new Point(460, 0);
            flpSendButtons.Name = "flpSendButtons";
            flpSendButtons.Padding = new Padding(0, 4, 0, 0);
            flpSendButtons.Size = new Size(240, 42);
            flpSendButtons.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.BackColor = Color.FromArgb(31, 78, 121);
            btnSend.Enabled = false;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.ForeColor = Color.White;
            btnSend.Location = new Point(137, 7);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(100, 32);
            btnSend.TabIndex = 0;
            btnSend.Text = "Send";
            btnSend.UseVisualStyleBackColor = false;
            btnSend.Click += btnSend_Click;
            // 
            // btnClearChat
            // 
            btnClearChat.Location = new Point(31, 7);
            btnClearChat.Name = "btnClearChat";
            btnClearChat.Size = new Size(100, 32);
            btnClearChat.TabIndex = 1;
            btnClearChat.Text = "Clear chat";
            btnClearChat.UseVisualStyleBackColor = true;
            btnClearChat.Click += btnClearChat_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 720);
            Controls.Add(splitMain);
            Controls.Add(statusStrip);
            Controls.Add(pnlHeader);
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edureka Capstone";
            Load += MainForm_Load;
            pnlHeader.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            grpDocuments.ResumeLayout(false);
            flpDocButtons.ResumeLayout(false);
            grpConversation.ResumeLayout(false);
            pnlInput.ResumeLayout(false);
            pnlInputButtons.ResumeLayout(false);
            pnlInputButtons.PerformLayout();
            flpSendButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlHeader;
        private Label lblTitle;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripStatusLabel lblDocSummary;
        private SplitContainer splitMain;
        private GroupBox grpDocuments;
        private ListView lvDocuments;
        private ColumnHeader colName;
        private ColumnHeader colType;
        private ColumnHeader colSize;
        private Label lblDropHint;
        private FlowLayoutPanel flpDocButtons;
        private Button btnAddFiles;
        private Button btnRemoveFile;
        private Button btnClearFiles;
        private GroupBox grpConversation;
        private RichTextBox rtbConversation;
        private Panel pnlInput;
        private Edureka.Capstone.Forms.Controls.PromptRichTextBox txtPrompt;
        private FlowLayoutPanel flpAttachments;
        private Label lblPromptCaption;
        private Panel pnlInputButtons;
        private Label lblHint;
        private FlowLayoutPanel flpSendButtons;
        private Button btnSend;
        private Button btnClearChat;
    }
}
