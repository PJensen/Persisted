# Persisted
> Extends Object for quick, generic programmed, serialization.

# Usage
```.cs

// Include persisted
using System.Xml.Serialization.Persisted;

namespace Demo { 

   void Main(string[] argv) {

      #region Writing Objects

      var myObject = new MyObject();

      // method 1
      Persisted.Write<MyObject>(myObject, strFileName);

      // method 2
      myObject.Write<myObject>(strFileName);

      #endregion


      #region Reading Objects

      // method 1
      var myObject = Persisted.Read<MyObject>(strFileName);

      // method 2
      var myObject = strFileName.Read<MyObject>();

      #endregion
   }

}
```

# License
Copyright (C) 2011  Peter Jensen

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
