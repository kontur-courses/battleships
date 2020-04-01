namespace Battleships.Views
{
    partial class FinishedControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.winnerLabel = new System.Windows.Forms.Label();
            this.humanFieldControl = new Battleships.Views.FieldControl();
            this.aiFieldControl = new Battleships.Views.FieldControl();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel.Controls.Add(this.winnerLabel, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.humanFieldControl, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.aiFieldControl, 2, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(800, 600);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // winnerLabel
            // 
            this.winnerLabel.AutoSize = true;
            this.winnerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.winnerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.winnerLabel.Location = new System.Drawing.Point(303, 0);
            this.winnerLabel.Name = "winnerLabel";
            this.winnerLabel.Size = new System.Drawing.Size(194, 100);
            this.winnerLabel.TabIndex = 0;
            this.winnerLabel.Text = "Победил";
            this.winnerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // humanFieldControl
            // 
            this.humanFieldControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.humanFieldControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.humanFieldControl.Location = new System.Drawing.Point(3, 103);
            this.humanFieldControl.Name = "humanFieldControl";
            this.humanFieldControl.Size = new System.Drawing.Size(294, 294);
            this.humanFieldControl.TabIndex = 1;
            // 
            // aiFieldControl
            // 
            this.aiFieldControl.BackColor = System.Drawing.SystemColors.ControlDark;
            this.aiFieldControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aiFieldControl.Location = new System.Drawing.Point(503, 103);
            this.aiFieldControl.Name = "aiFieldControl";
            this.aiFieldControl.Size = new System.Drawing.Size(294, 294);
            this.aiFieldControl.TabIndex = 2;
            // 
            // FinishedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "FinishedControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label winnerLabel;
        private FieldControl humanFieldControl;
        private FieldControl aiFieldControl;
    }
}
