using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Helpers.Session
{
    public static class SessionHelper
    {
        /// <summary>
        /// Add Session
        /// </summary>
        /// <param name="strSessionName">Session Name</param>
        /// <param name="strValue">Session Value</param>
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 20;
        }

        /// <summary>
        /// Add Mutile value to session
        /// </summary>
        /// <param name="strSessionName">Session Name</param>
        /// <param name="strValues">Session Value</param>
        public static void Adds(string strSessionName, string[] strValues)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = 20;
        }

        /// <summary>
        /// Add Session with expire time
        /// </summary>
        /// <param name="strSessionName">Session Name</param>
        /// <param name="strValue">Session Value</param>
        /// <param name="iExpires">Expire Time</param>
        public static void Add(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        /// Add Mutile value to session with time
        /// </summary>
        /// <param name="strSessionName">Session Name</param>
        /// <param name="strValues">Session Value</param>
        /// <param name="iExpires">Expire Time</param>
        public static void Adds(string strSessionName, string[] strValues, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValues;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        /// Get session name
        /// </summary>
        /// <param name="strSessionName">Session Name</param>
        /// <returns>Session Name</returns>
        public static string Get(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName].ToString();
            }
        }

        /// <summary>
        /// Get Session String Array
        /// </summary>
        /// <param name="strSessionName">Session Names</param>
        /// <returns>Session Name</returns>
        public static string[] Gets(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return (string[])HttpContext.Current.Session[strSessionName];
            }
        }

        /// <summary>
        /// Delete Session
        /// </summary>
        /// <param name="strSessionName">Session Name</param>
        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
    }
}
