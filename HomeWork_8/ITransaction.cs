using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_8
{
    public interface ITransaction
    {
        DateTime Date { get; set; } // Дата транзакции
        ulong TransactionId { get; set; } // Уникальный ID транзакции
        float Amount { get; set; } // Сумма транзакции
        public string Name_Currency { get; }
        public string Description { get; }
    }
}
