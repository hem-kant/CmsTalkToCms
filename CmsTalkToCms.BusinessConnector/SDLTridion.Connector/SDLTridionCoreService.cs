using System;
using System.Collections.Generic;
using SDLTridion.CoreService;
using System.Configuration;
using System.Net;
using Tridion.ContentManager.CoreService.Client;

namespace CmsTalkToCms.BusinessConnector.SDLTridion.Connector
{
    public class SDLTridionCoreService
    {
        public static ICoreServiceFrameworkContext coreService = null;

       

        public static List<string>  writeDataFromTridionToUmbraco()
        {
            coreService = CoreServiceFactory.GetCoreServiceContext(new Uri(ConfigurationManager.AppSettings["CoreServiceURL"].ToString()), new NetworkCredential(ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), ConfigurationManager.AppSettings["Domain"].ToString()));
            SchemaFieldsData schemaFieldData = coreService.Client.ReadSchemaFields(ConfigurationManager.AppSettings["SchemaID"].ToString(), true, new ReadOptions());
            UsingItemsFilterData f = new UsingItemsFilterData { ItemTypes = new[] { ItemType.Component } };
            IdentifiableObjectData[] items = coreService.Client.GetList(ConfigurationManager.AppSettings["SchemaID"].ToString(), f);
            List<string> newsList = new List<string>();
            foreach (var item in items)
            {
                ComponentData component = (ComponentData)coreService.Client.Read(item.Id,new ReadOptions());
                newsList.Add(component.Content);
                //Do something
            }
          return newsList;
        }
    }
}
