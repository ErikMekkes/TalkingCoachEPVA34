using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Models
{
	public class Viseme
	{
		private static Dictionary<VisemeCode, AnimationClip> visemeAnimationMapping;
		private readonly VisemeCode visemeCode;

		public Viseme(VisemeCode phonemeCode)
		{
			visemeCode = phonemeCode;
		}

		public VisemeCode getVisemeCode()
		{
			return visemeCode;
		}

		public double getDuration()
		{
			Debug.Log("Searching for duration of viseme: " + visemeCode.getName());
			return SpeechAnimationManager.instance.getVisemeTimingCalculator().getDictionary()[visemeCode.getName()];
		}

		/// <summary>
		/// Type-safe enum pattern for viseme codes<br/>
		/// See <a href="http://www.javacamp.org/designPattern/enum.html">Typesafe Enum</a>
		/// </summary>
		public sealed class VisemeCode
		{
			private readonly int value;
			private readonly string name;

			public static readonly VisemeCode silence = new VisemeCode(0, "Silence");
			public static readonly VisemeCode AY = new VisemeCode(1, "AY");
			public static readonly VisemeCode AW = new VisemeCode(2, "AW");
			public static readonly VisemeCode AH = new VisemeCode(3, "AH");
			public static readonly VisemeCode AE = new VisemeCode(4, "AE");
			public static readonly VisemeCode IH = new VisemeCode(5, "IH");
			public static readonly VisemeCode AA = new VisemeCode(6, "AA");
			public static readonly VisemeCode B = new VisemeCode(7, "B");
			public static readonly VisemeCode D = new VisemeCode(8, "D");
			public static readonly VisemeCode JH = new VisemeCode(9, "JH");
			public static readonly VisemeCode DH = new VisemeCode(10, "DH");
			public static readonly VisemeCode EY = new VisemeCode(11, "EY");
			public static readonly VisemeCode EH = new VisemeCode(12, "EH");
			public static readonly VisemeCode ER = new VisemeCode(13, "ER");
			public static readonly VisemeCode OW = new VisemeCode(14, "OW");
			public static readonly VisemeCode F = new VisemeCode(15, "F");
			public static readonly VisemeCode G = new VisemeCode(16, "G");
			public static readonly VisemeCode HX = new VisemeCode(17, "HX");
			public static readonly VisemeCode IY = new VisemeCode(18, "IY");
			public static readonly VisemeCode Y = new VisemeCode(19, "Y");
			public static readonly VisemeCode K = new VisemeCode(20, "K");
			public static readonly VisemeCode LL = new VisemeCode(21, "LL");
			public static readonly VisemeCode M = new VisemeCode(22, "M");
			public static readonly VisemeCode N = new VisemeCode(23, "N");
			public static readonly VisemeCode NX = new VisemeCode(24, "NX");
			public static readonly VisemeCode AO = new VisemeCode(25, "AO");
			public static readonly VisemeCode OY = new VisemeCode(26, "OY");
			public static readonly VisemeCode P = new VisemeCode(27, "P");
			public static readonly VisemeCode R = new VisemeCode(28, "R");
			public static readonly VisemeCode S = new VisemeCode(29, "S");
			public static readonly VisemeCode SH = new VisemeCode(30, "SH");
			public static readonly VisemeCode T = new VisemeCode(31, "T");
			public static readonly VisemeCode CH = new VisemeCode(32, "CH");
			public static readonly VisemeCode UW = new VisemeCode(33, "UW");
			public static readonly VisemeCode UH = new VisemeCode(34, "UH");
			public static readonly VisemeCode V = new VisemeCode(35, "V");
			public static readonly VisemeCode W = new VisemeCode(36, "W");
			public static readonly VisemeCode Z = new VisemeCode(37, "Z");
			public static readonly VisemeCode ZH = new VisemeCode(38, "ZH");
			public static readonly VisemeCode TH = new VisemeCode(39, "TH");
			public static readonly VisemeCode EL = new VisemeCode(40, "EL");
			public static readonly VisemeCode AA_nl = new VisemeCode(41, "AA_nl");
			public static readonly VisemeCode EE_nl = new VisemeCode(42, "EE_nl");
			public static readonly VisemeCode EI_nl = new VisemeCode(43, "EI_nl");
			public static readonly VisemeCode G_nl = new VisemeCode(44, "G_nl");
			public static readonly VisemeCode UI_nl = new VisemeCode(45, "UI_nl");
			public static readonly VisemeCode wordspace = new VisemeCode(46, "Silence");
			public static readonly VisemeCode invalid = new VisemeCode(47, "invalid");

			private static readonly List<VisemeCode> _visemeCodeList = new List<VisemeCode>
			{
				silence,
				AY,
				AW,
				AH,
				AE,
				IH,
				AA,
				B,
				D,
				JH,
				DH,
				EY,
				EH,
				ER,
				OW,
				F,
				G,
				HX,
				IY,
				Y,
				K,
				LL,
				M,
				N,
				NX,
				AO,
				OY,
				P,
				R,
				S,
				SH,
				T,
				CH,
				UW,
				UH,
				V,
				W,
				Z,
				ZH,
				TH,
				EL,
				AA_nl, 
				EE_nl,
				EI_nl,
				G_nl,
				UI_nl,
				wordspace,
				invalid
			};

			private static Dictionary<string, VisemeCode> visemeCodes = null;

			private VisemeCode(int value, string name)
			{
				this.value = value;
				this.name = name;
			}

			public string getName()
			{
				return name;
			}
		}
	}
}