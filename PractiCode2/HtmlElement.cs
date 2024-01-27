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
        public List<string> Attributes { get; set; }=new List<string>();
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

        public IEnumerable<HtmlElement> FindElements(Selector selector)
        {
            List<HtmlElement> result = new List<HtmlElement>();
            FindElementsRecursive(this, selector, result);
            return result;
        }

        private void FindElementsRecursive(HtmlElement currentElement, Selector selector, List<HtmlElement> result)
        {
            var descendants = currentElement.Descendants();

            foreach (var descendant in descendants)
            {
                if (descendant.MatchesSelector(selector))
                {
                      result.Add(descendant);

                    if (selector.Child != null)
                    {
                        descendant.FindElementsRecursive(descendant, selector.Child, result);
                    }
                }
            }
        }
        public bool MatchesSelector(Selector selector)
        {

            if (!string.IsNullOrEmpty(selector.TagName) && Name != selector.TagName)
               return false;   

            if (!string.IsNullOrEmpty(selector.Id) && Id != selector.Id)
                return false;

            if (selector.Classes != null && selector.Classes.Count > 0)
            {
                foreach (var className in selector.Classes)
                {
                    if (!Classes.Contains(className))
                        return false;
                }
            }
            return true;

        }
    }
}
