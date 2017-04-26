using System;

//todo: This is a complete hack because StackTrace() and StackFrame are not currently in the .net core APIs
//todo: When and if they become available (in 2.0???) their use should be re-introduced.

namespace PactNet.TestContextInfo
{
    public class ContextInfo
    {
        private static string _currentContextName = null;

        // todo: remove this when 'new StackTrace()' becomes available
        public static void SetTestContextName(string nameSpace, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            _currentContextName = $"{nameSpace}.{memberName}";
        }

        // todo: remove this when 'new StackTrace()' becomes available
        public static string CurrentContextName
        {
            get
            {
                if (_currentContextName == null)
                {
                    throw new InvalidOperationException("A call to SetTestContextName() must precede attempt to retrieve CurrentContextName.");
                }
                return _currentContextName;
            }
        }


        // todo: uncomment this when 'new StackTrace()' becomes available
        //public static string CurrentContextName
        //{
        //    get
        //    {
        //        var stack = new StackTrace(true);
        //        var stackFrames = stack.GetFrames() ?? new StackFrame[0];

        //        var relevantStackFrameSummaries = new List<string>();

        //        foreach (var stackFrame in stackFrames)
        //        {
        //            var type = stackFrame.GetMethod().ReflectedType;

        //            if (type == null ||
        //                (type.Assembly.GetName().Name.StartsWith("PactNet", StringComparison.CurrentCultureIgnoreCase) &&
        //                 !type.Assembly.GetName().Name.Equals("PactNet.Tests", StringComparison.CurrentCultureIgnoreCase)))
        //            {
        //                continue;
        //            }

        //            //Don't care about any mscorlib frames down
        //            if (type.Assembly.GetName().Name.Equals("mscorlib", StringComparison.CurrentCultureIgnoreCase))
        //            {
        //                break;
        //            }

        //            relevantStackFrameSummaries.Add(String.Format("{0}.{1}", type.Name, stackFrame.GetMethod().Name));
        //        }

        //        return String.Join(" ", relevantStackFrameSummaries);
        //    }
        //}
    }
}
