namespace Chess
{
    partial class ChessBoardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChessBoardForm));
            toolStrip1 = new ToolStrip();
            undoButton = new ToolStripButton();
            redoButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            replayButton = new ToolStripButton();
            toolStripSeparator2 = new ToolStripSeparator();
            resetButton = new ToolStripButton();
            chessBoardControl = new ChessBoardControl();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { undoButton, redoButton, toolStripSeparator1, replayButton, toolStripSeparator2, resetButton });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(884, 34);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // undoButton
            // 
            undoButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            undoButton.Image = (Image)resources.GetObject("undoButton.Image");
            undoButton.ImageTransparentColor = Color.Magenta;
            undoButton.Name = "undoButton";
            undoButton.Size = new Size(60, 29);
            undoButton.Text = "Undo";
            undoButton.Click += undoButton_Click;
            // 
            // redoButton
            // 
            redoButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            redoButton.Image = (Image)resources.GetObject("redoButton.Image");
            redoButton.ImageTransparentColor = Color.Magenta;
            redoButton.Name = "redoButton";
            redoButton.Size = new Size(57, 29);
            redoButton.Text = "Redo";
            redoButton.Click += redoButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 34);
            // 
            // replayButton
            // 
            replayButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            replayButton.Image = (Image)resources.GetObject("replayButton.Image");
            replayButton.ImageTransparentColor = Color.Magenta;
            replayButton.Name = "replayButton";
            replayButton.Size = new Size(68, 29);
            replayButton.Text = "Replay";
            replayButton.Click += replayButton_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 34);
            // 
            // resetButton
            // 
            resetButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            resetButton.Image = (Image)resources.GetObject("resetButton.Image");
            resetButton.ImageTransparentColor = Color.Magenta;
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(58, 29);
            resetButton.Text = "Reset";
            resetButton.Click += resetButton_Click;
            // 
            // chessBoardControl
            // 
            chessBoardControl.ChessBoard = null;
            chessBoardControl.Dock = DockStyle.Fill;
            chessBoardControl.Location = new Point(0, 34);
            chessBoardControl.Name = "chessBoardControl";
            chessBoardControl.Size = new Size(884, 791);
            chessBoardControl.TabIndex = 1;
            chessBoardControl.Text = "chessBoardControl";
            // 
            // ChessBoardForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 825);
            Controls.Add(chessBoardControl);
            Controls.Add(toolStrip1);
            Name = "ChessBoardForm";
            Text = "Szachy";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ChessBoardControl chessBoardControl;
        private ToolStripButton undoButton;
        private ToolStripButton redoButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton replayButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton resetButton;
    }
}