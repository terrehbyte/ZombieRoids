/// <list type="table">
/// <listheader><term>Ref.cs</term><description>
///     Class for wrapping value types (or value type getters and setters)
/// </description></listheader>
/// <item><term>Author</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Date Created</term><description>
///     June 6, 2014
/// </description></item>
/// <item><term>Last Modified By</term><description>
///     Elizabeth Lowry
/// </description></item>
/// <item><term>Last Modified</term><description>
///     June 6, 2014
/// </description></item>
/// <item><term>Last Modification</term><description>
///     Creation
/// </description></item>
/// </list>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    /// <summary>
    /// <para>Allows the user to wrap a value or getter/setter functions for a
    /// value so it can be passed around like a reference type, allowing
    /// multiple variables to point to the same value.</para>
    /// 
    /// <para>Sadly, C# doesn't provide a way to overload the assignment
    /// operator in a way that would let <c>myRefInt = myInt</c> set the value
    /// myRefInt wraps to the value of myInt instead of creating a new Ref
    /// wrapping the value of myInt, so you have to say <c>myRefInt.Value =
    /// myInt</c> instead.</para>
    /// 
    /// <para>To make a ref pointing to a pre-existing variable myInt, use
    /// <c>new Ref&lt;int&gt;(() => myInt, (int i) => myInt = i)</c></para>
    /// 
    /// <example><code>
    /// int myInt = 3;
    /// Ref&lt;int&gt; myRefInt =
    ///     new Ref&lt;int&gt;(() => myInt, (int i) => myInt = i);
    /// Console.WriteLine("myRefInt = " + myRefInt);    // myRefInt = 3
    /// //myRefInt = 5; // error
    /// myRefInt.Value = 5;
    /// Console.WriteLine("myInt = " myInt);            // myInt = 5
    /// myInt = 7;
    /// Console.WriteLine("myRefInt = " + myRefInt);    // myRefInt = 7
    /// int myOtherInt = 9;
    /// myOtherInt = myRefInt;
    /// Console.WriteLine("myOtherInt = " myOtherInt);  // myOtherInt = 7
    /// </code></example>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Ref<T>
    {
        // getter and setter delegate types
        public delegate T GetValue();
        public delegate void SetValue(T value);

        // getters and setters used by the Value property
        private GetValue m_get = null;
        private SetValue m_set = null;

        // value retrieved if no getter delegate is provided
        private T m_value = default(T);

        /// <summary>
        /// Property that allows getting/setting of the wrapped value
        /// </summary>
        public T Value
        {
            get { return (null == m_get ? m_value : m_get()); }
            set
            {
                if (null != m_set)
                {
                    m_set(value);
                }
                if (null == m_get)
                {
                    m_value = value;
                }
            }
        }

        /// <summary>
        /// Constructs a Ref object that simply wraps a mutable value
        /// </summary>
        /// <param name="a_value">Initial value</param>
        public Ref(T a_value = default(T)) { Value = a_value; }

        /// <summary>
        /// Constructs a Ref object that calls provided getter and setter
        /// delegates to get/set the wrapped value.
        /// </summary>
        /// <param name="a_get">Value getter delegate</param>
        /// <param name="a_set">Value setter delegate</param>
        public Ref(GetValue a_get, SetValue a_set) { m_get = a_get; m_set = a_set; }
        public Ref(GetValue a_get, SetValue a_set, T a_value)
        {
            m_get = a_get;
            m_set = a_set;
            Value = a_value;
        }

        /// <summary>
        /// Implicit casting to wrapped type allows assigning the wrapped value
        /// to variables of type T.
        /// </summary>
        /// <param name="r">The Ref to cast to its wrapped type</param>
        /// <returns>The current value returned by the Value property</returns>
        public static implicit operator T(Ref<T> r) { return r.Value; }

        /// <summary>
        /// Equality comparison uses wrapped type equality comparison
        /// </summary>
        /// <param name="obj">Object to compare this to</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Value.Equals(
                (null != obj && obj is Ref<T>) ? (obj as Ref<T>).Value : obj);
        }

        /// <summary>
        /// ToString uses wrapped value's ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString() { return Value.ToString(); }
    }
}
