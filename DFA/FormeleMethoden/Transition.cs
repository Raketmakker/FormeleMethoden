using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week1
{
    public class Transition<T> : IComparable<Transition<T>> where T : class, IComparable
    {
        public static readonly char EPSILON = '$';

        public T FromState { get; private set; }
        public char Symbol { get; private set; }
        public T ToState { get; private set; }

        // this constructor can be used to define loops:
        public Transition(T fromOrTo, char s) : this (fromOrTo, s, fromOrTo) { }

        public Transition(T from, T to) : this(from, EPSILON, to) { }

        public Transition(T from, char s, T to)
        {
            this.FromState = from;
            this.Symbol = s;
            this.ToState = to;
        }

        public override bool Equals(object? obj)
        {
            return obj is Transition<T> transition &&
                   EqualityComparer<T>.Default.Equals(FromState, transition.FromState) &&
                   Symbol == transition.Symbol &&
                   EqualityComparer<T>.Default.Equals(ToState, transition.ToState);
        }

        public int CompareTo(Week1.Transition<T> other)
        {
            int fromCmp = FromState.CompareTo(other.FromState);
            int symbolCmp = Symbol.CompareTo(other.Symbol);
            int toCmp = ToState.CompareTo(other.ToState);

            return (fromCmp != 0 ? fromCmp : (symbolCmp != 0 ? symbolCmp : toCmp));
        }

        public override string ToString()
        {
            return "(" + this.FromState + ", " + this.Symbol + ")" + "-->" + this.ToState;
        }
    }
}
