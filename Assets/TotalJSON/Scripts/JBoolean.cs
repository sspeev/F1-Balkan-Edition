//  JValue - JBoolean


using System;
using Leguar.TotalJSON.Internal;

namespace Leguar.TotalJSON {

	/// <summary>
	/// Class to store boolean value in JSON format. Once JBoolean instance is created, its value can't be changed.
	/// </summary>
	public class JBoolean : JValue {

		private const string TRUE="true";
		private const string FALSE="false";

		private readonly bool boolValue;

		/// <summary>
		/// Creates new instance of JBoolean class.
		/// </summary>
		/// <param name="boolValue">
		/// Boolean value stored to this object.
		/// </param>
		public JBoolean(bool boolValue) : base() {
			this.boolValue = boolValue;
		}

		/// <summary>
		/// Returns compact information of this JBoolean object as string, for debug purposes.
		/// </summary>
		/// <returns>
		/// Single string with information of this JBoolean object.
		/// </returns>
		public override string ToString() {
			return ("[JBoolean: "+(boolValue?TRUE:FALSE)+"]");
		}

		/// <summary>
		/// Test if another object equals to this object. Always returns false if parameter object is null or it is not instance of JBoolean.
		/// Two JBoolean objects are equal if both contains same boolean value.
		/// </summary>
		/// <param name="anotherObject">
		/// Another object that is compared to this one.
		/// </param>
		/// <returns>
		/// True if objects are equal, false otherwise.
		/// </returns>
		public override bool Equals(object anotherObject) {
			if (anotherObject==null) {
				return false;
			}
			if (!(anotherObject is JBoolean)) {
				return false;
			}
			JBoolean anotherJBoolean=(JBoolean)(anotherObject);
			return (boolValue==anotherJBoolean.AsBool());
		}

		public override int GetHashCode() {
			return (boolValue?1:0);
		}

		/// <summary>
		/// Get value of this JSON boolean as c# system bool.
		/// </summary>
		/// <returns>
		/// System bool value.
		/// </returns>
		public bool AsBool() {
			return boolValue;
		}

		internal override void zCreate(CreateStringRunner createStringRunner) {
			createStringRunner.appendColoring(CreateStringRunner.COLOR_BOOL);
			createStringRunner.append(boolValue?TRUE:FALSE);
			createStringRunner.appendColoring(CreateStringRunner.COLOR_END);
		}

		internal static JBoolean zParse(ParseStringRunner parseStringRunner, bool expectingTrue) {

			StringPointer sp = parseStringRunner.getStringPointer();

			if (expectingTrue) {
				if (sp.isNextChars(TRUE.Substring(1))) {
					return (new JBoolean(true));
				} else {
					throw ParseException.forInvalidCharacter("Invalid string when expecting '"+TRUE+"'",parseStringRunner);
				}
			} else {
				if (sp.isNextChars(FALSE.Substring(1))) {
					return (new JBoolean(false));
				} else {
					throw ParseException.forInvalidCharacter("Invalid string when expecting '"+FALSE+"'",parseStringRunner);
				}
			}

		}

		internal override object zDeserialize(Type type, string toFieldName, DeserializeSettings deserializeSettings) {

			// In case type is nullable type "bool?"
			Type nullableType = Nullable.GetUnderlyingType(type);
			if (nullableType != null) {
				return this.zDeserialize(nullableType, toFieldName, deserializeSettings);
			}

			if (type==typeof(bool)) {
				return this.AsBool();
			}

			if (type==typeof(object)) {
				if (deserializeSettings.AllowFieldsToBeObjects) {
					return this.AsBool();
				}
				throw (DeserializeException.forNonMatchingTypeObject(this, toFieldName));
			}

			throw (DeserializeException.forNonMatchingType(this,type,toFieldName));

		}

	}

}
