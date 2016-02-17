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
using mIDE.DocumentHelpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace mIDE
{
    public sealed partial class MainPage : Page
    {
        private AutoCompleteList cmdList;
        private ActiveDocumentList docList;
        private ActiveDocument docShow;

        public MainPage()
        {
            cmdList = new AutoCompleteList();
            docList = new ActiveDocumentList();

            this.InitializeComponent();

            OpenCode.CustomKeyDown += OpenCode_KeyDown;

            docShow = new ActiveDocument("");
        }

        private void ShowDocument(ActiveDocument doc)
        {
            docShow = doc;
            OpenCode.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, docShow.Text);
            UpdateVisibleFilesTab();
            UpdateAllLines();
        }

        private void UpdateAllLines()
        {
            string allLines;
            OpenCode.Document.GetText(Windows.UI.Text.TextGetOptions.None, out allLines);
            allLines.Replace('\v', '\r');
            string[] splitLines = allLines.Split('\r');

            //find the line that the caret is on
            int offset = 0;
            int currentLine = splitLines.Length - 1;
            while (offset < allLines.Length)
            {
                if (splitLines[currentLine].Length > 0)
                {
                    SearchLineForCommands(
                        allLines.Length - offset - splitLines[currentLine].Length,
                        allLines.Length - offset);
                }

                offset += splitLines[currentLine].Length + 1;
                currentLine--;
            }
        }

        private void FileTab_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowDocument(docList.GetActiveDocument((sender as Button).Name));
        }

        private void UpdateVisibleFilesTab()
        {
            VisibleFilesTab.Children.Clear();
            foreach (ActiveDocument actDoc in docList.VisibleDocuents())
            {
                var button = new Button();
                button.Height = 20;
                button.Width = 20 + actDoc.FileName.Length * 5;
                button.Padding = new Thickness(0);
                button.BorderThickness = new Thickness(1);
                button.BorderBrush = new SolidColorBrush(Colors.Black);
                button.Content = actDoc.FileName;
                button.Name = actDoc.FilePath;
                button.FontSize = 10;
                button.FontFamily = new FontFamily("Consolas");
                button.Tapped += new TappedEventHandler(FileTab_Tapped);
                if (docShow.FilePath == actDoc.FilePath) { button.Background = new SolidColorBrush(Colors.DarkGray); }
                VisibleFilesTab.Children.Add(button);
            }
        }

        private void UpdateLineNumbers(int numLines)
        {
            string lnText = "  1";
            for (int i = 2; i < numLines; i++)
            {
                if (i < 10) { lnText += "\n  " + i; }
                else if (i < 100) { lnText += "\n " + i; }
                else { lnText += "\n" + i; }
                
            }
            LineNumberEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, lnText);
        }

        private string CaretLine = null;
        private int CaretLineStart = 0, CaretLineEnd = 0;
        private string CaretWord = null;
        private int CaretWordStart = 0, CaretWordEnd = 0;
        private int CaretLocationInWord = 0;
        private bool UpdateCaretText()
        {
            string[] returning = new string[2];
            string allLines;
            OpenCode.Document.GetText(Windows.UI.Text.TextGetOptions.None, out allLines);
            allLines.Replace('\v', '\r');
            string[] splitLines = allLines.Split('\r');

            UpdateLineNumbers(splitLines.Length);

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
                    CaretLineStart = lineOffset;
                    CaretLineEnd = lineOffset + splitLines[currentLine].Length;
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
                    CaretWordStart = wordOffset;
                    CaretWordEnd = wordOffset + splitWords[currentWord].Length;
                    CaretLocationInWord = OpenCode.Document.Selection.StartPosition - wordOffset;
                }
            }

            return true;
        }

        private void UpdateAndClearCaretLineFormatting()
        {
            UpdateCaretText();

            var cf = OpenCode.Document.GetRange(CaretLineStart, CaretLineEnd).CharacterFormat;
            cf.ForegroundColor = Colors.GhostWhite;
        }

        private void SearchForAutoComplete()
        {
            autofillListBox.Items.Clear();
            autoCompleteRemove = "";
            if (CaretWord != "")
            {
                //search for auto complete
                if (CaretLocationInWord == CaretWord.Length)
                {
                    //search for exact commands and fill the autocomplete
                    List<InstructionFramework> exactFrame = cmdList.findExactInstruction(CaretWord);
                    /*foreach (Instruction cmd in exactCmds)
                    {
                        autofillListBox.Items.Add(new ListBoxItem { Padding = new Thickness(3), BorderThickness = new Thickness(0.1), Height = 24, Content = cmd.framework });
                        autofillListBox.SelectedIndex = 0;
                    }*/
                    if (exactFrame.Count == 0)
                    {
                        //if no exact commands were found, search for partial commands and fill autocomplete
                        List<InstructionFramework> insts = cmdList.findCommandStartingWith(CaretWord);
                        foreach (InstructionFramework inst in insts)
                        {
                            string addStr = inst.Name.Remove(0, CaretWord.Length);
                            if (autofillListBox.Items.Count > 0 &&
                                (string)((autofillListBox.Items[autofillListBox.Items.Count - 1]) as ListBoxItem).Content == inst.Framework)
                            { }
                            else
                            {
                                autofillListBox.Items.Add(new ListBoxItem { Padding = new Thickness(3), BorderThickness = new Thickness(0.1), Height = 24, Content = inst.Framework });
                                //autofillListBox.Items.Add(cmd.name);
                                autoCompleteRemove = CaretWord;
                                autofillListBox.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
            if (autofillListBox.Items.Count == 0)
            {
                //if no commands were found, search for links and fill autocomplete
                /*List<InsertLink> links = cmdList.findAutoCompleteLinks(CaretWord);
                foreach (InsertLink link in links)
                {
                    autofillListBox.Items.Add(new ListBoxItem { Padding = new Thickness(3), BorderThickness = new Thickness(0.1), Height = 24, Content = link.LinkAutoComplete });
                    autoCompleteRemove = link.LinkString;
                    autofillListBox.SelectedIndex = 0;
                }*/
            }
            //commandSerchTextBox.Text = text3 + " - " + text4 + "  :  " + text3 + text4;
        }

        private void SearchCaretLineForCommands()
        {
            UpdateAndClearCaretLineFormatting();
            commandSerchTextBox.Text = "";
            SearchLineForCommands(CaretLineStart, CaretLineEnd);
            SearchForAutoComplete();
            SearchAllLinesForErrors(); //TODO : set to search caret line for errors when that is available
        }

        private char[] stringSeparators = new char[7] { ' ', '(', ')', ',', '.', '\t', ':'};
        private void SearchLineForCommands(int lineStart, int lineEnd)
        {
            //search for commands from back to front
            var textRange = OpenCode.Document.GetRange(lineStart, lineEnd);
            textRange.CharacterFormat.BackgroundColor = Colors.Transparent;
            string outString;
            textRange.GetText(Windows.UI.Text.TextGetOptions.None, out outString);
            string[] strings = outString.Split(stringSeparators);
            int endIndex = lineEnd;
            for (int i = strings.Length - 1; i >= 0; i--)
            {
                if (strings[i] != "")
                {
                    var localWord = OpenCode.Document.GetRange(endIndex - strings[i].Length, endIndex);

                    List<InstructionFramework> coms = cmdList.findExactInstruction(strings[i]);
                    if (coms.Count > 0)
                    {
                        localWord.CharacterFormat.ForegroundColor = coms[0].ContextColor;
                    }
                    else
                    {
                        localWord.CharacterFormat.ForegroundColor = Colors.GhostWhite;
                    }

                    endIndex -= strings[i].Length + 1;
                }
                else { endIndex--; }
            }
            commandSerchTextBox.Text += " : " + CaretLine.Length;
        }

        private void SearchAllLinesForErrors()
        {
            //string allLines;
            //OpenCode.Document.GetText(Windows.UI.Text.TextGetOptions.None, out allLines);
            //allLines.Replace('\v', '\r');
            //string[] splitLines = allLines.Split('\r');
            if (docShow != null)
            {
                string allLines;
                OpenCode.Document.GetText(Windows.UI.Text.TextGetOptions.None, out allLines);
                docShow.Text = allLines;

                foreach (AutoCheckError ace in cmdList.CheckAutoCompleteErrors(docShow))
                {
                    Color col = Colors.DeepPink;
                    if (ace.Severity == ErrorSeverity.none) { col = Colors.DarkGoldenrod; }
                    if (ace.Severity == ErrorSeverity.buildError) { col = Colors.DarkOrange; }
                    if (ace.Severity == ErrorSeverity.runtimeError) { col = Colors.Red; }
                    //Colors.DarkGoldenrod
                    OpenCode.Document.GetRange(ace.StartIndex, ace.EndIndex).CharacterFormat.BackgroundColor = col;
                }
            }
        }

        string autoCompleteRemove = "";
        private bool InsertAutocomplete()
        {
            var selectedAutofillItem = autofillListBox.Items[autofillListBox.SelectedIndex] as ListBoxItem;
            //if (autofillListBox.Items.Count > 0 && autofillListBox.Items[autofillListBox.SelectedIndex].ToString() != "")
            if (autofillListBox.Items.Count > 0 && selectedAutofillItem.Content as string != "")
            {
                UpdateAndClearCaretLineFormatting();
                if (autoCompleteRemove == "")
                {
                    //insert autocomplete
                    OpenCode.Document.Selection.TypeText(selectedAutofillItem.Content as string);
                    return true;
                }
                else
                {
                    if (CaretWord == autoCompleteRemove ||
                        CaretWord.Remove(CaretWord.Length - 1, 1).Remove(0, 1) == autoCompleteRemove)
                    {
                        //remove word and insert autocomplete
                        string insert = selectedAutofillItem.Content as string;
                        OpenCode.Document.GetRange(CaretWordStart, CaretWordEnd).Text = insert;
                        OpenCode.Document.Selection.StartPosition += insert.Length;
                        OpenCode.Document.Selection.EndPosition = OpenCode.Document.Selection.StartPosition;
                        return true;
                    }
                }
            }
            SearchCaretLineForCommands();
            return false;
        }

        private bool SelectNextAutocomplete(KeyRoutedEventArgs e)
        {
            if (autofillListBox.Items.Count > 0)
            {
                if (e.Key == VirtualKey.Down && autofillListBox.SelectedIndex < autofillListBox.Items.Count - 1)
                {
                    autofillListBox.SelectedIndex++;
                    return true;
                }
                if (e.Key == VirtualKey.Up && autofillListBox.SelectedIndex > 0)
                {
                    autofillListBox.SelectedIndex--;
                    return true;
                }
            }
            return false;
        }

        private void OpenCode_SelectionChanged(object sender, RoutedEventArgs e)
        {
            SearchCaretLineForCommands();
        }

        private async void Open_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Open a text file.
            var open = new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".ks");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();
            //File.SetAttributes(file.Path, FileAttributes.Normal);

            if (file != null)
            {
                try
                {
                    var doc = new ActiveDocument(await Windows.Storage.FileIO.ReadTextAsync(file));
                    doc.Visible = true;
                    doc.FilePath = file.Path;
                    docList.Add(doc);
                    ShowDocument(doc);
                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = ex.ToString(),//"File open error",
                        Content = "Sorry, I can't do that...",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private async void Save_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (docShow != null)
            {
                string text;
                OpenCode.Document.GetText(Windows.UI.Text.TextGetOptions.None, out text);
                docShow.Text = text;
                if (docShow.FilePath != null && docShow.FileName != null)
                {
                    try
                    {
                        var storeFolder = await Windows.Storage.StorageFolder.GetFolderFromPathAsync(docShow.FilePath);
                        File.SetAttributes(docShow.FilePath, FileAttributes.Normal);
                        var storeFile = await storeFolder.GetFileAsync(docShow.FileName);
                        await Windows.Storage.FileIO.WriteTextAsync(storeFile, docShow.Text);
                    }
                    catch (Exception ex)
                    {
                        /*ContentDialog errorDialog = new ContentDialog()
                        {
                            Title = ex.ToString(),//"File save error",
                            Content = "Sorry, I can't do that...",
                            PrimaryButtonText = "Ok"
                        };

                        await errorDialog.ShowAsync();*/

                        SaveAs();
                    }
                }
                else { SaveAs(); }
            }
            else { SaveAs(); }
        }

        private async void SaveAs()
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("kerboscript", new List<string>() { ".ks" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = docShow.FileName;

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                try
                {
                    // Prevent updates to the remote version of the file until
                    // we finish making changes and call CompleteUpdatesAsync.
                    Windows.Storage.CachedFileManager.DeferUpdates(file);
                    // write to file
                    string text;
                    OpenCode.Document.GetText(Windows.UI.Text.TextGetOptions.None, out text);
                    docShow.Text = text;
                    await Windows.Storage.FileIO.WriteTextAsync(file, docShow.Text);
                    // Let Windows know that we're finished changing the file so
                    // the other app can update the remote version of the file.
                    // Completing updates may require Windows to ask for user input.
                    Windows.Storage.Provider.FileUpdateStatus status =
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = ex.ToString(),//"File save as error",
                        Content = "Sorry, I can't do that...",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private void SaveAll_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Compact_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void Page_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Down:
                case VirtualKey.Up:
                    break;
            }
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch(e.Key)
            {
                case VirtualKey.Down:
                case VirtualKey.Up:
                    break;
            }
        }

        private void RichTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Down:
                case VirtualKey.Up:
                    //if (autofillListBox.Items.Count == 0) { SearchCaretLineForCommands(); }
                    //else
                    //{
                    //    SelectNextAutocomplete(e);
                    //    OpenCode.Document.Selection.StartPosition = CaretPositionStart;
                    //    OpenCode.Document.Selection.EndPosition = CaretPositionEnd;
                    //}
                    break;
                default:
                    SearchCaretLineForCommands();
                    break;
            }
        }

        private void OpenCode_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Tab:
                    if (!InsertAutocomplete()) { OpenCode.Document.Selection.TypeText("\t"); }
                    e.Handled = true;
                    break;
                case VirtualKey.Down:
                case VirtualKey.Up:
                    //is never called
                    if (SelectNextAutocomplete(e)) { e.Handled = true; }
                    break;
            }
        }
    }
}
