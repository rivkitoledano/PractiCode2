using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractiCode2
{
    internal class HtmlElement
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
        public Selector GetLastChild(Selector selector)
        {
            if(selector.Child == null)
                return selector;
            return GetLastChild(selector.Child);
        }
        public HashSet<HtmlElement> FindElements(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            var descendants = this.Descendants();
            selector = GetLastChild(selector);
            foreach (var descendant in descendants)
            {
                if (descendant.MatchesSelector(selector))
                {
                    result.Add(descendant);
                }
            }
            return result;
        }




        public bool MatchesSelector(Selector selector)
        {
            if (PartOfMatchCheck(selector, this) && selector.Parent == null)
                return true;
            if (Parent != null  && selector.Parent != null&& PartOfMatchCheck(selector, this) && MatchesSelectorRec(selector.Parent, Parent))
                return true;
            return false;
            
        }
        public bool MatchesSelectorRec(Selector selector, HtmlElement parent)
        {
            if (selector.Parent == null && parent != null)
            {
                if (PartOfMatchCheck(selector, parent))
                    return true;
                else if (parent.Parent != null)
                    return MatchesSelectorRec(selector, parent.Parent);
                return false;
            }


            if (parent != null&& parent.Parent != null && PartOfMatchCheck(selector, parent))
                return MatchesSelectorRec(selector.Parent, parent.Parent);
            else 
               if(parent.Parent != null)
                return MatchesSelectorRec(selector, parent.Parent);
            return false;
        }
        public bool PartOfMatchCheck(Selector selector, HtmlElement parent)
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
           
            if(Parent != null)
            result.Append("\n parent" + Parent.ToString());


            result.Append($"</{Name}>");

            return result.ToString();
        }
    }

}