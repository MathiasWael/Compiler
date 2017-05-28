using System.ComponentModel;
using System.Windows.Forms;

namespace Compiler
{
    partial class DelevopmentEnvironment
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
            this.BuildButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InputProgram
            // 
            this.InputProgram.Location = new System.Drawing.Point(16, 15);
            this.InputProgram.Margin = new System.Windows.Forms.Padding(4);
            this.InputProgram.Name = "InputProgram";
            this.InputProgram.Size = new System.Drawing.Size(808, 714);
            this.InputProgram.TabIndex = 0;
            this.InputProgram.Text = "";
            this.InputProgram.TextChanged += new System.EventHandler(this.InputProgram_TextChanged);
            // 
            // ErrorDisplay
            // 
            this.ErrorDisplay.FormattingEnabled = true;
            this.ErrorDisplay.ItemHeight = 16;
            this.ErrorDisplay.Location = new System.Drawing.Point(833, 22);
            this.ErrorDisplay.Margin = new System.Windows.Forms.Padding(4);
            this.ErrorDisplay.Name = "ErrorDisplay";
            this.ErrorDisplay.Size = new System.Drawing.Size(696, 484);
            this.ErrorDisplay.TabIndex = 1;
            // 
            // Run
            // 
            this.Run.Location = new System.Drawing.Point(879, 538);
            this.Run.Margin = new System.Windows.Forms.Padding(4);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(276, 168);
            this.Run.TabIndex = 2;
            this.Run.Text = "Compile";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // BuildButton
            // 
            this.BuildButton.Enabled = false;
            this.BuildButton.Location = new System.Drawing.Point(1215, 538);
            this.BuildButton.Name = "BuildButton";
            this.BuildButton.Size = new System.Drawing.Size(276, 168);
            this.BuildButton.TabIndex = 3;
            this.BuildButton.Text = "Build Game";
            this.BuildButton.UseVisualStyleBackColor = true;
            this.BuildButton.Click += new System.EventHandler(this.BuildButton_Click);
            // 
            // DelevopmentEnvironment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1543, 750);
            this.Controls.Add(this.BuildButton);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.ErrorDisplay);
            this.Controls.Add(this.InputProgram);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DelevopmentEnvironment";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private RichTextBox InputProgram;
        private ListBox ErrorDisplay;
        private Button Run;
        private Button BuildButton;
    }
}

