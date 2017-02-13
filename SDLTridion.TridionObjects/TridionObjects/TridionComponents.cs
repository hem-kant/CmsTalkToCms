using CmsTalkToCms.GenericDataStructure.Content;
using SDLTridion.CoreService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml.Linq;
using Tridion.ContentManager.CoreService.Client;
namespace SDLTridion.TridionObjects.TridionObjects
{
    public class TridionComponent
    {
        #region Generate Component
        public static string GenerateComponent(ICoreServiceFrameworkContext coreService, string xml, string schemaID, EnumType.SchemaType schemaType, string folderUri, string title, string ext_Name)
        {
            try
            {

                string Title = string.Empty;
                string Tcmuri = string.Empty;
                SearchQueryData filter = new SearchQueryData();
                filter.FullTextQuery = "title";
                filter.ItemTypes = new ItemType[] { ItemType.Component };

                BasedOnSchemaData basedSchema = new BasedOnSchemaData();
                basedSchema.Schema = new LinkToSchemaData() { IdRef = schemaID };
                basedSchema.Field = "title";
                basedSchema.FieldValue = title;


                filter.BasedOnSchemas = new BasedOnSchemaData[] { basedSchema };
                XElement results = coreService.Client.GetSearchResultsXml(filter);
                for (IEnumerator<XElement> e = results.Descendants().GetEnumerator(); e.MoveNext();)
                {
                    Title = e.Current.Attribute(XName.Get("Title")).Value;
                    Tcmuri = e.Current.FirstAttribute.Value != null ? e.Current.FirstAttribute.Value : null;
                }
                Title = Title != string.Empty ? Title : title;
                ComponentData componentData = GetNewComponent(folderUri, schemaID, schemaType, Title);



                componentData.ComponentType = ComponentType.Normal;
                SchemaData sd = coreService.Client.Read(schemaID, null) as SchemaData;

                var content = XElement.Parse(xml);
                var xmlns = content;

                componentData.Content = xmlns.ToString();


                TridionObjectInfo tridionObjectInfo = Helper.Helper.GetTridionObject(coreService, ItemType.Component, folderUri, Title);
                if (tridionObjectInfo.TcmUri != null)
                {
                    componentData.Id = tridionObjectInfo.TcmUri;
                    componentData = (ComponentData)(coreService.Client.SynchronizeWithSchema(
                                                                       componentData,
                                                                       new SynchronizeOptions
                                                                       {
                                                                           SynchronizeFlags = SynchronizeFlags.All
                                                                       }
                                                                       ).SynchronizedItem);
                   
                    componentData = (ComponentData)coreService.Client.Update(componentData, new ReadOptions());
                }
                else
                {
                    componentData = (ComponentData)(coreService.Client.SynchronizeWithSchema(
                                                                        componentData,
                                                                        new SynchronizeOptions
                                                                        {
                                                                            SynchronizeFlags = SynchronizeFlags.All
                                                                        }
                                                                        ).SynchronizedItem);
                    componentData = (ComponentData)coreService.Client.Create(componentData, new ReadOptions());


                }
                return componentData.Id.ToString();
            }
            catch (Exception ex)
            {
                
                return "";
            }
        }
        #endregion
        
        #region publish component
        public static void Publish(string targetTcmId, string targets, ICoreServiceFrameworkContext coreService)
        {
            var pubData = new PublishInstructionData
            {
                ResolveInstruction = new ResolveInstructionData() { IncludeChildPublications = false },
                RenderInstruction = new RenderInstructionData()
            };


            //get the component info
            var component = coreService.Client.Read(targetTcmId, null) as ComponentData;
            var componentId = component.Id;

            //strip out the version number if it contains onercmd
            if (component.Id.Contains("v"))
            {
                componentId = component.Id.Substring(0, component.Id.LastIndexOf('-'));
            }

            //publish to the target supplied into this method on low priority
            try
            {
                string[] pubid = ConfigurationManager.AppSettings["PublicationID"].ToString().Split(',');
                string[] pubtargets = targets.ToString().Split(',');
                foreach (var item in pubid)
                {
                    string componentIdTcm = componentId.Substring(componentId.IndexOf("tcm:"), componentId.IndexOf("-"));
                    string publicationid = "tcm:" + item;
                    foreach (var target in pubtargets)
                    {
                        coreService.Client.Publish(new[] { componentId.Replace(componentIdTcm, publicationid) }, pubData, new[] { target.ToString() }, PublishPriority.High, null);

                    }

                }

            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region Get New Blank Component Property
        public static ComponentData GetNewComponent(string folderUri, string schemaUri, EnumType.SchemaType schemaType, string title = null)
        {

            return new ComponentData
            {
                LocationInfo = new LocationInfo
                {
                    OrganizationalItem = new LinkToOrganizationalItemData
                    {
                        IdRef = folderUri
                        // WebDavUrl  = folderUri

                    },
                },

                ComponentType = schemaType == EnumType.SchemaType.Multimedia ? ComponentType.Multimedia : ComponentType.Normal,
                Title = title,

                Schema = new LinkToSchemaData
                {
                    IdRef = schemaUri //schemaData.IdRef
                    //WebDavUrl = schemaUri
                },

                IsBasedOnMandatorySchema = false,
                IsBasedOnTridionWebSchema = true,
                ApprovalStatus = new LinkToApprovalStatusData
                {
                    IdRef = "tcm:0-0-0"
                },
                Id = "tcm:0-0-0"
            };
        }
        #endregion
         
    }
}
