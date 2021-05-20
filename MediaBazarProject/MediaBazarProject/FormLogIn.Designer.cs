namespace MediaBazarProject
{
    partial class FormLogIn
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogIn));
            this.panelLogIn = new Bunifu.Framework.UI.BunifuGradientPanel();
            this.btnCloseApp = new Bunifu.Framework.UI.BunifuFlatButton();
            this.label2 = new System.Windows.Forms.Label();
            this.lblRememberMe = new System.Windows.Forms.Label();
            this.cbxRememberMe = new Bunifu.Framework.UI.BunifuCheckbox();
            this.tbPassword = new Bunifu.Framework.UI.BunifuTextbox();
            this.tbUsername = new Bunifu.Framework.UI.BunifuTextbox();
            this.btnLogIn = new Bunifu.Framework.UI.BunifuThinButton2();
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.panelLogIn.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLogIn
            // 
            this.panelLogIn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelLogIn.BackgroundImage")));
            this.panelLogIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelLogIn.Controls.Add(this.btnCloseApp);
            this.panelLogIn.Controls.Add(this.label2);
            this.panelLogIn.Controls.Add(this.lblRememberMe);
            this.panelLogIn.Controls.Add(this.cbxRememberMe);
            this.panelLogIn.Controls.Add(this.tbPassword);
            this.panelLogIn.Controls.Add(this.tbUsername);
            this.panelLogIn.Controls.Add(this.btnLogIn);
            this.panelLogIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLogIn.GradientBottomLeft = System.Drawing.Color.CornflowerBlue;
            this.panelLogIn.GradientBottomRight = System.Drawing.Color.Orchid;
            this.panelLogIn.GradientTopLeft = System.Drawing.Color.Salmon;
            this.panelLogIn.GradientTopRight = System.Drawing.Color.White;
            this.panelLogIn.Location = new System.Drawing.Point(0, 0);
            this.panelLogIn.Name = "panelLogIn";
            this.panelLogIn.Quality = 10;
            this.panelLogIn.Size = new System.Drawing.Size(532, 621);
            this.panelLogIn.TabIndex = 0;
            // 
            // btnCloseApp
            // 
            this.btnCloseApp.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnCloseApp.BackColor = System.Drawing.Color.Thistle;
            this.btnCloseApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCloseApp.BorderRadius = 0;
            this.btnCloseApp.ButtonText = "";
            this.btnCloseApp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseApp.DisabledColor = System.Drawing.Color.Gray;
            this.btnCloseApp.ForeColor = System.Drawing.Color.White;
            this.btnCloseApp.Iconcolor = System.Drawing.Color.Transparent;
            this.btnCloseApp.Iconimage = ((System.Drawing.Image)(resources.GetObject("btnCloseApp.Iconimage")));
            this.btnCloseApp.Iconimage_right = null;
            this.btnCloseApp.Iconimage_right_Selected = null;
            this.btnCloseApp.Iconimage_Selected = null;
            this.btnCloseApp.IconMarginLeft = 0;
            this.btnCloseApp.IconMarginRight = 0;
            this.btnCloseApp.IconRightVisible = true;
            this.btnCloseApp.IconRightZoom = 0D;
            this.btnCloseApp.IconVisible = true;
            this.btnCloseApp.IconZoom = 90D;
            this.btnCloseApp.IsTab = false;
            this.btnCloseApp.Location = new System.Drawing.Point(494, 0);
            this.btnCloseApp.Name = "btnCloseApp";
            this.btnCloseApp.Normalcolor = System.Drawing.Color.Thistle;
            this.btnCloseApp.OnHovercolor = System.Drawing.Color.Thistle;
            this.btnCloseApp.OnHoverTextColor = System.Drawing.Color.White;
            this.btnCloseApp.selected = false;
            this.btnCloseApp.Size = new System.Drawing.Size(42, 41);
            this.btnCloseApp.TabIndex = 9;
            this.btnCloseApp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseApp.Textcolor = System.Drawing.Color.White;
            this.btnCloseApp.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseApp.Click += new System.EventHandler(this.btnCloseApp_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Lucida Sans", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(116, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 43);
            this.label2.TabIndex = 8;
            this.label2.Text = "MEDIA BAZAR";
            // 
            // lblRememberMe
            // 
            this.lblRememberMe.AutoSize = true;
            this.lblRememberMe.BackColor = System.Drawing.Color.Transparent;
            this.lblRememberMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRememberMe.ForeColor = System.Drawing.Color.White;
            this.lblRememberMe.Location = new System.Drawing.Point(121, 361);
            this.lblRememberMe.Name = "lblRememberMe";
            this.lblRememberMe.Size = new System.Drawing.Size(107, 18);
            this.lblRememberMe.TabIndex = 7;
            this.lblRememberMe.Text = "Remember me";
            this.lblRememberMe.Click += new System.EventHandler(this.lblRememberMe_Click);
            // 
            // cbxRememberMe
            // 
            this.cbxRememberMe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(36)))), ((int)(((byte)(136)))));
            this.cbxRememberMe.ChechedOffColor = System.Drawing.Color.LavenderBlush;
            this.cbxRememberMe.Checked = true;
            this.cbxRememberMe.CheckedOnColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(36)))), ((int)(((byte)(136)))));
            this.cbxRememberMe.ForeColor = System.Drawing.Color.White;
            this.cbxRememberMe.Location = new System.Drawing.Point(94, 360);
            this.cbxRememberMe.Name = "cbxRememberMe";
            this.cbxRememberMe.Size = new System.Drawing.Size(20, 20);
            this.cbxRememberMe.TabIndex = 6;
            // 
            // tbPassword
            // 
            this.tbPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(155)))), ((int)(((byte)(199)))));
            this.tbPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbPassword.BackgroundImage")));
            this.tbPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tbPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbPassword.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tbPassword.Icon = ((System.Drawing.Image)(resources.GetObject("tbPassword.Icon")));
            this.tbPassword.Location = new System.Drawing.Point(94, 280);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(332, 52);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.text = "";
            this.tbPassword.OnTextChange += new System.EventHandler(this.tbPassword_OnTextChange);
            // 
            // tbUsername
            // 
            this.tbUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(155)))), ((int)(((byte)(199)))));
            this.tbUsername.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tbUsername.BackgroundImage")));
            this.tbUsername.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tbUsername.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbUsername.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tbUsername.Icon = ((System.Drawing.Image)(resources.GetObject("tbUsername.Icon")));
            this.tbUsername.Location = new System.Drawing.Point(94, 199);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(332, 52);
            this.tbUsername.TabIndex = 4;
            this.tbUsername.text = "";
            // 
            // btnLogIn
            // 
            this.btnLogIn.ActiveBorderThickness = 1;
            this.btnLogIn.ActiveCornerRadius = 20;
            this.btnLogIn.ActiveFillColor = System.Drawing.Color.Transparent;
            this.btnLogIn.ActiveForecolor = System.Drawing.Color.DarkViolet;
            this.btnLogIn.ActiveLineColor = System.Drawing.Color.MediumPurple;
            this.btnLogIn.BackColor = System.Drawing.Color.Transparent;
            this.btnLogIn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLogIn.BackgroundImage")));
            this.btnLogIn.ButtonText = "LOG IN";
            this.btnLogIn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogIn.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogIn.ForeColor = System.Drawing.Color.Transparent;
            this.btnLogIn.IdleBorderThickness = 1;
            this.btnLogIn.IdleCornerRadius = 20;
            this.btnLogIn.IdleFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(36)))), ((int)(((byte)(136)))));
            this.btnLogIn.IdleForecolor = System.Drawing.Color.Transparent;
            this.btnLogIn.IdleLineColor = System.Drawing.Color.Purple;
            this.btnLogIn.Location = new System.Drawing.Point(94, 419);
            this.btnLogIn.Margin = new System.Windows.Forms.Padding(5);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(332, 66);
            this.btnLogIn.TabIndex = 0;
            this.btnLogIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = null;
            this.bunifuDragControl1.Vertical = true;
            // 
            // FormLogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 621);
            this.Controls.Add(this.panelLogIn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormLogIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLogIn";
            this.panelLogIn.ResumeLayout(false);
            this.panelLogIn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.Framework.UI.BunifuGradientPanel panelLogIn;
        private Bunifu.Framework.UI.BunifuThinButton2 btnLogIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblRememberMe;
        private Bunifu.Framework.UI.BunifuCheckbox cbxRememberMe;
        private Bunifu.Framework.UI.BunifuTextbox tbPassword;
        private Bunifu.Framework.UI.BunifuTextbox tbUsername;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private Bunifu.Framework.UI.BunifuFlatButton btnCloseApp;
    }
}