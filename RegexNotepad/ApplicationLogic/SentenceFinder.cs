using RegexNotepad.Automaton;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad.ApplicationLogic
{
    public class SentenceFinder : StringFinder
    {
        public async override Task CreateSearchablesAsync(string text)
        {
            SplitAutomaton<char> automaton = new SplitAutomaton<char>();

            char split = '.';

            automaton.AddTransition(new AdvancedTransition<char>('S', split, 'S'));
            automaton.AddTransition(new AdvancedTransition<char>('S', split, 'A', true));
            automaton.AddTransition(new AdvancedTransition<char>('A', split, 'S'));
            automaton.AddTransition(new AdvancedTransition<char>('A', split, 'A', true));

            automaton.DefineAsStartState('S');
            automaton.DefineAsFinalState('S');
            automaton.DefineAsFinalState('A');

            automaton.DefineAsSuccesState('A');

            this.Searchables = automaton.Split(text);
        }
    }
}
