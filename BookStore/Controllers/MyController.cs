using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.Controllers
{
    //public interface IController
    //{
    //    void Execute(RequestContext requestContext); // Интерфейс IController определяет один единственный метод Execute,
    //    //который отвечает за обработку контекста запроса
    //}
    // Чтобы создать свой класс контроллера, достаточно создать класс, реализующий интерфейс IController и имеющий в имени суффикс Controller.
    public class MyController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            string ip = requestContext.HttpContext.Request.UserHostAddress;
            var response = requestContext.HttpContext.Response;
            response.Write("<h2> Ваш IP-адрес : " + ip + "</h2>");
        }
    }

    /*Но в реальности чаще оперируют более высокоуровневыми классами,
    как например класс Controller, поскольку он предоставляет более мощные средства для обработки запросов.*/
}