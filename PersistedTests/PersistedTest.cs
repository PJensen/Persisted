
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
/// 
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization.Persisted;
using System.IO;

namespace PersistedTests
{
    /// <summary>
    ///This is a test class for PersistedTest and is intended
    ///to contain all PersistedTest Unit Tests
    ///</summary>
    [TestClass]
    public class PersistedTest
    {
        /// <summary>
        /// Random
        /// </summary>
        static readonly Random Random = new Random();

        /// <summary>
        /// Creates a new instance of PersistedTest
        /// </summary>
        public PersistedTest() : base() { }

        /// <summary>
        /// ParameterHelper
        /// </summary>
        [Serializable]
        public class ParameterHelper
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            protected bool Equals(ParameterHelper other)
            {
                return IntData == other.IntData && Guid.Equals(other.Guid) && DoubleData.Equals(other.DoubleData);
            }

            /// <summary>
            /// GetHashCode
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = IntData;
                    hashCode = (hashCode * 397) ^ Guid.GetHashCode();
                    hashCode = (hashCode * 397) ^ DoubleData.GetHashCode();
                    return hashCode;
                }
            }

            /// <summary>
            /// NewRandom
            /// </summary>
            /// <returns>returns a new ParameterHelper w/ random data in fields.</returns>
            public static ParameterHelper NewRandom()
            {
                return new ParameterHelper()
                {
                    DoubleData = PersistedTest.Random.NextDouble(),
                    Guid = Guid.NewGuid(),
                    IntData = PersistedTest.Random.Next(
                        Int32.MinValue, Int32.MaxValue),
                };
            }

            /// <summary>
            /// Int Data
            /// </summary>
            public int IntData { get; set; }

            /// <summary>
            /// Guid
            /// </summary>
            public Guid Guid { get; set; }

            /// <summary>
            /// DoubleData
            /// </summary>
            public double DoubleData { get; set; }

            /// <summary>
            /// Equals
            /// </summary>
            /// <param name="obj">the other <see cref="ParameterHelper"/> to compare this to</param>
            /// <returns>true if they're equal to one-another.</returns>
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj.GetType() == this.GetType() && Equals((ParameterHelper)obj);
            }
        }

        /// <summary>
        /// ReadTest
        /// </summary>
        [TestMethod()]
        [Description("Ensures that reading functions properly")]
        public void ReadTest()
        {
            var xmlFileName = GetNewXmlFile();
            var parameterHelperExpected = ParameterHelper.NewRandom();

            // get some file on disk; "read" data will be compared against parameterHelper (above)
            try { WriteTestHelper<ParameterHelper>(parameterHelperExpected, xmlFileName, true); }
            catch { Assert.Inconclusive("Unable to assert test correctness."); }

            // read from disk as outlined above.
            var parameterHelperActual = xmlFileName.Read<ParameterHelper>();

            // compare what we wrote with what we read.
            Assert.AreEqual(parameterHelperExpected, parameterHelperActual);

            // make sure they're not the "same" object.
            Assert.AreNotSame(parameterHelperExpected, parameterHelperActual);
        }

        /// <summary>
        /// A test for Write
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to be persisted.</typeparam>
        public void WriteTestHelper<T>(T aObj, string strFullPath, bool expected = true) 
            where T : class, new()
        {
            Assert.AreEqual(expected, aObj.Write(strFullPath));
            Assert.IsTrue(File.Exists(strFullPath));
        }

        /// <summary>
        /// WriteTest
        /// </summary>
        [TestMethod()]
        [Description("Ensures that writing functions properly")]
        public void WriteTest()
        {
            WriteTestHelper<ParameterHelper>(
                new ParameterHelper(), GetNewXmlFile(), true);
        }

        [TestMethod]
        [Description("Ensure empty file names cannot be read")]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadBreaksOnEmptyFileName()
        {
            "".Read<Object>();
        }

        [TestMethod]
        [Description("Ensure null file names cannot be read")]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadBreaksOnNullFileName()
        {
            string v = null;
            v.Read<Object>();
        }

        [TestMethod]
        [Description("Ensure files that dont exist kick off IOException.")]
        [ExpectedException(typeof(IOException))]
        public void ReadBreaksOnFileNotFound()
        {
            var strFileName = Guid.NewGuid().ToString();
            var v = new ParameterHelper();
            var readValue = strFileName.Read<ParameterHelper>();
        }

        /// <summary>
        /// WriteBreaksOnNullObject
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteBreaksOnNullObject()
        {
            Persisted.Write<object>(null, "");
        }

        /// <summary>
        /// WriteBreaksOnEmptyPath
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteBreaksOnEmptyPath()
        {
            new object().Write("");
        }

        /// <summary>
        /// WriteBreaksOnNullPath
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteBreaksOnNullPath()
        {
            new object().Write(null);
        }

        /// <summary>
        /// Kicked off on test initialize
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            TestContext.WriteLine("Starting {0}", TestContext.TestName);
        }

        /// <summary>
        /// Kicked off on test clean-up
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            TestContext.WriteLine("Terminated {0}", TestContext.TestName);
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Returns a new uniquely named XML inside of the TestDir
        /// </summary>
        /// <returns>a new uniquely named XML inside of the TestDir</returns>
        string GetNewXmlFile()
        {
            return Path.Combine(TestContext.TestDir,
                Guid.NewGuid() + ".xml");
        }
    }
}
