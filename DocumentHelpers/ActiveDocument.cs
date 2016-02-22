using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using mIDE.InsertClasses;

namespace mIDE.DocumentHelpers
{
    class ActiveDocumentList
    {
        private List<ActiveDocument> activeDocuments;

        public ActiveDocumentList()
        {
            activeDocuments = new List<ActiveDocument>();
        }

        public bool IsFileOpen(string filePath)
        {
            if (GetActiveDocument(filePath) != null) { return true; }
            return false;
        }

        public ActiveDocument GetActiveDocument(string filePath)
        {
            foreach (ActiveDocument ac in activeDocuments)
            {
                if (ac.FilePath == filePath) { return ac; }
            }
            return null;
        }

        public List<ActiveDocument> VisibleDocuents()
        {
            List<ActiveDocument> visibleDocs = new List<ActiveDocument>();
            foreach (ActiveDocument doc in activeDocuments)
            {
                if (doc.Visible) { visibleDocs.Add(doc); }
            }
            return visibleDocs;
        }

        public void Add(ActiveDocument Document)
        {
            activeDocuments.Add(Document);
            //refresh stuff...
        }

        public void Remove(ActiveDocument Document)
        {
            activeDocuments.Remove(Document);
            //refresh stuff...
        }
    }

    class ActiveDocument
    {
        private string document;
        private string filePath;
        private DateTime lastSave;
        private DateTime lastChange;
        private bool visible;

        public string FilePath { get { return filePath; } set { filePath = value; } }
        public bool Visible { get { return visible; } set { visible = value; } }
        public string FileName
        {
            get
            {
                if (filePath != null)
                {
                    string[] strs = filePath.Split('\\');
                    if (strs.Length > 0)
                    {
                        strs = strs[strs.Length - 1].Split('.');

                        if (strs.Length > 0 && strs[0] != "") return strs[0];
                    }
                }
                return null;
            }
        }
        public string Text { get { return document; } set { document = value; } }

        public ActiveDocument(string Document)
        {
            this.document = Document;
            this.filePath = null;
            this.lastSave = DateTime.MinValue;
            this.lastChange = DateTime.Now;
            this.visible = false;
        }

        public bool IsSaved { get { return (lastSave > lastChange); } }

        public async void Load(Windows.Storage.StorageFile file)
        {

        }

        public List<CodePiece> FindCommmentIndexes()
        {
            var returning = new List<CodePiece>();

            int curIndex = 0;
            while (document.Remove(0, curIndex).Contains("//"))
            {
                int comStart = document.IndexOf("//", curIndex);
                int comEnd = document.IndexOf("\n", comStart);
                returning.Add(new CodePiece(document.Substring(comStart, comEnd - comStart), comStart, comEnd));
            }

            return returning;
        }

        public string ClearComments()
        {
            var returning = document;

            //remove comments from back to front
            var comPieces = FindCommmentIndexes();
            for (int i = comPieces.Count - 1; i >= 0; i--)
            {
                returning = returning.Remove(comPieces[i].StartLocation, comPieces[i].EndLocation - comPieces[i].StartLocation);
            }

            return returning;
        }

        string[] InstructionEnds = new string[4] { ". ", ".\t", ".\n", ".\r" };
        public List<CodePiece> GetInstructionStrings()
        {
            var returning = new List<CodePiece>();

            var comments = FindCommmentIndexes();

            string[] texts = document.Split(InstructionEnds, StringSplitOptions.None);

            int startIndex = 0;
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i] += '.';

                CodePiece ins = new CodePiece(texts[i],
                    startIndex,
                    startIndex + texts[i].Length);

                returning.Add(ins);

                startIndex += texts[i].Length + 2;
            }

            return returning;
        }
    }

    public class CodePiece
    {
        public string Text;
        public int StartLocation, EndLocation;
        public InstructionFramework Framework;
        public List<CodePiece> ChildPieces;

        public CodePiece(string Text, int StartLocation, int EndLocation)
        {
            this.Text = Text;
            this.StartLocation = StartLocation;
            this.EndLocation = EndLocation;
            Framework = null;
            ChildPieces = new List<CodePiece>();
        }

        public CodePiece(string Text, int StartLocation, int EndLocation, InstructionFramework Framework)
        {
            this.Text = Text;
            this.StartLocation = StartLocation;
            this.EndLocation = EndLocation;
            this.Framework = Framework;
            ChildPieces = new List<CodePiece>();
        }
    }
}
