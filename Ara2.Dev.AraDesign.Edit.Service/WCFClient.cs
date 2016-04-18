using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Ara2.Dev.AraDesign.Edit.Service
{
    public class ClienteServerChannel<TInterfaceClient, TSerever, TSereverInterface>
        where TInterfaceClient : class
    {
        public Cliente<TInterfaceClient> Cliente;
        public Server<TSerever, TSereverInterface> Server;
       

        public ClienteServerChannel(TSerever vServer, int vPortServer, int vPortClient)
        {
            try
            {
                Server = new Server<TSerever, TSereverInterface>(vServer, vPortServer);
            }
            catch (Exception err)
            {
                throw new Exception("Erro on active server port " + vPortServer, err);
            }

            Cliente = new Cliente<TInterfaceClient>(vPortClient);
        }

        
    }

    public class Cliente<T>
        : ClientBase<T>
        where T : class
    {
        public readonly int Porta;

        public Cliente( int vPorta) :
            base(
                new NetTcpBinding()
                {
                    Security =new NetTcpSecurity() { Mode = SecurityMode.None },
                    MaxReceivedMessageSize = 256 * 256 * 1024,
                    MaxBufferPoolSize = 256 * 256 * 1024,
                    //ReceiveTimeout = TimeSpan.FromSeconds(5),
                    //SendTimeout = TimeSpan.FromSeconds(5)
                },
                new EndpointAddress(new Uri("net.tcp://127.0.0.1:" + vPorta + "/AraDevEdit"))
            )
        {
            Porta = vPorta;
        }

        public void Channel(Action<T> vEvent)
        {
            Channel<object>((a) =>
            {
                vEvent(a);
                return null;
            });
        }

        public T2 Channel<T2>(Func<T,T2> vEvent)
        {
            var vC = this.CreateChannel();
            try
            {
                return vEvent(vC);
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.Print("Erro Channel -----------------\n" + err.ToDetailedString());
                throw err;
            }
            finally
            {
                if (((ICommunicationObject)vC).State == CommunicationState.Faulted)
                    ((ICommunicationObject)vC).Abort();
                else
                    ((ICommunicationObject)vC).Close();
            }
        }
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
                 ConcurrencyMode = ConcurrencyMode.Single)]
    public class Server<T, TSereverInterface> : ServiceHost
    {
        public readonly T Instance;
        public readonly int Porta;
        public Server(T vObjInstance, int vPorta) :
            base(vObjInstance, new Uri("net.tcp://127.0.0.1:" + vPorta + "/AraDevEdit"))
        {
            
            Porta = vPorta;
            Instance = vObjInstance;

            this.Faulted += this_Faulted;

            NetTcpBinding BTmp = new NetTcpBinding();
            BTmp.Security.Mode = SecurityMode.None;
            BTmp.MaxReceivedMessageSize = 256 * 256 * 1024;
            BTmp.MaxBufferPoolSize = 256 * 256 * 1024;
            //BTmp.ReceiveTimeout = TimeSpan.FromSeconds(5);
            //BTmp.SendTimeout = TimeSpan.FromSeconds(5);
            try
            {
                this.AddServiceEndpoint(typeof(TSereverInterface), BTmp,"" );
                this.Open();
            }
            catch(Exception err)
            {
                throw new Exception("Erro on active server port " + vPorta, err);
            }
        }

        private void this_Faulted(object sender, EventArgs e)
        {
            if (this.State != CommunicationState.Opened)
            {
                try
                {
                    this.Abort();
                    this.Open();
                }
                catch(Exception err)
                {

                }
            }
        }
    }


}
