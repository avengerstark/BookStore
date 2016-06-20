using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models;
using BookStore.Util;

/*При использовании контроллеров существуют некоторые условности. Так, по соглашениям об именовании названия контроллеров
должны оканчиваться на суффикс "Controller", остальная же часть до этого префикса считается именем контроллера.*/

/*Чтобы обратиться контроллеру из веб-браузера, нам надо в адресной строке набрать адрес_сайта/Имя_контроллера/.*/

/*Методы действий всегда имеют модификатор public. Закрытых приватных методов действий не бывает. 
Но контроллер может также включать и обычные методы, которые могут использоваться в вспомогательных целях.*/


/*Стандартный get-запрос принимает примерно следующую форму: название_ресурса?параметр1=значение1&параметр2=значение2.*/
namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        BookContext db = new BookContext();


        // Контроллер производит поиск представления в проекте по следующему пути: /Views/Имя_контроллера/Имя_представления.cshtml
        public ActionResult Index()
        {
            // получаем из бд все объекты Book
            IEnumerable<Book> books = db.Books;
            // передаем все объекты в динамическое свойство Books в ViewBag
            ViewBag.Books = books;
            // возвращаем представление
            return View(); // Возвращаем объект ViewResult
            //return View("Index"); // Задаем имя представления явным образом
            //return View("~/View/Home/Index.cshtml"); // Можно переопределить путь до представления
        }

        public ViewResult SomeMethod()
        { 
            ViewData["Head"] = "I'm Iron man!"; // ViewData представляет словарь из пар ключ-значение
            return View("SomeView");
        }

        [HttpGet]
        public ActionResult Buy(int id)
        {
            if (id > 3)
            {
                return Redirect("/Home/Index"); // Для временной переадресации применяется метод Redirect
                //return RedirectPermanent("/Home/Index"); // Для постоянной переадресации подобным образом используется метод RedirectPermanent
            }
            ViewBag.BookId = id;
            return View();
        }

        // Еще один класс для создания переадресации - RedirectToRouteResult - позволяет выполнить более детальную настройку перенаправлений.
        // Он возвращается двумя методами: RedirectToAction и RedirectToRoute.
        public RedirectToRouteResult SomeMethodRedirectToRoute()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index"}); // Метод RedirectToRoute позволяет произвести перенаправление
                                                                                 //по определенному маршруту внутри домена
        }

        public RedirectToRouteResult SomeMethodRedirectToAction()
        {
            // Метод RedirectToAction позволяет перейти к определенному действию определенного контроллера. Он также позволяет задать передаваемые параметры
            return RedirectToAction("Square", "Home", new { a = 10, h=12 });
        }

        [HttpPost]
        public string Buy(Purchase purchase)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
        }
        //В этом случае мы можем обратиться к действию, набрав в адресной строке Home/Square?a=10&h=3
        public string Square(int a, int h)
        {
            double s = a * h / 2;
            return "<h2> Площадь треугольника с основанием " + a + 
                " и высотой " + h + " равна " + s + "</h2>";
        }

        //Получение данных из контекста запроса
        public string Square()
        {
            int a = Int32.Parse(Request.Params["a"]); // Объект Request содержит коллекцию Params, 
            int h = Int32.Parse(Request.Params["h"]); // которая хранит все параметры, переданные в запросы.
            double s = a * h / 2;
            return "<h2> Площадь треугольника с основанием " + a +
                " и высотой " + h + " равна " + s + "</h2>";
        }


        // используем созданный класс HtmlResult
        public ActionResult GetHtml()
        {
            return new HtmlResult("<h1> I'm Batman! </h1>");
        }



        // Встроенные классы, производные от ActionResult

        // ContentResult: пишет указанный контент напрямую в ответ в виде строки
        public ContentResult SquareActionResult(int a, int h)
        {
            double s = a * h / 2;
            return Content("<h2>Площадь треугольника с основанием " + a + 
                " и высотой " + h + " равна " + s + "</h2>");
        }


        // Отправка ошибок и статусных кодов

        public ActionResult Check(int age)
        {
            if (age < 21)
            {
                // Мы смотрим введенный возраст, и если он попадает под ограничение, мы можем выслать статусный код ошибки
                return new HttpStatusCodeResult(404);
            }
            return View();
        }

        public ActionResult Check2(int age)
        {
            if (age < 21)
            {
                // В качестве альтернативы также можно возвращать объект HttpNotFoundResult с помощью метода HttpNotFound
                return HttpNotFound();
            }
            return View("Check");
        }

        public ActionResult Check3(int age)
        {
            if (age < 21)
            {
                // Класс HttpUnauthorizedResult извещает пользователя, что тот не имеет права доступа к ресурсу, отправляя браузеру статусный код 401
                return new HttpUnauthorizedResult();
            }
            return View("Check");
        }


        // Отправка файлов в ASP.NET MVC 5

        // Для отправки клиенту абстрактный класс файлов предназначен FileResult, от него наследуются:
        // FileContentResult: отправляет клиенту массив байтов, считанный из файла
        // FilePathResult: представляет простую отправку файла напрямую с сервера
        // FileStreamResult: данный класс создает поток - объект System.IO.Stream, с помощью которого считывает и отправляет файл клиенту
        // Во всех трех случаях для отправки файлов применяется метод File, который и возвращает объект FileResult.
        public FileResult GetFile()
        {
            // Путь к файлу
            string file_path = Server.MapPath("~/Files/ProblemBook.NET-ru.pdf"); // Server.MapPath позволяет построить полный путь к ресурсу из каталога в проекте.
            // Тип файла - content-type
            string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = "ProblemBook.NET-ru.pdf";
            return File(file_path, file_type, file_name);
        }

        // Похожим образом работает и классы FileContentResult, только вместо имени файла в методе File указывается массив байтов, в который был считан файл

        //Отправка массива байтов
        public FileResult GetBytes()
        {
            string file_path = Server.MapPath("~/Files/ProblemBook.NET-ru.pdf");
            byte[] mas = System.IO.File.ReadAllBytes(file_path);
            string file_type = "application/pdf";
            string file_name = "ProblemBook.NET-ru.pdf";
            return File(mas,file_type, file_name);
        }

        /*И если мы хотим возвратить объект FileStreamResult, 
       то в качестве первого аргумента в методе File идет объект Stream для отправляемого файла: */

        // Отправка потока
        public FileResult GetStream()
        {
            string file_path = Server.MapPath("~/Files/ProblemBook.NET-ru.pdf");
            // Объект Stream
            System.IO.FileStream fs = new System.IO.FileStream(file_path, System.IO.FileMode.Open);
            string file_type = "application/pdf";
            string file_name = "ProblemBook.NET-ru.pdf";
            return File(fs,file_type, file_name);
        }
    }
}
