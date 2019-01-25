using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adminlte.Common
{
    /// <summary>
    /// Defines interface for Temporary Storage Session/Cookie Variable
    /// </summary>
    public interface ISessionStorage
    {
        /// <summary>
        /// Set Value to Temporary Storage Variable
        /// </summary>
        /// <param name="key">Provide Key</param>
        /// <param name="value">Provide Value</param>
        void SetValue(string key, object value);

        /// <summary>
        /// Get Value to Temporary Storage Variable
        /// </summary>
        /// <param name="key">Provide Key</param>
        /// <returns>string of Temporary Storage Variable</returns>
        object GetValue(string key);

        /// <summary>
        /// Remove Value of All Temporary Storage Variable
        /// </summary>
        /// <param name="key">Provide Key</param>
        void RemoveValue(string key);

        /// <summary>
        /// Remove All Value of All Temporary Storage Variable
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// The is session has value.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsSessionHasValue(string key);
    }
}
