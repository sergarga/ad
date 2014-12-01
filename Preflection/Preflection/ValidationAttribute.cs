using System;

namespace Preflection
{
	public abstract class ValidationAttribute : Attribute {

		public abstract string Validate(object value);
	}
}

