using MahApps.Metro.Controls;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// DataBaseControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBaseControl : UserControl
    {
        // 변수 또는 속성선언

        Thread MqttThread { get; set; } // 없으면 UI컨트롤과 충돌나서 Log를 못찍음(응답없음)!
        int MaxCount { get; set; } = 10; // MQTT로그 과적으로 속도저하를 방지하기 위해
        public bool IsConnected { get; private set; }

        public DataBaseControl()
        {
            InitializeComponent();
        }

        // 유저컨트롤 화면 로드된 이후 초기화
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TxtBrokeUrl.Text = Commons.BROKERHOST;
            TxtMqttTopic.Text = Commons.MQTTTOPIC;
            TxtConnString.Text = Commons.CONNSTRING;

            IsConnected = false;
            BtnConnect.IsChecked = false;
        }

        private async Task ConnectSystemAsync()
        {
            if (IsConnected == false) // 연결이 안됐으면 처리
            {
                var mqttFactory = new MqttFactory();
                Commons.MQTT_CLENT = mqttFactory.CreateMqttClient();
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(TxtBrokeUrl.Text).Build();

                await Commons.MQTT_CLENT.ConnectAsync(mqttClientOptions, CancellationToken.None);
                Commons.MQTT_CLENT.ApplicationMessageReceivedAsync += MQTT_CLENT_ApplicationMessageReceivedAsync;

                var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicFilter(f => { f.WithTopic(Commons.MQTTTOPIC); }).Build();

                await Commons.MQTT_CLENT.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

                IsConnected=true;
                BtnConnect.IsChecked = true;
                BtnConnect.Content = "MQTT 연결중";

            }
            else
            {
                // 연결 후 연결 끊기
                if (Commons.MQTT_CLENT.IsConnected)
                {
                    Commons.MQTT_CLENT.ApplicationMessageReceivedAsync -= MQTT_CLENT_ApplicationMessageReceivedAsync;
                    await Commons.MQTT_CLENT.DisconnectAsync();
                    IsConnected = false;
                    BtnConnect.IsChecked = false;
                    BtnConnect.Content = "Connect";
                }
            }
        }

        private Task MQTT_CLENT_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.Payload);

            //Debug.WriteLine(payload);
           
            UpdateLog(payload);
            InsertData(payload);    // DB에 저장

            return Task.CompletedTask; // Async에서 Task값을 넘겨줄려면 이렇게...
        }

        private void InsertData(string payload)
        {
            this.Invoke(() => { 
            var currValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(payload);

            //Debug.WriteLine("InsertData:" + currValue["CURR_DT"]);
            //currValue["DEV_ID"],currValue["TYPE"],currValue["Value"]
            if (currValue != null)
            {
                try
                {
                    using(SqlConnection conn = new SqlConnection(TxtConnString.Text))
                    {
                        conn.Open();
                        var insQuery = @"INSERT INTO [dbo].[smarthomedata]
                                               (
                                               [DEV_ID]
                                               ,[CURR_DT]
                                               ,[TEMP]
                                               ,[HUMID])
                                         VALUES
                                               (
                                               @DEV_ID
                                               ,@CURR_DT
                                               ,@TEMP
                                               ,@HUMID)";

                        SqlCommand cmd = new SqlCommand(insQuery, conn);
                        cmd.Parameters.AddWithValue("@DEV_ID", currValue["DEV_ID"]);
                        cmd.Parameters.AddWithValue("@CURR_DT", currValue["CURR_DT"]);
                        var splitValue = currValue["VALUE"].Split('|'); // splitValue[0] = 온도 splitValue[1] = 습도
                        cmd.Parameters.AddWithValue("@TEMP", splitValue[0]);
                        cmd.Parameters.AddWithValue("@HUMID", splitValue[1]);

                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            UpdateLog(">>>> DB Insert Succeed");
                        }
                        else
                        {
                            UpdateLog(">>>> DB Insert Failed");
                        }
                    }
                }
                catch (Exception ex){
                    UpdateLog($"DB 에러 발생 : {ex.Message}");
                }
            }
            });
        }

        private void UpdateLog(string payload)
        {
            this.Invoke(() =>
            {
                TxtLog.Text += $"{payload}\n";
                TxtLog.ScrollToEnd();

            });
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            ConnectSystemAsync();
        }
    }
}
