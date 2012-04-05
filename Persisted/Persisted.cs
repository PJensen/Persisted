
/// Extends Object for quick, generic programmed, serialization.
/// Copyright (C) 2011  Peter Jensen
/// 
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
/// 
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU General Public License for more details.
/// 
/// You should have received a copy of the GNU General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;

namespace System.Xml.Serialization.Persisted
{
    #region Various supported interfaces may reside here.


    #endregion

    /// <summary>
    /// Extension method that rides on top of all obects.
    /// </summary>
    public static class Persisted
    {
        /// <summary>
        /// Read a templated type from a fully qualified file-path; return the deserialized object.
        /// </summary>
        /// <typeparam name="T">The templated type that willl be returned.</typeparam>
        /// <param name="strFullPath">The fully qualified file path that will be deserialized.</param>
        /// <returns>The result of deserialize, casted as <c>T</c></returns>
        /// <exception cref="IOException">{strFullPath} not found</exception>
        public static T Read<T>(this string strFullPath)
        {
            if (string.Empty.Equals(strFullPath ?? string.Empty))
                throw new ArgumentException("strFullPath cannot be null or empty");
            if (!File.Exists(strFullPath))
                throw new IOException(string.Format("{0} not found", strFullPath));
            using (Stream fs = new FileStream(strFullPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
        }

        /// <summary>
        /// Write the templated type (aObj) <c>to</c> the fully qualified file-path.  
        /// <remarks>If the path <c>does not</c> exist, the full-directory-tree <c>will</c> be created.</remarks>
        /// </summary>
        /// <typeparam name="T">The templated type that willl be written to disk.</typeparam>
        /// <param name="aObj">The actual object that is written to disk.</param>
        /// <param name="strFullPath">The fully qualified file-path.
        /// <example>c:\temp\out\1.xml</example>
        /// <example>.\out\2.xml</example>
        /// <example>3.xml</example>
        /// </param>
        /// <returns>true when the operation was a success.</returns>
        public static bool Write<T>(this T aObj, string strFullPath)
        {
            try
            {
                string strDirectory = Path.GetDirectoryName(strFullPath);
                if (!Directory.Exists(strDirectory) && !string.Empty.Equals(strDirectory))
                    Directory.CreateDirectory(strDirectory);
                using (FileStream tmpRawStream = File.Open(strFullPath, FileMode.OpenOrCreate))
                using (XmlTextWriter tmpXmlWriter = new XmlTextWriter(tmpRawStream, new System.Text.UTF8Encoding()))
                    new XmlSerializer(aObj.GetType()).Serialize(tmpXmlWriter, aObj);
                return true;
            }
            catch { return false; }
        }
    }
}
