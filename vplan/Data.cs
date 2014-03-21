using System;
using System.Text;
using System.Globalization;

namespace vplan
{
	public class Data
	{
		public Data()
		{
			Line1 = "Keine Vertretungen.";
			Line2 = "";
			Veranstaltung = false;
			Head = true;
		}
		public Data(DateTime date)
		{
			Line1 = date.ToString("dddd", new System.Globalization.CultureInfo("de-DE")) + ", " + Convert.ToString(date.Day) + "." + Convert.ToString(date.Month);
			Line2 = "";
			Head = true;
		}
		public void refresh()
		{
			Head = false;
			Fach = faecherSchreib(Fach);
			AltFach = faecherSchreib(AltFach);
			if (EntfallStr != "" && EntfallStr != null)
			{
				Entfall = true;
			}
			else
			{
				Entfall = false;
			}
			if (MitbeStr != "" && EntfallStr != null)
			{
				Mitbetreung = true;
			}
			else
			{
				Mitbetreung = false;
			}
			Line1 = Stunde + ". Std: " + Fach;

			if (Entfall == true)
			{
				Line1 = Stunde + ". Std: " + AltFach;
				Line2 = AltFach + " bei " + Lehrer + " entfällt. ";
			}
			else if (Mitbetreung == true)
			{
				Line2 = Fach + " wird durch " + Vertreter + " mitbetreut. | " + Raum;
			}
			else if (Veranstaltung == true)
			{
				Line1 = Stunde + ". Std: Veranstaltung";
				if (Raum != "")
				{
					Line2 = "Raum: " + Raum + " | " + "Mit " + Lehrer + " und " + Klasse;
				}
				else {
					Line2 = "Mit " + Lehrer + " und " + Klasse;
				}
			}
			else if (Fach != AltFach)
			{
				Line2 = Fach + " bei " + Vertreter + " statt " + AltFach;
			}
			else if (Vertreter != Lehrer)
			{
				if (Vertreter == "selbst") {
					Line2 = "Selbstbeschäftgung statt " + Lehrer + " | " + Raum;
				} else {
					Line2 = Vertreter + " vertritt " + Lehrer + " | " + Raum;
				}
			}
			else
			{
				Line2 = "Raum: " + Raum + " | " + Lehrer;
			}
			if (Notiz != "")
			{
				Line2 = Notiz + "; " + Line2;
			}
		}
		public Data(string std, string fach, string lehr, string vertr, string raum, string notiz)
		{
			Fach = fach;
			Lehrer = lehr;
			Vertreter = vertr;
			Stunde = std;
			Raum = raum;
			Notiz = notiz;
			refresh();
		}
		private string faecherSchreib(string fach)
		{
			if (fach != null)
				fach = fach.ToUpper();
			switch (fach)
			{
			case "DE":
				fach = "Deutsch";
				break;
			case "EN":
				fach = "Englisch";
				break;
			case "EN2":
				fach = "Englisch";
				break;
			case "ET":
				fach = "Ethik";
				break;
			case "ER":
				fach = "Ev. Religion";
				break;
			case "KR":
				fach = "Kath. Religion";
				break;
			case "MA":
				fach = "Mathe";
				break;
			case "BI":
				fach = "Bio";
				break;
			case "CH":
				fach = "Chemie";
				break;
			case "PH":
				fach = "Physik";
				break;
			case "POWI":
				fach = "PoWi";
				break;
			case "GE":
				fach = "Geschichte";
				break;
			case "MU":
				fach = "Musik";
				break;
			case "SPA4":
				fach = "Spanisch";
				break;
			case "SPA3":
				fach = "Spanisch";
				break;
			case "FR":
				fach = "Französich";
				break;
			case "FR2":
				fach = "Französich";
				break;
			case "SP":
				fach = "Spanisch";
				break;
			case "SP3":
				fach = "Spanisch";
				break;
			case "DS":
				fach = "Darstellendes Spiel";
				break;
			case "KU":
				fach = "Kunst";
				break;
			case "INFO":
				fach = "Informatik";
				break;
			case "LA":
				fach = "Latein";
				break;
			case "LA2":
				fach = "Latein";
				break;
			}
			return fach;
		}
		public string Fach { get; set; }
		public string AltFach { get; set; }

		public string Stunde { get; set; }

		public string Lehrer { get; set; }

		public string Vertreter { get; set; }

		public string Notiz { get; set; }

		public string Raum { get; set; }
		public string EntfallStr { get; set; }
		public bool Entfall { get; set; }
		public string MitbeStr { get; set; }
		public bool Mitbetreung { get; set; }
		public bool Veranstaltung { get; set; }

		public string Line1 { get; set; }

		public string Klasse { get; set; }

		public string Line2 { get; set; }

		public DateTime Date { get; set; }

		public bool Head { get; set; }
	}
}
