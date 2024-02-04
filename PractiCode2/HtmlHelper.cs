using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PractiCode2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _htmlHelper = new HtmlHelper();
        public static HtmlHelper Helper => _htmlHelper;
        public string[] SelfClosingTags { get; set; }
        public string[] HtmlTags { get; set; }
        private HtmlHelper() {
            string htmlTagsJson = File.ReadAllText("C:\\Users\\user1\\Documents\\רבקי למודים\\פרקטיקוד\\PractiCode2\\htmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(htmlTagsJson);

            // Load self-closing tags from JSON file
            string selfClosingTagsJson = File.ReadAllText("C:\\Users\\user1\\Documents\\רבקי למודים\\פרקטיקוד\\PractiCode2\\HtmlVoidTags.json");
            SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
        }


    }
}
