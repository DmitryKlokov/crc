using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace CRC
{
    public partial class Form1 : Form
    {
        crc32 c;
        List<file> lf;
        public Form1()
        {
            InitializeComponent();
            Deserialize();        
            c = new crc32();
        }

        private void Serialize()
        {
            FileStream fs = new FileStream("crc.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(List<file>));
            serializer.Serialize(fs, lf);
            fs.Close();
        }

        private void Deserialize()
        {
            FileStream fs = new FileStream("crc.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<file>));
            lf = (List<file>)serializer.Deserialize(fs);
            fs.Close();
        }
        
        public void CompareCRC(file f)
        {
            int index = lf.FindIndex((fa) => fa.name == f.name);
            if (index >= 0)
            {
                if (lf[index].crc == f.crc)
                {
                    richTextBox1.Text += ("CRC файла " + f.name + " совпал\n");
                }
                else
                {
                    richTextBox1.Text += ("CRC файла " + f.name + " не совпал\n");
                    lf[index] = f;
                }
            }
            else
            {
                lf.Add(f);
                richTextBox1.Text += ("Файл " + f.name + " добавлен в базу\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo di = new DirectoryInfo(folder.SelectedPath);
                foreach(FileInfo fl in di.GetFiles())
                {
                    file f = new file(fl.FullName, c.GetFileCrc(fl.FullName));
                    CompareCRC(f);
                }
            }
            Serialize();
        }
    }
}
