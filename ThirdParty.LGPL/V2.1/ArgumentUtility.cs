using System;

namespace ThirdParty.LGPL.V2._1 {
    /// <summary>
    /// This class was created without reference to the code by the same name but is placed under the
    /// LGPL 2.1
    /// </summary>
    public class ArgumentUtility
    {
        public static T CheckNotNull<T>(string empty, T type)
        {
            if (type == null)
                throw new ArgumentNullException();

            return type;
        }
    }
}
