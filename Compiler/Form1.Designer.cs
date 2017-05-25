using System.ComponentModel;
using System.Windows.Forms;

namespace Compiler
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.InputProgram = new System.Windows.Forms.RichTextBox();
            this.ErrorDisplay = new System.Windows.Forms.ListBox();
            this.Run = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InputProgram
            // 
            this.InputProgram.Location = new System.Drawing.Point(12, 12);
            this.InputProgram.Name = "InputProgram";
            this.InputProgram.Size = new System.Drawing.Size(607, 581);
            this.InputProgram.TabIndex = 0;
            this.InputProgram.Text = "";
            // 
            // ErrorDisplay
            // 
            this.ErrorDisplay.FormattingEnabled = true;
            this.ErrorDisplay.Location = new System.Drawing.Point(625, 12);
            this.ErrorDisplay.Name = "ErrorDisplay";
            this.ErrorDisplay.Size = new System.Drawing.Size(514, 186);
            this.ErrorDisplay.TabIndex = 1;
            // 
            // Run
            // 
            this.Run.Location = new System.Drawing.Point(625, 204);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(514, 389);
            this.Run.TabIndex = 2;
            this.Run.Text = "LETS GO :KAJLUL:";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 605);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.ErrorDisplay);
            this.Controls.Add(this.InputProgram);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox InputProgram;
        private ListBox ErrorDisplay;
        private Button Run;
    }
}

