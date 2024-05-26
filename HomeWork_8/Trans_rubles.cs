using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_8
{
    public class Trans_rubles:ITransaction
    {
        public string Name_Currency { get; }
        public string Description { get; }
        public DateTime Date { get; set; } // Дата транзакции
        public ulong TransactionId { get; set;  } // Уникальный ID транзакции
        public float Amount { get; set; } // Сумма транзакции
        public Trans_rubles(DateTime date, ulong transactionId, float amount, string description)
        {
            Date = date;
            TransactionId = transactionId;
            Amount = amount;
            Name_Currency = "Rubles";
            Description = description;
        }
    }
}
