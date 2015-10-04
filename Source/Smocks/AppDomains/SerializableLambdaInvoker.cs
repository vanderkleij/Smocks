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
using System.Collections.ObjectModel;
using System.Linq;
using Smocks.IL.Resolvers;
using Smocks.Serialization;
using Smocks.Utility;

namespace Smocks.AppDomains
{
    internal class SerializableLambdaInvoker : MarshalByRefObject
    {
        private static readonly IEnumerable<IReturnValueTransformer> DefaultReturnValueTransformers
            = DiscoverDefaultReturnValueTransformers();

        private readonly ReadOnlyCollection<IReturnValueTransformer> _returnValueTransformers;

        public SerializableLambdaInvoker()
            : this(DefaultReturnValueTransformers)
        {
        }

        private SerializableLambdaInvoker(IEnumerable<IReturnValueTransformer> returnValueTransformers)
        {
            _returnValueTransformers = new List<IReturnValueTransformer>(
                returnValueTransformers ?? Enumerable.Empty<IReturnValueTransformer>()).AsReadOnly();
        }

        public InvocationResult<TReturnValue> Invoke<TReturnValue>(
            SerializableLambda<TReturnValue> serializableLambda,
            ISerializer serializer,
            params object[] arguments)
        {
            object target = serializer.Deserialize(serializableLambda.TargetType, serializableLambda.TargetValues);

            object rawReturnValue = serializableLambda.Method.IsStatic && target != null
                ? serializableLambda.Method.Invoke(null, new[] { target }.Concat(arguments).ToArray())
                : serializableLambda.Method.Invoke(target, arguments);

            TReturnValue returnValue = Transform((TReturnValue)rawReturnValue);

            var serialized = serializer.Serialize(target);
            return new InvocationResult<TReturnValue>(serialized, returnValue);
        }

        private static IEnumerable<IReturnValueTransformer> DiscoverDefaultReturnValueTransformers()
        {
            Discoverer discoverer = new Discoverer(
                new AssemblyTypeContainer(typeof(SerializableLambdaInvoker).Assembly));
            return discoverer.GetAll<IReturnValueTransformer>();
        }

        private TReturnValue Transform<TReturnValue>(TReturnValue returnValue)
        {
            var matchingTransformer = _returnValueTransformers.FirstOrDefault(transformer =>
                transformer.CanTransform(typeof(TReturnValue), returnValue));

            return matchingTransformer != null ? matchingTransformer.Transform(returnValue) : returnValue;
        }
    }
}