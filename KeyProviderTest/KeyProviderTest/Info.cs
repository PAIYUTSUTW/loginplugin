using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using KeePassLib.Cryptography;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using KeePassLib.Utility;
using KeyProviderTest.Forms;

namespace KeyProviderTest
{
    public sealed class Info
    {
        private String m_password = String.Empty;
        public String Password
        {
            get { return m_password; }
            set
            {
                
                if (value == null) throw new ArgumentNullException("value");
                m_password = value;
            }
        }
        private byte[] m_pbSecret = null;
        public byte[] Secret
        {
            get { return m_pbSecret; }
            set { m_pbSecret = value; }
        }
        public static Info Load(IOConnectionInfo ioc)
        {
            Stream sIn = null;

            try
            {

                sIn = IOConnection.OpenRead(ioc);

                XmlSerializer xs = new XmlSerializer(typeof(Info));
                return (Info)xs.Deserialize(sIn);
            }
            catch (Exception) { }
            finally
            {
                if (sIn != null) sIn.Close();
            }

            return null;
        
        }
        public static bool Save(IOConnectionInfo ioc,Info Info)
        {
            Stream sOut = null;

            try
            {
                sOut = IOConnection.OpenWrite(ioc);

                XmlWriterSettings xws = new XmlWriterSettings();
                xws.CloseOutput = true;
                xws.Encoding = StrUtil.Utf8;
                xws.Indent = true;
                xws.IndentChars = "\t";

                XmlWriter xw = XmlWriter.Create(sOut, xws);

                XmlSerializer xs = new XmlSerializer(typeof(Info));
                xs.Serialize(xw, Info);

                xw.Close();
                return true;
            }
            catch (Exception) { Debug.Assert(false); }
            finally
            {
                if (sOut != null) sOut.Close();
            }

            return false;
        }
    }
}
