using System.Collections.Generic;
using UnityEngine;

namespace Models
{
	public class Phoneme
	{
		private static readonly Dictionary<PhonemeCode, Viseme.VisemeCode> phonemeVisemeMapping;
		private readonly PhonemeCode phonemeCode;

		static Phoneme()
		{
			phonemeVisemeMapping = new Dictionary<PhonemeCode, Viseme.VisemeCode>();
			mapPhonemesToVisemes(phonemeVisemeMapping);
		}
		
		public Phoneme(PhonemeCode phonemeCode)
		{
			this.phonemeCode = phonemeCode;
		}

		public Viseme toViseme()
		{
			Debug.Log("Searching for code of " + phonemeCode.getName());
			Viseme.VisemeCode visemeCode = phonemeVisemeMapping[phonemeCode];
			Viseme result = new Viseme(visemeCode);
			return result;
		}

		public static List<Viseme> toVisemes(List<Phoneme> phonemes)
		{
			List<Viseme> result = new List<Viseme>();
			for (int i = 0; i < phonemes.Count; i++)
			{
				result.Add(phonemes[i].toViseme());
			}

			return result;
		}

		public PhonemeCode getPhonemeCode()
		{
			return phonemeCode;
		}

		public static Phoneme getPhonemeFromCode(string phonemeCode)
		{
			return new Phoneme(PhonemeCode.getPhonemeCode(phonemeCode));
		}
		
		public static List<Phoneme> getPhonemeFromCode(IEnumerable<string> phonemeCodes)
		{
			var result = new List<Phoneme>();
			foreach (string phonemeCode in phonemeCodes)
			{
				result.Add(getPhonemeFromCode(phonemeCode));
			}
			return result;
		}

		private static void mapPhonemesToVisemes(IDictionary<PhonemeCode, Viseme.VisemeCode> phonemeMapping)
		{
			phonemeMapping.Add(PhonemeCode.AY, Viseme.VisemeCode.AY);
			phonemeMapping.Add(PhonemeCode.AW, Viseme.VisemeCode.AW);
			phonemeMapping.Add(PhonemeCode.AH, Viseme.VisemeCode.AH);
			phonemeMapping.Add(PhonemeCode.AE, Viseme.VisemeCode.AE);
			phonemeMapping.Add(PhonemeCode.IH, Viseme.VisemeCode.IH);
			phonemeMapping.Add(PhonemeCode.AA, Viseme.VisemeCode.AA);
			phonemeMapping.Add(PhonemeCode.AX, Viseme.VisemeCode.AH);
			phonemeMapping.Add(PhonemeCode.B, Viseme.VisemeCode.B);
			phonemeMapping.Add(PhonemeCode.D, Viseme.VisemeCode.D);
			phonemeMapping.Add(PhonemeCode.JH, Viseme.VisemeCode.JH);
			phonemeMapping.Add(PhonemeCode.DH, Viseme.VisemeCode.DH);
			phonemeMapping.Add(PhonemeCode.DX, Viseme.VisemeCode.T);
			phonemeMapping.Add(PhonemeCode.EY, Viseme.VisemeCode.EY);
			phonemeMapping.Add(PhonemeCode.EH, Viseme.VisemeCode.EH);
			phonemeMapping.Add(PhonemeCode.ER, Viseme.VisemeCode.ER);
			phonemeMapping.Add(PhonemeCode.OW, Viseme.VisemeCode.OW);
			phonemeMapping.Add(PhonemeCode.F, Viseme.VisemeCode.F);
			phonemeMapping.Add(PhonemeCode.G, Viseme.VisemeCode.G);
			phonemeMapping.Add(PhonemeCode.HH, Viseme.VisemeCode.HX);
			phonemeMapping.Add(PhonemeCode.IX, Viseme.VisemeCode.IH);
			phonemeMapping.Add(PhonemeCode.IY, Viseme.VisemeCode.IY);
			phonemeMapping.Add(PhonemeCode.Y, Viseme.VisemeCode.Y);
			phonemeMapping.Add(PhonemeCode.K, Viseme.VisemeCode.K);
			phonemeMapping.Add(PhonemeCode.L, Viseme.VisemeCode.LL);
			phonemeMapping.Add(PhonemeCode.M, Viseme.VisemeCode.M);
			phonemeMapping.Add(PhonemeCode.N, Viseme.VisemeCode.N);
			phonemeMapping.Add(PhonemeCode.NG, Viseme.VisemeCode.NX);
			phonemeMapping.Add(PhonemeCode.AO, Viseme.VisemeCode.AO);
			phonemeMapping.Add(PhonemeCode.OY, Viseme.VisemeCode.OY);
			phonemeMapping.Add(PhonemeCode.P, Viseme.VisemeCode.P);
			phonemeMapping.Add(PhonemeCode.R, Viseme.VisemeCode.R);
			phonemeMapping.Add(PhonemeCode.S, Viseme.VisemeCode.S);
			phonemeMapping.Add(PhonemeCode.SH, Viseme.VisemeCode.SH);
			phonemeMapping.Add(PhonemeCode.T, Viseme.VisemeCode.T);
			phonemeMapping.Add(PhonemeCode.CH, Viseme.VisemeCode.CH);
			phonemeMapping.Add(PhonemeCode.UW, Viseme.VisemeCode.UW);
			phonemeMapping.Add(PhonemeCode.UH, Viseme.VisemeCode.UH);
			phonemeMapping.Add(PhonemeCode.V, Viseme.VisemeCode.V);
			phonemeMapping.Add(PhonemeCode.W, Viseme.VisemeCode.W);
			phonemeMapping.Add(PhonemeCode.Z, Viseme.VisemeCode.Z);
			phonemeMapping.Add(PhonemeCode.ZH, Viseme.VisemeCode.ZH);
			phonemeMapping.Add(PhonemeCode.TH, Viseme.VisemeCode.TH);
			phonemeMapping.Add(PhonemeCode.EL, Viseme.VisemeCode.EL);
			phonemeMapping.Add(PhonemeCode.AA_nl, Viseme.VisemeCode.AA_nl);
			phonemeMapping.Add(PhonemeCode.EE_nl, Viseme.VisemeCode.EE_nl);
			phonemeMapping.Add(PhonemeCode.EI_nl, Viseme.VisemeCode.EI_nl);
			phonemeMapping.Add(PhonemeCode.G_nl, Viseme.VisemeCode.G_nl);
			phonemeMapping.Add(PhonemeCode.UI_nl, Viseme.VisemeCode.UI_nl);
			phonemeMapping.Add(PhonemeCode.wordspace, Viseme.VisemeCode.wordspace);
		}
		
