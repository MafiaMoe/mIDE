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
        private List<InstructionFramework> Instructions;
        //private List<InsertLink> AutoCompleteLinks;
        private List<VariableTree> VariableTreeNodes;

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
            if (Instructions == null) throw new Exception("Commands list not initialized");
            //Instructions.Add(new InstructionFramework("PRINT", "PRINT <GET>.", Colors.Blue));
            Instructions.Add(new InstructionFramework("PRINT", "PRINT <GET> AT(<INT.GET>,<INT.GET>).", null, Colors.Blue));
            //Instructions.Add(new InstructionFramework("AT", "AT", Colors.Blue));
            //Instructions.Add(new InstructionFramework("SET", "SET <OBJA.SET> TO <OBJA.GET>.", Colors.Plum));
            //Instructions.Add(new InstructionFramework("LOCK", "LOCK <OBJA.SET> TO <OBJA.GET>.", Colors.Red));
            //Instructions.Add(new InstructionFramework("TO", "TO", Colors.Teal));
            //Instructions.Add(new InstructionFramework("LIST", "LIST <LIST.GET> IN <LIST.SET>.", Colors.Orange));
            //Instructions.Add(new InstructionFramework("IN", "IN", Colors.DarkOrange));
            //Functions.Add(new Instruction("PIDLOOP", "(<NUM>, <NUM>, <NUM>, <NUM>, <NUM>)", Colors.Green));
            //Functions.Add(new Instruction("PIDLOOP", "(<NUM>, <NUM>, <NUM>)", Colors.Green));
            //Functions.Add(new Instruction("PIDLOOP", "()", Colors.Green));

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

        //find best framework match
        private char[] frameworkSeperators = new char[3] { ' ', '\r', '\t' };
        public InstructionFramework FindMatchingFramework(string instructionText)
        {
            if (instructionText == "") return null;

            int textIndexStart = 0;
            //remove white space and comments
            while (instructionText[textIndexStart] == ' ')
            {
                textIndexStart++;
                //if at end of line, return null
                if (textIndexStart == instructionText.Length - 2) { return null; }
                //if we have come accross a comment
                if (instructionText.Substring(textIndexStart, 2) == "//")
                {
                    //if there is and end to the comment
                    if (instructionText.Substring(textIndexStart).Contains('\n'))
                    {
                        //move to end of comment
                        textIndexStart = instructionText.IndexOf('\n', textIndexStart);
                    }
                    else { return null; }
                }
            }

            //framework ex :    "PRINT <GET> AT(<INT.SET>, <INT.SET>)."
            //line example :    "PRINT "hello " + strVar AT (0, <INT.SET>)"

            //check each framework
            foreach (InstructionFramework insFram in Instructions)
            {
                int textIndex = textIndexStart;
                int frameIndex = 0;
                bool match = true;
                while (match && frameIndex < insFram.Framework.Length && textIndex < instructionText.Length)
                {
                    //if the frame is a variable, skip to end of variable
                    if (insFram.Framework[frameIndex] == '<')
                    {
                        frameIndex = insFram.Framework.IndexOf(' ', frameIndex) + 1;
                        //find end of variable for text
                        textIndex++;
                        bool repeat = true;
                        while (repeat)
                        {
                            textIndex = instructionText.IndexOf(' ', textIndex) + 1;
                            repeat = (instructionText[textIndex] == ' ' ||
                                instructionText[textIndex] == '+' ||
                                instructionText[textIndex] == '-' ||
                                instructionText[textIndex] == '*' ||
                                instructionText[textIndex] == '/');
                        }
                    }
                    //if frame enters bracket, make sure text does
                    else if (insFram.Framework[frameIndex] == '(')
                    {
                        while (instructionText[textIndex] == ' ') textIndex++;
                        if (instructionText[textIndex] != '(') match = false;
                        frameIndex++;
                        textIndex++;
                    }
                    //if frame exits bracket, make sure text does
                    else if (insFram.Framework[frameIndex] == ')')
                    {
                        while (instructionText[textIndex] == ' ') textIndex++;
                        if (instructionText[textIndex] != ')') match = false;
                        frameIndex++;
                        textIndex++;
                    }
                    //if the frame is on a space, advance frame search by one
                    else if (insFram.Framework[frameIndex] == ' ')
                    {
                        frameIndex++;
                    }
                    //if the text in on a space, advance text search by one
                    else if (instructionText[textIndex] == ' ')
                    {
                        textIndex++;
                    }
                    //if the two are equal, move on
                    else if (insFram.Framework.Substring(frameIndex, 1).Equals(instructionText.Substring(textIndex, 1), StringComparison.CurrentCultureIgnoreCase))
                    {
                        textIndex++;
                        frameIndex++;
                    }
                    else
                    {
                        match = false;
                    }
                }
            }

            /*string[] frameworkParts = instructionText.Split(frameworkSeperators);
            for (int i = 0; i < frameworkParts.Length; i++)
            {
                if (frameworkParts[i] != "")
                {
                    foreach (InstructionFramework insFram in Instructions)
                    {
                        int textIndex = textIndexStart;
                        int frameIndex = 0;
                        if (frameworkParts[i] == insFram.name)
                        {

                        }
                    }
                }
            }*/

            return null;
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
                InstructionFramework iFrame = FindMatchingFramework(instructions[instNum].Text);
                instructions[instNum].Framework = iFrame;
            }

            //check for labels not implemented
            int startError = 0;
            for (int instNum = 0; instNum < instructions.Count; instNum++)
            {
                string[] strs = instructions[instNum].Text.Split(stringSeparatorsCheckLabels);
                for (int i = 0; i < strs.Length; i++)
                {
                    if (strs[i].Length > 0 && 
                        (strs[i][0] == '^' || strs[i][0] == '<') &&
                        (strs[i][strs[i].Length - 1] == '^' || strs[i][strs[i].Length - 1] == '>'))
                    {
                        returning.Add(new AutoCheckError(doc.FilePath,
                            instNum, startError, startError + strs[i].Length,
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
