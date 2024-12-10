namespace Kursovai
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PASSWORD = new System.Windows.Forms.TextBox();
            this.LOGIN = new System.Windows.Forms.TextBox();
            this.VOITI = new System.Windows.Forms.Button();
            this.ZAREG = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(139, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Логин";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(130, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Пароль";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(117, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 25);
            this.label3.TabIndex = 2;
            // 
            // PASSWORD
            // 
            this.PASSWORD.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PASSWORD.Location = new System.Drawing.Point(98, 154);
            this.PASSWORD.Name = "PASSWORD";
            this.PASSWORD.Size = new System.Drawing.Size(159, 31);
            this.PASSWORD.TabIndex = 3;
            // 
            // LOGIN
            // 
            this.LOGIN.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LOGIN.Location = new System.Drawing.Point(98, 72);
            this.LOGIN.Name = "LOGIN";
            this.LOGIN.Size = new System.Drawing.Size(159, 31);
            this.LOGIN.TabIndex = 4;
            // 
            // VOITI
            // 
            this.VOITI.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.VOITI.Location = new System.Drawing.Point(59, 255);
            this.VOITI.Name = "VOITI";
            this.VOITI.Size = new System.Drawing.Size(226, 45);
            this.VOITI.TabIndex = 5;
            this.VOITI.Text = "Войти";
            this.VOITI.UseVisualStyleBackColor = true;
            this.VOITI.Click += new System.EventHandler(this.VOITI_Click);
            // 
            // ZAREG
            // 
            this.ZAREG.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ZAREG.Location = new System.Drawing.Point(59, 204);
            this.ZAREG.Name = "ZAREG";
            this.ZAREG.Size = new System.Drawing.Size(226, 45);
            this.ZAREG.TabIndex = 6;
            this.ZAREG.Text = "Зарегистрироваться";
            this.ZAREG.UseVisualStyleBackColor = true;
            this.ZAREG.Click += new System.EventHandler(this.ZAREG_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 312);
            this.Controls.Add(this.ZAREG);
            this.Controls.Add(this.VOITI);
            this.Controls.Add(this.LOGIN);
            this.Controls.Add(this.PASSWORD);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Авторизация";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PASSWORD;
        private System.Windows.Forms.TextBox LOGIN;
        private System.Windows.Forms.Button VOITI;
        private System.Windows.Forms.Button ZAREG;
    }
}