		/// <summary>
		/// Type-safe enum pattern for phoneme codes<br/>
		/// See <a href="http://www.javacamp.org/designPattern/enum.html">Typesafe Enum</a>
		/// </summary>
		public sealed class PhonemeCode
		{
			private readonly int value;
			private readonly string name;

			public static readonly PhonemeCode AA = new PhonemeCode(1, "AA");
			public static readonly PhonemeCode AE = new PhonemeCode(2, "AE");
			public static readonly PhonemeCode AH = new PhonemeCode(3, "AH");
			public static readonly PhonemeCode AO = new PhonemeCode(4, "AO");
			public static readonly PhonemeCode AW = new PhonemeCode(5, "AW");
			public static readonly PhonemeCode AX = new PhonemeCode(6, "AX");
			public static readonly PhonemeCode AY = new PhonemeCode(7, "AY");
			public static readonly PhonemeCode EH = new PhonemeCode(8, "EH");
			public static readonly PhonemeCode ER = new PhonemeCode(9, "ER");
			public static readonly PhonemeCode EY = new PhonemeCode(10, "EY");
			public static readonly PhonemeCode IH = new PhonemeCode(11, "IH");
			public static readonly PhonemeCode IX = new PhonemeCode(12, "IX");
			public static readonly PhonemeCode IY = new PhonemeCode(13, "IY");
			public static readonly PhonemeCode OW = new PhonemeCode(14, "OW");
			public static readonly PhonemeCode OY = new PhonemeCode(15, "OY");
			public static readonly PhonemeCode UH = new PhonemeCode(16, "UH");
			public static readonly PhonemeCode UW = new PhonemeCode(17, "UW");
			public static readonly PhonemeCode B = new PhonemeCode(18, "B");
			public static readonly PhonemeCode CH = new PhonemeCode(19, "CH");
			public static readonly PhonemeCode D = new PhonemeCode(20, "D");
			public static readonly PhonemeCode DH = new PhonemeCode(21, "DH");
			public static readonly PhonemeCode DX = new PhonemeCode(22, "DX");
			public static readonly PhonemeCode EL = new PhonemeCode(23, "EL");
			public static readonly PhonemeCode EN = new PhonemeCode(24, "EN");
			public static readonly PhonemeCode F = new PhonemeCode(25, "F");
			public static readonly PhonemeCode G = new PhonemeCode(26, "G");
			public static readonly PhonemeCode HH = new PhonemeCode(27, "HH");
			public static readonly PhonemeCode JH = new PhonemeCode(28, "JH");
			public static readonly PhonemeCode K = new PhonemeCode(29, "K");
			public static readonly PhonemeCode L = new PhonemeCode(30, "L");
			public static readonly PhonemeCode M = new PhonemeCode(31, "M");
			public static readonly PhonemeCode N = new PhonemeCode(32, "N");
			public static readonly PhonemeCode NG = new PhonemeCode(33, "NG");
			public static readonly PhonemeCode P = new PhonemeCode(34, "P");
			public static readonly PhonemeCode R = new PhonemeCode(35, "R");
			public static readonly PhonemeCode S = new PhonemeCode(36, "S");
			public static readonly PhonemeCode SH = new PhonemeCode(37, "SH");
			public static readonly PhonemeCode T = new PhonemeCode(38, "T");
			public static readonly PhonemeCode TH = new PhonemeCode(39, "TH");
			public static readonly PhonemeCode V = new PhonemeCode(40, "V");
			public static readonly PhonemeCode W = new PhonemeCode(41, "W");
			public static readonly PhonemeCode Y = new PhonemeCode(42, "Y");
			public static readonly PhonemeCode Z = new PhonemeCode(43, "Z");
			public static readonly PhonemeCode ZH = new PhonemeCode(44, "ZH");
			public static readonly PhonemeCode wordspace = new PhonemeCode(45, "/");
			public static readonly PhonemeCode invalid = new PhonemeCode(46, "invalid");
			public static readonly PhonemeCode G_nl = new PhonemeCode(47, "G_nl");
			public static readonly PhonemeCode EE_nl = new PhonemeCode(51, "EE_nl");
			public static readonly PhonemeCode AA_nl = new PhonemeCode(52, "AA_nl");
			public static readonly PhonemeCode EI_nl = new PhonemeCode(53, "EI_nl");
			public static readonly PhonemeCode UI_nl = new PhonemeCode(54, "UI_nl");

