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
        public bool StartBoxChecked { get; set; }
        public string StartText { get; set; }
        public bool ContainsBoxChecked { get; set; }
        public string ContainsText { get; set; }
        public bool EndBoxChecked { get; set; }
        public string EndText { get; set; }
    }
}
