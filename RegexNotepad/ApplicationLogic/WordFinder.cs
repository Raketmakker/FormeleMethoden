using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad.ApplicationLogic
{
    public class WordFinder : StringFinder
    {
        public async override Task CreateSearchablesAsync(string text)
        {
            SearchAutomaton<char> automaton = new SearchAutomaton<char>();
            
            char[] splitCharacters = { ' ', '\n', '\r' };
            
            foreach (char c in splitCharacters)
            {
                automaton.AddTransition(new AdvancedTransition<char>('S', c, 'S'));
                automaton.AddTransition(new AdvancedTransition<char>('A', c, 'B'));
                automaton.AddTransition(new AdvancedTransition<char>('B', c, 'B'));
            }
            
            automaton.AddTransition(new AdvancedTransition<char>('S', ' ', 'A', true));            
            automaton.AddTransition(new AdvancedTransition<char>('A', ' ', 'A', true));            
            automaton.AddTransition(new AdvancedTransition<char>('B', ' ', 'A', true));

            automaton.DefineAsStartState('S');
            
            automaton.DefineAsFinalState('S');
            automaton.DefineAsFinalState('A');
            automaton.DefineAsFinalState('B');
            
            automaton.DefineAsSuccesState('A');

            this.Searchables = automaton.Split(text);
        }
    }
}
