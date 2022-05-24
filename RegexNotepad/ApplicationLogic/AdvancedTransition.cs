using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RegexNotepad.ApplicationLogic
{
    public class AdvancedTransition<T> : IComparable<AdvancedTransition<T>> where T : IComparable<T>
    {
        public T FromState { get; set; }
        public char Symbol { get; set; }
        public T ToState { get; set; }
        public bool InvertedTransition { get; set; }

        public AdvancedTransition(T fromState, char symbol, T toState, bool invertedTransition = false)
        {
            FromState = fromState;
            Symbol = symbol;
            ToState = toState;
            InvertedTransition = invertedTransition;
        }

        public override bool Equals(object obj)
        {
            return obj is AdvancedTransition<T> transition &&
                   EqualityComparer<T>.Default.Equals(FromState, transition.FromState) &&
                   Symbol == transition.Symbol &&
                   EqualityComparer<T>.Default.Equals(ToState, transition.ToState) &&
                   InvertedTransition == transition.InvertedTransition;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FromState, Symbol, ToState, InvertedTransition);
        }

        public int CompareTo(AdvancedTransition<T> other)
        {
            if (other == null) return 1;
            int fromStateComparison = FromState.CompareTo(other.FromState);
            int symbolComparison = Symbol.CompareTo(other.Symbol);
            int toStateComparison = ToState.CompareTo(other.ToState);
            int isInverseSymbolComparison = InvertedTransition.CompareTo(other.InvertedTransition);
            return (fromStateComparison != 0 ? fromStateComparison :
                (symbolComparison != 0 ? symbolComparison :
                (toStateComparison != 0 ? toStateComparison :
                isInverseSymbolComparison)));
        }
    }
}
