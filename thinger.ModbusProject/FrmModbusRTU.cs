using System.IO.Ports;
using thinger.DataConvertLib;
using thinger.ModbusRTULib;


namespace thinger.ModbusProject
{
    public partial class FrmModbusRTU : Form
    {
        private ModbusRTU modbus = new ModbusRTU();
        public FrmModbusRTU()
        {
            InitializeComponent();

            InitParam();
        }

        private void InitParam()
        {
            //获取本机的端口号列表
            string[] portList = SerialPort.GetPortNames();

            if(portList.Length > 0 )
            {
                this.cmb_Port.Items.AddRange( portList );
                this.cmb_Port.SelectedIndex = 0;
            }

            //波特率初始化
            this.cmb_BaudRate.Items.AddRange(new string[] {"2400", "4800", "9600", "19200", "38400"});
            this.cmb_BaudRate.SelectedIndex = 2;

            //校验位初始化
            this.cmb_Parity.DataSource = Enum.GetNames(typeof(Parity));
            this.cmb_Parity.SelectedIndex = 0;

            //停止位初始化
            this.cmb_StopBits.DataSource = Enum.GetNames(typeof(StopBits));
            this.cmb_StopBits.SelectedIndex = 1;

            //数据位初始化
            this.cmb_DataBits.Items.AddRange(new string[] { "7", "8"});
            this.cmb_DataBits.SelectedIndex = 1;

            this.cmb_DataFormat.DataSource = Enum.GetNames(typeof(DataFormat));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool result = modbus.Connect("COM9", 9600);

            MessageBox.Show(result ? "连接成功" : "连接失败");
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            byte[] result = modbus.ReadOutputColis(2, 0, 10);

            if (result == null)
            {
                MessageBox.Show("读取失败");
                return;
            }

            bool coil0 = (result[0] & 0x01) != 0;

            MessageBox.Show($"第一个线圈状态：{coil0}");
        }
    }
}
