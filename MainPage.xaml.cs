using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using mIDE.InsertClasses;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mIDE
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AutoCompleteList cmdList;

        public MainPage()
        {
            cmdList = new AutoCompleteList();

            this.InitializeComponent();
        }

        private void SearchForAutoComplete()
        {

        }

        private string CaretLine = null;
        private int LineStart = 0, LineEnd = 0;
        private string CaretWord = null;
        private int WordStart = 0, WordEnd = 0;
        private int CaretLocationInWord = 0;
        private bool UpdateCaretText()
        {
            string[] returning = new string[2];
            string allLines;
            OpenCode.Document.GetText(Windows.UI.Text.TextGetOptions.None, out allLines);
            allLines.Replace('\v', '\r');
            string[] splitLines = allLines.Split('\r');

            //find the line that the caret is on
            int lineOffset = 0;
            int currentLine = 0;
            CaretLine = null;
            while (CaretLine == null)
            {
                if (OpenCode.Document.Selection.StartPosition > splitLines[currentLine].Length + lineOffset)
                {
                    lineOffset += splitLines[currentLine].Length + 1;
                    currentLine++;
                }
                else
                {
                    CaretLine = splitLines[currentLine];
                    LineStart = lineOffset;
                    LineEnd = lineOffset + splitLines[currentLine].Length;
                }
            }

            //find the word that the caret is on
            string[] splitWords = CaretLine.Split(stringSeparators);
            int wordOffset = lineOffset;
            int currentWord = 0;
            CaretWord = null;
            while (CaretWord == null)
            {
                if (OpenCode.Document.Selection.StartPosition > splitWords[currentWord].Length + wordOffset)
                {
                    wordOffset += splitWords[currentWord].Length + 1;
                    currentWord++;
                }
                else
                {
                    CaretWord = splitWords[currentWord];
                    WordStart = wordOffset;
                    WordEnd = wordOffset + splitWords[currentWord].Length;
                    CaretLocationInWord = OpenCode.Document.Selection.StartPosition - wordOffset;
                }
            }

            lastCaretPosition = OpenCode.Document.Selection.StartPosition;

            return true;
        }

        private void UpdateAndClearCaretLineFormatting()
        {
            UpdateCaretText();

            var cf = OpenCode.Document.GetRange(LineStart, LineEnd).CharacterFormat;
            cf.ForegroundColor = Windows.UI.Colors.GhostWhite;
        }

        private char[] stringSeparators = new char[5] { ' ', '(', ')', ',', '.'};
        private void SearchLineForCommands()
        {
            UpdateAndClearCaretLineFormatting();
            commandSerchTextBox.Text = "";

            autofillListBox.Items.Clear();
            autoCompleteRemove = "";
            if (CaretWord != "")
            {
                //search for auto complete
                if (CaretLocationInWord == CaretWord.Length)
                {
                    //search for exact commands and fill the autocomplete
                    List<AutoComplete> exactCmds = cmdList.findExactCommand(CaretWord);
                    foreach (AutoComplete cmd in exactCmds)
                    {
                        autofillListBox.Items.Add(cmd.succeedingString);
                        autofillListBox.SelectedIndex = 0;
                    }
                    if (exactCmds.Count == 0)
                    {
                        //if no exact commands were found, search for partial commands and fill autocomplete
                        List<AutoComplete> cmds = cmdList.findCommandStartingWith(CaretWord);
                        foreach (AutoComplete cmd in cmds)
                        {
                            string addStr = cmd.name.Remove(0, CaretWord.Length);
                            autofillListBox.Items.Add(cmd.name);
                            autoCompleteRemove = CaretWord;
                            autofillListBox.SelectedIndex = 0;
                        }
                    }
                }
            }
            if (autofillListBox.Items.Count == 0)
            {
                //if no commands were found, search for links and fill autocomplete
                List<InsertLink> links = cmdList.findAutoCompleteLinks(CaretWord);
                foreach (InsertLink link in links)
                {
                    autofillListBox.Items.Add(link.LinkAutoComplete);
                    autoCompleteRemove = link.LinkString;
                    autofillListBox.SelectedIndex = 0;
                }
            }
            //commandSerchTextBox.Text = text3 + " - " + text4 + "  :  " + text3 + text4;       

            //search for commands from back to front
            string[] strings = CaretLine.Split(stringSeparators);
            int endIndex = LineEnd;
            for (int i = strings.Length - 1; i >= 0; i--)
            {
                if (strings[i] != "")
                {
                    var localWord = OpenCode.Document.GetRange(endIndex - strings[i].Length, endIndex);
                    /*var localWord = new TextRange(OpenCode.CaretPosition.GetLineStartPosition(0).
                            GetPositionAtOffset(endIndex - strings[i].Length),
                            OpenCode.CaretPosition.GetLineStartPosition(0).
                            GetPositionAtOffset(endIndex));*/
                    string test = localWord.Text;

                    List<AutoComplete> coms = cmdList.findExactCommand(strings[i]);
                    if (coms.Count > 0)
                    {
                        localWord.CharacterFormat.ForegroundColor = coms[0].contextColor;
                        //localWord.ApplyPropertyValue(TextElement.ForegroundProperty, coms[0].contextBrush);
                    }
                    else
                    {
                        //word.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.WhiteSmoke);
                    }

                    endIndex -= strings[i].Length + 1;
                }
                else { endIndex--; }
            }
            commandSerchTextBox.Text += " : " + CaretLine.Length;
        }

        string autoCompleteRemove = "";
        private bool InsertAutocomplete()
        {
            if (autofillListBox.Items.Count > 0 && autofillListBox.Items[autofillListBox.SelectedIndex].ToString() != "")
            {
                UpdateAndClearCaretLineFormatting();
                if (autoCompleteRemove == "")
                {
                    //insert autocomplete
                    OpenCode.Document.Selection.TypeText(autofillListBox.Items[autofillListBox.SelectedIndex].ToString());
                    SearchLineForCommands();
                    return true;
                }
                else
                {
                    if (CaretWord == autoCompleteRemove)
                    {
                        //remove word and insert autocomplete
                        string insert = autofillListBox.Items[autofillListBox.SelectedIndex].ToString();
                        OpenCode.Document.GetRange(WordStart, WordEnd).Text = insert;
                        OpenCode.Document.Selection.StartPosition += insert.Length;
                        OpenCode.Document.Selection.EndPosition = OpenCode.Document.Selection.StartPosition;
                        SearchLineForCommands();
                        return true;
                    }
                }
                SearchLineForCommands();
            }
            return false;
        }

        private void SelectNextAutocomplete(KeyRoutedEventArgs e)
        {
            if (autofillListBox.Items.Count > 0 && autofillListBox.Items[0].ToString() != "")
            {
                if (e.Key == VirtualKey.Down && autofillListBox.SelectedIndex < autofillListBox.Items.Count - 1)
                {
                    autofillListBox.SelectedIndex++;
                    OpenCode.Document.Selection.StartPosition = lastCaretPosition;
                    OpenCode.Document.Selection.EndPosition = lastCaretPosition;
                }
                if (e.Key == VirtualKey.Up && autofillListBox.SelectedIndex > 0)
                {
                    autofillListBox.SelectedIndex--;
                    OpenCode.Document.Selection.StartPosition = lastCaretPosition;
                    OpenCode.Document.Selection.EndPosition = lastCaretPosition;
                }
            }
        }

        private int lastCaretPosition = 0;
        private void RichTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Down:
                case VirtualKey.Up:
                    SelectNextAutocomplete(e);
                    break;
                default:
                    SearchLineForCommands();
                    break;
            }
            //lastCaretPosition = OpenCode.CaretPosition;
        }

        private void OpenCode_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            SearchLineForCommands();
            //lastCaretPosition = OpenCode.CaretPosition;
        }

        private void OpenCode_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Tab:
                    if (!InsertAutocomplete()) { OpenCode.Document.Selection.TypeText("\t"); }
                    e.Handled = true;
                    break;
            }
        }
    }
}
