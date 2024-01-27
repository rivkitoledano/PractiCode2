using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractiCode2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        //"b #n .k
        //b#n.k
        //b#n .k
        public static Selector FromQueryStringRec(string query)
        {

        }

        public static Selector FromQueryString(string query)
        {
            var selectors = query.Split(' ');
            if (selectors.Length > 1)
                FromQueryStringRec(query);

            var currentSelector = new Selector();
            string[] parts = new string[10];
            if (query.IndexOf('#') > 0)
                parts = query.Split('#'); 
            else if (query.IndexOf('.') > 0)
                parts = query.Split('.'); //"#n .k.l
            var tagName = parts[0];
            if (HtmlHelper.Helper.HtmlTags.Contains(tagName))
            {
                currentSelector.TagName = tagName;
            }
            if (parts[1].StartsWith('#'))
            {
                parts = parts[1].Split('.');//#g .h
                currentSelector.Id = parts[0].Substring(1); 
            }
            if (parts.Length > 1)
            {
                currentSelector.Classes = new List<string>(parts.Skip(1));

            }



            return currentSelector;
        }
    }





}
