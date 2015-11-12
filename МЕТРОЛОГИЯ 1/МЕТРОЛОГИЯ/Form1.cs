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
        string PHPCode = "";

        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonLoadPHP_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                System.IO.StreamReader PHPFile = new System.IO.StreamReader(openFileDialog.FileName, Encoding.Default);
                string Temp = PHPFile.ReadToEnd();
                Temp = Temp.Replace("\t", "    ");
                TextBoxSource.Text = Temp;
                
                PHPFile.Close();
            
            }
        }

        private void ButtonAnalyze_Click(object sender, EventArgs e)
        {
            string Code = TextBoxSource.Text.ToLower();
            PHPCode = "";
            TextBoxStatus.Text = "";
            AnalyzeStepOne(Code);
        }
        
        void AddStatus(string Message)
        {
            TextBoxStatus.AppendText( Message + Environment.NewLine);
        }

        void AddStatus(string Message, int value)
        {
            TextBoxStatus.AppendText(Message + ' ' + (char)(8212) + ' ' + value + Environment.NewLine);
        }
        string DeletingComments(string Code)
        {
            if (Code.Contains("//"))
            {
                int counterOfComments = CountObjects(PHPCode, "//");
                int commentLength = 0;
                int startpos = 0;
                int j = 0;
                for (int i=0; i<counterOfComments; i++)
                {
                    j = Code.IndexOf("//");
                    startpos = j; 
                    while (Code[j] != '\n')
                    {
                        j++;
                        commentLength++;
                    }
                    Code = Code.Remove(startpos, commentLength+2);
                }
            }
            if (Code.Contains("/*"))
            {
                int counterOfComments = CountObjects(PHPCode, "//");
                int commentLength = 0;
                int startpos = 0;
                int j = 0;
                for (int i = 0; i < counterOfComments; i++)
                {
                    j = Code.IndexOf("/*");
                    startpos = j;
                    while (Code[j] != '*' || Code[j+1] !='/')
                    {
                        j++;
                        commentLength++;
                    }
                    Code = Code.Remove(startpos, j + 2);
                }
            }
            return Code;
        }

        int MainTagsIsHere(string Code)
        {
            int StartLoc = 0;
            int FinishLoc = 0;
            int Error = 0;
            if (Code.Contains("<?php") && Code.Contains("?>"))
            {
                StartLoc = Code.IndexOf("<?php");
                FinishLoc = Code.IndexOf("?>");
                if (StartLoc > FinishLoc)
                    Error = InvalidInterposition;
            }
            else
                Error = NoPHPTags;
            if (Error == PHPTagsAreCorrect)
            {
                PHPCode = Code.Substring(StartLoc + 4, FinishLoc - StartLoc - 4);
                PHPCode = DeletingComments(PHPCode);
            }
            return Error;
        }

        const int PHPTagsAreCorrect =0;
        const int NoPHPTags = 1;
        const int InvalidInterposition = 2;

        void AnalyzeStepOne(string Code)
        {
             switch (MainTagsIsHere(Code))
             {
                 case PHPTagsAreCorrect: 
                     {
                         AddStatus("PHP Tags are correct");
                         AnalyzeStepTwo(PHPCode);
                         break;
                     }

                 case NoPHPTags:
                     {
                         AddStatus("No PHP Tags");
                         break;
                     }
                 case InvalidInterposition:
                     {
                         AddStatus("Interposition of PHP Tags is uncorrectly");
                         break;
                     }
             }
        }


        int CountObjects(string Code, string Object)
        {
            return ((Code.Length - Code.Replace(Object, "").Length) / Object.Length);
        }
        int CountObjects(string Code, string FirstPath, string SecondPath)
        {
            return (((Code.Length - Code.Replace(FirstPath, "").Length) / FirstPath.Length) - ((Code.Length - Code.Replace(SecondPath, "").Length) / SecondPath.Length));
        }




        ControlStructers CountStructures(string Code)
        {
            ControlStructers NewStruct;
            NewStruct.For = CountObjects(Code, "for");
            NewStruct.Foreach = CountObjects(Code, "foreach");
            NewStruct.ElseIf = CountObjects(Code, "elseif");
            NewStruct.If = CountObjects(Code, "if");
            NewStruct.Switch = CountObjects(Code, "switch");
            NewStruct.Break = CountObjects(Code, "break");
            NewStruct.Continue = CountObjects(Code, "continue");
            NewStruct.Return = CountObjects(Code, "return");
            NewStruct.GoTo = CountObjects(Code, "goto");
            NewStruct.Include = CountObjects(Code, "include");
            NewStruct.While = CountObjects(Code, "while") ;
            NewStruct.Case = CountObjects(Code, "case");
            NewStruct.Default = CountObjects(Code, "default");
            return NewStruct;
        }
        




        public struct ControlStructers
        {
            public int For;
            public int Foreach;
            public int While;
          //  public int DoWhile;
            public int If;
            public int Switch;
            public int Break;
            public int Continue;
            public int Return;
            public int GoTo;
            public int Include;
            public int Case;
            public int Default;
            public int ElseIf;
        }


        void AnalyzeStepThree(int AmountOfOperators, ControlStructers Structers, int FunctionCounter)
        {
            int BLOCKS = 0;
            int ARC = 0;
            const int MainTerminators = 2;
            const int DoubleBlocks = 2;
            const int CounterOfSeparators = 2;

            AmountOfOperators -= (Structers.For * CounterOfSeparators);
            
            BLOCKS = AmountOfOperators;
            BLOCKS += Structers.If + Structers.For * DoubleBlocks + Structers.Foreach * DoubleBlocks + Structers.While * DoubleBlocks +  Structers.ElseIf + Structers.Switch;
            BLOCKS += FunctionCounter * DoubleBlocks + MainTerminators;

            AddStatus("Amount of BLOCKS is", BLOCKS);

            ARC = MainTerminators / DoubleBlocks + FunctionCounter;
            ARC += AmountOfOperators;
            ARC += Structers.If * DoubleBlocks + Structers.For * DoubleBlocks + Structers.Foreach * DoubleBlocks + Structers.While * DoubleBlocks  + Structers.ElseIf * DoubleBlocks + Structers.Default + Structers.Case + Structers.Switch;
            if (Structers.Default == 0 && Structers.Switch != 0 && Structers.Case !=0)
                ARC++;
            AddStatus("Amount of ARCS is", ARC);

            int p = (MainTerminators + FunctionCounter * DoubleBlocks) / DoubleBlocks;

            AddStatus("Amount of P is", p);
            int MakkeibNumber = 0;

            //ARC 

            MakkeibNumber = ARC - BLOCKS + 2 * p;
            AddStatus("MakkeibNumber is", MakkeibNumber);


        }

        void AnalyzeStepTwo(string Code)
        {



            int FunctionCounter = CountObjects(Code, "function");
            switch (FunctionCounter)
            {
                case 0:
                    {
                        AddStatus("Functions are not detected");
                        break;
                    }
                    
                default:
                    {
                        AddStatus("Amount of functions is", FunctionCounter);
                        break;
                    }
            }

            int AmountOfOperators = CountObjects(Code, ";");
            AddStatus("Amount of process/subprocesses is", AmountOfOperators);
            ControlStructers CodeStructures;

            CodeStructures = CountStructures(Code);
            int AmountOfControlStructures = CodeStructures.Break + CodeStructures.Continue + CodeStructures.For + CodeStructures.Foreach + CodeStructures.If + CodeStructures.Include + CodeStructures.Return + CodeStructures.Switch + CodeStructures.While + CodeStructures.GoTo;
            AddStatus("Amount of Control Structures is", AmountOfControlStructures);

            AnalyzeStepThree(AmountOfOperators, CodeStructures, FunctionCounter);


        }

        private void TextBoxSource_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Tab)
            {
                int Index = 0;
                Index = TextBoxSource.SelectionStart;
                TextBoxSource.Text.Insert(Index, "    ");
            }
        }

    
    }
}
