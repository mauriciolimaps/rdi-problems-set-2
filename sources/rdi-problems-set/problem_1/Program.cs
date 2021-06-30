using System;
using System.Text;


namespace conversion
{
	class Conversion
	{
		private static string HundredExpanded(decimal value)
		{
			StringBuilder strings;
			string[,] expansions = new string[4, 9] {
				{ "cento", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos" },
				{ "",      "vinte",    "trinta",    "quarenta",     "cinquenta",  "sessenta",   "setenta",    "oitenta",    "noventa"    },
				{ "onze",  "doze",     "treze",     "catorze",      "quinze",     "dezesseis",  "dezessete",  "dezoito",    "dezenove"   },
				{ "um",    "dois",     "três",      "quatro",       "cinco",      "seis",       "sete",       "oito",       "nove" }
			};

			if (value > 999)
				throw new Exception("The parameter should be less than 1000");

			int hundreds = Decimal.ToInt32(value / 100);
			int dozens = Decimal.ToInt32(value - hundreds * 100) / 10;
			int units = Decimal.ToInt32(value - hundreds * 100 - dozens * 10);

			StringBuilder result = new StringBuilder();

			if (hundreds > 0)
				result.Append(expansions[0, hundreds - 1]);

			if (dozens > 0)
			{
				if (result.Length > 0)
					result.Append(" e ");

				result.Append(dozens > 1 ? expansions[1, dozens - 1] : expansions[2, units - 1]);
			}

			if ((units > 0) && (dozens != 1))
			{
				if (result.Length > 0)
					result.Append(" e ");

				result.Append(expansions[3, units - 1]);
			}

			return result.ToString();
		}

		public static string Expanded(decimal value)
		{
			decimal divisor;
			decimal remainder;
			int[] milleages = { 0, 0, 0, 0 };
			int index;

			index = 3;
			divisor = 1_000_000_000;
			remainder = value;
			while (index >= 0)
			{
				milleages[index] = Decimal.ToInt32(remainder / divisor);
				remainder -= milleages[index] * divisor;
				divisor /= 1_000;
				index--;
			}

			StringBuilder result = new StringBuilder();

			if (milleages[3] > 0)
			{
				result.Append(HundredExpanded(milleages[3]));
				result.Append(milleages[3] > 1 ? " bilhões " : " bilhão ");
			}

			if (milleages[2] > 0)
			{
				result.Append(HundredExpanded(milleages[2]));
				result.Append(milleages[2] > 1 ? " milhões " : " milhão ");
			}

			if (milleages[1] > 0)
			{
				result.Append(HundredExpanded(milleages[1]));
				result.Append(" mil ");
			}

			if (milleages[0] > 0)
			{
				if (result.Length > 0)
					result.Append("e ");

				result.Append(HundredExpanded(milleages[0]));
			}

			return result.ToString();
		}
	}


	class Program
	{
		static void convertAmount2Words(int m, int n)
		{
			if ( ( m == 0) && (n == 0) )
			{
				Console.WriteLine("0 reais");
				return;
			}

			if ( m > 0 )
			{
				Console.Write(Conversion.Expanded(m));
				Console.Write( (m > 1) ? " reais" : " real");

				if (n > 0)
					Console.Write(" e ");
			}

			if ( n > 0 )
			{
				Console.Write("{0} centavo", Conversion.Expanded(n));
				if (n > 1)
					Console.Write("s");
			}

			Console.WriteLine();
		}


		static void Print(decimal value)
		{
			string result = Conversion.Expanded(value);
			Console.WriteLine(result);
		}


		static void Tests()
		{
			Print(      3_234_123M );
			Print(             13M );
			Print(              4M );
			Print( 12_345_678_901M );
			Print(      1_000_003M );
		}


		static int Main(string[] args)
		{
			int value;
			int cents;

			try
			{
				value = Convert.ToInt32(Console.ReadLine());
				cents = Convert.ToInt32(Console.ReadLine());

				convertAmount2Words(value, cents);
			}
			catch (Exception exception)
			{
				Console.WriteLine("EXCEPTION: {0}", exception.Message);
				return 1;
			}

			return 0;
		}
	}
}
