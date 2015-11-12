namespace МЕТРОЛОГИЯ
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.LabelDescription = new System.Windows.Forms.Label();
            this.buttonLoadPHP = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.ButtonAnalyze = new System.Windows.Forms.Button();
            this.TextBoxStatus = new System.Windows.Forms.TextBox();
            this.TextBoxSource = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // LabelDescription
            // 
            this.LabelDescription.AutoSize = true;
            this.LabelDescription.Font = new System.Drawing.Font("Gill Sans Ultra Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelDescription.Location = new System.Drawing.Point(225, 9);
            this.LabelDescription.Name = "LabelDescription";
            this.LabelDescription.Size = new System.Drawing.Size(134, 20);
            this.LabelDescription.TabIndex = 1;
            this.LabelDescription.Text = "SOURCE CODE";
            // 
            // buttonLoadPHP
            // 
            this.buttonLoadPHP.Font = new System.Drawing.Font("Goudy Stout", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoadPHP.Location = new System.Drawing.Point(12, 606);
            this.buttonLoadPHP.Name = "buttonLoadPHP";
            this.buttonLoadPHP.Size = new System.Drawing.Size(582, 62);
            this.buttonLoadPHP.TabIndex = 2;
            this.buttonLoadPHP.Text = "LOAD PHP SCRIPT";
            this.buttonLoadPHP.UseVisualStyleBackColor = true;
            this.buttonLoadPHP.Click += new System.EventHandler(this.buttonLoadPHP_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "PHP Source code|*.php";
            // 
            // ButtonAnalyze
            // 
            this.ButtonAnalyze.Font = new System.Drawing.Font("Goudy Stout", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonAnalyze.Location = new System.Drawing.Point(708, 35);
            this.ButtonAnalyze.Name = "ButtonAnalyze";
            this.ButtonAnalyze.Size = new System.Drawing.Size(327, 62);
            this.ButtonAnalyze.TabIndex = 3;
            this.ButtonAnalyze.Text = "ANALYZE";
            this.ButtonAnalyze.UseVisualStyleBackColor = true;
            this.ButtonAnalyze.Click += new System.EventHandler(this.ButtonAnalyze_Click);
            // 
            // TextBoxStatus
            // 
            this.TextBoxStatus.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextBoxStatus.Location = new System.Drawing.Point(708, 115);
            this.TextBoxStatus.Multiline = true;
            this.TextBoxStatus.Name = "TextBoxStatus";
            this.TextBoxStatus.ReadOnly = true;
            this.TextBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxStatus.Size = new System.Drawing.Size(327, 416);
            this.TextBoxStatus.TabIndex = 4;
            this.TextBoxStatus.WordWrap = false;
            // 
            // TextBoxSource
            // 
            this.TextBoxSource.Location = new System.Drawing.Point(12, 32);
            this.TextBoxSource.Name = "TextBoxSource";
            this.TextBoxSource.Size = new System.Drawing.Size(582, 550);
            this.TextBoxSource.TabIndex = 5;
            this.TextBoxSource.TabStop = false;
            this.TextBoxSource.Text = "";
            this.TextBoxSource.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBoxSource_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 680);
            this.Controls.Add(this.TextBoxSource);
            this.Controls.Add(this.TextBoxStatus);
            this.Controls.Add(this.ButtonAnalyze);
            this.Controls.Add(this.buttonLoadPHP);
            this.Controls.Add(this.LabelDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "МЕТРОЛОГИЯ, АНАЛИЗ PHP - КОДА, МЕТРИКА МАККЕЙБА БОГДАН";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelDescription;
        private System.Windows.Forms.Button buttonLoadPHP;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button ButtonAnalyze;
        private System.Windows.Forms.TextBox TextBoxStatus;
        public System.Windows.Forms.RichTextBox TextBoxSource;
    }
}

