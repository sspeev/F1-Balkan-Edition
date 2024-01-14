//  CreateStringRunner


using System;
using System.Text;

namespace Leguar.TotalJSON.Internal {

	internal class CreateStringRunner {

		private readonly CreateStringSettings settings;
		private readonly StringBuilder builder;

		private readonly string newLineString;
		private readonly string indentString;
		private int currentIndent;

		internal CreateStringRunner(CreateStringSettings settings) {
			this.settings = settings;
			builder=new StringBuilder();
			newLineString=getNewLineString();
			if (settings.HumanReadable) {
				if (settings.IndentUsingTab) {
					indentString="\t";
				} else {
					indentString=new string(' ',settings.IndentSpaceCount);
				}
				currentIndent=0;
			}
		}

		internal void append(char chr) {
			builder.Append(chr);
		}

		internal void append(char chr, bool space) {
			builder.Append(chr);
			if (space && settings.HumanReadable) {
				builder.Append(' ');
			}
		}

		internal void append(string str) {
			builder.Append(str);
		}

		internal void append(char chr1, char chr2) {
			builder.Append(chr1);
			if (settings.HumanReadable) {
				builder.Append(' ');
			}
			builder.Append(chr2);
		}

		internal void append(char chr, int indentChange) {
			builder.Append(chr);
			if (settings.HumanReadable) {
				builder.Append(newLineString);
				currentIndent += indentChange;
				for (int n = 0; n<currentIndent; n++) {
					builder.Append(indentString);
				}
			}
		}

		internal void append(int indentChange, char chr) {
			if (settings.HumanReadable) {
				builder.Append(newLineString);
				currentIndent += indentChange;
				for (int n = 0; n<currentIndent; n++) {
					builder.Append(indentString);
				}
			}
			builder.Append(chr);
		}

		internal const string COLOR_STRING_VALUE = "<color=#00dd00>";
		internal const string COLOR_NUMBER = "<color=#ff3333>";
		internal const string COLOR_BOOL = "<color=#888800>";
		internal const string COLOR_NULL = "<color=#5555ff>";
		internal const string COLOR_END = "</color>";
		
		internal void appendColoring(string str) {
			if (isColoredOutput()) {
				builder.Append(str);
			}
		}

		private const string BOLD_ON = "<b>";
		private const string BOLD_OFF = "</b>";

		internal void appendBolding(bool on) {
			if (isColoredOutput()) {
				builder.Append(on?BOLD_ON:BOLD_OFF);
			}
		}

		internal bool isColoredOutput() {
			return settings.ColoredOutput;
		}

		internal bool isEscapeForwardSlashes() {
			return settings.EscapeForwardSlashes;
		}

		internal string getFinalString() {
			return builder.ToString();
		}

		private string getNewLineString() {
			if (settings.NewLine==CreateStringSettings.NewLineTypes.EnvironmentDefault) {
				return Environment.NewLine;
			} else if (settings.NewLine==CreateStringSettings.NewLineTypes.LF) {
				return "\n";
			} else if (settings.NewLine==CreateStringSettings.NewLineTypes.CR_LF) {
				return "\r\n";
			} else {
				DebugLogger.LogInternalError("CreateStringRunner.getNewLine(): Unspecified new line type");
				return Environment.NewLine;
			}
		}

	}

}
