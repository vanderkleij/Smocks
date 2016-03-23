using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smocks.Extension
{
    public static class ExternalGenericMatch
    {
        public static bool IsFullNameGenericMatching(this string targetFullName, string MethodFullName)
        {
            if (targetFullName == MethodFullName)
                return true;
            if (targetFullName.Split('(').First() == MethodFullName.Split('(').First())
            {
                var targetParams = targetFullName.Split('(').Last().Split(')').First().Split(',').Select(p=>p.Trim()).ToArray();
                var methodParams = MethodFullName.Split('(').Last().Split(')').First().Split(',').Select(p=>p.Trim()).ToArray();
                if(methodParams.Count()!= targetParams.Count())
                    return false;
                int i;
                for(i =0;i<methodParams.Count();i++)
                {
                    if (targetParams.ElementAt(i) == "T")
                    {
                        methodParams[i] = "T";
                    }
                }
                return targetParams.SequenceEqual(methodParams);
            }
            return false;
        }
    }
}
