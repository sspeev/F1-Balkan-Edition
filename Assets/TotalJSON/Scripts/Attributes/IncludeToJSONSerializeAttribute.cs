//  IncludeToJSONSerialize


using System;

namespace Leguar.TotalJSON {

	/// <summary>
	/// Attribute that can be used to include single field that normally would not be serialized to JSON serialization.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	public sealed class IncludeToJSONSerializeAttribute : Attribute {

		/// <summary>
		/// Constructor for new IncludeToJSONSerialize attribute.
		/// </summary>
		public IncludeToJSONSerializeAttribute() {
		}

	}

}
