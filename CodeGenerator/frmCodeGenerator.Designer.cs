namespace CodeGenerator
{
	partial class frmCodeGenerator
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
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.btnSQLCode = new System.Windows.Forms.Button();
            this.btnCSCode = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Until Microsoft puts this in VS...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Table name:";
            // 
            // txtTableName
            // 
            this.txtTableName.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.txtTableName.Location = new System.Drawing.Point(334, 18);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(100, 20);
            this.txtTableName.TabIndex = 2;
            // 
            // btnSQLCode
            // 
            this.btnSQLCode.Location = new System.Drawing.Point(265, 44);
            this.btnSQLCode.Name = "btnSQLCode";
            this.btnSQLCode.Size = new System.Drawing.Size(75, 23);
            this.btnSQLCode.TabIndex = 3;
            this.btnSQLCode.Text = "SQL Code";
            this.btnSQLCode.UseVisualStyleBackColor = true;
            this.btnSQLCode.Click += new System.EventHandler(this.btnSQLCode_Click);
            // 
            // btnCSCode
            // 
            this.btnCSCode.Location = new System.Drawing.Point(359, 44);
            this.btnCSCode.Name = "btnCSCode";
            this.btnCSCode.Size = new System.Drawing.Size(75, 23);
            this.btnCSCode.TabIndex = 4;
            this.btnCSCode.Text = "C# Code";
            this.btnCSCode.UseVisualStyleBackColor = true;
            this.btnCSCode.Click += new System.EventHandler(this.btnCSCode_Click);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(15, 73);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCode.Size = new System.Drawing.Size(419, 272);
            this.txtCode.TabIndex = 5;
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Location = new System.Drawing.Point(15, 352);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(108, 23);
            this.btnCopyToClipboard.TabIndex = 6;
            this.btnCopyToClipboard.Text = "Copy to clipboard";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // frmCodeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 388);
            this.Controls.Add(this.btnCopyToClipboard);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.btnCSCode);
            this.Controls.Add(this.btnSQLCode);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmCodeGenerator";
            this.Text = "doLogic Code Generator";
            this.Load += new System.EventHandler(this.frmCodeGenerator_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtTableName;
		private System.Windows.Forms.Button btnSQLCode;
		private System.Windows.Forms.Button btnCSCode;
		private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnCopyToClipboard;
	}
}

