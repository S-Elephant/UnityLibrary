#nullable enable

using UnityEngine;
using UnityEngine.Networking;

namespace Elephant.UnityLibrary.Other
{
	/// <summary>
	/// Provides cross-platform email functionality (PC, Android, and iOS).
	/// Works for PC, Android and iOS.
	/// </summary>
	public class Mail
	{
		/// <summary>
		/// Open the default email client with the specified recipient.
		/// </summary>
		/// <param name="recipient">Recipient e-mail address.</param>
		public virtual void OpenMail(string recipient)
		{
			Application.OpenURL("mailto: " + recipient);
		}

		/// <summary>
		/// Open the default email client with recipient, subject, and optional body.
		/// </summary>
		/// <param name="recipient">Recipient e-mail address.</param>
		/// <param name="subject">E-mail subject.</param>
		/// <param name="body">E-mail body contents (optional).</param>
		public virtual void OpenMail(string recipient, string subject, string body = "")
		{
			Application.OpenURL("mailto:" + recipient + "?subject=" + EscapeUrl(subject) + "&body=" + EscapeUrl(body));
		}

		/// <summary>
		/// Escapes a URL string for use in e-mail links.
		/// </summary>
		/// <param name="url">String to escape.</param>
		/// <returns>Escaped URL string.</returns>
		public virtual string EscapeUrl(string url) => UnityWebRequest.EscapeURL(url).Replace("+", "%20");
	}
}
