using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.IO;

namespace FileCreation
{
    class CreateFiles
    {
        //Constructor
        public CreateFiles() { }

        //Folders Created
        private List<string> FoldersCreated;

        #region CreateXMLFiles
        /// <summary>
        /// Create XML Files with specified Data
        /// </summary>
        /// <param name="Path">Where to Create the Files</param>
        /// <param name="TotalFiles">How many files to create. Minumum is 1, Maximum is 100</param>
        /// <param name="RootElement">What the Root Element is supposed to be</param>
        /// <param name="Elements">Elements list's values can be separated by a comma. Meaning first part is the tag, second the value</param>
        public void CreateXMLFiles(string Path, int TotalFiles, string RootElement, List<string> Elements = null)
        {
            try
            {
                //Check if total files is too many
                if (TotalFiles < 1 || TotalFiles > 100)
                {
                  //Error - Don't process
                }
                else if (!Directory.Exists(Path))
                {
                    //Error - Don't process
                }
                else if (Elements != null && CheckElements(Elements) == false)
                {
                    //Error - Don't process
                }
                else
                {
                    //Fix Path
                    if (Path[Path.Length - 1].ToString() != @"\")
                        Path = Path + @"\";

                    //Xml Settings
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.NewLineOnAttributes = true;

                    //Write Files
                    for (int i = 0; i < TotalFiles; i++)
                    {
                        using (XmlWriter writer = XmlWriter.Create(Path + Guid.NewGuid().ToString() + ".xml", settings))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement(RootElement);
                            //Elements add
                            if (Elements != null)
                            {
                                foreach (string el in Elements)
                                {
                                    if (el.Split(',').Count() > 1)
                                        writer.WriteElementString(el.Split(',')[0], el.Split(',')[1]);
                                    else
                                        writer.WriteElementString(el, "");
                                }
                            }
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Exception
            }
        }
        private bool CheckElements(List<string> Elements)
        {
            foreach (string el in Elements)
            {
                if (string.IsNullOrEmpty(el))
                    return false;
                else if (el.Split(',').Count() > 2)
                    return false;
            }
            return true;
        }
        #endregion
        #region CreateTxtFiles
        /// <summary>
        /// Create Empty Text Files
        /// </summary>
        /// <param name="Path">Where to Create the Files</param>
        /// <param name="TotalFiles">How many files to create. Minumum is 1, Maximum is 100</param>
        public void CreateTxtFiles(string Path, int TotalFiles)
        {
            try
            {
                //Check if total files is too many
                if (TotalFiles < 1 || TotalFiles > 100)
                {
                    //Error - Don't process
                }
                else if (!Directory.Exists(Path))
                {
                    //Error - Don't process
                }
                else
                {
                    //Fix Path
                    if (Path[Path.Length - 1].ToString() != @"\")
                        Path = Path + @"\";

                    //Write Files
                    for (int i = 0; i < TotalFiles; i++)
                    {
                        File.Create(Path + Guid.NewGuid().ToString() + ".txt").Dispose();
                    }
                }
            }
            catch (Exception)
            {
                //Exception
            }
        }
        #endregion
        #region CreateFolders
        /// <summary>
        /// Create Folders
        /// </summary>
        /// <param name="Path">Where to Create the Folders</param>
        /// <param name="TotalFolders">How many Folders to create. Minumum is 1, Maximum is 100</param>
        public void CreateFolders(string Path, int TotalFolders)
        {
            try
            {
                //Create new FoldersCreated
                FoldersCreated = new List<string>();

                //Check if total files is too many
                if (TotalFolders < 1 || TotalFolders > 100)
                {
                    //Error - Don't process
                }
                else if (!Directory.Exists(Path))
                {
                    //Error - Don't process
                }
                else
                {
                    //Fix Path
                    if (Path[Path.Length - 1].ToString() != @"\")
                        Path = Path + @"\";

                    //Create Folders
                    for (int i = 0; i < TotalFolders; i++)
                    {
                        string dir = Path + Guid.NewGuid().ToString();
                        Directory.CreateDirectory(dir);

                        //Store Locations
                        FoldersCreated.Add(dir);
                    }
                }
            }
            catch (Exception)
            {
                //Exception
            }
        }
        #endregion
        #region CreateFilesInFolders
        /// <summary>
        /// Create Files In Folders - Create new total files in total folders
        /// </summary>
        /// <param name="Path">Where to Create the Files & Folders</param>
        /// <param name="FileType">"XML" or "Text" Files to Create</param>
        /// <param name="TotalFolders">How many Folders to create. Minumum is 1, Maximum is 100</param>
        /// <param name="TotalFiles">How many Files to create. Minumum is 1, Maximum is 100</param>
        /// <param name="RootElement">If it's XML File, then you need RootElement</param>
        /// <param name="Elements">If it's XML File, then you want you can add Elements</param>
        public void CreateFilesInFolders(string Path, string FileType, int TotalFiles, int TotalFolders,
            string RootElement = null, List<string> Elements = null)
        {
            try
            {
                //Check if total folders/files are too many
                if ((TotalFiles < 1 || TotalFiles > 100) && (TotalFolders < 1 || TotalFolders > 100))
                {
                    //Error - Don't process
                }
                else if (!Directory.Exists(Path))
                {
                    //Error - Don't process
                }
                else
                {
                    //Fix Path
                    if (Path[Path.Length - 1].ToString() != @"\")
                        Path = Path + @"\";

                    //First create Folders
                    CreateFolders(Path, TotalFolders);

                    //Get all Directories
                    foreach (string Dir in FoldersCreated)
                    {
                        if (FileType.Trim() == "XML")
                            CreateXMLFiles(Dir, TotalFiles, RootElement, Elements);
                        else if (FileType.Trim() == "Text")
                            CreateTxtFiles(Dir, TotalFiles);
                    }
                }
            }
            catch (Exception)
            {
                //Exception
            }
        }
        #endregion
        #region CreateAnyEmptyFiles
        /// <summary>
        /// Create Files In Folders - Create new total files in total folders
        /// </summary>
        /// <param name="Path">Where to Create the Files & Folders</param>
        /// <param name="FileType">Files to Create</param>
        /// <param name="TotalFiles">How many Files to create. Minumum is 1, Maximum is 100</param>
        public void CreateAnyEmptyFiles(string Path, string FileType, int TotalFiles)
        {
            try
            {
                //Check if total files is too many
                if (TotalFiles < 1 || TotalFiles > 100)
                {
                    //Error - Don't process
                }
                else if (!Directory.Exists(Path))
                {
                    //Error - Don't process
                }
                else if (!CheckFileType(FileType))
                {
                    //Error - Don't process
                }
                else
                {
                    //Fix Path
                    if (Path[Path.Length - 1].ToString() != @"\")
                        Path = Path + @"\";

                    //Write Files
                    for (int i = 0; i < TotalFiles; i++)
                    {
                        File.Create(Path + Guid.NewGuid().ToString() + "." + FileType.ToLower()).Dispose();
                    }
                }
            }
            catch (Exception)
            {
                //Exception
            }
        }
        private bool CheckFileType(string FileType)
        {
            switch (FileType.ToLower())
            {     
                case "txt":
                case "log":
                case "csv":
                case "resx":
                case "ico":
                case "cur":
                case "cs":
                case "config":
                case "js":
                case "jsl":
                case "htm":
                case "vb":
                case "xslt":
                case "rpt":
                case "bmp":
                case "cd":
                case "rdlc":
                case "vbs":
                case "wsf":
                case "xml":
                case "master":
                case "ascx":
                case "css":
                case "asax":
                case "xsd":
                case "mdf":
                case "ashx":
                case "sitemap":
                case "skin":
                case "aspx":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region GetLog
        /// <summary>
        /// Creates a Log File
        /// </summary>
        /// <param name="Path">Where to Create the LogFile/Look for All Directories</param>
        public void GetLog(string Path)
        {
            try
            {
                if (!Directory.Exists(Path))
                {
                    //Error - Don't process
                }
                else
                {
                    //Fix Path
                    if (Path[Path.Length - 1].ToString() != @"\")
                        Path = Path + @"\";

                    StringBuilder Log = new StringBuilder();
                    foreach (string dir in Directory.GetDirectories(Path).ToList<string>())
                    {
                        Log.Append("Folder: " + System.IO.Path.GetFileName(dir)).Append("\r\n");
                        foreach (string file in Directory.GetFiles(dir))
                        {
                            Log.Append("File: " + System.IO.Path.GetFileName(file)).Append("\r\n");
                        }
                        Log.Append("\r\n");
                    }
                    File.WriteAllText(Path + "LogFile.txt", Log.ToString());
                }
            }
            catch (Exception)
            {
                //Exception
            }
        }
        #endregion
    }
}
