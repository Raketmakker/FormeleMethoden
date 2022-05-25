using RegexNotepad.ApplicationLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad
{
    public abstract class StringFinder
    {
        protected List<Tuple<string, int>> Searchables { get; set; }

        public List<Tuple<string, int>> Occurrences { get; } = new List<Tuple<string, int>>();

        /// <summary>
        /// Create substrings of the text for words or sentences.
        /// When textmode is selected, insert it as one string.
        /// The integer is the startindex of the text.
        /// </summary>
        /// <param name="text"></param>
        public abstract Task CreateSearchablesAsync(string text);

        public async Task<SearchAutomaton<char>> GenerateStartWithAutomatonAsync(string startsWith)
        {
            var automaton = new SearchAutomaton<char>();

            for (int i = 0; i < startsWith.Length; i++)
            {
                //Transition from previous correct state to the next
                automaton.AddTransition(new AdvancedTransition<char>(char.Parse(i.ToString()), startsWith[i], char.Parse((i + 1).ToString())));
                //Transition from correct state to error state
                automaton.AddTransition(new AdvancedTransition<char>(char.Parse(i.ToString()), startsWith[i], 'E', true));
            }

            //Recursive final state
            char finalState = char.Parse(startsWith.Length.ToString());
            automaton.AddTransition(new AdvancedTransition<char>(finalState, ' ', finalState, true));
            //Recursive error state
            automaton.AddTransition(new AdvancedTransition<char>('E', ' ', 'E', true));

            automaton.DefineAsStartState('0');
            automaton.DefineAsErrorState('E');
            automaton.DefineAsFinalState(finalState);

            return automaton;
        }

        //public Automaton GenerateContainsAutomaton(string contains)
        //{

        //}

        //public Automaton GenerateEndsWithAutomaton(string endsWith)
        //{

        //}

        public async void Find(SearchAutomaton<char> automaton)
        {
            var taskList = new List<Task<List<Tuple<string, int>>>>();
            
            foreach (var searchable in Searchables)
            {
                taskList.Add(automaton.AcceptDFAOnly(searchable));
            }

            await Task.WhenAll(taskList);

            foreach (var task in taskList)
            {
                Occurrences.AddRange(task.Result);
            }
            Console.WriteLine(Occurrences);
        }
    }
}
