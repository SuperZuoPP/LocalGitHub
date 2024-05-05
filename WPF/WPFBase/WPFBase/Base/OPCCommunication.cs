using Opc.Ua.Client;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using ImTools;
using System.Threading;
using RestSharp;

namespace WPFBase.Base
{
    public class OPCCommunication
    {
        public Session _session;
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

            
            //ReadValueIdCollection readValueIds = new ReadValueIdCollection();


            //for (int i = 1; i <= 4; i++)
            //{
            //    ReadValueId readValueId = new ReadValueId();
            //    readValueId.NodeId = "ns=2;s=数据类型示例.16 位设备.R 寄存器.Word" + i.ToString();
            //    readValueId.AttributeId = Attributes.Value;
            //    readValueIds.Add(readValueId);
            //}

            //_session.Read(null, 0, timestampsToReturn: TimestampsToReturn.Server, readValueIds, out DataValueCollection values, out DiagnosticInfoCollection diagnos);

            //foreach (var item in values)
            //{
            //    //if (StatusCode.IsGood(item.StatusCode))
            //    //{

            //    //}
            //    await Console.Out.WriteLineAsync(item.ToString());
            //}


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

        public async void DisConnectOpcServer() 
        {
            if (_session != null && _session.Connected)
            {
                try
                {
                    // 尝试正常关闭会话
                    await _session.CloseAsync();
                }
                catch (Exception ex)
                {
                    // 如果正常关闭失败，则强制终止会话
                    //_session.Abort();
                    // 可选：记录异常或进行其他错误处理
                    Console.WriteLine("Failed to close the session, aborting instead: " + ex.Message);
                }
                finally
                {
                    // 清除会话对象引用，以便垃圾回收
                    _session = null;
                }
            }
        }
        public async Task<Dictionary<string, object>> ReadMultiple(IEnumerable<string> tags)
        {
            if (_session == null)
            {
                return null;
            }
            var result = new Dictionary<string, object>();
            try
            {
                
                ReadValueIdCollection readValueIds = new ReadValueIdCollection();

                foreach (var tag in tags)
                {
                    ReadValueId readValueId = new ReadValueId();
                    readValueId.NodeId = "ns=2;s=数据类型示例.16 位设备.R 寄存器."+ tag; // 假设tags集合中的每个字符串都是一个有效的NodeId
                    readValueId.AttributeId = Attributes.Value;
                    readValueIds.Add(readValueId);
                }

                ReadResponse readResponse = await _session.ReadAsync(null,
                   0,
                   timestampsToReturn: TimestampsToReturn.Server,
                   readValueIds,
                   ct: CancellationToken.None);


                for (int i = 0; i < readResponse.Results.Count; i++)
                {
                    if (StatusCode.IsGood(readResponse.Results[i].StatusCode))
                    {
                        result.Add(tags.ElementAt(i), readResponse.Results[i]);
                    }
                    else
                    {
                        // 处理错误情况，例如通过记录错误或抛出异常
                        Console.WriteLine($"Error reading value from tag {tags.ElementAt(i)}: {readResponse.Results[i]}");
                    }
                }

                return result;
            }
            catch
            {
                Console.WriteLine($"获取OPC服务数据失败！");
                return result; 
            }
          
        }
            //    // ====================
            //    //DataValueCollection values;
            //    //DiagnosticInfoCollection diagnostics;
            //    //StatusCodeCollection statusCodes;

            //    // 调用Read方法并传递readValueIds
            //    //StatusCodeCollection statuses = _session.Read(
            //    //    null,
            //    //    0,
            //    //    TimestampsToReturn.Server,
            //    //    readValueIds,
            //    //    out values,
            //    //    out diagnostics
            //    //);

            //    //// 检查每个读取操作的状态
            //    //for (int i = 0; i < values.Count; i++)
            //    //{
            //    //    if (statuses[i].IsGood)
            //    //    {
            //    //        result.Add(tags.ElementAt(i), values[i].Value);
            //    //    }
            //    //    else
            //    //    {
            //    //        // 处理错误情况，例如通过记录错误或抛出异常
            //    //        Console.WriteLine($"Error reading value from tag {tags.ElementAt(i)}: {statuses[i]}");
            //    //    }
            //    //}

            //    //return result;
            //}

            // 假设OPC服务器有一个名为WriteMultiple的方法，用于设置多个标签的值
            public void WriteMultiple(Dictionary<string, object> tagValues)
        {
            // 实现设置多个OPC标签值的逻辑
        }

    }
}
