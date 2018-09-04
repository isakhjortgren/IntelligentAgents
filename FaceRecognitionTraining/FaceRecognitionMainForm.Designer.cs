namespace FaceRecognitionTraining
{
    partial class FaceRecognitionMainForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.UpdatePrincipalComponentsButton = new System.Windows.Forms.Button();
            this.nbrOfEigenfacesTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1117, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(220, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load training dataset";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.LoadTrainingDataset);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1117, 62);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(220, 40);
            this.button2.TabIndex = 1;
            this.button2.Text = "Initialize eigenfaces";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.InitializeEigenfaces);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1117, 108);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(220, 38);
            this.button3.TabIndex = 2;
            this.button3.Text = "Train eigenfaces";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.TrainEigenfacesOnClick);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1099, 811);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1117, 179);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(220, 26);
            this.textBox1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1127, 156);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Training iterations:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1117, 211);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(220, 60);
            this.button4.TabIndex = 6;
            this.button4.Text = "Calculate eigenface orthogonality";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.CalculateEigenfaceOrthogonalityOnClick);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1117, 760);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(220, 63);
            this.button5.TabIndex = 7;
            this.button5.Text = "Exit Program";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.ExitProgramOnClick);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1116, 430);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(220, 26);
            this.textBox2.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1116, 404);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Learning rate";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1116, 462);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(220, 43);
            this.button6.TabIndex = 13;
            this.button6.Text = "Update learning rates";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.UpdateLearningRule);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1116, 556);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(220, 39);
            this.button7.TabIndex = 14;
            this.button7.Text = "Save eigenfaces";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.SaveTrainingData);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(1116, 511);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(220, 39);
            this.button8.TabIndex = 15;
            this.button8.Text = "Pause training";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.PauseTrainingOnClick);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(1116, 602);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(220, 35);
            this.button9.TabIndex = 16;
            this.button9.Text = "Load eigenfaces";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.LoadDataTest);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(1116, 666);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(220, 36);
            this.button10.TabIndex = 17;
            this.button10.Text = "Initialize dummy data";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.InitializeDummyData);
            // 
            // UpdatePrincipalComponentsButton
            // 
            this.UpdatePrincipalComponentsButton.Location = new System.Drawing.Point(1117, 355);
            this.UpdatePrincipalComponentsButton.Name = "UpdatePrincipalComponentsButton";
            this.UpdatePrincipalComponentsButton.Size = new System.Drawing.Size(219, 34);
            this.UpdatePrincipalComponentsButton.TabIndex = 18;
            this.UpdatePrincipalComponentsButton.Text = "Update nbr of eigenfaces";
            this.UpdatePrincipalComponentsButton.UseVisualStyleBackColor = true;
            this.UpdatePrincipalComponentsButton.Click += new System.EventHandler(this.UpdatePrincipalComponentsButton_Click);
            // 
            // nbrOfEigenfacesTextBox
            // 
            this.nbrOfEigenfacesTextBox.Location = new System.Drawing.Point(1117, 323);
            this.nbrOfEigenfacesTextBox.Name = "nbrOfEigenfacesTextBox";
            this.nbrOfEigenfacesTextBox.Size = new System.Drawing.Size(219, 26);
            this.nbrOfEigenfacesTextBox.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1116, 297);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "Number of Eigenfaces";
            // 
            // FaceRecognitionMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1349, 835);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nbrOfEigenfacesTextBox);
            this.Controls.Add(this.UpdatePrincipalComponentsButton);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "FaceRecognitionMainForm";
            this.Text = "Eigenface trainer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button UpdatePrincipalComponentsButton;
        private System.Windows.Forms.TextBox nbrOfEigenfacesTextBox;
        private System.Windows.Forms.Label label3;
    }
}

