using System;

namespace Preflection
{
	public class EntityAttribute : Attribute{
		public EntityAttribute(){
			Console.WriteLine("EntityAttribute constructor");
		}
		public string TableName { get; set; }
	}
}

