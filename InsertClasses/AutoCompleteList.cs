using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using mIDE.DocumentHelpers;

namespace mIDE.InsertClasses
{
    class AutoCompleteList
    {
        public List<InstructionFramework> Instructions;
        //private List<InsertLink> AutoCompleteLinks;
        public List<VariableTree> VariableTreeNodes;

        public AutoCompleteList()
        {
            Instructions = new List<InstructionFramework>();
            //AutoCompleteLinks = new List<InsertLink>();
            VariableTreeNodes = new List<VariableTree>();
            LoadDefaults();
        }

        //load in default commands
        /// <summary>
        /// 
        /// </summary>
        private void LoadDefaults()
        {
            //all instructions specified should have the MINIMAL required spacing
            if (Instructions == null) throw new Exception("Commands list not initialized");
            //Instructions.Add(new InstructionFramework("PRINT", "PRINT <GET>.", Colors.Blue));
            Instructions.Add(new InstructionFramework("PRINT", "PRINT <GET> AT(<INT.GET>,<INT.GET>).", null, Colors.Blue));
            Instructions.Add(new InstructionFramework("AT", "AT", "<AT>", Colors.Blue));
            Instructions.Add(new InstructionFramework("SET", "SET <OBJA.SET> TO <OBJA.GET>.", null, Colors.Plum));
            Instructions.Add(new InstructionFramework("LOCK", "LOCK <OBJA.SET> TO <OBJA.GET>.", null, Colors.Red));
            Instructions.Add(new InstructionFramework("TO", "TO", "<TO>", Colors.Teal));
            Instructions.Add(new InstructionFramework("LIST", "LIST <LIST.GET> IN <LIST.SET>.", null, Colors.Orange));
            Instructions.Add(new InstructionFramework("IN", "IN", "<IN>", Colors.DarkOrange));
            Instructions.Add(new InstructionFramework("PIDLOOP", "PIDLOOP(<NUM>, <NUM>, <NUM>, <NUM>, <NUM>)", "<PIDLOOP.GET>", Colors.Green));
            Instructions.Add(new InstructionFramework("PIDLOOP", "PIDLOOP(<NUM>, <NUM>, <NUM>)", "<PIDLOOP.GET>", Colors.Green));
            Instructions.Add(new InstructionFramework("PIDLOOP", "PIDLOOP()", "<PIDLOOP.GET>", Colors.Green));

            Instructions.Add(new InstructionFramework("+", "<NUM.GET>+<NUM.GET>", "<NUM.GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("+", "<STR.GET>+<NUM.GET>", "<STR.GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("+", "<NUM.GET>+<STR.GET>", "<STR.GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("+", "<STR.GET>+<STR.GET>", "<STR.GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("-", "<NUM.GET>-<NUM.GET>", "<NUM.GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("-", "<NUM.GET>*<NUM.GET>", "<NUM.GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("-", "<NUM.GET>/<NUM.GET>", "<NUM.GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("(", "(<GET>)", "<GET>", Colors.Teal));
            Instructions.Add(new InstructionFramework("\"", "\"\"", "<STR.GET>", Colors.Teal));

            /*if (AutoCompleteLinks == null) throw new Exception("AutoComplete list not initialized");
            //AutoCompleteLinks.Add(new InsertLink("<VAR>", "testVariable", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "HEADING", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "PROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "RETROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "FACING", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "MAXTRUST", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "VELOCITY", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "GEOPOSITION", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "LATITUDE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "LONGITUDE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "UP", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "NORTH", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "BODY", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "ANGULARMOMENTUM", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "ANGULARVEL", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "ANGULARVELOCITY", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "COMMRANGE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "MASS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "VERTICALSPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "GROUNDSPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SURFACESPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "AIRSPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "VESSELNAME", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "ALTITUDE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "APOAPSIS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "PERIAPSIS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SENSORS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SRFPROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SRFREROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "OBT", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "STATUS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "SHIPNAME", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:HEADING", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:PROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:RETROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:FACING", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:MAXTRUST", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:VELOCITY", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:GEOPOSITION", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:LATITUDE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:LONGITUDE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:UP", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:NORTH", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:BODY", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:ANGULARMOMENTUM", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:ANGULARVEL", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:ANGULARVELOCITY", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:COMMRANGE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:MASS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:VERTICALSPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:GROUNDSPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:SURFACESPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:AIRSPEED", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:VESSELNAME", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:ALTITUDE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:APOAPSIS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:PERIAPSIS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:SENSORS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:SRFPROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:SRFREROGRADE", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:OBT", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:STATUS", null));
            AutoCompleteLinks.Add(new InsertLink("PROP", "<VESSEL>:SHIPNAME", null));

            AutoCompleteLinks.Add(new InsertLink("VESSEL", "SHIP", null));


            AutoCompleteLinks.Add(new InsertLink("LIST", "ENGINES", null));
            AutoCompleteLinks.Add(new InsertLink("CONSTR", "PIDLOOP", null));*/

            if (VariableTreeNodes == null) throw new Exception("VariableLinks list not initialized");
            VariableTreeNodes.Add(new VariableTree("INT", "INT", null, Colors.AliceBlue, true, false));
            VariableTreeNodes.Add(new VariableTree("NUM", "NUM", null, Colors.AliceBlue, true, false));
            VariableTreeNodes.Add(new VariableTree("STR", "STR", null, Colors.Aquamarine, true, false));
        }

        //clear autocomplete
        public void ClearAutoCompleteFromFile(string filePath)
        {
            /*foreach (InsertLink link in AutoCompleteLinks)
            {
                if (link.FileSource == filePath)
                {
                    AutoCompleteLinks.Remove(link);
                }
            }*/
        }

        private bool ResultCheck(string expected, string check)
        {
            if (expected == null && check == null) return true;
            if (expected == null || check == null) return false;
            if (expected[0] != '<' || expected[expected.Length - 1] != '>') throw new Exception("Expected issue");
            if (check[0] != '<' || check[check.Length - 1] != '>') throw new Exception("Check issue");

            string[] reqs = expected.Remove(0, 1).Remove(expected.Length - 2, 1).Split('.');
            string[] cprops = check.Remove(0, 1).Remove(check.Length - 2, 1).Split('.');

            foreach (string req in reqs)
            {
                bool match = false;
                for (int i = 0; i < cprops.Length; i++)
                {
                    if (cprops[i] == req)
                    {
                        match = true;
                    }
                }

                if (!match)
                    return false;
            }

            return true;
        }

        private int FindMatchingString(string str, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (str == array[i]) return i;
            }

            return -1;
        }

        private List<string> BreakDownLine(string str)
        {
            List<string> allTextParts = new List<string>();

            string[] textParts = str.Split(frameworkSeparators);
            char[] textSeperators = new char[textParts.Length - 1];
            int offset = -1;
            for (int i = 0; i < textParts.Length - 1; i++)
            {
                offset += textParts[i].Length + 1;
                textSeperators[i] = str[offset];

                if (textParts[i] != "") { allTextParts.Add(textParts[i]); }
                allTextParts.Add(str[offset].ToString());
            }
            allTextParts.Add(textParts[textParts.Length - 1]);

            return allTextParts;
        }

        //find best framework match
        private char[] frameworkSeparators = new char[10] { ' ', '\r', '\t', '(', ')', ',', '+', '-', '*', '/' };
        public CodePiece FindMatchingFramework(string instructionText, string expectedResult)
        {
            CodePiece returning = new CodePiece(instructionText, 0, instructionText.Length);

            if (instructionText == null || instructionText == "" || instructionText == "\r" || instructionText == "/t") return null;
            //InstructionFramework match = null;

            //gather up all the text parts into a single list, including separators
            List<string> allTextParts = BreakDownLine(instructionText);

            for (int ic = 0; ic < Instructions.Count && returning.Framework == null; ic++)
            {
                //if the result we are looking for matches the instruction result
                if (ResultCheck(expectedResult, Instructions[ic].Result))
                {
                    //gather up all the instruction parts into a single list, including separators
                    List<string> allInstParts = BreakDownLine(Instructions[ic].Framework);

                    //collect any areas in the text that need to be saved as child code pieces
                    List<CodePiece> childCodePieces = new List<CodePiece>();
                    string childCodePieceResult = null;

                    //check to make sure all parts are accounted for
                    int curTextPart = 0;
                    int lastTextPartAccountedFor = 0;
                    bool txtMatch = true; //true until proven wrong
                    for (int curInstPart = 0; curInstPart < allInstParts.Count && txtMatch; curInstPart++)
                    {
                        string curInst = allInstParts[curInstPart];
                        string lastInst = (curInstPart > 0 ? allInstParts[curInstPart - 1] : "");
                        string nextInst = (curInstPart < allInstParts.Count - 1 ? allInstParts[curInstPart + 1] : "");

                        //if the instruction part is a variable, start a new code part
                        if (curInst[0] == '<' && curInst[curInst.Length - 1] == '>')
                        {
                            //find the start of the instruction in the text
                            int childStart = 0;
                            for (int i = 0; i < curTextPart; i++)
                            {
                                childStart += allTextParts[i].Length;
                            }
                            if (nextInst == " ") childStart++;

                            childCodePieces.Add(new CodePiece("", childStart, 0));
                            childCodePieceResult = allInstParts[curInstPart];
                        }

                        //if the instruction part is not blank or a space
                        else if (curInst != " " && curInst != "")
                        {
                            //find text part that coincides with the instruction part
                            bool instMatch = false;
                            while (curTextPart < allTextParts.Count && !instMatch)
                            {
                                //check if text matches instruction
                                string curText = allTextParts[curTextPart];
                                if (curInst.Equals(curText, StringComparison.CurrentCulture))
                                {
                                    //check if text before instuction part is correct
                                    bool lastMatch = false;
                                    if (curInstPart == 0) lastMatch = true; //if the instruction part is the first, match
                                    else
                                    {
                                        //if the last instruction part was a space, make sure the text is as well
                                        if (lastInst == " " && allTextParts[curTextPart - 1] == " ")
                                        {
                                            lastMatch = true;
                                        }
                                        //else if the last instruction was a variable
                                        else if (lastInst[0] == '<' && lastInst[lastInst.Length - 1] == '>')
                                        {
                                            lastMatch = true;
                                        }
                                        //else find the last text part that was not a space and check
                                        else
                                        {
                                            //find the last text part that was not a space
                                            string lastText = " ";
                                            for (int i = curTextPart - 1; i > 0 && lastText == " "; i--)
                                            {
                                                lastText = allTextParts[i];
                                            }
                                            //check if the last text part matches the last instruction part
                                            if (lastInst.Equals(lastText, StringComparison.CurrentCulture))
                                            {
                                                lastMatch = true;
                                            }
                                        }
                                    }

                                    //check if text after instruction part is correct
                                    bool nextMatch = false;
                                    if (curInstPart == allInstParts.Count - 1) nextMatch = true; //if the instruction part is the last, match
                                    else
                                    {
                                        //if the next instruction part is a space, make sure the text is as well
                                        if (nextInst == " " && allTextParts[curTextPart + 1] == " ")
                                        {
                                            nextMatch = true;
                                        }
                                        //else if the next instruction is a variable
                                        else if (nextInst[0] == '<' && nextInst[nextInst.Length - 1] == '>')
                                        {
                                            nextMatch = true;
                                        }
                                        //else find the next text part that is a space and check
                                        else
                                        {
                                            //find the next text part that is not a space
                                            string nextText = " ";
                                            for (int i = curTextPart + 1; i < allTextParts.Count && nextText == " "; i++)
                                            {
                                                nextText = allTextParts[i];
                                            }
                                            //check if the last text part matches the last instruction part
                                            if (nextInst.Equals(nextText, StringComparison.CurrentCulture))
                                            {
                                                nextMatch = true;
                                            }
                                        }
                                    }

                                    if (lastMatch && nextMatch)
                                    {
                                        instMatch = true;
                                    }
                                }

                                curTextPart++;
                            }

                            //if the instruction matches and there is a child code piece open, complete it
                            if (instMatch && childCodePieceResult != null)
                            {
                                int childEnd = 0;
                                for (int i = 0; i < curTextPart - 1; i++)
                                {
                                    childEnd += allTextParts[i].Length;
                                }
                                if (lastInst == " ") childEnd--;

                                CodePiece cp = childCodePieces.Last<CodePiece>();

                                cp.EndLocation = childEnd;
                                if (cp.EndLocation - cp.StartLocation >= 0)
                                {
                                    cp.Text = instructionText.Substring(cp.StartLocation, cp.EndLocation - cp.StartLocation);
                                }

                                CodePiece tempCp = FindMatchingFramework(cp.Text, childCodePieceResult);
                                if (tempCp != null)
                                {
                                    cp.Framework = tempCp.Framework;
                                    cp.ChildPieces = tempCp.ChildPieces;
                                }

                                childCodePieceResult = null;
                            }

                            txtMatch = instMatch;
                        }

                            /*txtMatch = false;
                            while (curTextPart < allTextParts.Count && !txtMatch)
                            {
                                string curText = allTextParts[curTextPart];
                                string lastText = allTextParts[curTextPart - 1];
                                string curInst = allInstParts[curInstPart];
                                string lastInst = allInstParts[curInstPart - 1];
                                if (allTextParts[curTextPart].Equals(allInstParts[curInstPart], StringComparison.CurrentCultureIgnoreCase) &&
                                    (allTextParts[curTextPart - 1].Equals(allInstParts[curInstPart - 1], StringComparison.CurrentCultureIgnoreCase) ||
                                    (allInstParts[curInstPart - 1][0] == '<' && allInstParts[curInstPart - 1][allInstParts[curInstPart - 1].Length - 1] == '>')))
                                {
                                    txtMatch = true;
                                    //close the open code piece
                                    if (childCodePieceResult != null && allTextParts[curTextPart] != " ")
                                    {
                                        int childEnd = -1;
                                        for (int i = 0; i < curTextPart; i++)
                                        {
                                            childEnd += allTextParts[i].Length;
                                        }

                                        CodePiece cp = childCodePieces.Last<CodePiece>();

                                        cp.EndLocation = childEnd;
                                        if (childEnd >= 0)
                                        {
                                            cp.Text = instructionText.Substring(cp.StartLocation, cp.EndLocation - cp.StartLocation);
                                        }

                                        CodePiece tempCp = FindMatchingFramework(cp.Text, childCodePieceResult);
                                        if (tempCp != null)
                                        {
                                            cp.Framework = tempCp.Framework;
                                        }

                                        childCodePieceResult = null;
                                    }
                                }
                                //else if the current instruction part starts with '<' and ends with '>'
                                else if (allInstParts[curInstPart][0] == '<' && allInstParts[curInstPart][allInstParts[curInstPart].Length - 1] == '>')
                                {
                                    txtMatch = true;

                                    int childStart = 0;
                                    for (int i = 0; i < curTextPart; i++)
                                    {
                                        childStart += allTextParts[i].Length;
                                    }

                                    childCodePieces.Add(new CodePiece("", childStart, 0));
                                    childCodePieceResult = allInstParts[curInstPart];

                                }
                                //else if the last instruction part starts with '<' and ends with '>'
                                else if (allInstParts[curInstPart - 1][0] == '<' && allInstParts[curInstPart - 1][allInstParts[curInstPart - 1].Length - 1] == '>')
                                {
                                    txtMatch = true;
                                }
                                else if (allTextParts[curTextPart] == " " || allTextParts[curTextPart] == "")
                                {

                                }
                                curTextPart++;
                            }*/
                    }

                    if (txtMatch) //found a match!!!1!11!!
                    {
                        returning.Framework = Instructions[ic];
                        returning.ChildPieces = childCodePieces;
                    }
                }
            }

            /*int curTextPartInspection = -1;
            for (int ic = 0; ic < Instructions.Count; ic++)
            {
                if (ResultCheck(expectedResult, Instructions[ic].Result))
                {
                    bool findingMatch = true;
                    string[] instParts = Instructions[ic].Framework.Split(frameworkSeparators);
                    char[] instSeperators = new char[instParts.Length - 1];
                    offset = -1;
                    for (int i = 0; i < instParts.Length - 1; i++)
                    {
                        offset += instParts[i].Length + 1;
                        instSeperators[i] = Instructions[ic].Framework[offset];
                    }

                    //collect any areas in the text that need to be saved as child code pieces
                    List<CodePiece> childCodePieces = new List<CodePiece>();
                    bool codePieceOpen = false;

                    for (int iff = 0; iff < instParts.Length && findingMatch; iff++)
                    {
                        //check if the text matches the instruction
                        string instPart = instParts[iff];

                        //if instruction part is text
                        if (instPart[0] != '<' && instPart[instPart.Length - 1] != '>')
                        {
                            int textPartInspection = FindMatchingString(instPart, textParts);
                            
                            //if the instruction exists in the correct order as a textPart, move on
                            if (textPartInspection > curTextPartInspection)
                            {
                                //check separators
                                //spacable separators

                                //non-spacable separators

                                curTextPartInspection = textPartInspection;
                            }
                            //else stop searching for a match
                            else
                            {
                                findingMatch = false;
                            }
                        }
                        //if instruction is part of a variableTree
                        else if (instPart[0] == '<' && (instPart[instPart.Length - 1] == '>' || instPart.Substring(instPart.Length - 2, 2) == ">."))
                        {
                            //create a new code part
                            childCodePieces.Add(new CodePiece("", -1, -1));

                            int childStart = 0;
                            for (int i = 0; i < curTextPartInspection; i++)
                            {
                                childStart += textParts[i].Length + 1;
                            }
                            childCodePieces[childCodePieces.Count - 1].StartLocation = childStart;

                            codePieceOpen = true;
                        }
                        else
                        {
                            throw new Exception("instruction is not correctly configured");
                        }
                    }

                    if (findingMatch)
                    {
                        //!!!! Found a match !!!!
                        match = Instructions[ic];
                    }
                }
            }

            returning.Framework = match;*/

            return returning;
        }

        //add autocomplete
        private char[] stringSeparators = new char[5] { ' ', '(', ')', ',', '\t' };
        private char[] stringSeparatorsCheckLabels = new char[6] { ' ', '(', ')', ',', '\t', ':' };
        public List<AutoCheckError> CheckAutoCompleteErrors(ActiveDocument doc)
        {
            var returning = new List<AutoCheckError>();

            List<CodePiece> instructions = doc.GetInstructionStrings();

            //check for framework
            for (int instNum = 0; instNum < instructions.Count; instNum++)
            {
                CodePiece code = FindMatchingFramework(instructions[instNum].Text, null);
                instructions[instNum].Framework = (code == null ? null : code.Framework);
            }

            //check for labels not implemented
            int startError = 0;
            for (int instNum = 0; instNum < instructions.Count; instNum++)
            {
                string text = instructions[instNum].Text;
                if (text != "" && text[text.Length - 1] == '\r') text = text.Remove(text.Length - 1, 1);
                if (text != "" && text[text.Length - 1] == '\t') text = text.Remove(text.Length - 1, 1);
                string[] strs = text.Split(stringSeparatorsCheckLabels);
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i].Length > 0 && (strs[i][0] == '<'))
                    {
                        if (strs[i][strs[i].Length - 1] == '>')
                        {
                            returning.Add(new AutoCheckError(doc.FilePath,
                                instNum, startError, startError + strs[i].Length,
                                "Not set : " + strs[i], ErrorSeverity.none));
                        }
                        if (strs[i].Length > 1 && strs[i].Substring(strs[i].Length - 2, 2) == ">.")
                        {
                            returning.Add(new AutoCheckError(doc.FilePath,
                                instNum, startError, startError + strs[i].Length - 1,
                                "Not set : " + strs[i], ErrorSeverity.none));
                        }
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
        public List<InstructionFramework> findCommandStartingWith(string commandName)
        {
            var coms = new List<InstructionFramework>();
            foreach (InstructionFramework com in Instructions)
            {
                if (com.Name.StartsWith(commandName, StringComparison.CurrentCultureIgnoreCase)) coms.Add(com);
            }
            return coms;
        }

        //search for a command containing complete string
        public List<InstructionFramework> findExactInstruction(string commandName)
        {
            var insts = new List<InstructionFramework>();
            foreach (InstructionFramework inst in Instructions)
            {
                if (inst.Name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase)) insts.Add(inst);
            }
            return insts;
        }

        //search for insertions
        /*public List<InsertLink> findAutoCompleteLinks(string insertionName)
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
        }*/
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
