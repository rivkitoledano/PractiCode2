using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractiCode2
{
    public class HtmlElement
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var currentElement = queue.Dequeue();
                yield return currentElement;

                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            var currentElement = this;

            while (currentElement.Parent != null)
            {
                yield return currentElement.Parent;
                currentElement = currentElement.Parent;
            }
        }
      
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append($"<{Name}");

            if (!string.IsNullOrEmpty(Id))
            {
                result.Append($" id=\"{Id}\"");
            }

            if (Classes.Count > 0)
            {
                result.Append($" class=\"{string.Join(" ", Classes)}\"");
            }

            //if (Attributes.Count > 0)
            //{
            //    result.Append(" " + string.Join(" ", Attributes));
            //}

            result.Append(">");

            if (!string.IsNullOrEmpty(InnerHtml))
            {
                result.Append(InnerHtml);
            }

            if (Parent != null)
                result.Append("\n parent" + Parent.ToString());


            result.Append($"</{Name}>");

            return result.ToString();
        }
    }
    public static class HtmlElementExtensions
    {
        public static  Selector GetLastChild(this HtmlElement htmlElement,Selector selector)
        {
            if (selector.Child == null)
                return selector;
            return GetLastChild(htmlElement,selector.Child);
        }
        public static HashSet<HtmlElement> FindElements(this HtmlElement htmlElement,Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            var descendants = htmlElement.Descendants();
            selector = GetLastChild(htmlElement,selector);
            foreach (var descendant in descendants)
            {
                if (descendant.MatchesSelector(selector))
                {
                    result.Add(descendant);
                }
            }
            return result;
        }

        public static  bool MatchesSelector(this HtmlElement htmlElement,Selector selector)
        {
            if (PartOfMatchCheck(selector, htmlElement) && selector.Parent == null)
                return true;
            if (htmlElement.Parent != null && selector.Parent != null && PartOfMatchCheck(selector, htmlElement) && MatchesSelectorRec(htmlElement,selector.Parent, htmlElement.Parent))
                return true;
            return false;

        }
        public  static bool MatchesSelectorRec(this HtmlElement htmlElement,Selector selector, HtmlElement parent)
        {
            if (selector.Parent == null && parent != null)
            {
                if (PartOfMatchCheck(selector, parent))
                    return true;
                else if (parent.Parent != null)
                    return MatchesSelectorRec(htmlElement,  selector, parent.Parent);
                return false;
            }

            if (parent != null && parent.Parent != null && PartOfMatchCheck(selector, parent))
                return MatchesSelectorRec(htmlElement,selector.Parent, parent.Parent);
            else
               if (parent.Parent != null)
                return MatchesSelectorRec(htmlElement,selector, parent.Parent);
            return false;
        }
        public static bool PartOfMatchCheck(Selector selector, HtmlElement parent)
        {
            if (!string.IsNullOrEmpty(selector.TagName) && parent.Name != selector.TagName)
                return false;

            if (!string.IsNullOrEmpty(selector.Id) && parent.Id != selector.Id)
                return false;

            if (selector.Classes != null && selector.Classes.Count > 0)
            {
                foreach (var className in selector.Classes)
                {
                    if (!parent.Classes.Contains(className))
                        return false;
                }
            }
            return true;
        }
    }
}