using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace HomeWork_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // ------------------ Задача 1 - Список работников ------------------
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Задача 1 - Список сотрудников");
            Console.ForegroundColor= ConsoleColor.White;
            List<Employee> employees = new List<Employee>()
            {
                new Employee("Маша", "Руководство", "Директор", 150000 , 1),
                new Employee("Паша", "Руководство", "Главный инженер", 105000, 2),
                new Employee("Таня", "Руководство", "Главный бухгалтер", 85000, 2),
                new Employee("Вася", "Инженерный отдел", "Технолог", 45000, 4),
                new Employee("Витя", "Инженерный отдел", "Ведущий технолог", 65000, 3),
                new Employee("Даша", "Инженерный отдел", "Конструктор", 45500, 4),
                new Employee("Даша", "Инженерный отдел", "Ведущий конструктор", 65500, 3),
                new Employee("Петя", "Производство", "Токарь", 75000, 5),
                new Employee("Женя", "Производство", "Сварщик", 85000, 5),
                new Employee("Гоша", "Производство", "Сборщик", 79500, 5),
                new Employee("Саша", "Производство", "Газорезчик", 59500, 5),
                new Employee("Света", "Отдел продаж", "Менеджер", 55000, 6),
                new Employee("Галя", "Отдел продаж", "Менеджер", 55000, 6),
                new Employee("Галя", "Отдел продаж", "Помощник менеджера", 35000, 7),
            };
            Console.WriteLine("\nИсходный список сотрудников:\n");
            foreach (Employee emp in employees)
                Console.WriteLine(emp.ToString());
            var name_dep = from x in employees // Вытаскиваем все наименования отделов в список name_departments
                           select x.Department;             
            name_dep = name_dep.Distinct(); // Удаляем из списка отделов дублирующиеся наименования
            // Объявляем словарь <наименование отдела, средняя зарплата отдела>
            Dictionary<string, double> salary_dep = new Dictionary<string, double>();
            foreach (var el in name_dep) // Цикл заполнения словаря парами "ключ-значение"
            {
                var result = from x in employees // Формируем коллекцию зарплат по каждому из отделов
                             where x.Department == el
                             select x.Salary;
                salary_dep.Add(el, result.Average());
            }            
            Console.WriteLine("\nСредние зарплаты по отделам:\n");            
            foreach (var el in salary_dep)
                Console.WriteLine($"{el.Key} = {el.Value} рублей.");
            Console.WriteLine("\nОтдел с самой высокой средней зарплатой: " + salary_dep.MaxBy(x => x.Value).Key);
            Console.WriteLine();
            foreach (var el in name_dep)
            {
                var result = from x in employees // По каждому отделу формируем коллекцию сотрудников с ЗП > средней по отделу
                             where x.Department == el && x.Salary > salary_dep[el]
                             select x;
                Console.WriteLine($"Список сотрудников отдела \'{el}\' с зарплатами, выше средней по отделу в {salary_dep[el]} рублей: ");
                foreach (var num in result)
                    Console.WriteLine($"{num.Name}, {num.Title}");                 
            }
            Console.WriteLine("\nСуммарная зарплата для каждого уровня должности:\n");
            var pos_levels = from x in employees // Создаём коллекцию уровней должности
                             select x.PositionLevel;
            pos_levels.OrderBy(x => x).ToList(); // Сортируем коллекцию уровней должностей по возрастанию
            pos_levels = pos_levels.Distinct(); // Уникальная коллекция уровней должности
            foreach (var el in pos_levels) 
            {                
                var result = from x in employees // По каждому уровню должности формируем коллекцию сотрудников 
                             where x.PositionLevel == el
                             select x.Salary;
                // Вычисляем суммарную ЗП сотрудников с одинаковым уровнем должности и выводим её в консоль
                Console.WriteLine($"Уровень должности {el} = {result.Sum()} рублей.");              
            }

            // ------------------ Задача 2 - Транзакции за период ------------------
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nЗадача 2 - Транзакции за период");
            Console.ForegroundColor = ConsoleColor.White;
            List<Trans_rubles> trans_russia = new List<Trans_rubles>()
            {
                new Trans_rubles(new DateTime(2015, 12, 31), 141432424234, 25.41f, "Покупка яблок"),
                new Trans_rubles(new DateTime(2021, 11, 13), 21432143214324, 254.98f, "Парихмахерская"),
                new Trans_rubles(new DateTime(2012, 8, 27), 443214324, 154.00f, "Бассейн"),
                new Trans_rubles(new DateTime(2023, 5, 1), 9132432143214324, 1004.50f, "Билет в трамвай"),
                new Trans_rubles(new DateTime(2007, 11, 13), 3214324, 4.10f, "Оплата учёбы"),
                new Trans_rubles(new DateTime(2003, 11, 13), 7241432143214324, 50f, "Перевод физ.лицу"),
                new Trans_rubles(new DateTime(2017, 10, 9), 832141432143214324, 100000.00f, "Сервисные услуги"),
                new Trans_rubles(new DateTime(2019, 9, 30), 5332143214324, 5025.75f, "Доставка еды")
            };
            List<Trans_dollars> trans_oa = new List<Trans_dollars>()
            {
                new Trans_dollars(new DateTime(2015, 11, 30), 1431432424234, 5.01f, "Оплата визового сбора"),
                new Trans_dollars(new DateTime(2015, 12, 15), 04314324244, 199.99f, "Билеты на самолёт"),
                new Trans_dollars(new DateTime(2015, 12, 16), 94031432424234, 99.99f, "Бронирование отеля"),
                new Trans_dollars(new DateTime(2016, 1, 2), 55423042340, 5.01f, "Экскурсия"),
                new Trans_dollars(new DateTime(2016, 5, 2), 6623042340, 5.01f, "Ресторан"),
                new Trans_dollars(new DateTime(2016, 10, 2), 77423042340, 5.01f, "Аквапарк"),
                new Trans_dollars(new DateTime(2016, 9, 2), 8823042340, 5.01f, "Такси")
            };
            Console.Write("\nВведите интервал дат, за который нужно рассчитать среднюю сумму транзакций:\n" +
                "Дата вводится в формате год, месяц, число (2021,5,30) или день, месяц, год (30,5,21), вместо запятых допустимы точки\n" +
                "Введите начальную дату -> ");
            DateTime time_start = Date();
            Console.Write("Введите конечную дату -> ");
            DateTime time_end = Date();
            Console.WriteLine($"Средняя сумма транзакции за указанный период в России = " +
                $"{Sum_Avverage<Trans_rubles>(trans_russia, time_start, time_end)} {trans_russia[0].Name_Currency}"); ;
            Console.WriteLine($"Средняя сумма транзакции за указанный период в ОАЭ = " +
                $"{Sum_Avverage<Trans_dollars>(trans_oa, time_start, time_end)} {trans_oa[0].Name_Currency}");

            // ------------------ Задача 3 - Словарь разнотиповых объектов ------------------
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nЗадача 3 - Словарь <Тип объекта, среднее занчение длины всех строковых свойств>");
            Console.ForegroundColor = ConsoleColor.White;
            var general_list = trans_russia.Cast<ITransaction>() // Коллекция объектов разных классов (Trans_dollars и Trans_rubles)
                .Concat(trans_oa.Cast<ITransaction>()) // Объединяем коллекций из задачи 2
                .ToList(); // Формируем из них общий список general_list       
            foreach (var el in CalculateAverageStringLength<ITransaction>(general_list)) // Цикл формирования словаря по условию задачи
                Console.WriteLine($"\'{el.Key}\': {el.Value}");          
        }
        static Dictionary<string, double> CalculateAverageStringLength<T>(List<T> list)  // Обобщённый метод для задачи 3
        {
            Dictionary<string, double> result = new Dictionary<string, double>();            
            var result_string = from el in list // Коллекция уникальных наименований типов данных из коллекции list
                                select el.GetType().Name;
            result_string = result_string.Distinct(); 
            foreach ( var el in result_string ) // Цикл формирования словаря по условию задачи
            {                   
                var res_unique_type = from x in list // Уникальная коллекция (тип el) из исходной коллекции list (разные типы)
                                      where x.GetType().Name == el
                                      select x;                
                var res = from e in res_unique_type // Коллекция длин полей строкового типа всех элементов коллекции с уникальными типом
                          from t in e.GetType().GetProperties() // В каждом элементе смотрим поля на предмет типа string
                          where t.PropertyType.Name == "String"
                          select t.GetValue(e).ToString().Length; 
                result.Add(el, (double)res.Sum() / res_unique_type.Count());

                // ---------------- Второй вариант (неоптимальный) ----------------
                /*double length = 0; // Длина всех полей коллекции res_unique_type с типом string
                foreach (var e in res_unique_type) // Цикл проверки каждого элемента в уникальной коллекции типа
                {
                    var res = from t in e.GetType().GetProperties() // Коллекция длин полей строкового типа каждого элемента
                              where t.PropertyType.Name == "String"
                              select t.GetValue(e).ToString().Length;
                    length += res.Sum(); // Увеличиваем результат на длину полей строкового типа каждого элемента уникальной коллекции 
                }                          
                result.Add(el, length / res_unique_type.Count());*/

                // ------------------- Первый вариант (неоптимальный) ----------------
                /*double length = 0; // Длина всех полей коллекции res_unique_type с типом string
                foreach ( var i in res_unique_type)
                {
                    foreach (var p in i.GetType().GetProperties())
                        if (p.PropertyType.Name == "String")
                            length += p.GetValue(i).ToString().Length;
                }
                result.Add(el, (double)length / res_unique_type.Count());*/
            }
            return result;
        }
        // Метод расчёта средней суммы транзакций за период (для задачи 2)
        static float Sum_Avverage<T>(List<T> trans, DateTime date_start, DateTime date_end) where T : ITransaction
        {
            if (date_start > date_end) // Если начало диапазона больше конца, то просто свапаем их
            {
                DateTime tmp = date_start;
                date_start = date_end;
                date_end = tmp;                
            }            
            var result = from el in trans
                         where el.Date >= date_start && el.Date <= date_end
                         select el.Amount;
            if (result.Count() == 0) // если в указанный период не было транзакций
            {
                Console.WriteLine("В указанный период не было транзакций!");
                return 0f;
            }                
            return result.Average();
        }
        static DateTime Date() // Функция проверки и возврата введённой пользователем даты (для задачи 2)
        {
            DateTime date = new DateTime(2000,1,1);
            bool flag = false;
            while (!flag)
            {
                if (DateTime.TryParse(Console.ReadLine(), out date))
                    flag = true;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введена некорректная дата!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Введите начальную дату ещё раз -> ");
                }
            }
            Console.WriteLine("\nДля расчёта будет использована дата " + date.ToShortDateString() + "\n");
            return date;
        }
    }
}