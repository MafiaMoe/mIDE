using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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
    }
}
