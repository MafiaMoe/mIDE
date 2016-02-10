using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mIDE.AutoComplete
{
    class AutoCompleteList
    {
        private List<AutoComplete> Commands;
        private List<InsertLink> AutoCompleteLinks;
        private List<VariableLink> VariableLinks;

        public AutoCompleteList()
        {
            Commands = new List<AutoComplete>();
            AutoCompleteLinks = new List<InsertLink>();
            VariableLinks = new List<VariableLink>();
            LoadDefaults();
        }

        //load in default commands
        private void LoadDefaults()
        {
            if (Commands == null) throw new Exception("Commands list not initialized");
            Commands.Add(new AutoComplete("PRINT", " <STR> AT"));//, Brushes.Blue));
            Commands.Add(new AutoComplete("PRINT", " <VAR> AT"));//, Brushes.Blue));
            Commands.Add(new AutoComplete("PRINT", " <NUM> AT"));//, Brushes.Blue));
            Commands.Add(new AutoComplete("AT", "(<INT>, <INT>)."));//, Brushes.LightBlue));
            Commands.Add(new AutoComplete("SET", " <VAR> TO"));//, Brushes.Plum));
            Commands.Add(new AutoComplete("SET", " <PROP> TO"));//, Brushes.Plum));
            Commands.Add(new AutoComplete("LOCK", " <VAR> TO"));//, Brushes.Red));
            Commands.Add(new AutoComplete("LOCK", " <PROP> TO"));//, Brushes.Red));
            Commands.Add(new AutoComplete("TO", " <VAR>."));//, Brushes.Teal));
            Commands.Add(new AutoComplete("TO", " <NUM>."));//, Brushes.Teal));
            Commands.Add(new AutoComplete("TO", " <STR>."));//, Brushes.Teal));
            Commands.Add(new AutoComplete("TO", " <CONSTR>."));//, Brushes.Teal));
            Commands.Add(new AutoComplete("LIST", " <LIST> IN."));//, Brushes.Orange));
            Commands.Add(new AutoComplete("IN", " <VAR>."));//, Brushes.DarkOrange));
            Commands.Add(new AutoComplete("PIDLOOP", "(<NUM>, <NUM>, <NUM>, <NUM>, <NUM>)."));//, Brushes.Green));
            Commands.Add(new AutoComplete("PIDLOOP", "(<NUM>, <NUM>, <NUM>)."));//, Brushes.Green));
            Commands.Add(new AutoComplete("PIDLOOP", "()."));//, Brushes.Green));

            if (AutoCompleteLinks == null) throw new Exception("AutoComplete list not initialized");
            AutoCompleteLinks.Add(new InsertLink("<VAR>", "testVariable"));
            AutoCompleteLinks.Add(new InsertLink("<PROP>", "THROTTLE"));
            AutoCompleteLinks.Add(new InsertLink("<PROP>", "BRAKES"));
            AutoCompleteLinks.Add(new InsertLink("<PROP>", "LIGHTS"));
            AutoCompleteLinks.Add(new InsertLink("<PROP>", "SHIP:CONTROL:YAW"));
            AutoCompleteLinks.Add(new InsertLink("<PROP>", "SHIP:CONTROL:PITCH"));
            AutoCompleteLinks.Add(new InsertLink("<PROP>", "SHIP:CONTROL:ROLL"));
            AutoCompleteLinks.Add(new InsertLink("<LIST>", "ENGINES"));
            AutoCompleteLinks.Add(new InsertLink("<CONSTR>", "PIDLOOP"));

            if (VariableLinks == null) throw new Exception("VariableLinks list not initialized");
            VariableLinks.Add(new VariableLink("<INT>", VariableType.Integer));
            VariableLinks.Add(new VariableLink("<NUM>", VariableType.Decimal));
            VariableLinks.Add(new VariableLink("<STR>", VariableType.String));
        }

        //add in new command (variable)
        public bool ADD()
        {
            return false;
        }

        //remove a command (variable)

        //search for a command containing partial string
        public List<AutoComplete> findCommandStartingWith(string commandName)
        {
            var coms = new List<AutoComplete>();
            foreach (AutoComplete com in Commands)
            {
                if (com.name.StartsWith(commandName, StringComparison.CurrentCultureIgnoreCase)) coms.Add(com);
            }
            return coms;
        }

        //search for a command containing complete string
        public List<AutoComplete> findExactCommand(string commandName)
        {
            var coms = new List<AutoComplete>();
            foreach (AutoComplete com in Commands)
            {
                if (com.name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase)) coms.Add(com);
            }
            return coms;
        }

        //search for insertions
        public List<InsertLink> findAutoCompleteLinks(string insertionName)
        {
            var links = new List<InsertLink>();
            foreach (InsertLink link in AutoCompleteLinks)
            {
                if (link.LinkString.Equals(insertionName, StringComparison.CurrentCultureIgnoreCase)) links.Add(link);
            }
            return links;
        }
    }
}
