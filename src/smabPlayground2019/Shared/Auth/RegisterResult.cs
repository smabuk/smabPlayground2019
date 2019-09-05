using System.Collections.Generic;

namespace smabPlayground2019.Shared
{
	public class RegisterResult
	{
		public bool Successful { get; set; }
		public IEnumerable<string> Errors { get; set; }
	}
}
