using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PotionMaster
{
    public class GameDateTime
    {
        private float counter;

        private static readonly float millisecondsToAdvanceMinute = 700;
        private static readonly int daysInMonth = 28;
        private static readonly int seasonsInYear = 4;
        public static readonly string[] DaysOfWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        public static readonly string[] Seasons = { "Winter", "Spring", "Summer", "Fall" };

        public int Minute { get; set; }
        public int Hour { get; set; }
        public int Date { get; set; }
        public int Season { get; set; }
        public int Year { get; set; }
        public int DaysSinceStart { get; set; }
        public string AMPM { get; set; }

        public GameDateTime()
        {
            counter = 0;
            Minute = 0;
            Hour = 6;
            Date = 1;
            Season = 1;
            Year = 1;
            AMPM = "AM";
            DaysSinceStart = Date + ((Season - 1) * daysInMonth) + ((Year - 1) * daysInMonth * seasonsInYear);
        }

        public string GetCurrentMinute()
        {
            if (Minute < 10)
                return "0" + Minute.ToString();
            return Minute.ToString();
        }

        public string GetCurrentHour()
        {
            if (Hour < 10)
                return "0" + Hour.ToString();
            return Hour.ToString();
        }

        public string GetCurrentDayOfWeek()
        {
            int d = Date % 7;
            return DaysOfWeek[d];
        }

        public string GetCurrentSeason()
        {
            int s = Season % seasonsInYear;
            return Seasons[s];
        }

        public void AdvanceSeason()
        {
            Season += 1;
            if (Season > seasonsInYear)
            {
                Season = 1;
                Year += 1;
            }
        }

        public void AdvanceDay()
        {
            Date += 1;
            DaysSinceStart += 1;
            if (Date > daysInMonth)
            {
                AdvanceSeason();
                Date = 1;
            }
        }

        public void AdvanceHour()
        {
            Hour += 1;
            if (Hour >= 13)
            {
                Hour = 1;
            }
            else if (Hour == 12)
            {
                if (AMPM == "AM")
                    AMPM = "PM";
                else
                    AMPM = "AM";
            }
        }

        public void AdvanceMinute()
        {
            Minute += 1;
            if (Minute >= 60)
            {
                Minute = 0;
                AdvanceHour();
            }
        }

        public void Update()
        {
            counter += Game1.dt;
            if (counter >= millisecondsToAdvanceMinute)
            {
                counter -= millisecondsToAdvanceMinute;
                AdvanceMinute();
            }
        }

        public void DrawHud()
        {
            Game1.spriteBatch.DrawString(Game1.font, GetCurrentSeason() + " - " + GetCurrentDayOfWeek() + " " + Date.ToString(), new Vector2(0, 0), Color.Aquamarine);
            Game1.spriteBatch.DrawString(Game1.font, GetCurrentHour() + ":" + GetCurrentMinute() + " " + AMPM, new Vector2(0, Game1.tileSize), Color.Bisque);
        }
    }
}
