using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

using WindowsMediaLib;
using System.Runtime.InteropServices.ComTypes;

namespace v1._0
{
    [ComVisible(true)]
    public class IWMMetadataEditorTest
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMMetadataEditor m_edit;

        public void DoTests()
        {
            Config();

            m_edit.Open(sFileName);
            m_edit.Close();
            m_edit.Open(sFileName);
            m_edit.Flush();
        }

        private void Config()
        {
            WMUtils.WMCreateEditor(out m_edit);
        }
    }
}
