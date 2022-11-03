using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using Spire.Doc;
using Spire.Pdf;
using FileFormat = Spire.Doc.FileFormat;


namespace Controller
{
    public partial class WTP : Page
    {
        public WTP()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Main.mainWindow.F1.Content = Main.main;
        }

        private void SelectIn(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = ".docx;.doc|*.docx;*.doc";
            dialog.Multiselect = false;
            if (dialog.ShowDialog(Main.mainWindow) == false) return;
            String _fileName = dialog.FileName;
            InputBox.Text = _fileName;
        }

        private void SelectOut(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveDg = new System.Windows.Forms.SaveFileDialog();
            saveDg.Filter = ".pdf|*.pdf";
            saveDg.FileName = "";
            saveDg.AddExtension = true;
            saveDg.RestoreDirectory = true;
            if (saveDg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //此处做你想做的事
                string filePath = saveDg.FileName;
                OutputBox.Text = filePath;
            }
        }


        private string t1 = "";
        private string t2 = "";
        private int limit;

        private void Begin(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(InputBox.Text))
            {
                MessageBox.Show("无效的文件路径，请重新输入！");
            }
            else
            {
                if (OutputBox.Text.Length < 3 || !Directory.Exists((OutputBox.Text).Substring(0, 3)) ||
                    Directory.Exists(OutputBox.Text))
                {
                    MessageBox.Show("无效的输出位置！请重新输入");
                }
                else
                {
                    br1.IsEnabled = false;
                    br2.IsEnabled = false;
                    begin.IsEnabled = false;
                    back.IsEnabled = false;
                    InputBox.IsEnabled = false;
                    OutputBox.IsEnabled = false;
                    limit = 0;
                    t1 = InputBox.Text;
                    t2 = OutputBox.Text;
                    new Thread(Thr).Start();
                    new Thread(prog).Start();
                }
            }
        }


        public void Thr()
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                Bar.Visibility = Visibility.Visible;
                Bar.Value = 0;
            }));
            //new WTPGG.Class1().Execute(t1,t2);
            String source = t1.Replace(".docx", "dir");
            String outs = t1.Replace(".docx", "-chged.docx");
            limit = 10;
            (new FastZip()).ExtractZip(t1, source, "");
            FileAttributes MyAttributes = File.GetAttributes(source);
            File.SetAttributes(source, MyAttributes | FileAttributes.Hidden);
            limit = 30;
            XmlDocument xml = new XmlDocument();
            xml.Load(source + "/word/document.xml");
            XmlNamespaceManager nsp = new XmlNamespaceManager(xml.NameTable);
            nsp.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            /*XmlNodeList list = xml.SelectNodes("w:document/w:body/w:p/w:pPr/w:sectPr/w:pgNumType",nsp);
            foreach (XmlNode node in list)
            {   
                node.Attributes["w:start"].Value = (int.Parse(node.Attributes["w:start"].Value) + 1).ToString();
            }*/
            XmlNode node = xml.SelectSingleNode("w:document/w:body", nsp);
            XmlElement child = xml.CreateElement("w:p", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            node.InsertBefore(child, node.ChildNodes[0]);
            child.SetAttribute("paraId", "http://schemas.openxmlformats.org/wordprocessingml/2006/main", "21BE8DF6");
            child.SetAttribute("textId", "http://schemas.openxmlformats.org/wordprocessingml/2006/main", "77777777");
            child.SetAttribute("rsidR", "http://schemas.openxmlformats.org/wordprocessingml/2006/main", "0076D07");
            child.SetAttribute("rsidRDefault", "http://schemas.openxmlformats.org/wordprocessingml/2006/main",
                "0076D07");
            XmlElement ppr = xml.CreateElement("w:pPr", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            child.AppendChild(ppr);
            XmlElement wcl = xml.CreateElement("w:widowControl",
                "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            XmlElement jc = xml.CreateElement("w:jc", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            jc.SetAttribute("w:val", "left");
            ppr.AppendChild(wcl);
            ppr.AppendChild(jc);
            XmlElement r = xml.CreateElement("w:r", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            child.AppendChild(r);
            XmlElement br = xml.CreateElement("w:br", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
            r.AppendChild(br);
            br.SetAttribute("type", "page");
            xml.Save(source + "/word/document.xml");
            limit = 90;
            (new FastZip()).CreateZip(outs, source, true, "");
            MyAttributes = File.GetAttributes(outs);
            File.SetAttributes(outs, MyAttributes | FileAttributes.Hidden);
            DirectoryInfo di = new DirectoryInfo(source);
            di.Delete(true);
            Document doc = new Document(outs);
            doc.SaveToFile(t2.Replace(".pdf", "-pre.pdf"), FileFormat.PDF);
            MyAttributes = File.GetAttributes(t2.Replace(".pdf", "-pre.pdf"));
            File.SetAttributes(t2.Replace(".pdf", "-pre.pdf"),
                MyAttributes | FileAttributes.Hidden);
            PdfDocument pdf = new PdfDocument(t2.Replace(".pdf", "-pre.pdf"));
            pdf.Pages.RemoveAt(0);
            pdf.SaveToFile(t2, Spire.Pdf.FileFormat.PDF);
            limit = 90;
            File.Delete(t2.Replace(".pdf", "-pre.pdf"));
            File.Delete(outs);
            limit = 100;
            MessageBox.Show("转换完成");
            Dispatcher.BeginInvoke(new Action(delegate
            {
                doc.Close();
                pdf.Close();
                br1.IsEnabled = true;
                br2.IsEnabled = true;
                begin.IsEnabled = true;
                back.IsEnabled = true;
                InputBox.IsEnabled = true;
                OutputBox.IsEnabled = true;
                Bar.Visibility = Visibility.Hidden;
            }));
            Thread.CurrentThread.Abort();
        }

        private void prog()
        {
            double x = 0;
            while (limit <= 100)
            {
                while (x < limit)
                {
                    Thread.Sleep(10);
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        Bar.Value+=2;
                        x = Bar.Value;
                    }));
                }

                if (x >= 100)
                {
                    break;
                }
            }
            Thread.CurrentThread.Abort();
        }
    }
}