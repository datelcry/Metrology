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
                    Error = 2;
            }
            else
                Error = 1;
            if (Error == 0)
                PHPCode = Code.Substring(StartLoc + 4, FinishLoc - StartLoc - 4);
                
            return Error;
        }



        void AnalyzeStepOne(string Code)
        {
             switch (MainTagsIsHere(Code))
             {
                 case 0: 
                     {
                         AddStatus("PHP Tags are correct");
                         AnalyzeStepTwo(PHPCode);
                         break;
                     }

                 case 1:
                     {
                         AddStatus("No PHP Tags");
                         break;
                     }
                 case 2:
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

        int[] FindIndexes(string Code, int FunctionCounter)
        {

            int temp = 0;
            int[] FunctionIndexes = new int[FunctionCounter];
            for (int i = 0; i < FunctionCounter; i++)
            {
                
                FunctionIndexes[i] = Code.IndexOf("function", 0 + temp);
                temp = FunctionIndexes[i] + 8;
            }


            return FunctionIndexes;
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
            NewStruct.DoWhile = CountObjects(Code, "do", "while");
            NewStruct.While = CountObjects(Code, "while") - NewStruct.DoWhile;
            NewStruct.Case = CountObjects(Code, "case");
            NewStruct.Case = CountObjects(Code, "elseif");
            NewStruct.Default = CountObjects(Code, "default");

            return NewStruct;
        }
        




        public struct ControlStructers
        {
            public int For;
            public int Foreach;
            public int While;
            public int DoWhile;
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


        void AnalyzeStepThree(int AmountOfOperators, ControlStructers Structers, int[] FunctionCounter)
        {
            int BLOCKS = 0;
            int ARC = 0;
            const int MainTerminators = 2;
            AmountOfOperators -= (Structers.For*2);
            BLOCKS = AmountOfOperators;
            BLOCKS += Structers.If + Structers.For * 2 + Structers.Foreach * 2 + Structers.While * 2 + Structers.DoWhile * 2 + Structers.ElseIf;
            BLOCKS += FunctionCounter.Length * 2 + MainTerminators;

            AddStatus("Amount of BLOCKS is", BLOCKS);

            ARC = MainTerminators / 2 + FunctionCounter.Length;
            ARC += AmountOfOperators;
            ARC += Structers.If * 2 + Structers.For * 2 + Structers.Foreach * 2 + Structers.While * 2 + Structers.DoWhile * 2 + Structers.ElseIf * 2;

            AddStatus("Amount of ARCS is", ARC);

            int Svyaznost = (MainTerminators + FunctionCounter.Length * 2) / 2;

            AddStatus("Amount of SVYAZNOST is", Svyaznost);
            int MakkeibNumber = 0;

            //ARC 

            MakkeibNumber = ARC - BLOCKS + 2 * Svyaznost;
            AddStatus("MakkeibNumber is", MakkeibNumber);


        }

        void AnalyzeStepTwo(string Code)
        {



            int FunctionCounter = CountObjects(Code, "function");
            int[] FunctionIndexes = new int[FunctionCounter];
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
                        FunctionIndexes = FindIndexes(Code, FunctionCounter);
                        break;
                    }
            }

            int AmountOfOperators = CountObjects(Code, ";");
            AddStatus("Amount of process/subprocesses is", AmountOfOperators);
            ControlStructers CodeStructures;

            CodeStructures = CountStructures(Code);
            int AmountOfControlStructures = CodeStructures.Break + CodeStructures.Continue + CodeStructures.DoWhile + CodeStructures.For + CodeStructures.Foreach + CodeStructures.If + CodeStructures.Include + CodeStructures.Return + CodeStructures.Switch + CodeStructures.While + CodeStructures.GoTo;
            AddStatus("Amount of Control Structures is", AmountOfControlStructures);

            AnalyzeStepThree(AmountOfOperators, CodeStructures, FunctionIndexes);
            //int[] IndexOfFunction = new int[PHPCode.Take(4).Equals("function")] 
             //   Удалять комментарии; EPTA

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
