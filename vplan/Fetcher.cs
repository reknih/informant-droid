using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;

namespace vplan
{
	class Fetcher
	{
		private WebClient webcl = new WebClient();
		private MainActivity mp;
		private SettingsActivity sp;
		private int Group;
		public Fetcher(MainActivity mainP)
		{
			mp = mainP;
		}
		public Fetcher(SettingsActivity mainP)
		{
			sp = mainP;
		}
		public async void getClasses()
		{
			webcl = new WebClient();
			webcl.DownloadStringCompleted += new DownloadStringCompletedEventHandler(groups_DownloadStringCompleted);
			webcl.DownloadStringAsync(new Uri("http://www.cws-usingen.de/stupla/Schueler/frames/navbar.htm"));
		}
		private void groups_DownloadStringCompleted(object send, DownloadStringCompletedEventArgs e)
		{
			try {
			List<Group> groupObj = new List<Group>();
			if (e.Error != null)
			{
				if (mp != null) {
					mp.toast ("Verbindung fehlgeschlagen");
				} else {
					sp.toast ("Verbindung fehlgeschlagen");
				}
				return;
			}
		    string raw = e.Result.Replace(" ", string.Empty);
				raw = raw.Substring(raw.IndexOf("varclasses=[") + 12);
			raw = raw.Substring(0, raw.IndexOf("];"));
			raw = raw.Replace("\"", string.Empty);
			raw = raw.Replace("\n", string.Empty);
			string[] arr = raw.Split(',');
			int i = 0;
			foreach (var item in arr)
			{
				groupObj.Add(new Group(i, item));
				i++;
			}
			sp.refresh(groupObj);
			} catch {
				if (mp != null) {
					mp.toast ("Da ist was mächtig schiefgegangen.");
				} else {
					sp.toast ("Da ist was mächtig schiefgegangen.");
				}
			}
		}
		public async void getTimes(int group, bool follow)
		{
			try {
			webcl = new WebClient();
			Group = group;
			if (follow == true) {
				webcl.DownloadStringCompleted += new DownloadStringCompletedEventHandler(timesNext_DownloadStringCompleted);
			}
			else
			{
				webcl.DownloadStringCompleted += new DownloadStringCompletedEventHandler(times_DownloadStringCompleted);
			}
			string groupStr = "";
			string weekStr = "";
			if (group < 10)
			{
				groupStr = "w0000" + Convert.ToString(group);
			}
			else if (group < 100)
			{
				groupStr = "w000" + Convert.ToString(group);
			}
			else if (group < 1000)
			{
				groupStr = "w00" + Convert.ToString(group);
			}
			else if (group < 10000)
			{
				groupStr = "w0" + Convert.ToString(group);
			}
			else if (group < 100000)
			{
				groupStr = "w" + Convert.ToString(group);
			}
			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			Calendar cal = dfi.Calendar;
			int week = cal.GetWeekOfYear(DateTime.Now, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
			if (follow == true)
				week++;
			if (week < 10)
			{
				weekStr = "0" + Convert.ToString(week);
			}
			else if (week < 100)
			{
					weekStr = Convert.ToString(week);
			}
			webcl.DownloadStringAsync(new Uri("http://www.cws-usingen.de/stupla/Schueler/" + weekStr + "/w/" + groupStr + ".htm"));
			} catch {
				if (mp != null) {
					mp.toast ("Da ist was mächtig schiefgegangen.");
				} else {
					sp.toast ("Da ist was mächtig schiefgegangen.");
				}
			}
		}
		private void times_DownloadStringCompleted(object send, DownloadStringCompletedEventArgs e)
		{
			if (e.Error != null) {
				if (mp != null) {
					mp.toast ("Verbindung fehlgeschlagen");
				} else {
					sp.toast ("Verbindung fehlgeschlagen");
				}
				return;
			}
			//TO-DO: Parse VPlan
			string[] raw = new string[] { "", "", "", "", "" };
			string comp = e.Result;
			comp = comp.Replace (" ", string.Empty);
			for (int i = 0; i < 5; i++)
			{
				raw[i] = comp.Substring(comp.IndexOf("<table"), comp.IndexOf("</table>") - comp.IndexOf("<table") + 8);
				comp = comp.Substring(comp.IndexOf("</table>") + 8);
			}
			List<Data> v1 = new List<Data>();

			int iOuter = 0;
			int daysRec = 0;
			bool doNot = false;
			foreach (var item in raw)
			{
				string it = item;
				if (item.IndexOf("freigegeben") == -1)
				{
					daysRec++;
					it = item.Replace("&nbsp;", String.Empty);
					Regex regex = new Regex("<t{1}d{1}.*?>.*?</td>");
					MatchCollection mc;
					if (item.IndexOf("KeineVertretungen") == -1)
					{
						int i = 0;
						while (it.IndexOf("<trclass='list") != -1)
						{
							if (i == 0)
								it = it.Substring(it.IndexOf("</tr>") + 5, it.Length - it.IndexOf("</tr>") - 5);
							string w;
							Data data = new Data();
							w = it.Substring(it.IndexOf("<trclass='list"));
							w = w.Substring(0, w.IndexOf("</tr>"));
							it = it.Substring(it.IndexOf("</tr>") + 5, it.Length - it.IndexOf("</tr>") - 5);
							mc = regex.Matches(w);
							int iInner = 0;
							foreach (var thing in mc)
							{
								string thingy = thing.ToString();
								thingy = thingy.Substring(thingy.IndexOf(">") + 1, thingy.Length - thingy.IndexOf(">") - 1);
								thingy = thingy.Substring(0, thingy.IndexOf("<"));
								switch (iInner)
								{
								case 0:
									if (thingy == "Veranst.")
									{ data.Veranstaltung = true; }
									break;
								case 1:
									if (i == 0)
									{
										int day = Convert.ToInt16(thingy.Substring(0, thingy.IndexOf(".")));
										string dayStr = thingy.Substring(thingy.IndexOf(".") + 1);
										dayStr = dayStr.Replace(".", string.Empty);
										int month = Convert.ToInt16(dayStr);
										int year = DateTime.Now.Year;
										DateTime dt = new DateTime(year, month, day);
										v1.Add(new Data(dt));
									}
									break;
								case 2:
									data.Stunde = thingy;
									break;
								case 3:
									data.Vertreter = thingy;
									break;
								case 4:
									data.Fach = thingy;
									break;
								case 5:
									data.AltFach = thingy;
									break;
								case 6:
									data.Raum = thingy;
									break;
								case 7:
									data.Klasse = thingy;
									break;
								case 8:
									data.Lehrer = thingy;
									break;
								case 13:
									data.Notiz = thingy;
									break;
								case 14:
									data.EntfallStr = thingy;
									break;
								case 15:
									data.MitbeStr = thingy;
									break;
								}
								iInner++;
							}
							data.refresh();
							v1.Add(data);
							i++;
						}
					}
				}
				iOuter++;
				if (iOuter == 5 && daysRec == 1)
				{
					getTimes(Group, true);
					mp.clear ();
					mp.refresh(v1);
					doNot = true;
				}
			}
			if (doNot != true) {
				mp.refresh (v1);
			}
		}
		private void timesNext_DownloadStringCompleted(object send, DownloadStringCompletedEventArgs e)
		{
			try {
			if (e.Error != null)
				return;
			//TO-DO: Parse VPlan
			string[] raw = new string[] { "", "", "", "", "" };
			string comp = e.Result;
			comp = comp.Replace(" ", string.Empty);
			for (int i = 0; i < 5; i++)
			{
				raw[i] = comp.Substring(comp.IndexOf("<table"), comp.IndexOf("</table>") - comp.IndexOf("<table") + 8);
				comp = comp.Substring(comp.IndexOf("</table>") + 8);
			}
			int iOuter = 0;
			int daysRec = 0;
			foreach (var item in raw)
			{
				string it = item;
				if (item.IndexOf("freigegeben") == -1 && daysRec == 0)
				{
					daysRec++;
					it = item.Replace("&nbsp;", String.Empty);
					Regex regex = new Regex("<t{1}d{1}.*?>.*?</td>");
					MatchCollection mc;
					if (item.IndexOf("KeineVertretungen") == -1)
					{
						int i = 0;
						while (it.IndexOf("<trclass='list") != -1)
						{
							if (i == 0)
								it = it.Substring(it.IndexOf("</tr>") + 5, it.Length - it.IndexOf("</tr>") - 5);
							string w;
							Data data = new Data();
							w = it.Substring(it.IndexOf("<trclass='list"));
							w = w.Substring(0, w.IndexOf("</tr>"));
							it = it.Substring(it.IndexOf("</tr>") + 5, it.Length - it.IndexOf("</tr>") - 5);
							mc = regex.Matches(w);
							int iInner = 0;
							foreach (var thing in mc)
							{
								string thingy = thing.ToString();
								thingy = thingy.Substring(thingy.IndexOf(">") + 1, thingy.Length - thingy.IndexOf(">") - 1);
								thingy = thingy.Substring(0, thingy.IndexOf("<"));
								switch (iInner)
								{
								case 0:
									if (thingy == "Veranst.")
									{ data.Veranstaltung = true; }
									break;
								case 1:
									if (i == 0)
									{
										int day = Convert.ToInt16(thingy.Substring(0, thingy.IndexOf(".")));
										string dayStr = thingy.Substring(thingy.IndexOf(".") + 1);
										dayStr = dayStr.Replace(".", string.Empty);
										int month = Convert.ToInt16(dayStr);
										int year = DateTime.Now.Year;
										DateTime dt = new DateTime(year, month, day);
										mp.add(new Data(dt));
									}
									break;
								case 2:
									data.Stunde = thingy;
									break;
								case 3:
									data.Vertreter = thingy;
									break;
								case 4:
									data.Fach = thingy;
									break;
								case 5:
									data.AltFach = thingy;
									break;
								case 6:
									data.Raum = thingy;
									break;
								case 7:
									data.Klasse = thingy;
									break;
								case 8:
									data.Lehrer = thingy;
									break;
								case 13:
									data.Notiz = thingy;
									break;
								case 14:
									data.EntfallStr = thingy;
									break;
								case 15:
									data.MitbeStr = thingy;
									break;
								default:
									break;
								}
								iInner++;
							}
							data.refresh();
							mp.add(data);
							i++;
						}
					}
				}
				iOuter++;
			}
			} catch {
				if (mp != null) {
					mp.toast ("Da ist was mächtig schiefgegangen.");
				} else {
					sp.toast ("Da ist was mächtig schiefgegangen.");
				}
			}
		}
	}
}
