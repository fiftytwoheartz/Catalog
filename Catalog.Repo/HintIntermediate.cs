using System.Collections.Generic;
using System.Net.Http;

namespace Catalog.Repo
{
	public class HintIntermediate
	{
		public readonly HttpVerb HttpVerb;

		public bool StaysForSuccess;

		public bool StaysForFailure;

		public string Description;

		public string RelativeURL;

		public HintIntermediate(
			bool staysForSuccess,
			bool staysForFailure,
			HttpVerb httpVerb,
			string description,
			string relativeURL)
		{
			StaysForSuccess = staysForSuccess;
			StaysForFailure = staysForFailure;
			HttpVerb = httpVerb;
			Description = description;
			RelativeURL = relativeURL;
		}

		public Hint Build()
		{
			return new Hint(HttpVerb, Description, RelativeURL);
		}
	}

	public enum HttpVerb
	{
		GET,
		POST
	}

	public class Hint
	{
		public readonly string Description;

		public readonly string RelativeURL;

		public readonly string HttpMethod;

		public Hint(HttpVerb httpVerb, string description, string relativeUrl)
		{
			HttpMethod = httpVerb.ToString();
			Description = description;
			RelativeURL = relativeUrl;
		}
	}
}