using System ;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using log=System.Console;
namespace ConsoleApp35
{
    public class clsFileIO
    {

        private string FileName { get; set; }
        private string FilePath { get; set; }
        private string Extention { get; set; }
        private string DirectoryName { get; set; }
        private DateTime LastUpdate { get; set; }
        private bool IsFileExisted { get; set; }
        private List<string> ListBeforeContain { get; set; }
        private int CountWordInFile { get; set; } = 0;
        public static clsFileIO FindFile(string path)
        {
            if (!File.Exists(path))
            {
                log.WriteLine("File not found ", path);
                return null;
            }
            else
            {
                clsFileIO file = new clsFileIO(path);
                return file;
            }
        }
        private clsFileIO(string path)
        {
            if (!File.Exists(path))
            {
                log.WriteLine("File not found ", path);
                IsFileExisted = false;
                return;
            }
            else
            {
                this.CountWordInFile = 0;
                this.IsFileExisted = true;
                this.DirectoryName = Path.GetDirectoryName(path);
                this.LastUpdate = File.GetLastWriteTime(path);
                this.Extention = Path.GetExtension(path);
                this.FilePath = path;
                this.FileName = ExtractFileName();
                this.ListBeforeContain = new List<string>();
            }

        }
        private void ReLoadFileInfo(string path)
        {
            this.DirectoryName = Path.GetDirectoryName(path);
            this.LastUpdate = File.GetLastWriteTime(path);
            this.Extention = Path.GetExtension(path);
            this.FilePath = path;
            this.FileName = ExtractFileName();
            this.ListBeforeContain = new List<string>();
        }
        private bool confirmFileExisted()
        {
            if (!IsFileExisted)
            {
                // log.WriteLine("File not found ", FilePath);
                return false;
            }
            return true;
        }
        public void PrintInfo()
        {
            if (!confirmFileExisted())
            {
                return;
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            log.WriteLine($"FileName        : {this.FileName}");
            log.WriteLine($"PathName        : {this.FilePath}");
            log.WriteLine($"Extention File  : {this.Extention}");
            log.WriteLine($"Dictionary Name : {this.DirectoryName}");
            log.WriteLine($"LastUpdateWrite : {this.LastUpdate}");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
        public void ReturnContain()
        {
            if (!confirmFileExisted())
            {
                return;
            }
            if (ListBeforeContain.Count == 0)
            {
                log.WriteLine("Not Perform clear OR Rewriting into File");
                return;
            }
            log.WriteLine("Return Contain File Before Clear OR Rewriting");
            string result = string.Join("\n", ListBeforeContain);
            ReWriteFile(result);
        }
        private string ExtractFileName()
        {

            return Path.GetFileName(FilePath);

        }
        public static bool IsExited(string path)
        {
            return File.Exists(path);
        }
        public bool IsExited()
        {

            return File.Exists(FilePath);
        }
        public void ReadFile()
        {
            if (!confirmFileExisted())
            {
                return;
            }
            if (!IsExited())
            {
                log.WriteLine("Not Found File");
                return;
            }
            try
            {
                using (StreamReader R = new StreamReader(FilePath))
                {
                    string line;
                    log.WriteLine("\n=================================================");
                    while ((line = R.ReadLine()) != null)
                    {
                        log.WriteLine(line);
                    }
                    log.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");


                }
            }
            catch (Exception e) { log.WriteLine(e.Message); }
        }
        public bool clearFile()
        {
            if (!confirmFileExisted())
            {
                return false;
            }
            bool clear = false;
            List<string> list = ExtractContainFile();
            if (list.Count == 0)
            {
                log.WriteLine("this File Already Empty");
                clear = true;
            }
            else
            {
                ListBeforeContain = ExtractContainFile();
                ReWriteFile("");
                clear = true;
            }

            return clear;

        }
        public List<string> ExtractContainFile()
        {
            if (!confirmFileExisted())
            {
                return null;
            }
            List<string> list = new List<string>();

            if (!IsExited())
            {
                log.WriteLine("Not Found File");
                list = null;
                return list;
            }
            try
            {
                using (StreamReader R = new StreamReader(FilePath))
                {
                    string line;

                    while ((line = R.ReadLine()) != null)
                    {

                        list.Add(line);
                    }


                }
            }
            catch (Exception e) { log.WriteLine(e.Message); }

            return list;
        }
        public static List<string> ExtractContainFile(string path)
        {

            List<string> list = new List<string>();

            if (!File.Exists(path))
            {
                log.WriteLine("Not Found File");
                list = null;
                return list;
            }
            try
            {
                using (StreamReader R = new StreamReader(path))
                {
                    string line;

                    while ((line = R.ReadLine()) != null)
                    {

                        list.Add(line);
                    }
                }
            }
            catch (Exception e) { log.WriteLine(e.Message); }

            return list;
        }
        private void HandlerinsertText(int line, int index, string message)
        {
            if (!confirmFileExisted())
            {
                return;
            }
            List<string> list = ExtractContainFile();
            for (int x = 0; x < list.Count; x++)
            {
                if (x == line)
                {
                    list[x] = list[x].Insert(index, message);
                    log.WriteLine(list[x]);

                    string Resultfinal = string.Join("\n", list);
                    log.WriteLine("Result Final");
                    log.WriteLine(Resultfinal);
                    ReWriteFile(Resultfinal);
                    return;
                }
            }

        }
        public void ReWriteFile(string Message)
        {
            if (!confirmFileExisted())
            {
                return;
            }
            ListBeforeContain = ExtractContainFile();
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("Not Found File");

                return;
            }
            if (Message == "")
            {
                File.WriteAllText(FilePath, string.Empty);
                return;
            }
            try
            {
                using (StreamWriter W = new StreamWriter(FilePath, false))
                {

                    W.WriteLine(Message);
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void AppendFile(string Message)
        {

            if (!File.Exists(FilePath))
            {
                Console.WriteLine("Not Found File");
                return;
            }
            try
            {
                using (StreamWriter W = new StreamWriter(FilePath, true))
                {

                    W.WriteLine(Message);
                }

            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void FindLoactionWord(string str, bool Reverse = false, bool CaseSenstive = true, bool ShowAllResult = false)
        {
            if (!confirmFileExisted())
            {
                return;
            }
            int counter = 0;

            List<string> list = ExtractContainFile();
            if (Reverse)
            {
                list.Reverse();
                list.ForEach((x) => log.WriteLine(x));
                counter = list.Count - 1;
            }

            foreach (string i in list)
            {
                string temp = i;
                if (!CaseSenstive)
                {
                    temp = i.ToLower();
                    str = str.ToLower();
                }
                if (temp.Contains(str))
                {
                    int count = 0;
                    char? perv = '*';
                    for (int ch = 0; ch < temp.Length - 1; ch++)
                    {

                        if (str.Contains(temp[ch]) && str.Contains((char)perv))
                        {
                            Console.WriteLine($"location Word [{str}], in File Contain In Line Number : [{counter + 1}], And Word Strated from index [{count - 1}]" +
                                $", And End Index [{(count - 1) + (str.Length - 1)}]");
                            if (!ShowAllResult)
                            {
                                return;
                            }
                            else
                            {
                                ch += str.Length - 1;
                            }
                        }
                        else
                        {
                            count++;
                        }
                        if (ch < temp.Length)
                        {
                            perv = temp[ch];
                        }
                    }
                    if (Reverse)
                    {
                        counter--;
                    }
                    else
                    {
                        counter++;
                    }
                }
                else
                {
                    if (Reverse)
                    {
                        counter--;
                    }
                    else
                    {
                        counter++;
                    }
                }
            }
        }
        public bool InsertTextAfter(string word, string message, bool Last)
        {
            if (!confirmFileExisted())
            {
                return false;
            }
            if (message[0] != ' ')
            {
                message = " " + message;
            }
            bool Result = false;
            bool Reverse = false;
            bool CaseSenstive = false;
            string str = word;

            int counter = 0;

            List<string> list = ExtractContainFile();
            if (str == "" && Last)
            {
                HandlerinsertText(list.Count - 1, list[list.Count - 1].Length, message);
                return true;
            }
            else
            {
                if (str == "")
                {
                    HandlerinsertText(0, 0, message);
                    return true;
                }
            }

            if (Reverse)
            {
                list.Reverse();
                list.ForEach((x) => log.WriteLine(x));
                counter = list.Count - 1;
            }

            foreach (string i in list)
            {
                string temp = i;
                if (!CaseSenstive)
                {
                    temp = i.ToLower();
                    str = str.ToLower();
                }
                if (temp.Contains(str))
                {
                    int count = 0;
                    char? perv = '*';
                    for (int ch = 0; ch < temp.Length - 1; ch++)
                    {

                        if (str.Contains(temp[ch]) && str.Contains((char)perv))
                        {

                            HandlerinsertText(counter, (count - 1) + (str.Length), message);
                            return true;
                        }
                        else
                        {
                            count++;
                        }
                        if (ch < temp.Length)
                        {
                            perv = temp[ch];
                        }
                    }
                    if (Reverse)
                    {
                        counter--;
                    }
                    else
                    {
                        counter++;
                    }
                }
                else
                {
                    if (Reverse)
                    {
                        counter--;
                    }
                    else
                    {
                        counter++;
                    }
                }
            }
            return Result;

        }
        public (int, int, int) ReturnLocation(string str, bool Reverse , bool CaseSenstive)
        {
            if (!confirmFileExisted())
            {
                return (-1, -1, -1);
            }
            string Orgstr = str;
            
           
            int lineNumber = -1;
            int indexStarted = -1;
            int LastIndex = -1;
            int counter = 0;
            
            List<string> list = ExtractContainFile();
            if (Reverse)
            {
                list.Reverse();
                
                counter = list.Count - 1;
            }

            foreach (string i in list)
            {
                string temp = i;
                if (!CaseSenstive)
                {
                    temp = i.ToLower();
                    str = str.ToLower();
                }
                if (temp.Contains(str))
                {
                    int count = 0;
                    char? perv = '*';
                    char? next = '*';
                    char?lastchar= '*';
                   
                    for (int ch = 0; ch < temp.Length; ch++)
                    {
                        if (ch + 1 < temp.Length-1)
                        {
                            next = temp[ch + 1];
                        }
                        else
                        {
                            next = temp[temp.Length - 1];
                        }
                      
                        if(ch + (str.Length-1) < temp.Length)
                        {
                            lastchar = temp[ch + (str.Length-1)];
                           
                        }
                        else
                        {
                            lastchar = temp[temp.Length-1];
                        }
                       
                        if (str[0]==((char)temp[ch]) && str[1] ==((char)next)&&lastchar==str[str.Length-1])
                        {
                          
                            lineNumber = counter + 1;
                            indexStarted = ch;
                            LastIndex = indexStarted + str.Length;
                            string Resultstring = "";
                            for (int w = indexStarted; w < LastIndex; w++)
                            {
                                Resultstring += temp[w];
                            }
                            return (lineNumber, indexStarted, LastIndex);
                            
                        }
                        else
                        {
                            count++;

                        }

                        perv = temp[ch];


                    }
                    if (Reverse)
                    {
                        counter--;
                    }
                    else
                    {
                        counter++;
                    }
                }
                else
                {
                    if (Reverse)
                    {
                        counter--;
                    }
                    else
                    {
                        counter++;
                    }
                }
            }

            return (lineNumber, indexStarted, LastIndex);
        }
        public bool IsExsitedWordInFile(string word)
        {
            if (!confirmFileExisted())
            {
                return false;
            }
            bool R = false;
            var items = ReturnLocation(word, false, false);
            if (items.Item1 != -1 && items.Item2 != -1 && items.Item3 != -1)
            {
                R = true;
            }
            return R;
        }
        public string DumpingContain()
        {
            if (!confirmFileExisted())
            {
                return null;
            }
            string R = string.Join("\n", ExtractContainFile());
            return R;
        }
        public static string DumpingContain(string pathfile)
        {
            string R = string.Join("\n", ExtractContainFile(pathfile));
            return R;
        }
        public bool ReNameFile(string newName, string extention = "")
        {
            if (!confirmFileExisted())
            {
                return false;
            }
            bool R = false;
            if (extention == "")
            {
                extention = this.Extention;
            }
            else
            {
                if (extention[0] != '.')
                {
                    extention = "." + extention;
                }
            }
            string NewPath = Path.Combine(this.DirectoryName, newName + extention);
            if (File.Exists(NewPath))
            {
                log.WriteLine("NewName To file Already Exists in Same Directory , Select Another Name");
                R = false;
            }
            else
            {

                R = true;
                string conatin = DumpingContain();
                File.Move(this.FilePath, NewPath);
                File.Delete(this.FilePath);
                ReLoadFileInfo(NewPath);
                ReWriteFile(conatin);

                log.WriteLine("Perform Renaming File");
            }
            return R;
        }
        private void HandlerReplaceWords(string wordnew, string wordReplaced, int line, int index, int lastindex, bool CaseSenstive)
        {
            
            string R = "";
            int deffer = lastindex - index;
            List<string> list = ExtractContainFile();
            List<string> listTemp = new List<string>();
           
            for (int i = 0; i < list.Count; i++)
            {

                string temp = list[i];
                if (line == i+1)
                {
                    wordReplaced = "";
                   
                    for(int ind = index; ind < lastindex; ind++)
                    {
                        wordReplaced += temp[ind];
                    }
                }
              
               
                if (temp.Contains(wordReplaced))
                { 
                    
                  
                    string finalString = "";
                    for (int ch = 0; ch < temp.Length; ch++)
                    {
                        if (ch == index && ch + deffer == lastindex)
                        {
                            if (wordReplaced.Contains(" ")) {
                                finalString = temp.Remove(ch , deffer);
                                finalString = finalString.Insert(ch , wordnew); 
                              
                            }
                            else
                            {
                                finalString = temp.Remove(ch, deffer);
                                finalString = finalString.Insert(ch, wordnew);
                            }
                            listTemp.Add(finalString);

                            for (int j = line; j <= list.Count - 1; j++)
                            {

                                listTemp.Add(list[j]);
                            }

                            R = string.Join("\n", listTemp);
                            ReWriteFile(R);
                            return;

                        }
                    }
                }
                else
                {
                    listTemp.Add(list[i]);
                }
            }
        }
        public void ReplaceWord(string wordnew, string wordReplaced, bool AllWords,bool caseSensitive,int iteration=0)
        {
            if (wordnew.Contains(wordReplaced))
            {
                log.WriteLine("Select Anther Word Because It Contains The Word To Be Replaced");
                return;
            }
            
            int line = 0;
            int index = 0;
            int lastindex = 0;
            int counter = 0;
            if (AllWords)
            {
                do
                {


                    var items = ReturnLocation(wordReplaced, false, caseSensitive);
                    if (items.Item1 == line && items.Item2 == index && items.Item3 == lastindex)
                    {
                        log.WriteLine("Word already replaced at this location");
                        return;
                    }
                    line = items.Item1;
                    index = items.Item2;
                    lastindex = items.Item3;


                    if (line == -1 || index == -1 || lastindex == -1)
                    {
                        if (counter == 0)
                        {
                            log.WriteLine("Not found Location Word in File");
                        }
                        return;
                    }
                    counter++;
                   
                    HandlerReplaceWords(wordnew, wordReplaced, line, index, lastindex, caseSensitive);
                    CountWordInFile++;
                    if(CountWordInFile>=iteration&&iteration!=0)
                    {
                        return;
                    }

                }
                while (true);

            }
            else
            {
                
                var items = ReturnLocation(wordReplaced, false, caseSensitive);
                line = items.Item1;
                index = items.Item2;
                lastindex = items.Item3;


                if (line == -1 || index == -1 || lastindex == -1)
                {
                    log.WriteLine("Not found Location Word in File");
                    return;
                }
                HandlerReplaceWords(wordnew, wordReplaced, line, index, lastindex,caseSensitive);
                CountWordInFile++;
            }
               
        }
        public int CountWordInFileReplaced()
        {
            return CountWordInFile;
        }

    }
}
