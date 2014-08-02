namespace QLearner
{
    partial class QLearner
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QLearner));
            this.Learn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.output = new System.Windows.Forms.RichTextBox();
            this.LearningRateLabel = new System.Windows.Forms.Label();
            this.DiscountFactorLabel = new System.Windows.Forms.Label();
            this.LearningTrialsLabel = new System.Windows.Forms.Label();
            this.Awaken = new System.Windows.Forms.Button();
            this.LearningTable = new System.Windows.Forms.DataGridView();
            this.Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HideOutput = new System.Windows.Forms.CheckBox();
            this.tabs = new System.Windows.Forms.TabControl();
            this.Setup = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.score = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.avgScore = new System.Windows.Forms.TextBox();
            this.Save = new System.Windows.Forms.PictureBox();
            this.Open = new System.Windows.Forms.PictureBox();
            this.QAlgoPlugins = new System.Windows.Forms.ComboBox();
            this.ClearPlugins = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.trialNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ExploreRateLabel = new System.Windows.Forms.Label();
            this.ExploreRate = new System.Windows.Forms.NumericUpDown();
            this.Settings = new System.Windows.Forms.Button();
            this.QStatePlugins = new System.Windows.Forms.ComboBox();
            this.LearningRate = new System.Windows.Forms.NumericUpDown();
            this.LearningTrials = new System.Windows.Forms.NumericUpDown();
            this.DiscountFactor = new System.Windows.Forms.NumericUpDown();
            this.About = new System.Windows.Forms.TabPage();
            this.AboutBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LearningTable)).BeginInit();
            this.tabs.SuspendLayout();
            this.Setup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Open)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExploreRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearningRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearningTrials)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscountFactor)).BeginInit();
            this.About.SuspendLayout();
            this.SuspendLayout();
            // 
            // Learn
            // 
            this.Learn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Learn.Location = new System.Drawing.Point(276, 123);
            this.Learn.Name = "Learn";
            this.Learn.Size = new System.Drawing.Size(117, 38);
            this.Learn.TabIndex = 0;
            this.Learn.Text = "Learn";
            this.Learn.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "QState:";
            // 
            // output
            // 
            this.output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.output.Location = new System.Drawing.Point(9, 168);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(776, 213);
            this.output.TabIndex = 3;
            this.output.Text = "";
            // 
            // LearningRateLabel
            // 
            this.LearningRateLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LearningRateLabel.AutoSize = true;
            this.LearningRateLabel.Location = new System.Drawing.Point(12, 92);
            this.LearningRateLabel.Name = "LearningRateLabel";
            this.LearningRateLabel.Size = new System.Drawing.Size(114, 20);
            this.LearningRateLabel.TabIndex = 4;
            this.LearningRateLabel.Text = "Learning Rate:";
            // 
            // DiscountFactorLabel
            // 
            this.DiscountFactorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DiscountFactorLabel.AutoSize = true;
            this.DiscountFactorLabel.Location = new System.Drawing.Point(206, 92);
            this.DiscountFactorLabel.Name = "DiscountFactorLabel";
            this.DiscountFactorLabel.Size = new System.Drawing.Size(126, 20);
            this.DiscountFactorLabel.TabIndex = 6;
            this.DiscountFactorLabel.Text = "Discount Factor:";
            // 
            // LearningTrialsLabel
            // 
            this.LearningTrialsLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LearningTrialsLabel.AutoSize = true;
            this.LearningTrialsLabel.Location = new System.Drawing.Point(594, 92);
            this.LearningTrialsLabel.Name = "LearningTrialsLabel";
            this.LearningTrialsLabel.Size = new System.Drawing.Size(91, 20);
            this.LearningTrialsLabel.TabIndex = 8;
            this.LearningTrialsLabel.Text = "Num. Trials:";
            // 
            // Awaken
            // 
            this.Awaken.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Awaken.Location = new System.Drawing.Point(399, 123);
            this.Awaken.Name = "Awaken";
            this.Awaken.Size = new System.Drawing.Size(117, 38);
            this.Awaken.TabIndex = 10;
            this.Awaken.Text = "Awaken";
            this.Awaken.UseVisualStyleBackColor = true;
            // 
            // LearningTable
            // 
            this.LearningTable.AllowUserToAddRows = false;
            this.LearningTable.AllowUserToDeleteRows = false;
            this.LearningTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LearningTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.LearningTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num,
            this.State,
            this.Action,
            this.QValue});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.LearningTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.LearningTable.Location = new System.Drawing.Point(9, 418);
            this.LearningTable.Name = "LearningTable";
            this.LearningTable.ReadOnly = true;
            this.LearningTable.RowHeadersVisible = false;
            this.LearningTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.LearningTable.RowTemplate.Height = 28;
            this.LearningTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LearningTable.Size = new System.Drawing.Size(776, 294);
            this.LearningTable.TabIndex = 11;
            // 
            // Num
            // 
            this.Num.HeaderText = "#";
            this.Num.Name = "Num";
            this.Num.ReadOnly = true;
            // 
            // State
            // 
            this.State.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // Action
            // 
            this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Action.HeaderText = "Action";
            this.Action.Name = "Action";
            this.Action.ReadOnly = true;
            // 
            // QValue
            // 
            this.QValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.QValue.HeaderText = "QValue";
            this.QValue.Name = "QValue";
            this.QValue.ReadOnly = true;
            // 
            // HideOutput
            // 
            this.HideOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HideOutput.AutoSize = true;
            this.HideOutput.BackColor = System.Drawing.Color.Transparent;
            this.HideOutput.Location = new System.Drawing.Point(664, 388);
            this.HideOutput.Name = "HideOutput";
            this.HideOutput.Size = new System.Drawing.Size(121, 24);
            this.HideOutput.TabIndex = 15;
            this.HideOutput.Text = "Hide Output";
            this.HideOutput.UseVisualStyleBackColor = false;
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.Setup);
            this.tabs.Controls.Add(this.About);
            this.tabs.Location = new System.Drawing.Point(3, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(801, 795);
            this.tabs.TabIndex = 16;
            // 
            // Setup
            // 
            this.Setup.Controls.Add(this.label5);
            this.Setup.Controls.Add(this.score);
            this.Setup.Controls.Add(this.label4);
            this.Setup.Controls.Add(this.avgScore);
            this.Setup.Controls.Add(this.Save);
            this.Setup.Controls.Add(this.Open);
            this.Setup.Controls.Add(this.QAlgoPlugins);
            this.Setup.Controls.Add(this.ClearPlugins);
            this.Setup.Controls.Add(this.label3);
            this.Setup.Controls.Add(this.trialNum);
            this.Setup.Controls.Add(this.label2);
            this.Setup.Controls.Add(this.ExploreRateLabel);
            this.Setup.Controls.Add(this.ExploreRate);
            this.Setup.Controls.Add(this.Settings);
            this.Setup.Controls.Add(this.QStatePlugins);
            this.Setup.Controls.Add(this.Learn);
            this.Setup.Controls.Add(this.HideOutput);
            this.Setup.Controls.Add(this.label1);
            this.Setup.Controls.Add(this.output);
            this.Setup.Controls.Add(this.LearningTable);
            this.Setup.Controls.Add(this.LearningRateLabel);
            this.Setup.Controls.Add(this.Awaken);
            this.Setup.Controls.Add(this.LearningRate);
            this.Setup.Controls.Add(this.LearningTrials);
            this.Setup.Controls.Add(this.DiscountFactorLabel);
            this.Setup.Controls.Add(this.LearningTrialsLabel);
            this.Setup.Controls.Add(this.DiscountFactor);
            this.Setup.Location = new System.Drawing.Point(4, 29);
            this.Setup.Name = "Setup";
            this.Setup.Padding = new System.Windows.Forms.Padding(3);
            this.Setup.Size = new System.Drawing.Size(793, 762);
            this.Setup.TabIndex = 0;
            this.Setup.Text = "QLearn";
            this.Setup.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(249, 726);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 20);
            this.label5.TabIndex = 31;
            this.label5.Text = "Score:";
            // 
            // score
            // 
            this.score.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.score.Location = new System.Drawing.Point(310, 722);
            this.score.Name = "score";
            this.score.ReadOnly = true;
            this.score.Size = new System.Drawing.Size(195, 26);
            this.score.TabIndex = 30;
            this.score.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(518, 726);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 20);
            this.label4.TabIndex = 29;
            this.label4.Text = "Avg Score:";
            // 
            // avgScore
            // 
            this.avgScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.avgScore.Location = new System.Drawing.Point(610, 723);
            this.avgScore.Name = "avgScore";
            this.avgScore.ReadOnly = true;
            this.avgScore.Size = new System.Drawing.Size(175, 26);
            this.avgScore.TabIndex = 28;
            this.avgScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.ErrorImage = ((System.Drawing.Image)(resources.GetObject("Save.ErrorImage")));
            this.Save.Image = ((System.Drawing.Image)(resources.GetObject("Save.Image")));
            this.Save.Location = new System.Drawing.Point(754, 132);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(20, 20);
            this.Save.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Save.TabIndex = 27;
            this.Save.TabStop = false;
            // 
            // Open
            // 
            this.Open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Open.ErrorImage = ((System.Drawing.Image)(resources.GetObject("Open.ErrorImage")));
            this.Open.Image = ((System.Drawing.Image)(resources.GetObject("Open.Image")));
            this.Open.Location = new System.Drawing.Point(717, 132);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(20, 20);
            this.Open.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Open.TabIndex = 26;
            this.Open.TabStop = false;
            // 
            // QAlgoPlugins
            // 
            this.QAlgoPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.QAlgoPlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QAlgoPlugins.FormattingEnabled = true;
            this.QAlgoPlugins.Location = new System.Drawing.Point(80, 52);
            this.QAlgoPlugins.Name = "QAlgoPlugins";
            this.QAlgoPlugins.Size = new System.Drawing.Size(570, 28);
            this.QAlgoPlugins.TabIndex = 25;
            // 
            // ClearPlugins
            // 
            this.ClearPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearPlugins.Location = new System.Drawing.Point(656, 48);
            this.ClearPlugins.Name = "ClearPlugins";
            this.ClearPlugins.Size = new System.Drawing.Size(126, 38);
            this.ClearPlugins.TabIndex = 24;
            this.ClearPlugins.Text = "Clear Plugins";
            this.ClearPlugins.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 725);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 23;
            this.label3.Text = "Trial #:";
            // 
            // trialNum
            // 
            this.trialNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trialNum.Location = new System.Drawing.Point(66, 722);
            this.trialNum.Name = "trialNum";
            this.trialNum.ReadOnly = true;
            this.trialNum.Size = new System.Drawing.Size(175, 26);
            this.trialNum.TabIndex = 22;
            this.trialNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 21;
            this.label2.Text = "QAlgo:";
            // 
            // ExploreRateLabel
            // 
            this.ExploreRateLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ExploreRateLabel.AutoSize = true;
            this.ExploreRateLabel.Location = new System.Drawing.Point(411, 92);
            this.ExploreRateLabel.Name = "ExploreRateLabel";
            this.ExploreRateLabel.Size = new System.Drawing.Size(105, 20);
            this.ExploreRateLabel.TabIndex = 18;
            this.ExploreRateLabel.Text = "Explore Rate:";
            // 
            // ExploreRate
            // 
            this.ExploreRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ExploreRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::QLearner.Properties.Settings.Default, "ExploreRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ExploreRate.DecimalPlaces = 2;
            this.ExploreRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ExploreRate.Location = new System.Drawing.Point(524, 89);
            this.ExploreRate.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ExploreRate.Name = "ExploreRate";
            this.ExploreRate.Size = new System.Drawing.Size(64, 26);
            this.ExploreRate.TabIndex = 19;
            this.ExploreRate.Value = global::QLearner.Properties.Settings.Default.ExploreRate;
            // 
            // Settings
            // 
            this.Settings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Settings.Location = new System.Drawing.Point(656, 6);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(126, 38);
            this.Settings.TabIndex = 17;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // QStatePlugins
            // 
            this.QStatePlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.QStatePlugins.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QStatePlugins.FormattingEnabled = true;
            this.QStatePlugins.Location = new System.Drawing.Point(80, 11);
            this.QStatePlugins.Name = "QStatePlugins";
            this.QStatePlugins.Size = new System.Drawing.Size(570, 28);
            this.QStatePlugins.TabIndex = 16;
            // 
            // LearningRate
            // 
            this.LearningRate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LearningRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::QLearner.Properties.Settings.Default, "LearningRate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LearningRate.DecimalPlaces = 2;
            this.LearningRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.LearningRate.Location = new System.Drawing.Point(132, 91);
            this.LearningRate.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LearningRate.Name = "LearningRate";
            this.LearningRate.Size = new System.Drawing.Size(68, 26);
            this.LearningRate.TabIndex = 5;
            this.LearningRate.Value = global::QLearner.Properties.Settings.Default.LearningRate;
            // 
            // LearningTrials
            // 
            this.LearningTrials.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LearningTrials.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::QLearner.Properties.Settings.Default, "LearningTrials", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.LearningTrials.Location = new System.Drawing.Point(694, 91);
            this.LearningTrials.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.LearningTrials.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LearningTrials.Name = "LearningTrials";
            this.LearningTrials.Size = new System.Drawing.Size(92, 26);
            this.LearningTrials.TabIndex = 9;
            this.LearningTrials.Value = global::QLearner.Properties.Settings.Default.LearningTrials;
            // 
            // DiscountFactor
            // 
            this.DiscountFactor.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DiscountFactor.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::QLearner.Properties.Settings.Default, "DiscountFactor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DiscountFactor.DecimalPlaces = 2;
            this.DiscountFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.DiscountFactor.Location = new System.Drawing.Point(339, 91);
            this.DiscountFactor.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiscountFactor.Name = "DiscountFactor";
            this.DiscountFactor.Size = new System.Drawing.Size(66, 26);
            this.DiscountFactor.TabIndex = 7;
            this.DiscountFactor.Value = global::QLearner.Properties.Settings.Default.DiscountFactor;
            // 
            // About
            // 
            this.About.Controls.Add(this.AboutBox);
            this.About.Location = new System.Drawing.Point(4, 29);
            this.About.Name = "About";
            this.About.Padding = new System.Windows.Forms.Padding(3);
            this.About.Size = new System.Drawing.Size(793, 762);
            this.About.TabIndex = 1;
            this.About.Text = "About";
            this.About.UseVisualStyleBackColor = true;
            // 
            // AboutBox
            // 
            this.AboutBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutBox.Location = new System.Drawing.Point(18, 22);
            this.AboutBox.Name = "AboutBox";
            this.AboutBox.Size = new System.Drawing.Size(757, 724);
            this.AboutBox.TabIndex = 0;
            this.AboutBox.Text = "";
            // 
            // QLearner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 798);
            this.Controls.Add(this.tabs);
            this.MinimumSize = new System.Drawing.Size(811, 571);
            this.Name = "QLearner";
            this.Text = "QLearner";
            ((System.ComponentModel.ISupportInitialize)(this.LearningTable)).EndInit();
            this.tabs.ResumeLayout(false);
            this.Setup.ResumeLayout(false);
            this.Setup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Open)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExploreRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearningRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LearningTrials)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiscountFactor)).EndInit();
            this.About.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Learn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox output;
        private System.Windows.Forms.Label LearningRateLabel;
        private System.Windows.Forms.NumericUpDown LearningRate;
        private System.Windows.Forms.NumericUpDown DiscountFactor;
        private System.Windows.Forms.Label DiscountFactorLabel;
        private System.Windows.Forms.NumericUpDown LearningTrials;
        private System.Windows.Forms.Label LearningTrialsLabel;
        private System.Windows.Forms.Button Awaken;
        private System.Windows.Forms.DataGridView LearningTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Num;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Action;
        private System.Windows.Forms.DataGridViewTextBoxColumn QValue;
        private System.Windows.Forms.CheckBox HideOutput;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage Setup;
        private System.Windows.Forms.TabPage About;
        private System.Windows.Forms.RichTextBox AboutBox;
        private System.Windows.Forms.ComboBox QStatePlugins;
        private System.Windows.Forms.Button Settings;
        private System.Windows.Forms.Label ExploreRateLabel;
        private System.Windows.Forms.NumericUpDown ExploreRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox trialNum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ClearPlugins;
        private System.Windows.Forms.ComboBox QAlgoPlugins;
        private System.Windows.Forms.PictureBox Open;
        private System.Windows.Forms.PictureBox Save;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox score;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox avgScore;
    }
}

