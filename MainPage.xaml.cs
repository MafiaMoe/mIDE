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
        }

        private void ShowDocument(ActiveDocument doc)
        {
            docShow = doc;
            OpenCode.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, docShow.Text);
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
                        autofillListBox.Items.Add(new ListBoxItem { Padding = new Thickness(3), BorderThickness = new Thickness(0.1), Height = 24, Content = cmd.succeedingString });
                        autofillListBox.SelectedIndex = 0;
                    }
                    if (exactCmds.Count == 0)
                    {
                        //if no exact commands were found, search for partial commands and fill autocomplete
                        List<AutoComplete> cmds = cmdList.findCommandStartingWith(CaretWord);
                        foreach (AutoComplete cmd in cmds)
                        {
                            string addStr = cmd.name.Remove(0, CaretWord.Length);
                            autofillListBox.Items.Add(new ListBoxItem { Padding = new Thickness(3), BorderThickness = new Thickness(0.1), Height = 24, Content = cmd.name });
                            //autofillListBox.Items.Add(cmd.name);
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
                    autofillListBox.Items.Add(new ListBoxItem { Padding = new Thickness(3), BorderThickness = new Thickness(0.1), Height = 24, Content = link.LinkAutoComplete });
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
                    string test = localWord.Text;

                    List<AutoComplete> coms = cmdList.findExactCommand(strings[i]);
                    if (coms.Count > 0)
                    {
                        localWord.CharacterFormat.ForegroundColor = coms[0].contextColor;
                    }
                    else
                    {
                        //localWord...
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
                    if (CaretWord == autoCompleteRemove)
                    {
                        //remove word and insert autocomplete
                        string insert = selectedAutofillItem.Content as string;
                        OpenCode.Document.GetRange(WordStart, WordEnd).Text = insert;
                        OpenCode.Document.Selection.StartPosition += insert.Length;
                        OpenCode.Document.Selection.EndPosition = OpenCode.Document.Selection.StartPosition;
                        return true;
                    }
                }
            }
            SearchLineForCommands();
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
            SearchLineForCommands();
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
                        ContentDialog errorDialog = new ContentDialog()
                        {
                            Title = ex.ToString(),//"File open error",
                            Content = "Sorry, I can't do that...",
                            PrimaryButtonText = "Ok"
                        };

                        await errorDialog.ShowAsync();

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
                        Title = ex.ToString(),//"File open error",
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

        private void RichTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case VirtualKey.Down:
                case VirtualKey.Up:
                    if (autofillListBox.Items.Count == 0) { SearchLineForCommands(); }
                    break;
                default:
                    SearchLineForCommands();
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
                    if (SelectNextAutocomplete(e)) { e.Handled = true; }
                    break;
            }
        }
    }
}
