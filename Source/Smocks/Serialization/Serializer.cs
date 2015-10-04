#region License
//// The MIT License (MIT)
//// 
//// Copyright (c) 2015 Tom van der Kleij
//// 
//// Permission is hereby granted, free of charge, to any person obtaining a copy of
//// this software and associated documentation files (the "Software"), to deal in
//// the Software without restriction, including without limitation the rights to
//// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//// the Software, and to permit persons to whom the Software is furnished to do so,
//// subject to the following conditions:
//// 
//// The above copyright notice and this permission notice shall be included in all
//// copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Smocks.Serialization
{
    [Serializable]
    internal class Serializer : ISerializer
    {
        private const string StringValueKey = "Value";

        public object Deserialize(Type targetType, Dictionary<string, object> serializedTarget)
        {
            if (targetType == null)
            {
                return null;
            }

            if (targetType == typeof(string))
            {
                return (string)serializedTarget[StringValueKey];
            }

            object result = Activator.CreateInstance(targetType);
            Populate(serializedTarget, result);

            return result;
        }

        public void Populate(Dictionary<string, object> targetValues, object target)
        {
            if (target == null)
            {
                return;
            }

            foreach (var field in target.GetType().GetFields())
            {
                object value;

                if (targetValues.TryGetValue(field.Name, out value))
                {
                    field.SetValue(target, value);
                }
            }
        }

        public Dictionary<string, object> Serialize(object target)
        {
            if (target == null)
            {
                return null;
            }

            var targetType = target.GetType();

            if (targetType == typeof(string))
            {
                return new Dictionary<string, object> { { StringValueKey, (string)target } };
            }

            return target
                .GetType()
                .GetFields()
                .Where(field => IsSerializable(field.FieldType))
                .ToDictionary(field => field.Name, field => field.GetValue(target));
        }

        private bool IsSerializable(Type fieldType)
        {
            bool isDelegate = typeof(Delegate).IsAssignableFrom(fieldType);
            bool isProxy = typeof (MarshalByRefObject).IsAssignableFrom(fieldType);
            bool result = (fieldType.IsSerializable || isProxy) && !isDelegate;

            return result;
        }
    }
}