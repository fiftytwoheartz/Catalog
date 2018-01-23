using System.Collections.Generic;

namespace Catalog.Repo
{
	public class HintIntermediate
	{
		public bool StaysForSuccess;

		public bool StaysForFailure;

		public string ActionDescription;

		public string RelativeURL;

		public HintIntermediate(bool staysForSuccess, bool staysForFailure, string actionDescription, string relativeURL)
		{
			StaysForSuccess = staysForSuccess;
			StaysForFailure = staysForFailure;
			ActionDescription = actionDescription;
			RelativeURL = relativeURL;
		}

		public Hint Build()
		{
			return new Hint(ActionDescription, RelativeURL);
		}
	}

	public class Hint
	{
		public readonly string ActionDescription;

		public readonly string RelativeURL;

		public Hint(string actionDescription, string relativeUrl)
		{
			ActionDescription = actionDescription;
			RelativeURL = relativeUrl;
		}
	}
}