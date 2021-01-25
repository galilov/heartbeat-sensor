
namespace WinFormsApp
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSnapshot = new System.Windows.Forms.Button();
            this.pbx = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pbx)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnSnapshot);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 438);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1898, 76);
            this.panel1.TabIndex = 2;
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.btnStop.Location = new System.Drawing.Point(177, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(168, 62);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSnapshot
            // 
            this.btnSnapshot.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.btnSnapshot.Location = new System.Drawing.Point(3, 3);
            this.btnSnapshot.Name = "btnSnapshot";
            this.btnSnapshot.Size = new System.Drawing.Size(168, 62);
            this.btnSnapshot.TabIndex = 0;
            this.btnSnapshot.Text = "Start";
            this.btnSnapshot.UseVisualStyleBackColor = true;
            this.btnSnapshot.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pbx
            // 
            this.pbx.BackColor = System.Drawing.Color.Black;
            this.pbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbx.Location = new System.Drawing.Point(0, 0);
            this.pbx.Name = "pbx";
            this.pbx.Size = new System.Drawing.Size(1898, 438);
            this.pbx.TabIndex = 3;
            this.pbx.TabStop = false;
            this.pbx.Paint += new System.Windows.Forms.PaintEventHandler(this.pbx_Paint);
            this.pbx.Resize += new System.EventHandler(this.pbx_Resize);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1898, 514);
            this.Controls.Add(this.pbx);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Signal graph";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pbx)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.PictureBox pbx;

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSnapshot;
        private System.Windows.Forms.Button btnStop;
    }
}

