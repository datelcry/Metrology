

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace МЕТРОЛОГИЯ
{
    public partial class MainForm : Form
    {


        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonLoadPHP_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                System.IO.StreamReader CobolFile = new System.IO.StreamReader(openFileDialog.FileName, Encoding.Default);
                string Temp = CobolFile.ReadToEnd();
                TextBoxSource.Text = Temp;
                CobolFile.Close();

            }
        }

        private void ButtonAnalyze_Click(object sender, EventArgs e)
        {
            string Code = TextBoxSource.Text.ToLower();
            TextBoxStatus.Text = "";
            AnalyzeStepOne(TextBoxSource);
        }

        void AddStatus(string Message)
        {
            TextBoxStatus.AppendText(Message + Environment.NewLine);
        }

        void AddStatus(string Message, int value)
        {
            TextBoxStatus.AppendText(Message + ' ' + (char)(8212) + ' ' + value + Environment.NewLine);
        }
        List<string> DeleteComments(List<string> Object)
        {
            for (int i = 0; i < Object.Count; i++ )
            {
                if (Object[i].Contains('*'))
                {
                    Object[i] = Object[i].Remove(0);
                }
            }
            return Object;
        }
        List<string> DeleteFILLERS(List<string> Object)
        {
            for (int i = 0; i < Object.Count; i++ )
            {
                if (Object[i].Contains("FILLER"))
                {
                    Object[i] = Object[i].Remove(0);
                }
            }
            return Object;
        }

        List<string> DoTopCase(List<string> Object)
        {
            for (int i = 0; i < Object.Count; i++)
            {
                Object[i] = Object[i].ToUpper();
            }
            return Object;
        }
        void AnalyzeStepOne(RichTextBox Component)
        {
            List<string> StringList = new List<string>();
            for (int i = 0; i < Component.Lines.Count(); i++)
            {
                StringList.Add(Component.Lines[i]);
            }
            StringList = DoTopCase(StringList);
            StringList = DeleteComments(StringList);
            StringList = DeleteFILLERS(StringList);
            AnalyzeStepTwo(StringList);
        }


        int FindIndex(List<string> StringList, string Phrase)
        {
            const int ItsNotHere = -1;
            int Index = 0;
            bool ItsHere = false;

            for (int i = 0; i < StringList.Count; i++ )
            {
                if (StringList[i].Contains(Phrase))
                {
                    Index = i;
                    ItsHere = true;
                    break;
                }   
            }

            if (ItsHere)
                return Index;
            else
                return ItsNotHere; 
        }


        void AnalyzeStepThree(List<string> UnicalVariables, List<TInfo> Segments, List<string> StringList)
        {
            int SpenNumber = 0;
            int CurrCounter = 0;
            AddStatus("Analysing DIVISION PROCEDURE...");
            for (int i = 0; i < UnicalVariables.Count; i++)
            {
                for (int j = Segments[2].StringIndex; j < StringList.Count; j++)
                {
                    if (StringList[j].Contains(UnicalVariables[i]))
                    {
                        CurrCounter++;
                        SpenNumber++;
                    }
                }
                AddStatus("Spen number of " + UnicalVariables[i] + " is", CurrCounter);
                CurrCounter = 0;  
            }
            AddStatus("General Spen Number is ", SpenNumber);
        }

        public struct TInfo
        {
            public string Title;
            public int StringIndex;
            public TInfo(string Data, int Index)
            {
                Title = Data;
                StringIndex = Index;
            }
        }
        

        string FindVariable(string AnalysingString)
        {
            string[] TableOfPhrases;
            List<string> NormalTable = new List<string>();
            
            AnalysingString = AnalysingString.Trim();
            AnalysingString = AnalysingString.Replace('\t', ' ');
            AnalysingString = AnalysingString.Replace('\n', ' ');
            TableOfPhrases = AnalysingString.Split(' ');
            
            for (int i = 0; i < TableOfPhrases.Count(); i++)
                if (TableOfPhrases[i] != "")
                    NormalTable.Add(TableOfPhrases[i]);
            
            bool VarIsHere = false;
            const int SuitableValueOfVisibility = 100;
            int IndexOfVar = 0;
            long Temp = 0;

            for (int i = 0; i < NormalTable.Count; i++)
            {
                if (Int64.TryParse(NormalTable[i], out Temp) && (Temp < SuitableValueOfVisibility))
                {
                    VarIsHere = true;
                    IndexOfVar = i;
                    break;
                }
            }
            
            if (IndexOfVar == NormalTable.Count - 1)
                VarIsHere = false;
            if (VarIsHere)
                return (NormalTable[IndexOfVar + 1]);
            else
                return ("false");
        }

        List<string> FindUnVar(List<string> StringList, List<TInfo> Segments)
        {
            List<string> UnicalVariables = new List<string>();

            for(int i = Segments[0].StringIndex; i < Segments[2].StringIndex; i++)
            {
                if (StringList[i] != "")
                {
                    UnicalVariables.Add(FindVariable(StringList[i]));
                }
            }

            return UnicalVariables;
        }

        void AnalyzeStepTwo(List<string> StringList)
        {
            List<TInfo> Segments = new List<TInfo>();
            Segments.Add(new TInfo("DATA DIVISION.",FindIndex(StringList,"DATA DIVISION.")));
            Segments.Add(new TInfo("WORKING-STORAGE SECTION.",FindIndex(StringList,"WORKING-STORAGE SECTION.")));
            Segments.Add(new TInfo("PROCEDURE DIVISION.",FindIndex(StringList,"PROCEDURE DIVISION.")));
            
            List<string> UnicalVariables = new List<string>();
            AddStatus("Analysing DATA DIVISION...");
            AddStatus("Analysing WORKING-STORAGE SECTION...");
            UnicalVariables = FindUnVar(StringList, Segments);
            if (UnicalVariables.Count != 0)
            {
                UnicalVariables = UnicalVariables.Distinct().ToList();
                UnicalVariables.Remove("false");
                AnalyzeStepThree(UnicalVariables, Segments, StringList);
            }
            else
                AddStatus("There are not any unical variables");
        }



    }
}
