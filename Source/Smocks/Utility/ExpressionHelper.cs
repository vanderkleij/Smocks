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
using System.Linq.Expressions;
using System.Reflection;

namespace Smocks.Utility
{
    internal class ExpressionHelper : IExpressionHelper
    {
        public FieldInfo GetField(LambdaExpression expression)
        {
            if (expression == null)
                return null;

            return GetField(expression.Body as MemberExpression);
        }

        public FieldInfo GetField(MemberExpression expression)
        {
            if (expression == null)
                return null;

            return expression.Member as FieldInfo;
        }

        public MethodCallInfo GetMethod(LambdaExpression expression)
        {
            if (expression == null)
                return null;

            return GetMethod(expression.Body);
        }

        public MethodCallInfo GetMethod(Expression expression)
        {
            return GetPropertyGetCall(expression as LambdaExpression) ??
                GetMethod(expression as LambdaExpression) ??
                GetMethod(expression as MethodCallExpression) ??
                GetMethod(expression as NewExpression);
        }

        public PropertyInfo GetProperty(LambdaExpression expression)
        {
            if (expression == null)
                return null;

            return GetProperty(expression.Body as MemberExpression);
        }

        public PropertyInfo GetProperty(MemberExpression expression)
        {
            if (expression == null)
                return null;

            return expression.Member as PropertyInfo;
        }

        public MethodCallInfo GetPropertyGetCall(LambdaExpression expression)
        {
            if (expression == null)
                return null;

            return GetPropertyGetCall(expression.Body as MemberExpression);
        }

        public object GetValue(Expression expression)
        {
            var lambda = Expression.Lambda(expression);
            return lambda.Compile().DynamicInvoke();
        }

        public bool IsUnconditionalAny(Expression expression)
        {
            MethodCallInfo method = GetMethod(expression);

            return method != null &&
                method.Method != null &&
                string.Equals(method.Method.Name, "IsAny") &&
                string.Equals(method.Method.DeclaringType.Name, "It") &&
                method.Method.GetParameters().Length == 0;
        }

        private MethodCallInfo GetMethod(NewExpression expression)
        {
            if (expression == null)
                return null;

            return new MethodCallInfo(expression.Constructor, expression.Arguments);
        }

        private MethodCallInfo GetMethod(MethodCallExpression expression)
        {
            if (expression == null)
                return null;

            List<Expression> arguments = expression.Arguments.ToList();

            // If this is an instance method (i.e., it has a target object), we consider
            // the target an argument as well for our purposes.
            if (expression.Object != null)
            {
                arguments.Insert(0, expression.Object);
            }

            return new MethodCallInfo(expression.Method, arguments.AsReadOnly());
        }

        private MethodCallInfo GetPropertyGetCall(MemberExpression expression)
        {
            if (expression == null)
                return null;

            PropertyInfo property = expression.Member as PropertyInfo;

            if (property == null)
                return null;

            return expression.Expression != null
                ? new MethodCallInfo(property.GetGetMethod(), expression.Expression)
                : new MethodCallInfo(property.GetGetMethod());
        }
    }
}