using RegexNotepad.ApplicationLogic;
using RegexNotepad.Automaton;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RegexNotepad
{
    public abstract class StringFinder
    {
        private const int errorState = -1;
        protected List<Tuple<string, int>> Searchables { get; set; }

        public List<Tuple<string, int>> Occurrences { get; } = new List<Tuple<string, int>>();

        /// <summary>
        /// Create substrings of the text for words or sentences.
        /// When textmode is selected, insert it as one string.
        /// The integer is the startindex of the text.
        /// </summary>
        /// <param name="text"></param>
        public abstract Task CreateSearchablesAsync(string text);

        public async Task<SearchAutomaton<int>> GenerateStartWithAutomatonAsync(string startsWith)
        {
            var automaton = new SearchAutomaton<int>();

            for (int i = 0; i < startsWith.Length; i++)
            {
                //Transition from previous correct state to the next
                automaton.AddTransition(new AdvancedTransition<int>(i, startsWith[i], i + 1));
                //Transition from correct state to error state
                automaton.AddTransition(new AdvancedTransition<int>(i, startsWith[i], errorState, true));
            }

            //Recursive final state
            int finalState = startsWith.Length;
            automaton.AddTransition(new AdvancedTransition<int>(finalState, ' ', finalState, true));
            //Recursive error state
            automaton.AddTransition(new AdvancedTransition<int>(errorState, ' ', errorState, true));

            //First state (0) is always the start state for a START WITH function
            automaton.DefineAsStartState(0);
            //The sequence starts at state 0
            automaton.DefineAsEnterState(0);
            //The sequence exits at the length of the sequence - 1
            automaton.DefineAsExitState(startsWith.Length - 1);            
            automaton.DefineAsErrorState(errorState);
            automaton.DefineAsFinalState(finalState);

            return automaton;
        }

        public async Task<SearchAutomaton<int>> GenerateContainsAutomatonAsync(string contains)
        {
            var automaton = new SearchAutomaton<int>();

            for (int i = 0; i < contains.Length; i++)
            {
                //Transition from previous correct state to the next
                automaton.AddTransition(new AdvancedTransition<int>(i, contains[i], i + 1));

                //Return transition to start of sequence
                if (contains[i] != contains[0])
                {
                    automaton.AddTransition(new AdvancedTransition<int>(i, contains[0], 1));
                }

                //Inverted transition to start (0)
                automaton.AddTransition(new AdvancedTransition<int>(i, contains[i], 0, true));
            }

            int finalState = contains.Length;

            //From final state with good intacter to start of sequence
            automaton.AddTransition(new AdvancedTransition<int>(finalState, ' ', finalState, true));

            automaton.DefineAsStartState(0);
            automaton.DefineAsFinalState(finalState);
            return automaton;
        }

        public async Task<SearchAutomaton<int>> GenerateEndsWithAutomatonAsync(string endsWith)
        {
            var automaton = new SearchAutomaton<int>();

            for (int i = 0; i < endsWith.Length; i++)
            {
                //Transition from previous correct state to the next
                automaton.AddTransition(new AdvancedTransition<int>(i, endsWith[i], i + 1));
                
                //Return transition to start of sequence
                if(endsWith[i] != endsWith[0])
                {
                    automaton.AddTransition(new AdvancedTransition<int>(i, endsWith[0], 1));
                }

                //Inverted transition to start (0)
                automaton.AddTransition(new AdvancedTransition<int>(i, endsWith[i], 0, true));
            }

            int length = endsWith.Length;
            //From final state with good intacter to start of sequence
            automaton.AddTransition(new AdvancedTransition<int>(length, endsWith[0], 1));
            //From final state with bad intacter to start state
            automaton.AddTransition(new AdvancedTransition<int>(length, endsWith[0], 0, true));

            automaton.DefineAsStartState(0);
            automaton.DefineAsFinalState(length);
            return automaton;
        }

        public async void Find(SearchAutomaton<int> automaton)
        {
            var taskList = new List<Task<Tuple<string, int>>>();
            
            foreach (var searchable in Searchables)
            {
                taskList.Add(automaton.AcceptDFAOnly(searchable));
            }

            await Task.WhenAll(taskList);

            foreach (var task in taskList)
            {
                if(task.Result != null)
                    Occurrences.Add(Tuple.Create(task.Result.Item1, task.Result.Item2));
            }

            foreach (var occurence in Occurrences)
                System.Diagnostics.Debug.WriteLine(occurence);
        }
    }
}
