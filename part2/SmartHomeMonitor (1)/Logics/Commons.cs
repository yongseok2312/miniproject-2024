using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MQTTnet.Client;
using System.Threading.Tasks;
using System.Windows;

namespace SmartHomeMonitoringApp.Logics
{
    public class Commons
    {
        // Windows에 MQTT Broker가 설치되어 있으므로 가능한 아이피, 호스트
        // localhost
        public static string BROKERHOST { get; set; } = "192.168.5.2";
        public static string MQTTTOPIC { get; set; } = "pknu/data/";
        public static string CONNSTRING { get; set; } = "Data Source=127.0.0.1;Initial Catalog=EMS;Persist Security Info=True;User ID=sa;Encrypt=False;" +
                                                        "Password=mssql_p@ss ;";
        
        public static IMqttClient MQTT_CLENT { get; set; }
    }
}
