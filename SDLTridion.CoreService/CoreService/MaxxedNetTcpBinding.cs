using System;
using System.ServiceModel;
using System.Xml;

namespace SDLTridion.CoreService
{
    public class MaxxedNetTcpBinding : NetTcpBinding
    {
        public MaxxedNetTcpBinding(SecurityMode securityMode)
            : base(securityMode)
        {
            initializeMaxSettings();
        }

        public MaxxedNetTcpBinding()
            : base()
        {
            initializeMaxSettings();
        }

        private void initializeMaxSettings()
        {
            this.MaxReceivedMessageSize = Int32.MaxValue;
            this.ReaderQuotas = new XmlDictionaryReaderQuotas
            {
                MaxDepth = Int32.MaxValue,
                MaxStringContentLength = Int32.MaxValue,
                MaxArrayLength = Int32.MaxValue
            };
        }

    }
}
