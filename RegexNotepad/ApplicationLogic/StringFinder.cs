using System;
using System.Collections.Generic;
using System.Text;

namespace RegexNotepad
{
    public abstract class StringFinder
    {
        protected List<Tuple<string, int>> Searchables { get; set; }

        public List<Tuple<string, int>> Occurrences { get; private set; }

        /// <summary>
        /// Create substrings of the text for words or sentences.
        /// When textmode is selected, insert it as one string.
        /// The integer is the startindex of the text.
        /// </summary>
        /// <param name="text"></param>
        public abstract void CreateSearchables(string text);

        //public Automaton GenerateStartWithAutomaton(string startsWith)
        //{
            
        //}

        //public Automaton GenerateContainsAutomaton(string contains)
        //{

        //}

        //public Automaton GenerateEndsWithAutomaton(string endsWith)
        //{

        //}
    }
}
