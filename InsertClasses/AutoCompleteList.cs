using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace mIDE.InsertClasses
{
    class AutoCompleteList
    {
        private List<Function> Functions;
        private List<InsertLink> AutoCompleteLinks;
        private List<VariableLink> VariableLinks;

        public AutoCompleteList()
        {
            Functions = new List<Function>();
            AutoCompleteLinks = new List<InsertLink>();
            VariableLinks = new List<VariableLink>();
            LoadDefaults();
        }

        //load in default commands
        private void LoadDefaults()
        {
            if (Functions == null) throw new Exception("Commands list not initialized");
            Functions.Add(new Function("PRINT", " <STR> AT", Colors.Blue));
            Functions.Add(new Function("PRINT", " ^VAR^ AT", Colors.Blue));
            Functions.Add(new Function("PRINT", " <NUM> AT", Colors.Blue));
            Functions.Add(new Function("AT", "(<INT>, <INT>).", Colors.LightBlue));
            Functions.Add(new Function("SET", " <VAR> TO", Colors.Plum));
            Functions.Add(new Function("SET", " <PROP> TO", Colors.Plum));
            Functions.Add(new Function("LOCK", " <VAR> TO", Colors.Red));
            Functions.Add(new Function("LOCK", " <PROP> TO", Colors.Red));
            Functions.Add(new Function("TO", " ^VAR^.", Colors.Teal));
            Functions.Add(new Function("TO", " <NUM>.", Colors.Teal));
            Functions.Add(new Function("TO", " <STR>.", Colors.Teal));
            Functions.Add(new Function("TO", " <CONSTR>.", Colors.Teal));
            Functions.Add(new Function("LIST", " <LIST> IN.", Colors.Orange));
            Functions.Add(new Function("IN", " <VAR>.", Colors.DarkOrange));
            Functions.Add(new Function("PIDLOOP", "(<NUM>, <NUM>, <NUM>, <NUM>, <NUM>)", Colors.Green));
            Functions.Add(new Function("PIDLOOP", "(<NUM>, <NUM>, <NUM>)", Colors.Green));
            Functions.Add(new Function("PIDLOOP", "()", Colors.Green));

            if (AutoCompleteLinks == null) throw new Exception("AutoComplete list not initialized");
            //AutoCompleteLinks.Add(new InsertLink("<VAR>", "testVariable", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "THROTTLE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "BRAKES", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "LIGHTS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SHIP:CONTROL:YAW", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SHIP:CONTROL:PITCH", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SHIP:CONTROL:ROLL", null));
            AutoCompleteLinks.Add(new InsertLink("LIST", "ENGINES", null));
            AutoCompleteLinks.Add(new InsertLink("CONSTR", "PIDLOOP", null));

            if (VariableLinks == null) throw new Exception("VariableLinks list not initialized");
            VariableLinks.Add(new VariableLink("INT", VariableType.Integer));
            VariableLinks.Add(new VariableLink("NUM", VariableType.Decimal));
            VariableLinks.Add(new VariableLink("STR", VariableType.String));
        }

        //clear autocomplete
        public void ClearAutoCompleteFromFile(string filePath)
        {
            foreach (InsertLink link in AutoCompleteLinks)
            {
                if (link.FileSource == filePath)
                {
                    AutoCompleteLinks.Remove(link);
                }
            }
        }

        //add autocomplete
        private char[] stringSeparators = new char[5] { ' ', '(', ')', ',', '\t' };
        private char[] stringSeparatorsCheckLabels = new char[6] { ' ', '(', ')', ',', '\t', '.' };
        public List<AutoCheckError> CheckAutoCompleteErrors(string[] fileLines, string filePath)
        {
            var returning = new List<AutoCheckError>();
            //check for labels not implemented
            int startError = 0;
            for (int lineNum = 0; lineNum < fileLines.Length; lineNum++)
            {
                string[] strs = fileLines[lineNum].Split(stringSeparatorsCheckLabels);
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i].Length > 0 && 
                        (strs[i][0] == '^' || strs[i][0] == '<') &&
                        (strs[i][strs[i].Length - 1] == '^' || strs[i][strs[i].Length - 1] == '>'))
                    {
                        returning.Add(new AutoCheckError(filePath,
                            lineNum, startError, startError + strs[i].Length,
                            "Not set : " + strs[i], ErrorSeverity.none));
                    }
                    startError += strs[i].Length + 1;
                }
            }

            //check for variable not existing

            //check funcion implementation errors
            /*for (int lineNum = 0; lineNum < fileLines.Length; lineNum++)
            {
                string[] strs = fileLines[lineNum].Split(stringSeparators);

                for (int i = 0; i < strs.Length; i++)
                {
                    foreach (Function func in Functions)
                    {
                        //find the function name
                        if (strs[i] == func.name)
                        {
                            string variableType = null;
                            string variableName = null;
                            //find other parts of the function
                            string[] funcParts = func.succeedingString.Split(stringSeparators);
                            int si = i + 1;
                            int fi = 0;
                            bool match = true;
                            while (fi < funcParts.Length && match)
                            {
                                //if nothing in function part, move on to next function part
                                if (funcParts[fi] == "") { fi++; }
                                //if nothing left in the strings, no match
                                else if (si > strs.Length) { match = false; }
                                //if nothing in string part, move on to next string part
                                else if (strs[si] == "") { si++; }
                                //if the string parts match, move on with match check
                                else if (strs[si].Equals(funcParts[fi], 
                                    StringComparison.CurrentCultureIgnoreCase)) { fi++; si++; }
                                //if the string is a parameter, save the parameter values and move on
                                else if (strs[si][0] == '<' && strs[si][strs[si].Length - 1] == '>')
                                { si++; fi++; variableType = strs[si]; variableName = funcParts[fi]; }
                                //if the string is a variable that should already be set, check, move on
                                else if (strs[si][0] == '^' && strs[si][strs[si].Length - 1] == '^')
                                {
                                    bool linkFound = false;
                                    foreach (InsertLink IL in findAutoCompleteLinks(strs[si]))
                                    {
                                        if (IL.LinkString.Equals(funcParts[fi], 
                                            StringComparison.CurrentCultureIgnoreCase))
                                        {
                                            linkFound = true;
                                            break;
                                        }
                                    }
                                    //add error
                                    if (!linkFound)
                                    {
                                        returning.Add(new AutoCheckError(filePath,
                                            lineNum, 0, 0, 
                                            "Not defined : " + funcParts[fi]));
                                    }
                                    si++; fi++;
                                }
                                else { match = false; }
                            }
                            //if a match was made, set any variable found to a new autocomplete
                            if (match)
                            {
                                var yay = true;
                            }
                        }
                    }
                }
            }*/
            return returning;
        }

        //search for a command containing partial string
        public List<Function> findCommandStartingWith(string commandName)
        {
            var coms = new List<Function>();
            foreach (Function com in Functions)
            {
                if (com.name.StartsWith(commandName, StringComparison.CurrentCultureIgnoreCase)) coms.Add(com);
            }
            return coms;
        }

        //search for a command containing complete string
        public List<Function> findExactCommand(string commandName)
        {
            var coms = new List<Function>();
            foreach (Function com in Functions)
            {
                if (com.name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase)) coms.Add(com);
            }
            return coms;
        }

        //search for insertions
        public List<InsertLink> findAutoCompleteLinks(string insertionName)
        {
            var links = new List<InsertLink>();
            if (insertionName.Length > 1 &&
                (insertionName[0] == '<' || insertionName[0] == '^') &&
                (insertionName[insertionName.Length - 1] == '>' || insertionName[insertionName.Length - 1] == '^'))
            {
                foreach (InsertLink link in AutoCompleteLinks)
                {
                    if (link.LinkString.Equals(insertionName.Remove(insertionName.Length - 1, 1).Remove(0, 1),
                        StringComparison.CurrentCultureIgnoreCase)) links.Add(link);
                }
            }
            return links;
        }
    }

    public enum ErrorSeverity { none, buildError, runtimeError };
    public class AutoCheckError
    {
        public string FilePath { get; set; }
        public int LineNum { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string Text { get; set; }
        public ErrorSeverity Severity { get; set; }

        public AutoCheckError(string FilePath, int LineNum, int StartIndex, int EndIndex, string Text, ErrorSeverity Severity)
        {
            this.FilePath = FilePath;
            this.LineNum = LineNum;
            this.StartIndex = StartIndex;
            this.EndIndex = EndIndex;
            this.Text = Text;
            this.Severity = Severity;
        }
    }
}
