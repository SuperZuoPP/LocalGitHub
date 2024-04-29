using Opc.Ua.Client;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using ImTools;

namespace WPFBase.Base
{
    public class OPCCommunication
    {
        private Session _session;
        private Subscription _subscription;
        public async void ConnectToOpcServer()
        {
            var endpointUrl = "opc.tcp://127.0.0.1:49320";
            //var endpoint = new EndpointDescription(endpointUrl);
            EndpointDescription endpoint = CoreClientUtils.SelectEndpoint(endpointUrl, false);//TCP的三次握手
            ConfiguredEndpoint configuredEndpoint = new ConfiguredEndpoint(null, endpoint);
            //// 创建一个证书标识符
            //var certificateId = new CertificateIdentifier
            //{
            //    StorePath = "D:\\ProgramData\\Kepware\\KEPServerEX\\V6\\UA\\Client Driver\\cert", // 证书存储路径
            //    SubjectName = "kepserverex_ua_client_driver", // 证书主题名称
            //    StoreType = CertificateStoreType.Directory // 证书存储类型，可以是Directory、X509Store等
            //};

            //// 使用证书标识符来配置安全配置
            //var securityConfiguration = new SecurityConfiguration
            //{
            //    ApplicationCertificate = certificateId,
            //    // 其他安全配置项
            //};
            // 创建客户端配置
            var clientConfiguration = new ClientConfiguration
            {
                // 客户端配置项，根据需要添加
            };
            // 创建应用程序配置
            var applicationConfiguration = new ApplicationConfiguration
            {
                ApplicationName = "zuochao",
                ApplicationType = ApplicationType.Client,
                ApplicationUri = "urn:YourAppUri",
               // SecurityConfiguration = securityConfiguration,
                ClientConfiguration = clientConfiguration // 设置客户端配置
                // 其他配置项根据需要添加
            };

            // 创建OPC UA会话
            _session = await Session.Create(
                configuration: applicationConfiguration,
                endpoint: configuredEndpoint,
                updateBeforeConnect: false,
                sessionName: Guid.NewGuid().ToString(),
                sessionTimeout: 6000,
                identity: new UserIdentity(),
                preferredLocales:null) ;


            ReadValueIdCollection readValueIds = new ReadValueIdCollection();

         
            for (int i = 1; i <= 4; i++)
            {
                ReadValueId readValueId = new ReadValueId();
                readValueId.NodeId = "ns=2;s=数据类型示例.16 位设备.R 寄存器.Word" + i.ToString();
                readValueId.AttributeId = Attributes.Value;
                readValueIds.Add(readValueId);
            }

            _session.Read(null, 0, timestampsToReturn: TimestampsToReturn.Server, readValueIds, out DataValueCollection values, out DiagnosticInfoCollection diagnos);

            foreach (var item in values)
            {
                await Console.Out.WriteLineAsync(item.ToString());
            }


            // 订阅数据
            //_subscription = new Subscription(_session.DefaultSubscription) { PublishingInterval = 1000 };
            //_subscription.AddItems(new MonitoredItem(_subscription.DefaultItem) { StartNodeId = "ns=2;s=YourNodeId" });

            //// 处理数据变化事件
            //_subscription.ItemChanged += OnItemChanged;

            //// 开始订阅
            //await _subscription.Create();

            // 连接成功，更新UI等操作
            // MessageBox.Show("Connected to OPC UA server!");
        }

    }
}
