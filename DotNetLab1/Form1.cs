using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankFormsApp
{
    public partial class Form1 : Form
    {
        private Account currentAccount;
        private AutomatedTellerMachine atm;

        public Form1()
        {
            InitializeComponent();
            atm = new AutomatedTellerMachine("ATM001", "Main Street", 5000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cardNumber = textBox1.Text;
            string pinCode = textBox2.Text;

            // Створюємо тестовий рахунок для аутентифікації
            currentAccount = new Account(cardNumber, "1234", 1000); // "1234" - тестовий PIN

            if (currentAccount.Authenticate(pinCode))
            {
                MessageBox.Show("Аутентифікація успішна!");
            }
            else
            {
                currentAccount = null; // Не дозволяємо доступ
                MessageBox.Show("Неправильний PIN-код.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentAccount == null)
            {
                MessageBox.Show("Спочатку автентифікуйтеся.");
                return;
            }

            MessageBox.Show($"Ваш баланс: {currentAccount.Balance} {currentAccount.Currency}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (currentAccount == null)
            {
                MessageBox.Show("Спочатку автентифікуйтеся.");
                return;
            }

            if (decimal.TryParse(textBox3.Text, out decimal amount))
            {
                currentAccount.Deposit(amount);
                MessageBox.Show("Кошти зараховано.");
            }
            else
            {
                MessageBox.Show("Некоректна сума.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (currentAccount == null)
            {
                MessageBox.Show("Спочатку автентифікуйтеся.");
                return;
            }

            if (decimal.TryParse(textBox3.Text, out decimal amount))
            {
                if (atm.DispenseCash(amount) && currentAccount.Withdraw(amount))
                {
                    MessageBox.Show("Операція успішна.");
                }
                else
                {
                    MessageBox.Show("Недостатньо коштів.");
                }
            }
            else
            {
                MessageBox.Show("Некоректна сума.");
            }
        }
    }
    public class Account
    {
        public string CardNumber { get; set; }
        public string PinCode { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }

        public Account(string cardNumber, string pinCode, decimal initialBalance, string currency = "UAH")
        {
            CardNumber = cardNumber;
            PinCode = pinCode;
            Balance = initialBalance;
            Currency = currency;
        }

        public bool Authenticate(string pinCode) => PinCode == pinCode;

        public void Deposit(decimal amount) => Balance += amount;

        public bool Withdraw(decimal amount)
        {
            if (amount > Balance) return false;
            Balance -= amount;
            return true;
        }
    }

    // Клас для банкомату
    public class AutomatedTellerMachine
    {
        public string ATMId { get; set; }
        public string Location { get; set; }
        public decimal CashAvailable { get; private set; }

        public AutomatedTellerMachine(string atmId, string location, decimal initialCash)
        {
            ATMId = atmId;
            Location = location;
            CashAvailable = initialCash;
        }

        public bool DispenseCash(decimal amount)
        {
            if (amount > CashAvailable) return false;
            CashAvailable -= amount;
            return true;
        }

        public void AddCash(decimal amount) => CashAvailable += amount;
    }
}