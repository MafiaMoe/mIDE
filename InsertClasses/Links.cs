using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace mIDE.InsertClasses
{
    public enum VariableType { Integer, Decimal, String, Function, Unknown, Other };

    class VariableTree
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ParentType { get; set; }
        public Color DisplayColor { get; set; }
        public bool Gettable { get; }
        public bool Settable { get; }

        public VariableTree(string Name, string Type, string ParentType, Color DisplayColor, bool Gettable, bool Settable)
        {
            this.Name = Name;
            this.Type = Type;
            this.ParentType = ParentType;
            this.DisplayColor = DisplayColor;
            this.Gettable = Gettable;
            this.Settable = Settable;
        }

        public static string CheckLinkType(string varValue)
        {
            //check if the value is an int
            int tempInt = 0;
            decimal tempDec = 0;
            if (int.TryParse(varValue, out tempInt))
            {
                return "INT";
            }
            //check if the value is a decimal
            else if (decimal.TryParse(varValue, out tempDec))
            {
                return "NUM";
            }
            //else set the value as a string
            else if (varValue.Length > 1 && varValue[0] == '\"' && varValue[varValue.Length - 1] == '\"')
            {
                return "STR";
            }
            return null;
        }
    }

    /*class InsertLink
    {
        private string linkString;
        private string linkAutoComplete;
        private string fileSource;

        public InsertLink(string LinkString, string LinkAutoComplete, string FileSource)
        {
            this.linkString = LinkString;
            this.linkAutoComplete = LinkAutoComplete;
            this.fileSource = FileSource;
        }

        public string LinkString
        {
            get { return linkString; }
            set { linkString = value; }
        }

        public string LinkAutoComplete
        {
            get { return linkAutoComplete; }
            set { linkAutoComplete = value; }
        }

        public string FileSource
        {
            get { return fileSource; }
            set { fileSource = value; }
        }
    }*/
}
