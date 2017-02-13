using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLTridion.TridionObjects.Helper
{
    class Namespace
    {
        private static String UpdateNodesWithDefaultNamespace(String xml, String defaultNamespace)
        {
            try
            {
                if (!String.IsNullOrEmpty(xml) && !String.IsNullOrEmpty(defaultNamespace))
                {
                    int currentIndex = 0;


                    //find index of tag opening character
                    int tagOpenIndex = "<CoreServiceSchema".Length;

                    //no more tag openings are found
                    if (tagOpenIndex == -1)
                    {
                        return "";
                    }

                    //if it's a closing tag
                    if (xml[tagOpenIndex + 1] == '/')
                    {
                        currentIndex = tagOpenIndex + 1;
                    }
                    else
                    {
                        currentIndex = tagOpenIndex;
                    }

                    //find corresponding tag closing character
                    int tagCloseIndex = xml.IndexOf('>', tagOpenIndex);
                    if (tagCloseIndex <= tagOpenIndex)
                    {
                        throw new Exception("Invalid XML file.");
                    }

                    //look for a colon within currently processed tag
                    String currentTagSubstring = xml.Substring(tagOpenIndex, tagCloseIndex - tagOpenIndex);
                    int firstSpaceIndex = currentTagSubstring.IndexOf(' ');
                    int nameSpaceColonIndex;
                    //if space was found
                    if (firstSpaceIndex != -1)
                    {
                        //look for namespace colon between tag open character and the first space character
                        nameSpaceColonIndex = currentTagSubstring.IndexOf(':', 0, firstSpaceIndex);
                    }
                    else
                    {
                        //look for namespace colon between tag open character and tag close character
                        nameSpaceColonIndex = currentTagSubstring.IndexOf(':');
                    }

                    //if there is no namespace
                    if (nameSpaceColonIndex == -1)
                    {
                        //insert namespace after tag opening characters '<' or '</'
                        xml = xml.Insert(currentIndex + 1, String.Format("{0} ", defaultNamespace));
                    }

                    //look for next tags after current tag closing character
                    currentIndex = tagCloseIndex;

                }

                return xml;
            }
            catch (Exception ex)
            {

                return "";
            }

        }
    }
}