			private static readonly List<PhonemeCode> _phonemeCodeList = new List<PhonemeCode>
			{
				AA,
				AE,
				AH,
				AO,
				AW,
				AX,
				AY,
				EH,
				ER,
				EY,
				IH,
				IX,
				IY,
				OW,
				OY,
				UH,
				UW,
				B,
				CH,
				D,
				DH,
				DX,
				EL,
				EN,
				F,
				G,
				HH,
				JH,
				K,
				L,
				M,
				N,
				NG,
				P,
				R,
				S,
				SH,
				T,
				TH,
				V,
				W,
				Y,
				Z,
				ZH,
				G_nl,
				EE_nl,
				AA_nl,
				EI_nl,
				UI_nl,
				wordspace,
				invalid
			};

			private static Dictionary<string, PhonemeCode> _phonemeCodes = null;

			private PhonemeCode(int value, string name)
			{
				this.value = value;
				this.name = name;
			}

			public string getName()
			{
				return name;
			}

			public static PhonemeCode getPhonemeCode(string code)
			{
				if (_phonemeCodes == null)
				{
					_phonemeCodes = new Dictionary<string, PhonemeCode>();
					for (int i = 0; i < _phonemeCodeList.Count; i++)
					{
						PhonemeCode phonemeCode = _phonemeCodeList[i];
						_phonemeCodes.Add(phonemeCode.name, phonemeCode);
					}
				}

				Debug.Log("Searching for phoneme : " + code);
				
				return _phonemeCodes[code];
			}
		}
	}
}