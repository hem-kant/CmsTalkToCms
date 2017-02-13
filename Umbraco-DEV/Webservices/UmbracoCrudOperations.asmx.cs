using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Umbraco.Core;
using Umbraco.Core.Services;
using Umbraco.DEV.BAL.Helper;
using Umbraco.DEV.BAL.Model;
namespace Umbraco_DEV.Webservices
{
    /// <summary>
    /// Summary description for UmbracoCrudOperations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UmbracoCrudOperations : System.Web.Services.WebService
    {
        public UmbracoCrudOperations() { }

        //[WebMethod]
        //public string HelloWorld()
        //{
        //    return "Hello World";
        //}

        [WebMethod]
        public string AddNewsItems(int parentId, string name, string image, string subheader, string text)
        {
           
            var service = ApplicationContext.Current.Services.ContentService;
            var newNewsPost = service.CreateContentWithIdentity(name, parentId, "umbNewsItem");
            newNewsPost.SetValue("image", image);
            newNewsPost.SetValue("subheader", subheader);
            newNewsPost.SetValue("bodyText", text);


            var status = service.SaveAndPublishWithStatus(newNewsPost);
            return string.Format("Successfully installed .Post ID: {0} ", newNewsPost.Id);

        }

        [WebMethod]
        public News getNewsItems(int id)
        {

            var service = ApplicationContext.Current.Services.ContentService;
            var newNewsPost = service.GetById(id);
            Dictionary<string, string> Dictionary = new Dictionary<string, string>();
            foreach (var item in newNewsPost.Properties)
            {
                Dictionary.Add(item.Alias, item.Value!=null? item.Value.ToString() : "");
            }            
            News properties = TransformData.GetObject(Dictionary,typeof(News)) as News;



            return properties;

        }

        [WebMethod]
        public List<News> getNewsList(int id)
        {
            var Vservice = ApplicationContext.Current.Services.ContentService;
            var homeDetailsSection = Vservice.GetById(id);
            var homeDetailsSectionChildren = Vservice.GetChildren(homeDetailsSection.Id);
          
            List<News> listOfNews = new List<News>();
            foreach (var homeDetailsSectionChild in homeDetailsSectionChildren)
            {
               
                Dictionary<string, string> Dictionary = new Dictionary<string, string>();
                foreach (var item in homeDetailsSectionChild.Properties)
                {
                    Dictionary.Add(item.Alias, item.Value != null ? item.Value.ToString() : "");
                }
                News properties = TransformData.GetObject(Dictionary, typeof(News)) as News;
                listOfNews.Add(properties);


            }
            return listOfNews;

        }

        [WebMethod]
        public string DeleteNews(int id)
        {

            var service = ApplicationContext.Current.Services.ContentService;
            var newNewsPost = service.GetById(id);
            if (newNewsPost !=null)
            {
                service.Delete(newNewsPost);
            }          

            return string.Format("Successfully deleted ");

        }

        [WebMethod]
        public string UpdateNews(int id)
        {

            var service = ApplicationContext.Current.Services.ContentService;
            var newNewsPost = service.GetById(id);
            if (newNewsPost != null)
            {
                service.Delete(newNewsPost);
            }

            return string.Format("Successfully deleted ");

        }
    }

    
}
