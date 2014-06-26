﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Statistics
{
    public static class Extensions
    {
        #region List extension

        /// <summary>
        /// Adds a storage item into a list and returns the item
        /// </summary>
        /// <param name="list"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static StoredPlayer AddAndReturn(this List<StoredPlayer> list, StoredPlayer player)
        {
            list.Add(player);
            return player;
        }

        public static T AddAndReturn<T>(this List<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        #endregion

        public static string Suffix(this int number)
        {
            return number == 0 || number > 1 ? "s" : "";
        }

        #region Player extensions

        /// <summary>
        /// Saves a player's stats into stored values
        /// </summary>
        /// <param name="player"></param>
        public static void SaveStats(this SPlayer player)
        {
            if (player == null || player.storage == null) return;

            player.storage.totalTime = player.timePlayed;
            player.storage.firstLogin = player.firstLogin;
            player.storage.lastSeen = DateTime.Now.ToString("G");
            player.storage.loginCount = player.loginCount;
            player.storage.knownAccounts = player.knownAccounts;
            player.storage.knownIPs = player.knownIPs;

            player.storage.kills = player.kills;
            player.storage.deaths = player.deaths;
            player.storage.mobkills = player.mobkills;
            player.storage.bosskills = player.bosskills;
            Statistics.database.SaveUser(player.storage);
        }

        /// <summary>
        /// Used on logging in. Syncs a player's stats with stored values matching the player
        /// </summary>
        /// <param name="player"></param>
        public static void SyncStats(this SPlayer player)
        {
            if (player.storage == null) return;

            player.timePlayed = player.storage.totalTime;
            player.firstLogin = player.storage.firstLogin;
            player.lastSeen = DateTime.UtcNow.ToString("G");
            player.loginCount = player.storage.loginCount + 1;
            player.knownAccounts = player.storage.knownAccounts;
            player.knownIPs = player.storage.knownIPs;

            player.kills = player.storage.kills;
            player.deaths = player.storage.deaths;
            player.mobkills = player.storage.mobkills;
            player.bosskills = player.storage.bosskills;
        }

        /// <summary>
        /// Returns the amount of time a player has collected
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string TimePlayed(this SPlayer player)
        {
            double time = player.timePlayed;
            var weeks = Math.Floor(time/604800);
            var days = Math.Floor(((time/604800) - weeks)*7);

            var ts = new TimeSpan(0, 0, 0, (int) time);

            var sb = new StringBuilder();

            if (weeks > 0)
                sb.Append(string.Format("{0} week{1}{2}", weeks, Tools.Suffix((int)weeks),
                    days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 ? ", " : ""));
            if (days > 0)
                sb.Append(string.Format("{0} day{1}{2}", days, Tools.Suffix((int)days),
                    ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 ? ", " : ""));
            if (ts.Hours > 0)
                sb.Append(string.Format("{0} hour{1}{2}", ts.Hours, Tools.Suffix(ts.Hours),
                    ts.Minutes > 0 || ts.Seconds > 0 ? ", " : ""));

            if (ts.Minutes > 0)
                sb.Append(string.Format("{0} minute{1}{2}", ts.Minutes, Tools.Suffix(ts.Minutes),
                    ts.Seconds > 0 ? ", " : ""));

            if (ts.Seconds > 0)
                sb.Append(string.Format("{0} second{1}", ts.Seconds, Tools.Suffix(ts.Seconds)));

            return sb.ToString();
        }


        /// <summary>
        /// Returns the amount of time a stored item has collected
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static string TimePlayed(this StoredPlayer player)
        {
            var time = player.totalTime * 1d;
            var weeks = Math.Floor(time / 604800);
            var days = Math.Floor(((time / 604800) - weeks) * 7);

            var ts = new TimeSpan(0, 0, 0, (int)time);

            var sb = new StringBuilder();

            if (weeks > 0)
                sb.Append(string.Format("{0} week{1}{2}", weeks, Tools.Suffix((int)weeks),
                    days > 0 || ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 ? ", " : ""));
            if (days > 0)
                sb.Append(string.Format("{0} day{1}{2}", days, Tools.Suffix((int)days),
                    ts.Hours > 0 || ts.Minutes > 0 || ts.Seconds > 0 ? ", " : ""));
            if (ts.Hours > 0)
                sb.Append(string.Format("{0} hour{1}{2}", ts.Hours, Tools.Suffix(ts.Hours),
                    ts.Minutes > 0 || ts.Seconds > 0 ? ", " : ""));

            if (ts.Minutes > 0)
                sb.Append(string.Format("{0} minute{1}{2}", ts.Minutes, Tools.Suffix(ts.Minutes),
                    ts.Seconds > 0 ? ", " : ""));

            if (ts.Seconds > 0)
                sb.Append(string.Format("{0} second{1}", ts.Seconds, Tools.Suffix(ts.Seconds)));

            return sb.ToString();
        }

        #endregion

        #region TimeSpan extension

        /// <summary>
        /// Returns the amount of time in a timespan
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string TimeSpanPlayed(this TimeSpan ts)
        {
            var sb = new StringBuilder();
            if (ts.Hours > 0)
                sb.Append(string.Format("{0} hour{1}{2}", ts.Hours, Tools.Suffix(ts.Hours),
                    ts.Minutes > 0 || ts.Seconds > 0 ? ", " : ""));

            if (ts.Minutes > 0)
                sb.Append(string.Format("{0} minute{1}{2}", ts.Minutes, Tools.Suffix(ts.Minutes),
                    ts.Seconds > 0 ? ", " : ""));

            if (ts.Seconds > 0)
                sb.Append(string.Format("{0} second{1}", ts.Seconds, Tools.Suffix(ts.Seconds)));

            return sb.ToString();
        }

        #endregion

        #region HighScore extension

        public static HighScore GetHighScore(this IEnumerable<HighScore> list, string name)
        {
            return list.FirstOrDefault(h => String.Equals(h.name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public static HighScore GetTopScore(this List<HighScore> list)
        {
            return list.Count == 0 ? null : list[0];
        }

        #endregion
    }
}
