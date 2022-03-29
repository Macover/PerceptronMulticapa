
namespace PerceptronMulticapa
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEmpezar = new System.Windows.Forms.Button();
            this.btnGetPatrones = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEmpezar
            // 
            this.btnEmpezar.Location = new System.Drawing.Point(118, 39);
            this.btnEmpezar.Name = "btnEmpezar";
            this.btnEmpezar.Size = new System.Drawing.Size(75, 23);
            this.btnEmpezar.TabIndex = 0;
            this.btnEmpezar.Text = "Entrenar";
            this.btnEmpezar.UseVisualStyleBackColor = true;
            this.btnEmpezar.Click += new System.EventHandler(this.btnEmpezar_Click);
            // 
            // btnGetPatrones
            // 
            this.btnGetPatrones.Enabled = false;
            this.btnGetPatrones.Location = new System.Drawing.Point(118, 110);
            this.btnGetPatrones.Name = "btnGetPatrones";
            this.btnGetPatrones.Size = new System.Drawing.Size(75, 36);
            this.btnGetPatrones.TabIndex = 1;
            this.btnGetPatrones.Text = "Pedir patrones";
            this.btnGetPatrones.UseVisualStyleBackColor = true;
            this.btnGetPatrones.Click += new System.EventHandler(this.btnGetPatrones_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 193);
            this.Controls.Add(this.btnGetPatrones);
            this.Controls.Add(this.btnEmpezar);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEmpezar;
        private System.Windows.Forms.Button btnGetPatrones;
    }
}

