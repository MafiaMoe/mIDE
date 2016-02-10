using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mIDE.InsertClasses
{
    public enum VariableType { Integer, Decimal, String, Function, Unknown, Other };

    class VariableLink
    {
        private string variableName;
        private VariableType linkType;

        public VariableLink(string VariableName, VariableType LinkType)
        {
            this.variableName = VariableName;
            this.linkType = LinkType;
        }

        public VariableType LinkType { get; set; }

        public void SetLinkType(string varValue)
        {
            //check if the value is an int
            int tempInt = 0;
            decimal tempDec = 0;
            if (int.TryParse(varValue, out tempInt))
            {
                this.linkType = VariableType.Integer;
            }
            //check if the value is a decimal
            else if (decimal.TryParse(varValue, out tempDec))
            {
                this.linkType = VariableType.Decimal;
            }
            //check if the value is a function/property
            else if (varValue.Contains('('))
            {
                this.linkType = VariableType.Function;
            }
            //else set the value as a string
            else if (varValue.Length > 1 && varValue[0] == '\"' && varValue[varValue.Length - 1] == '\"')
            {
                this.linkType = VariableType.String;
            }
            else
            {
                this.linkType = VariableType.Other;
            }
        }
    }

    class InsertLink
    {
        private string linkString;
        private string linkAutoComplete;

        public InsertLink(string LinkString, string LinkAutoComplete)
        {
            this.linkString = LinkString;
            this.linkAutoComplete = LinkAutoComplete;
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
    }
}
