using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Util
{
    //public abstract class ActionResult
    //{
    //    public abstract void ExecuteResult(ControllerContext context);
    //}

    /* прошлой теме в примере с вычислением площади треугольника мы возвращали html-код в виде строки. Но, как правило,
     * возвращаемым результатом является объект класса, производного от ActionResult.
     * ActionResult представляет собой абстрактный класс, в котором определен один метод ExecuteResult, переопределяемый в классах-наследниках*/
    public class HtmlResult : ActionResult 
    {
        private string htmlCode;
        public HtmlResult(string html)
        {
            htmlCode = html;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string fullHtmlCode = "<!DOCTYPE  html> <html><head>";
            fullHtmlCode += "<title> Главная страница</title>";
            fullHtmlCode += "<meta charset=utf-8";
            fullHtmlCode += "</head > <body>";
            fullHtmlCode += htmlCode;
            fullHtmlCode += "</body> </html>";
            context.HttpContext.Response.Write(fullHtmlCode); // вклюаем нашу html разметку в выходной поток
        }
    }
}