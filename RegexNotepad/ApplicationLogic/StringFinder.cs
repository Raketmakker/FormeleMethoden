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

            //First state (0) is always the start state for a START WITH function
            automaton.DefineAsStartState('0');
            //The sequence starts at state 0
            automaton.DefineAsEnterState('0');
            //The sequence exits at the length of the sequence - 1
            automaton.DefineAsExitState(char.Parse((startsWith.Length - 1).ToString()));            
            automaton.DefineAsErrorState('E');
            automaton.DefineAsFinalState(finalState);

            return automaton;
        }

        public async Task<SearchAutomaton<char>> GenerateContainsAutomatonAsync(string contains)
        {
            var automaton = new SearchAutomaton<char>();

            for (int i = 0; i < contains.Length; i++)
            {
                //Transition from previous correct state to the next
                automaton.AddTransition(new AdvancedTransition<char>(ToChar(i), contains[i], ToChar(i + 1)));

                //Return transition to start of sequence
                if (contains[i] != contains[0])
                {
                    automaton.AddTransition(new AdvancedTransition<char>(ToChar(i), contains[0], '1'));
                }

                //Inverted transition to start (0)
                automaton.AddTransition(new AdvancedTransition<char>(ToChar(i), contains[i], '0', true));
            }

            char finalState = ToChar(contains.Length);

            //From final state with good character to start of sequence
            automaton.AddTransition(new AdvancedTransition<char>(finalState, ' ', finalState, true));

            automaton.DefineAsStartState('0');
            automaton.DefineAsFinalState(finalState);
            return automaton;
        }

        public async Task<SearchAutomaton<char>> GenerateEndsWithAutomatonAsync(string endsWith)
        {
            var automaton = new SearchAutomaton<char>();

            for (int i = 0; i < endsWith.Length; i++)
            {
                //Transition from previous correct state to the next
                automaton.AddTransition(new AdvancedTransition<char>(char.Parse(i.ToString()), endsWith[i], char.Parse((i + 1).ToString())));
                
                //Return transition to start of sequence
                if(endsWith[i] != endsWith[0])
                {
                    automaton.AddTransition(new AdvancedTransition<char>(char.Parse(i.ToString()), endsWith[0], '1'));
                }

                //Inverted transition to start (0)
                automaton.AddTransition(new AdvancedTransition<char>(char.Parse(i.ToString()), endsWith[i], '0', true));
            }

            //From final state with good character to start of sequence
            automaton.AddTransition(new AdvancedTransition<char>(char.Parse(endsWith.Length.ToString()), endsWith[0], '1'));
            //From final state with bad character to start state
            automaton.AddTransition(new AdvancedTransition<char>(char.Parse(endsWith.Length.ToString()), endsWith[0], '0', true));

            automaton.DefineAsStartState('0');
            automaton.DefineAsFinalState(char.Parse(endsWith.Length.ToString()));
            return automaton;
        }

        public async void Find(SearchAutomaton<char> automaton)
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
        
        private char ToChar(int number)
        {
            return Convert.ToChar(number + 48);
        }
    }
}
