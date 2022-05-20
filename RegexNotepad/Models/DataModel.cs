using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegexNotepad.Models
{
    public class DataModel : ObservableObject
    {
        public enum TextType { words, sentences, text }
        public string Text { get; set; }
        public TextType Type { get; set; }
    }
}
