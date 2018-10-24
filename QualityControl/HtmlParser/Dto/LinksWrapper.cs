using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HtmlParser.Dto
{
    public class LinksWrapper
    {
        public List<LinkResult> NormalLinks { get; set; }
        public List<LinkResult> BrokenLinks { get; set; }
    }
}
