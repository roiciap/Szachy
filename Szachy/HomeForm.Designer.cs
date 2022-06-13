namespace Szachy
{
    partial class HomeForm
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
            this.GraczVsGracz = new System.Windows.Forms.Button();
            this.GraczVsKomputer = new System.Windows.Forms.Button();
            this.KomputerVsGracz = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GraczVsGracz
            // 
            this.GraczVsGracz.Location = new System.Drawing.Point(300, 37);
            this.GraczVsGracz.Name = "GraczVsGracz";
            this.GraczVsGracz.Size = new System.Drawing.Size(145, 65);
            this.GraczVsGracz.TabIndex = 0;
            this.GraczVsGracz.Text = "Gracz vs Gracz";
            this.GraczVsGracz.UseVisualStyleBackColor = true;
            // 
            // GraczVsKomputer
            // 
            this.GraczVsKomputer.Location = new System.Drawing.Point(300, 126);
            this.GraczVsKomputer.Name = "GraczVsKomputer";
            this.GraczVsKomputer.Size = new System.Drawing.Size(145, 65);
            this.GraczVsKomputer.TabIndex = 1;
            this.GraczVsKomputer.Text = "Gracz vs Komputer";
            this.GraczVsKomputer.UseVisualStyleBackColor = true;
            // 
            // KomputerVsGracz
            // 
            this.KomputerVsGracz.Location = new System.Drawing.Point(300, 217);
            this.KomputerVsGracz.Name = "KomputerVsGracz";
            this.KomputerVsGracz.Size = new System.Drawing.Size(145, 65);
            this.KomputerVsGracz.TabIndex = 2;
            this.KomputerVsGracz.Text = "Komputer vs Gracz";
            this.KomputerVsGracz.UseVisualStyleBackColor = true;
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.KomputerVsGracz);
            this.Controls.Add(this.GraczVsKomputer);
            this.Controls.Add(this.GraczVsGracz);
            this.Name = "HomeForm";
            this.Text = "HomeForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button GraczVsGracz;
        private System.Windows.Forms.Button GraczVsKomputer;
        private System.Windows.Forms.Button KomputerVsGracz;
    }
}