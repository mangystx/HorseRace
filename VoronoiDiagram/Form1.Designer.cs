namespace VoronoiDiagram
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			RandomBtn = new Button();
			Multiеhread = new CheckBox();
			ClearBtn = new Button();
			SuspendLayout();
			// 
			// RandomBtn
			// 
			RandomBtn.Location = new Point(713, 12);
			RandomBtn.Name = "RandomBtn";
			RandomBtn.Size = new Size(75, 23);
			RandomBtn.TabIndex = 1;
			RandomBtn.Text = "Random";
			RandomBtn.UseVisualStyleBackColor = true;
			RandomBtn.Click += RandomBtn_Click;
			// 
			// Multiеhread
			// 
			Multiеhread.AutoSize = true;
			Multiеhread.BackColor = Color.Transparent;
			Multiеhread.Location = new Point(12, 12);
			Multiеhread.Name = "Multiеhread";
			Multiеhread.Size = new Size(88, 19);
			Multiеhread.TabIndex = 2;
			Multiеhread.Text = "Multithread";
			Multiеhread.UseVisualStyleBackColor = false;
			Multiеhread.CheckedChanged += Multiеhread_CheckedChanged;
			// 
			// ClearBtn
			// 
			ClearBtn.Location = new Point(713, 415);
			ClearBtn.Name = "ClearBtn";
			ClearBtn.Size = new Size(75, 23);
			ClearBtn.TabIndex = 3;
			ClearBtn.Text = "Clear";
			ClearBtn.UseVisualStyleBackColor = true;
			ClearBtn.Click += ClearBtn_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(ClearBtn);
			Controls.Add(Multiеhread);
			Controls.Add(RandomBtn);
			Name = "Form1";
			Text = "Form1";
			Paint += Form1_Paint;
			MouseClick += Form1_MouseClick;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Button RandomBtn;
		private CheckBox Multiеhread;
		private Button ClearBtn;
	}
}
