namespace Empresa.Infra.Data.Repositories
{
    using Empresa.Domain.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    public abstract class XmlRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly bool autoPersist;
        private readonly bool persistir;

        protected abstract object GetEntityId(TEntity entidad);

        public virtual XElement ParentElement { get; protected set; }

        protected XName ElementName { get; private set; }

        protected abstract Func<XElement, TEntity> Selector { get; }

        protected abstract XElement CreateXElement(TEntity model);

        protected abstract void SetXElementValue(TEntity model, XElement element);


        protected XmlRepositoryBase(XName elementName)
        {
            ElementName = elementName;

            // clears the "cached" ParentElement to allow hot file changes
            XDocumentProvider.Default.CurrentDocumentChanged += (sender, eventArgs) => ParentElement = null;
        }

        public TEntity GetById(int? id)
        {
            // I intend to remove this magic string "Id"
            return XDocumentProvider.Default.GetDocument().Descendants(ElementName)
                .Where(e => e.Attribute("Id").Value == id.ToString())
                .Select(Selector)
                .FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return ParentElement.Elements(ElementName).Select(Selector);
        }

        public void Add(TEntity entity)
        {
            ParentElement.Add(CreateXElement(entity));

            if (autoPersist)
                Save();
        }

        public virtual void Save()
        {
            XDocumentProvider.Default.Save();
        }

        public void Edit(TEntity entity)
        {
            // I intend to remove this magic string "Id"
            SetXElementValue(
                entity,
                ParentElement.Elements().FirstOrDefault(a => a.Attribute("Id").Value == GetEntityId(entity).ToString()
            ));

            if (persistir)
                Save();
        }

 
        public void Remove(TEntity entity)
        {
            ParentElement.Elements().FirstOrDefault(a => a.Attribute("Id").Value == GetEntityId(entity).ToString()).Remove();

            if (autoPersist)
                Save();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public abstract class XDocumentProvider
    {
        // not thread safe yet
        private static bool pendingChanges;

        private bool _documentLoadedFromFile;

        FileSystemWatcher fileWatcher;

        public static XDocumentProvider Default;

        public event EventHandler CurrentDocumentChanged;

        private XDocument _loadedDocument;

        public string FileName { get; set; }


        protected XDocumentProvider()
        {
            fileWatcher = new FileSystemWatcher();
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileWatcher.Changed += fileWatcher_Changed;
        }

        void fileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (_documentLoadedFromFile && !pendingChanges)
            {
                GetDocument(true);
            }
        }


        /// <summary>
        /// Returns an open XDocument or create a new document
        /// </summary>
        /// <returns></returns>
        public XDocument GetDocument(bool refresh = false)
        {
            if (refresh || _loadedDocument == null)
            {
                // we need to refactor it, but just to demonstrate how should work I will send this way ;P
                if (File.Exists(FileName))
                {
                    _loadedDocument = XDocument.Load(FileName);
                    _documentLoadedFromFile = true;

                    if (fileWatcher.Path != Environment.CurrentDirectory)
                    {
                        fileWatcher.Path = Environment.CurrentDirectory;
                        fileWatcher.Filter = FileName;
                        fileWatcher.EnableRaisingEvents = true;
                    }
                }
                else
                {
                    _loadedDocument = CreateNewDocument();
                    fileWatcher.EnableRaisingEvents = false;
                    _documentLoadedFromFile = false;
                }

                if (CurrentDocumentChanged != null) CurrentDocumentChanged(this, EventArgs.Empty);
            }

            return _loadedDocument;
        }

        /// <summary>
        /// Creates a new XDocument with a determined schemma.
        /// </summary>
        public abstract XDocument CreateNewDocument();

        public virtual void Save()
        {
            XDocumentProvider.Default.Save();
        }
    }
}
