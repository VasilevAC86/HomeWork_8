using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HomeWork_8
{
    public class Employee
    {
        public string Name { get; set; } 
        public string Department { get; set; } // Отдел
        public string Title { get; set; } // Должность
        public int Salary { get; set; } // Зарплата
        public int PositionLevel { get; set; } // Уровень должности
        public Employee(string name, string dep, string title, int salary, int pos)
        {
            this.Name = name;
            this.Department = dep;
            this.PositionLevel = pos;
            this.Salary = salary;
            this.Title = title;
        }
        public override string ToString()
        {
            return $"{Name}, \'{Department}\', \'{Title}\', {Salary} рублей, уровень долности = {PositionLevel}";
        }
    }
}
