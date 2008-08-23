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
    public class IWMMetadataEditor2Test
    {
        private const string sFileName = @"c:\so_lesson3c.wmv";
        private IWMMetadataEditor2 m_edit;

        public void DoTests()
        {
            Config();

            m_edit.OpenEx(sFileName, MetaDataAccess.Read, MetaDataShare.Read);
            m_edit.Close();
        }

        private void Config()
        {
            IWMMetadataEditor edit;

            WMUtils.WMCreateEditor(out edit);

            m_edit = edit as IWMMetadataEditor2;
        }
    }
}
