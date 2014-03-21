using System;
using System.Text;

namespace vplan
{
	public class Group
	{
		public Group() { }
		public Group(int id, string name) {
			ID = id;
			ClassName = name;
		}

		public string ClassName { get; set; }
		public override string ToString()
		{
			return ClassName;
		}
		public int ID { get; set; }
	}
}
