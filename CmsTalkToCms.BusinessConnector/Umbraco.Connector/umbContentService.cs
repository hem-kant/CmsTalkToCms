using CmsTalkToCms.Common.Helper;
using CmsTalkToCms.Common.Model;
using CmsTalkToCms.GenericDataStructure.Content;
using Generic.Common.Logging;
using SDLTridion.CoreService;
using SDLTridion.TridionObjects.Helper;
using SDLTridion.TridionObjects.TridionObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using Tridion.ContentManager.CoreService.Client;

namespace CmsTalkToCms.BusinessConnector.Umbraco.Connector
{
    public class umbContentService
    {
        public static ICoreServiceFrameworkContext coreService = null;
        UmbracoContentService.UmbracoCrudOperations umbCrud = new UmbracoContentService.UmbracoCrudOperations();

        #region Get all news based on parentID
        /// <summary>
        /// Call umbraco content service method getNewsList to get the list of all news items by passing the parent id as input.
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public List<News> getAllNewsList(int parentID)
        {
            Logger.WriteLog(Logger.LogLevel.INFO, "Calling getAllNewsList parentID " + parentID);
            List<News> nList = new List<News>();
            try
            {
                var newsList = umbCrud.getNewsList(1065);
                foreach (var news in newsList)
                {
                    News _news = new News();
                    _news.BodyText = news.bodyText != null ? news.bodyText : string.Empty;
                    _news.Image = news.image != null ? news.image : string.Empty;
                    _news.Subheader = news.subheader != null ? news.subheader : string.Empty;

                    nList.Add(_news);
                }
            }
            catch (System.Exception ex)
            {
                Logger.WriteException(Logger.LogLevel.ERROR, "Error while calling getAllNewsList ", ex);
                throw;
            }
           
            return nList;
        }

        #endregion

        #region Create Items in Umbraco from Tridion take News type object
        /// <summary>
        /// Create Items in Umbraco from Tridion take News type object
        /// </summary>
        /// <param name="news"></param>
        /// <returns></returns>
        public List<News> setNewsItems(News news)
        {
            Logger.WriteLog(Logger.LogLevel.INFO, "Calling getAllNewsList setNewsItems ");
            List<News> nList = new List<News>();
            try
            {
                var newsList = umbCrud.AddNewsItems(1065,news.Subheader,news.Image,news.Subheader,"umb"+news.Subheader);
                Logger.WriteLog(Logger.LogLevel.INFO, "Calling getAllNewsList setNewsItems "+ newsList);
            }
            catch (System.Exception ex)
            {
                Logger.WriteException(Logger.LogLevel.ERROR, "Error while calling getAllNewsList ", ex);
                throw;
            }

            return nList;
        }
        #endregion

        #region Read data from Umbraco and write data in Tridion 
        /// <summary>
        ///  Read data from Umbraco and write data in Tridion 
        /// </summary>
        /// <param name="news"></param>
        public static void writeDataFromUmbracoToTridion(List<News> news)
        {
            coreService = CoreServiceFactory.GetCoreServiceContext(new Uri(ConfigurationManager.AppSettings["CoreServiceURL"].ToString()), new NetworkCredential(ConfigurationManager.AppSettings["UserName"].ToString(), ConfigurationManager.AppSettings["Password"].ToString(), ConfigurationManager.AppSettings["Domain"].ToString()));
            SchemaFieldsData schemaFieldData = coreService.Client.ReadSchemaFields(ConfigurationManager.AppSettings["SchemaID"].ToString(), true, new ReadOptions());
            foreach (var item in news)
            {
                string serializeXml = string.Empty;
                bool bln = TransformObjectAndXml.Serialize<News>(item, ref serializeXml);
                string xml = serializeXml;
                string tcmuri = TridionComponent.GenerateComponent(coreService, xml, SetPublication.Publication(ConfigurationManager.AppSettings["FolderLocation"].ToString(), ConfigurationManager.AppSettings["SchemaID"].ToString()), EnumType.SchemaType.Component, ConfigurationManager.AppSettings["FolderLocation"].ToString(), !string.IsNullOrEmpty(item.Subheader) ? item.Subheader : "New Component", !string.IsNullOrEmpty(item.Subheader) ? item.Subheader : "New Component");
            }
        }
        #endregion
    }
}
