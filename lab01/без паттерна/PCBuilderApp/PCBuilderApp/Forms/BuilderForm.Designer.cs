namespace PCBuilderApp.Forms
{
    partial class BuilderForm
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

        private void InitializeComponent()
        {
            this.lblStep = new System.Windows.Forms.Label();
            this.lblComponent = new System.Windows.Forms.Label();
            this.cboComponent = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            this.lblStep.AutoSize = true;
            this.lblStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblStep.Location = new System.Drawing.Point(20, 20);
            this.lblStep.Name = "lblStep";
            this.lblStep.Text = "Шаг 1 из 7";

            this.lblComponent.AutoSize = true;
            this.lblComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblComponent.Location = new System.Drawing.Point(20, 60);
            this.lblComponent.Name = "lblComponent";
            this.lblComponent.Text = "Выберите компонент:";

            this.cboComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.cboComponent.Location = new System.Drawing.Point(20, 90);
            this.cboComponent.Name = "cboComponent";
            this.cboComponent.Size = new System.Drawing.Size(400, 26);

            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBack.Location = new System.Drawing.Point(20, 140);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(100, 35);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "Назад";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Enabled = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnNext.Location = new System.Drawing.Point(320, 140);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(100, 35);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Далее";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);

            this.btnFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFinish.Location = new System.Drawing.Point(320, 140);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(100, 35);
            this.btnFinish.TabIndex = 2;
            this.btnFinish.Text = "Завершить";
            this.btnFinish.UseVisualStyleBackColor = true;
            this.btnFinish.Visible = false;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);

            this.pnlSummary.Location = new System.Drawing.Point(20, 60);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(400, 300);
            this.pnlSummary.TabIndex = 3;
            this.pnlSummary.Visible = false;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 400);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.lblComponent);
            this.Controls.Add(this.cboComponent);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.pnlSummary);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "BuilderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сборка ПК";
            this.Load += new System.EventHandler(this.BuilderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.Label lblComponent;
        private System.Windows.Forms.ComboBox cboComponent;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Panel pnlSummary;
    }
}