using System;
using System.Windows.Forms;

using KeePass.UI;
using KeePass.Plugins;
using KeePassLib.Keys;
using KeePassLib.Utility;
using KeePassLib.Serialization;
using KeyProviderTest.Forms;

namespace KeyProviderTest
{
    public sealed class KeyProviderTestExt : Plugin
    {
        private IPluginHost m_host = null;
        private SampleKeyProvider m_prov = new SampleKeyProvider();

        public override bool Initialize(IPluginHost host)
        {
            m_host = host;

            m_host.KeyProviderPool.Add(m_prov);
            return true;
        }

        public override void Terminate()
        {
            m_host.KeyProviderPool.Remove(m_prov);
        }
    }

    public sealed class SampleKeyProvider : KeyProvider
    {
        private const string AuxFileExt = ".otp.xml";
        private const string ProvType = "OATH HOTP / RFC 4226";
        private const string ProvVersion = "2.0"; 
        public override string Name
        {
            get { return "Sample Key Provider"; }
        }

        public override byte[] GetKey(KeyProviderQueryContext ctx)
        {
            try
            {
                byte[] pb = (ctx.CreatingNewKey ? Create(ctx) : Open(ctx));

                
                if (pb == null) { return null; }
                
                if (pb == new byte[] { 1 }) { return null; }

                // KeePass clears the returned byte array, thus make a copy
                byte[] pbRet = new byte[pb.Length];
                Array.Copy(pb, pbRet, pb.Length);
                
                return pbRet;
            }
            catch (Exception ex) { MessageService.ShowWarning(ex.Message); }

            return null;

        }
        private static IOConnectionInfo GetAuxFileIoc(KeyProviderQueryContext ctx)
        {
            return GetAuxFileIoc(ctx.DatabaseIOInfo);
        }
        internal static IOConnectionInfo GetAuxFileIoc(IOConnectionInfo iocBase)
        {
            IOConnectionInfo ioc = iocBase.CloneDeep();
            ioc.Path = UrlUtil.StripExtension(ioc.Path) + AuxFileExt;
            return ioc;
        }
        private static byte[] Create(KeyProviderQueryContext ctx)
        {
            IOConnectionInfo iocPrev = GetAuxFileIoc(ctx);
            Info Info = Info.Load(iocPrev);
            if (Info == null) Info = new Info();
            Creation dlg = new Creation();
            dlg.InitEx(Info, ctx);

            UIUtil.ShowDialogAndDestroy(dlg);


            if (!CreateAuxFile(Info, ctx)) return null;
            
            return Info.Secret;
        }
        private static bool CreateAuxFile(Info Info,
            KeyProviderQueryContext ctx)
        {
            IOConnectionInfo ioc = GetAuxFileIoc(ctx);

            if (!Info.Save(ioc, Info))
            {
                MessageService.ShowWarning("Failed to save auxiliary OTP info file:",
                    ioc.GetDisplayName());
                return false;
            }
            return true;
        }
        private static byte[] Open(KeyProviderQueryContext ctx)
        {
            
            IOConnectionInfo ioc = GetAuxFileIoc(ctx);
            
            Info Info = Info.Load(ioc);
            
            if (Info == null)
            {
                MessageService.ShowWarning("Failed to load auxiliary OTP info file:",
                    ioc.GetDisplayName());

                Info = new Info();

                Login dlgRec = new Login();
                dlgRec.InitEx(Info, ctx);
                if (UIUtil.ShowDialogAndDestroy(dlgRec) != DialogResult.OK)
                    return null;

                return Info.Secret;
            }
            
            Login dlg = new Login();
            UIUtil.ShowDialogAndDestroy(dlg);
           
            if (System.Text.Encoding.UTF8.GetString(dlg.Access()) != System.Text.Encoding.UTF8.GetString(Info.Secret))
                return null;


            

            if (!CreateAuxFile(Info, ctx)) return null;
            
            return Info.Secret;
        }

     }
}
