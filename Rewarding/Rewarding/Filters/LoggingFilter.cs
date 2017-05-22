using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rewarding.Filters
{
    public class LoggingFilter : FilterAttribute, IActionFilter
    {
        string logRecord = "";
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (String)filterContext.RouteData.Values["Controller"];
            var action = (String)filterContext.RouteData.Values["Action"];
            var startExecution = DateTime.Now.ToString();

            logRecord = "Action : " + controller + "/" + action + ", " + "Start time : " + startExecution + ", ";
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var finishExecution = DateTime.Now.ToString();

            logRecord = logRecord + "Finish time :" + finishExecution;

            LogToFile();
        }

        public void LogToFile()
        {
            string filePath =@"D:\MENTORING\MVC_tasks\DONE\Task 9\Rewarding\Rewarding\Rewarding\Logs\Logs.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(logRecord);
            }
        }
    }
}