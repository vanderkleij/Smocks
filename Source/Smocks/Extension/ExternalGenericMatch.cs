using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Smocks.Extension
{
    public static class ExternalGenericMatch
    {
        public static bool IsFullNameGenericMatching(this string targetFullName, string MethodFullName)
        {
            if (targetFullName == MethodFullName)
                return true;
            string targetParamSect = "";
            string methodParamSect = "";
            if (!string.IsNullOrWhiteSpace(GetParameterSection(targetFullName)))
            {
                targetParamSect = GetParameterSection(targetFullName);
            }
            if (!string.IsNullOrWhiteSpace(GetParameterSection(MethodFullName)))
            {
                methodParamSect = GetParameterSection(MethodFullName);
            }
            var targetFullNameReplace = string.IsNullOrWhiteSpace(targetParamSect) ? targetFullName.Split('(').First() : targetFullName.Split('(').First().Replace(targetParamSect, "");
            var methodFullNameReplace = string.IsNullOrWhiteSpace(methodParamSect) ? MethodFullName.Split('(').First() : MethodFullName.Split('(').First().Replace(methodParamSect, "");
            if (targetFullNameReplace.Substring(targetFullNameReplace.IndexOf(" ")) == methodFullNameReplace.Substring(methodFullNameReplace.IndexOf(" ")))
            {
                var targetParams = Sanitize(targetFullName.Split('(').Last().Split(')').First()).Split(',').Select(p => p.Trim()).ToArray();
                var methodParams = Sanitize(MethodFullName.Split('(').Last().Split(')').First()).Split(',').Select(p => p.Trim()).ToArray();
                var targetTypeParams = string.IsNullOrWhiteSpace(targetParamSect) ? new string[] { } : Sanitize(GetParameterSection(targetFullName).Substring(1, GetParameterSection(targetFullName).Length - 2)).Split(',').Select(p => p.Trim()).ToArray();
                var methodTypeParams = string.IsNullOrWhiteSpace(methodParamSect) ? new string[] { } : Sanitize(GetParameterSection(MethodFullName).Substring(1, GetParameterSection(MethodFullName).Length - 2)).Split(',').Select(p => p.Trim()).ToArray();
                var targetReturnParamSect = targetFullNameReplace.Substring(0, targetFullNameReplace.IndexOf(" "));
                var methodReturnParamSect = methodFullNameReplace.Substring(0, methodFullNameReplace.IndexOf(" "));
                bool returnParamsAreGood = ParameterGenericEqual(targetReturnParamSect, methodReturnParamSect) || !methodReturnParamSect.Trim().HasNameSpaces() || !targetReturnParamSect.Trim().HasNameSpaces();
                bool typeParamsAreGood = ParameterGenericEqual(GetParameterSection(targetFullName), GetParameterSection(MethodFullName));
                if (methodParams.Count() != targetParams.Count())
                    return false;
                if (returnParamsAreGood && typeParamsAreGood)
                {

                }
                if (methodTypeParams.Count() != targetTypeParams.Count())
                    return false;
                int i;
                for (i = 0; i < methodParams.Count(); i++)
                {
                    if (!targetParams.ElementAt(i).HasNameSpaces())
                    {
                        methodParams[i] = targetParams.ElementAt(i);
                    }
                }
                for (i = 0; i < methodTypeParams.Count(); i++)
                {
                    if (!methodTypeParams.ElementAt(i).HasNameSpaces())
                    {
                        targetTypeParams[i] = methodTypeParams.ElementAt(i);
                    }
                }
                return targetParams.SequenceEqual(methodParams) && typeParamsAreGood && returnParamsAreGood;
            }
            return false;
        }
        private static string GetParameterSection(string methodFullName)
        {
            if (methodFullName.IndexOf('<') < 0)
            {
                return methodFullName;
            }
            var leftBraceContent = methodFullName.Split('(').First();
            var rightTypeDec = leftBraceContent.LastIndexOf('>');
            if (rightTypeDec < 0)
                return "";
            var index = rightTypeDec;
            var nestAngleBrace = 0;
            string paramsect = "";
            foreach (char character in leftBraceContent.Substring(0, rightTypeDec + 1).Reverse())
            {

                paramsect += character.ToString();
                if (character == '<')
                {
                    nestAngleBrace -= 1;

                }
                if (character == '>')
                    nestAngleBrace += 1;
                if (nestAngleBrace == 0)
                    return new string(paramsect.Reverse().ToArray());
            }
            return new string(paramsect.Reverse().ToArray());
        }

        private static string Sanitize(string Sanitizing)
        {
            string newString = "";
            var nestAngleBrace = 0;
            for (int i = 0; i < Sanitizing.Length; i++)
            {
                if (Sanitizing[i] == '<')
                {
                    nestAngleBrace += 1;
                }
                if (nestAngleBrace == 0)
                {
                    newString += Sanitizing[i];
                }
                else
                {
                    if (Sanitizing[i] == ',')
                        newString += " ";
                    else
                        newString += Sanitizing[i];
                }

                if (Sanitizing[i] == '>')
                    nestAngleBrace -= 1;
            }
            return newString;

        }
        private static bool HasNameSpaces(this string param)
        {
            return param.Split('.').Count() > 1;
        }
        private static bool ParameterGenericEqual(string param1, string param2)
        {
            if (param1.Split('<').ToList().SelectMany(p => p.Split('>')).SequenceEqual(param2.Split('<').ToList().SelectMany(p => p.Split('>'))))
                return true;
            if (!param1.HasNameSpaces() || !param2.HasNameSpaces())
                return true;
            if (param1.Split('<').First() != param2.Split('<').First() && (param1.Split('<').First().HasNameSpaces() && param2.Split('<').First().HasNameSpaces()))
                return false;
            string rawSec1 = "";
            string rawSec2 = "";
            var p1TypeSecs = Sanitize(rawSec1 = GetParameterSection(param1).Substring(1, GetParameterSection(param1).Length - 2));
            var p2TypeSecs = Sanitize(rawSec2 = GetParameterSection(param2).Substring(1, GetParameterSection(param2).Length - 2));



            if (p1TypeSecs.Split(',').Count() != p2TypeSecs.Split(',').Count())
                return false;

            for (int i = 0; i < p1TypeSecs.Split(',').Count(); i++)
            {
                if (p1TypeSecs.Split(',').ElementAt(i) != p2TypeSecs.Split(',').ElementAt(i) && p1TypeSecs.Split(',').ElementAt(i).HasNameSpaces() && p2TypeSecs.Split(',').ElementAt(i).HasNameSpaces())
                {
                    if (!rawSec1.Contains('<') || !rawSec2.Contains('<'))
                        return false;
                    return ParameterGenericEqual(rawSec1, rawSec2) && ParameterGenericEqual(param1.Substring(param1.IndexOf(rawSec1) + rawSec1.Length), param2.Substring(param2.IndexOf(rawSec2) + rawSec2.Length));
                }

            }
            return true;

        }
    }
}