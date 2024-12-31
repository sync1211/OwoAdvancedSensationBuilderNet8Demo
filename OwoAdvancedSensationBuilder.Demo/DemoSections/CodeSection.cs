using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OwoAdvancedSensationBuilder.Demo.DemoSections {
    public partial class CodeSection : UserControl {
        public CodeSection(string code) {
            InitializeComponent();

            tb = new FastColoredTextBox();
            tb.ReadOnly = true;
            tb.AutoSize = true;
            tb.AutoSizeMode = AutoSizeMode.GrowOnly;
            tb.MaximumSize = new Size(700, 300);
            tb.BorderStyle = BorderStyle.FixedSingle;
            tb.TextChanged += Tb_TextChanged;
            tb.Text = code;

            int lines = code.Split("\r\n").Length;
            tb.Size = new Size(700, (14 * lines) + 30);

            pnlCode.Controls.Add(tb);
        }

        FastColoredTextBox tb;

        Style CommentStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        Style KeywordStyle = new TextStyle(Brushes.Blue, null, FontStyle.Regular);
        Style ClassStyle = new TextStyle(Brushes.SkyBlue, null, FontStyle.Bold);
        Style StringStyle = new TextStyle(Brushes.DarkRed, null, FontStyle.Regular);
        Style MethodStyle = new TextStyle(Brushes.RosyBrown, null, FontStyle.Bold);
        Style NumberStyle = new TextStyle(Brushes.Navy, null, FontStyle.Regular);
        Style EnumConstantsStyle = new TextStyle(Brushes.Navy, null, FontStyle.Bold);
        Style LogicStyle = new TextStyle(Brushes.DarkRed, null, FontStyle.Bold);

        private void Tb_TextChanged(object? sender, TextChangedEventArgs e) {
            e.ChangedRange.SetStyle(CommentStyle, @"//.*$", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(StringStyle, @""".*""", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(MethodStyle, @"( |(?<=\.))(\w*?)(?=\()", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(MethodStyle, @"(?<=\+\= )(\w*?)(?=;)", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(StringStyle, @""".*""", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(NumberStyle, @"[0-9]+(\.?[0-9]*)f?", RegexOptions.Multiline);
            e.ChangedRange.SetStyle(EnumConstantsStyle, @"(?<=\.)(\w*?)((_L)|(_R))", RegexOptions.Multiline);

            List<string> keywords = new List<string>();
            keywords.Add("public");
            keywords.Add("private");
            keywords.Add("new");
            keywords.Add("true");
            keywords.Add("false");
            keywords.Add("null");
            keywords.Add("string");
            keywords.Add("int");
            keywords.Add("double");
            keywords.Add("float");
            keywords.Add("bool");
            keywords.Add("void");
            keywords.Add("out");
            keywords.Add("var");
            keywords.Add("throw");
            foreach (string keyword in keywords) {
                e.ChangedRange.SetStyle(KeywordStyle, @"" + keyword , RegexOptions.Multiline);
            }

            List<string> classes = new List<string>();
            classes.Add("Dictionary");
            classes.Add("OWO");
            classes.Add("SensationsFactory");
            classes.Add("Sensation");
            classes.Add("Muscle");
            classes.Add("AdvancedSensationManager");
            classes.Add("AdvancedSensationStreamInstance");
            foreach (string clazz in classes) {
                e.ChangedRange.SetStyle(ClassStyle, @"(^| |(?<=\())" + clazz , RegexOptions.Multiline);
            }

            List<string> logics = new List<string>();
            logics.Add("if");
            logics.Add("else");
            logics.Add("for");
            logics.Add("foreach");
            logics.Add("while");
            foreach (string logic in logics) {
                e.ChangedRange.SetStyle(LogicStyle, @"" + logic, RegexOptions.Multiline);
            }
        }

    }
}
