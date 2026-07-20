namespace thinger.ModbusProject
{
    partial class FrmModbusRTU
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmModbusRTU));
            btn_Connect = new Button();
            btn_Read = new Button();
            groupBox1 = new GroupBox();
            btn_DisConnect = new Button();
            cmb_DataFormat = new ComboBox();
            label6 = new Label();
            cmb_StopBits = new ComboBox();
            label5 = new Label();
            cmb_DataBits = new ComboBox();
            label4 = new Label();
            cmb_Parity = new ComboBox();
            label3 = new Label();
            cmb_BaudRate = new ComboBox();
            label2 = new Label();
            cmb_Port = new ComboBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            txt_Length = new TextBox();
            lst_Info = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            imageList1 = new ImageList(components);
            label13 = new Label();
            label12 = new Label();
            label9 = new Label();
            label11 = new Label();
            label8 = new Label();
            cmb_DataType = new ComboBox();
            cmb_StoreArea = new ComboBox();
            txt_Start = new TextBox();
            txt_WriteValue = new TextBox();
            txt_SlaveID = new TextBox();
            label10 = new Label();
            label7 = new Label();
            btn_Write = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btn_Connect
            // 
            btn_Connect.Location = new Point(440, 107);
            btn_Connect.Name = "btn_Connect";
            btn_Connect.Size = new Size(112, 34);
            btn_Connect.TabIndex = 0;
            btn_Connect.Text = "连接";
            btn_Connect.UseVisualStyleBackColor = true;
            btn_Connect.Click += btnConnect_Click;
            // 
            // btn_Read
            // 
            btn_Read.Location = new Point(474, 101);
            btn_Read.Name = "btn_Read";
            btn_Read.Size = new Size(112, 34);
            btn_Read.TabIndex = 1;
            btn_Read.Text = "读取";
            btn_Read.UseVisualStyleBackColor = true;
            btn_Read.Click += btnRead_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btn_DisConnect);
            groupBox1.Controls.Add(cmb_DataFormat);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(btn_Connect);
            groupBox1.Controls.Add(cmb_StopBits);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(cmb_DataBits);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(cmb_Parity);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(cmb_BaudRate);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(cmb_Port);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(47, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(798, 178);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "通信参数";
            // 
            // btn_DisConnect
            // 
            btn_DisConnect.Location = new Point(616, 107);
            btn_DisConnect.Name = "btn_DisConnect";
            btn_DisConnect.Size = new Size(112, 34);
            btn_DisConnect.TabIndex = 12;
            btn_DisConnect.Text = "断开连接";
            btn_DisConnect.UseVisualStyleBackColor = true;
            btn_DisConnect.Click += btn_DisConnect_Click;
            // 
            // cmb_DataFormat
            // 
            cmb_DataFormat.FormattingEnabled = true;
            cmb_DataFormat.Location = new Point(304, 105);
            cmb_DataFormat.Name = "cmb_DataFormat";
            cmb_DataFormat.Size = new Size(86, 32);
            cmb_DataFormat.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(230, 108);
            label6.Name = "label6";
            label6.Size = new Size(68, 24);
            label6.TabIndex = 10;
            label6.Text = "大小端:";
            // 
            // cmb_StopBits
            // 
            cmb_StopBits.FormattingEnabled = true;
            cmb_StopBits.Location = new Point(99, 105);
            cmb_StopBits.Name = "cmb_StopBits";
            cmb_StopBits.Size = new Size(125, 32);
            cmb_StopBits.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(25, 108);
            label5.Name = "label5";
            label5.Size = new Size(68, 24);
            label5.TabIndex = 8;
            label5.Text = "停止位:";
            // 
            // cmb_DataBits
            // 
            cmb_DataBits.FormattingEnabled = true;
            cmb_DataBits.Location = new Point(676, 48);
            cmb_DataBits.Name = "cmb_DataBits";
            cmb_DataBits.Size = new Size(70, 32);
            cmb_DataBits.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(602, 51);
            label4.Name = "label4";
            label4.Size = new Size(68, 24);
            label4.TabIndex = 6;
            label4.Text = "数据位:";
            // 
            // cmb_Parity
            // 
            cmb_Parity.FormattingEnabled = true;
            cmb_Parity.Location = new Point(498, 45);
            cmb_Parity.Name = "cmb_Parity";
            cmb_Parity.Size = new Size(88, 32);
            cmb_Parity.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(424, 48);
            label3.Name = "label3";
            label3.Size = new Size(68, 24);
            label3.TabIndex = 4;
            label3.Text = "校验位:";
            // 
            // cmb_BaudRate
            // 
            cmb_BaudRate.FormattingEnabled = true;
            cmb_BaudRate.Location = new Point(304, 45);
            cmb_BaudRate.Name = "cmb_BaudRate";
            cmb_BaudRate.Size = new Size(86, 32);
            cmb_BaudRate.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(230, 48);
            label2.Name = "label2";
            label2.Size = new Size(68, 24);
            label2.TabIndex = 2;
            label2.Text = "波特率:";
            // 
            // cmb_Port
            // 
            cmb_Port.FormattingEnabled = true;
            cmb_Port.Location = new Point(99, 45);
            cmb_Port.Name = "cmb_Port";
            cmb_Port.Size = new Size(125, 32);
            cmb_Port.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 48);
            label1.Name = "label1";
            label1.Size = new Size(68, 24);
            label1.TabIndex = 0;
            label1.Text = "端口号:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btn_Write);
            groupBox2.Controls.Add(txt_Length);
            groupBox2.Controls.Add(lst_Info);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(label12);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(btn_Read);
            groupBox2.Controls.Add(label11);
            groupBox2.Controls.Add(label8);
            groupBox2.Controls.Add(cmb_DataType);
            groupBox2.Controls.Add(cmb_StoreArea);
            groupBox2.Controls.Add(txt_Start);
            groupBox2.Controls.Add(txt_WriteValue);
            groupBox2.Controls.Add(txt_SlaveID);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(label7);
            groupBox2.Location = new Point(47, 222);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(798, 506);
            groupBox2.TabIndex = 13;
            groupBox2.TabStop = false;
            groupBox2.Text = "读写测试";
            // 
            // txt_Length
            // 
            txt_Length.Location = new Point(320, 101);
            txt_Length.Name = "txt_Length";
            txt_Length.Size = new Size(137, 30);
            txt_Length.TabIndex = 17;
            txt_Length.Text = "10";
            txt_Length.TextAlign = HorizontalAlignment.Center;
            // 
            // lst_Info
            // 
            lst_Info.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2 });
            lst_Info.HeaderStyle = ColumnHeaderStyle.None;
            lst_Info.Location = new Point(27, 285);
            lst_Info.MultiSelect = false;
            lst_Info.Name = "lst_Info";
            lst_Info.Size = new Size(719, 190);
            lst_Info.SmallImageList = imageList1;
            lst_Info.TabIndex = 16;
            lst_Info.UseCompatibleStateImageBehavior = false;
            lst_Info.View = View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "日期时间";
            columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "信息内容";
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "普通用户.png");
            imageList1.Images.SetKeyName(1, "流程图.png");
            imageList1.Images.SetKeyName(2, "管理员.png");
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(25, 238);
            label13.Name = "label13";
            label13.RightToLeft = RightToLeft.No;
            label13.Size = new Size(86, 24);
            label13.TabIndex = 15;
            label13.Text = "读取信息:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(25, 177);
            label12.Name = "label12";
            label12.RightToLeft = RightToLeft.No;
            label12.Size = new Size(170, 24);
            label12.TabIndex = 15;
            label12.Text = "写入数据(空格分割):";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(466, 45);
            label9.Name = "label9";
            label9.Size = new Size(86, 24);
            label9.TabIndex = 15;
            label9.Text = "数据类型:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(230, 104);
            label11.Name = "label11";
            label11.Size = new Size(86, 24);
            label11.TabIndex = 15;
            label11.Text = "读取长度:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(230, 45);
            label8.Name = "label8";
            label8.Size = new Size(68, 24);
            label8.TabIndex = 15;
            label8.Text = "存储区:";
            // 
            // cmb_DataType
            // 
            cmb_DataType.FormattingEnabled = true;
            cmb_DataType.Location = new Point(558, 45);
            cmb_DataType.Name = "cmb_DataType";
            cmb_DataType.Size = new Size(70, 32);
            cmb_DataType.TabIndex = 11;
            // 
            // cmb_StoreArea
            // 
            cmb_StoreArea.FormattingEnabled = true;
            cmb_StoreArea.Location = new Point(304, 42);
            cmb_StoreArea.Name = "cmb_StoreArea";
            cmb_StoreArea.Size = new Size(153, 32);
            cmb_StoreArea.TabIndex = 11;
            // 
            // txt_Start
            // 
            txt_Start.Location = new Point(117, 101);
            txt_Start.Name = "txt_Start";
            txt_Start.Size = new Size(66, 30);
            txt_Start.TabIndex = 14;
            txt_Start.Text = "0";
            txt_Start.TextAlign = HorizontalAlignment.Center;
            // 
            // txt_WriteValue
            // 
            txt_WriteValue.Location = new Point(201, 174);
            txt_WriteValue.Name = "txt_WriteValue";
            txt_WriteValue.Size = new Size(267, 30);
            txt_WriteValue.TabIndex = 14;
            txt_WriteValue.Text = " 1 1 1 1 1";
            // 
            // txt_SlaveID
            // 
            txt_SlaveID.Location = new Point(117, 42);
            txt_SlaveID.Name = "txt_SlaveID";
            txt_SlaveID.Size = new Size(66, 30);
            txt_SlaveID.TabIndex = 14;
            txt_SlaveID.Text = "1";
            txt_SlaveID.TextAlign = HorizontalAlignment.Center;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(25, 104);
            label10.Name = "label10";
            label10.Size = new Size(86, 24);
            label10.TabIndex = 13;
            label10.Text = "起始地址:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(25, 45);
            label7.Name = "label7";
            label7.Size = new Size(86, 24);
            label7.TabIndex = 13;
            label7.Text = "从站地址:";
            // 
            // btn_Write
            // 
            btn_Write.Location = new Point(474, 174);
            btn_Write.Name = "btn_Write";
            btn_Write.Size = new Size(112, 34);
            btn_Write.TabIndex = 18;
            btn_Write.Text = "写入";
            btn_Write.UseVisualStyleBackColor = true;
            btn_Write.Click += btnWrite_Click;
            // 
            // FrmModbusRTU
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(892, 754);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmModbusRTU";
            Text = "基于ModbusRTU开发提供通用测试平台";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btn_Connect;
        private Button btn_Read;
        private GroupBox groupBox1;
        private ComboBox cmb_DataBits;
        private Label label4;
        private ComboBox cmb_Parity;
        private Label label3;
        private ComboBox cmb_BaudRate;
        private Label label2;
        private ComboBox cmb_Port;
        private Label label1;
        private Button btn_DisConnect;
        private ComboBox cmb_DataFormat;
        private Label label6;
        private ComboBox cmb_StopBits;
        private Label label5;
        private GroupBox groupBox2;
        private Label label12;
        private Label label9;
        private Label label11;
        private Label label8;
        private ComboBox cmb_DataType;
        private ComboBox cmb_StoreArea;
        private TextBox txt_Start;
        private TextBox txt_SlaveID;
        private Label label10;
        private Label label7;
        private TextBox txt_WriteValue;
        private ListView lst_Info;
        private Label label13;
        private ImageList imageList1;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TextBox txt_Length;
        private Button btn_Write;
    }
}
