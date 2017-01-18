namespace Engine.Core.GUI
{
    partial class MapEditor
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbSet = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSeed = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.lblTile = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstTileSets = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstLayers = new System.Windows.Forms.ListBox();
            this.radTiles = new System.Windows.Forms.RadioButton();
            this.radBlocks = new System.Windows.Forms.RadioButton();
            this.radSpawn = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lstTriggers = new System.Windows.Forms.ListBox();
            this.triggerbox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTriggerName = new System.Windows.Forms.TextBox();
            this.txtTriggerData = new System.Windows.Forms.TextBox();
            this.btnSaveTrigger = new System.Windows.Forms.Button();
            this.btnCreateTrigger = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSet)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.triggerbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(616, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripMenuItem1,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(107, 6);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 24);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 429);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(608, 403);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tile Editor";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pbSet);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(402, 397);
            this.panel1.TabIndex = 1;
            // 
            // pbSet
            // 
            this.pbSet.Location = new System.Drawing.Point(5, 3);
            this.pbSet.Name = "pbSet";
            this.pbSet.Size = new System.Drawing.Size(266, 397);
            this.pbSet.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbSet.TabIndex = 0;
            this.pbSet.TabStop = false;
            this.pbSet.Click += new System.EventHandler(this.pbSet_Click);
            this.pbSet.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbSet_MouseDown);
            this.pbSet.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbSet_MouseMove);
            this.pbSet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbSet_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radSpawn);
            this.groupBox1.Controls.Add(this.radBlocks);
            this.groupBox1.Controls.Add(this.radTiles);
            this.groupBox1.Controls.Add(this.btnSeed);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(405, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 397);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // btnSeed
            // 
            this.btnSeed.Location = new System.Drawing.Point(9, 231);
            this.btnSeed.Name = "btnSeed";
            this.btnSeed.Size = new System.Drawing.Size(75, 23);
            this.btnSeed.TabIndex = 7;
            this.btnSeed.Text = "Seed Layer";
            this.btnSeed.UseVisualStyleBackColor = true;
            this.btnSeed.Click += new System.EventHandler(this.btnSeed_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.pbPreview);
            this.groupBox4.Controls.Add(this.lblTile);
            this.groupBox4.Location = new System.Drawing.Point(170, 376);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(76, 88);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Selected";
            this.groupBox4.Visible = false;
            // 
            // pbPreview
            // 
            this.pbPreview.Location = new System.Drawing.Point(6, 19);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(64, 64);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPreview.TabIndex = 3;
            this.pbPreview.TabStop = false;
            // 
            // lblTile
            // 
            this.lblTile.AutoSize = true;
            this.lblTile.Location = new System.Drawing.Point(15, 40);
            this.lblTile.Name = "lblTile";
            this.lblTile.Size = new System.Drawing.Size(55, 13);
            this.lblTile.TabIndex = 5;
            this.lblTile.Text = "Selected: ";
            this.lblTile.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstTileSets);
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(188, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tile Sets";
            // 
            // lstTileSets
            // 
            this.lstTileSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTileSets.FormattingEnabled = true;
            this.lstTileSets.Location = new System.Drawing.Point(3, 16);
            this.lstTileSets.Name = "lstTileSets";
            this.lstTileSets.Size = new System.Drawing.Size(182, 81);
            this.lstTileSets.TabIndex = 0;
            this.lstTileSets.SelectedIndexChanged += new System.EventHandler(this.lstTileSets_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstLayers);
            this.groupBox2.Location = new System.Drawing.Point(6, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(188, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layers";
            // 
            // lstLayers
            // 
            this.lstLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLayers.FormattingEnabled = true;
            this.lstLayers.Location = new System.Drawing.Point(3, 16);
            this.lstLayers.Name = "lstLayers";
            this.lstLayers.Size = new System.Drawing.Size(182, 81);
            this.lstLayers.TabIndex = 0;
            this.lstLayers.SelectedIndexChanged += new System.EventHandler(this.lstLayers_SelectedIndexChanged);
            // 
            // radTiles
            // 
            this.radTiles.AutoSize = true;
            this.radTiles.Checked = true;
            this.radTiles.Location = new System.Drawing.Point(9, 260);
            this.radTiles.Name = "radTiles";
            this.radTiles.Size = new System.Drawing.Size(68, 17);
            this.radTiles.TabIndex = 8;
            this.radTiles.TabStop = true;
            this.radTiles.Text = "Edit Tiles";
            this.radTiles.UseVisualStyleBackColor = true;
            // 
            // radBlocks
            // 
            this.radBlocks.AutoSize = true;
            this.radBlocks.Location = new System.Drawing.Point(9, 283);
            this.radBlocks.Name = "radBlocks";
            this.radBlocks.Size = new System.Drawing.Size(78, 17);
            this.radBlocks.TabIndex = 9;
            this.radBlocks.Text = "Edit Blocks";
            this.radBlocks.UseVisualStyleBackColor = true;
            // 
            // radSpawn
            // 
            this.radSpawn.AutoSize = true;
            this.radSpawn.Location = new System.Drawing.Point(9, 306);
            this.radSpawn.Name = "radSpawn";
            this.radSpawn.Size = new System.Drawing.Size(123, 17);
            this.radSpawn.TabIndex = 10;
            this.radSpawn.Text = "Edit Spawn Location";
            this.radSpawn.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.triggerbox);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(608, 403);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Triggers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lstTriggers);
            this.groupBox5.Location = new System.Drawing.Point(8, 6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(147, 391);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Trigger List";
            // 
            // lstTriggers
            // 
            this.lstTriggers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTriggers.FormattingEnabled = true;
            this.lstTriggers.Location = new System.Drawing.Point(3, 16);
            this.lstTriggers.Name = "lstTriggers";
            this.lstTriggers.Size = new System.Drawing.Size(141, 372);
            this.lstTriggers.TabIndex = 0;
            this.lstTriggers.SelectedIndexChanged += new System.EventHandler(this.lstTriggers_SelectedIndexChanged);
            // 
            // triggerbox
            // 
            this.triggerbox.Controls.Add(this.btnCreateTrigger);
            this.triggerbox.Controls.Add(this.btnSaveTrigger);
            this.triggerbox.Controls.Add(this.txtTriggerData);
            this.triggerbox.Controls.Add(this.txtTriggerName);
            this.triggerbox.Controls.Add(this.label3);
            this.triggerbox.Controls.Add(this.label1);
            this.triggerbox.Location = new System.Drawing.Point(161, 6);
            this.triggerbox.Name = "triggerbox";
            this.triggerbox.Size = new System.Drawing.Size(439, 391);
            this.triggerbox.TabIndex = 1;
            this.triggerbox.TabStop = false;
            this.triggerbox.Text = "Trigger: #";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(71, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Data:";
            // 
            // txtTriggerName
            // 
            this.txtTriggerName.Location = new System.Drawing.Point(110, 116);
            this.txtTriggerName.Name = "txtTriggerName";
            this.txtTriggerName.Size = new System.Drawing.Size(253, 20);
            this.txtTriggerName.TabIndex = 5;
            // 
            // txtTriggerData
            // 
            this.txtTriggerData.Location = new System.Drawing.Point(110, 142);
            this.txtTriggerData.Multiline = true;
            this.txtTriggerData.Name = "txtTriggerData";
            this.txtTriggerData.Size = new System.Drawing.Size(253, 88);
            this.txtTriggerData.TabIndex = 7;
            // 
            // btnSaveTrigger
            // 
            this.btnSaveTrigger.Location = new System.Drawing.Point(288, 236);
            this.btnSaveTrigger.Name = "btnSaveTrigger";
            this.btnSaveTrigger.Size = new System.Drawing.Size(75, 23);
            this.btnSaveTrigger.TabIndex = 8;
            this.btnSaveTrigger.Text = "&Save";
            this.btnSaveTrigger.UseVisualStyleBackColor = true;
            this.btnSaveTrigger.Click += new System.EventHandler(this.btnSaveTrigger_Click);
            // 
            // btnCreateTrigger
            // 
            this.btnCreateTrigger.Location = new System.Drawing.Point(110, 236);
            this.btnCreateTrigger.Name = "btnCreateTrigger";
            this.btnCreateTrigger.Size = new System.Drawing.Size(75, 23);
            this.btnCreateTrigger.TabIndex = 9;
            this.btnCreateTrigger.Text = "Create";
            this.btnCreateTrigger.UseVisualStyleBackColor = true;
            this.btnCreateTrigger.Click += new System.EventHandler(this.btnCreateTrigger_Click);
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 453);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MapEditor";
            this.Text = "MapEditor";
            this.Load += new System.EventHandler(this.MapEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSet)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.triggerbox.ResumeLayout(false);
            this.triggerbox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbSet;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstTileSets;
        private System.Windows.Forms.ListBox lstLayers;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Label lblTile;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.Button btnSeed;
        public System.Windows.Forms.RadioButton radSpawn;
        public System.Windows.Forms.RadioButton radBlocks;
        public System.Windows.Forms.RadioButton radTiles;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.ListBox lstTriggers;
        private System.Windows.Forms.GroupBox triggerbox;
        private System.Windows.Forms.Button btnCreateTrigger;
        private System.Windows.Forms.Button btnSaveTrigger;
        private System.Windows.Forms.TextBox txtTriggerData;
        private System.Windows.Forms.TextBox txtTriggerName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
    }
}