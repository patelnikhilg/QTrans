using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace adminlte.Common
{
    public class SessionStorage : ISessionStorage
    {
        /// <summary>
        /// Set Value to Temporary Storage Variable
        /// </summary>
        /// <param name="key">Provide Key</param>
        /// <param name="value">Provide Value</param>
        public void SetValue(string key, object value)
        {            
            HttpContext.Current.Session[key] = value;
        }

        /// <summary>
        /// Get Value to Temporary Storage Variable
        /// </summary>
        /// <param name="key">Provide Key</param>
        /// <returns>string of Temporary Storage Variable</returns>
        public object GetValue(string key)
        {
            return HttpContext.Current.Session[key];
        }

        /// <summary>
        /// Get Value to Temporary Storage Variable
        /// </summary>
        /// <param name="key">Provide Key</param>
        public void RemoveValue(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        /// <summary>
        /// Remove All Value of All Temporary Storage Variable
        /// </summary>
        public void RemoveAll()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// The is session has value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsSessionHasValue(string key)
        {
            return HttpContext.Current.Session[key] != null;
        }
    }
}