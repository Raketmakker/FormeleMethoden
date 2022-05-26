using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad.ApplicationLogic
{
    public class TextFinder : StringFinder
    {
        public async override Task CreateSearchablesAsync(string text)
        {
            this.Searchables = new List<Tuple<string, int>>();
            this.Searchables.Add(new Tuple<string, int>(text, 0));
        }
    }
}
