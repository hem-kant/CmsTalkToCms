using CmsTalkToCms.BusinessConnector.Umbraco.Connector;
using CmsTalkToCms.BusinessConnector.SDLTridion.Connector;
using Generic.Common.Logging;
using System.Collections.Generic;
using System;
using System.Xml;
using CmsTalkToCms.Common.Model;
using CmsTalkToCms.Common.Helper;

namespace CmsTalkToCms.Client
{
    class Program
    {        
        static void Main(string[] args)
        {
            Logger.WriteLog(Logger.LogLevel.INFO, "Calling CmsTalkToCms .Main");
            umbContentService umb = new umbContentService();
            Console.WriteLine("Please enter CMS source");
            string source = Console.ReadLine();

            if (source.ToLower() == "umbraco")
            {
               
                var newsList = umb.getAllNewsList(1);
                List<string> xml = new List<string>();

                if (newsList != null)
                {
                    // Send umbraco document to Tridion core service
                    umbContentService.writeDataFromUmbracoToTridion(newsList);
                }
            }
            else
            {
                // Send Tridion component data to Umbraco 
                List<string> tridionComponentXML =  SDLTridionCoreService.writeDataFromTridionToUmbraco();
                XmlDocument Xdoc = new XmlDocument();
                foreach (var item in tridionComponentXML)
                {
                    Xdoc.LoadXml(item);
                    News n = TransformObjectAndXml.Deserialize<News>(Xdoc);
                    umb.setNewsItems(n);
                }
            }       
        }
    }
}
