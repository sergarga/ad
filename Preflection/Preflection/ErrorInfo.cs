using System;

namespace Preflection
{
	public class ErrorInfo
	{
		private string property;
		private string message;

		public ErrorInfo (string property, string message){
			this.property = property;
			this.message = message;
		}

		public string Property { get { return property; } }
		public string Message { get { return message; } }
	}
}

