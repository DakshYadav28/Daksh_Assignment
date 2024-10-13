using BankingSystem.BusinessLayer;
using BankingSystem.BusinessLayer.Repository;
using BankingSystem.BusinessLayer.Service;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BankingSystem.UI
{
    internal class BankApp
    {
        static void Main(string[] args)
        {
            IBankServiceProvider bankServiceProvider = new BankServiceProviderImpl();

            while (true)
            {
                Console.Clear(); // Clear the console for a cleaner UI experience
                Console.WriteLine("=====================================");
                Console.WriteLine("          BANKING SYSTEM MENU        ");
                Console.WriteLine("=====================================");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Get Balance");
                Console.WriteLine("5. Transfer");
                Console.WriteLine("6. Get Account Details");
                Console.WriteLine("7. List All Accounts");
                Console.WriteLine("8. Get Transactions");
                Console.WriteLine("9. Exit");
                Console.WriteLine("=====================================");
                Console.Write("Enter your choice (1-9): ");

                string input = Console.ReadLine();

                if (!int.TryParse(input, out int choice) || choice < 1 || choice > 9)
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 9.");
                    WaitForUser();
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        CreateAccount(bankServiceProvider);
                        break;
                    case 2:
                        Deposit(bankServiceProvider);
                        break;
                    case 3:
                        Withdraw(bankServiceProvider);
                        break;
                    case 4:
                        GetBalance(bankServiceProvider);
                        break;
                    case 5:
                        Transfer(bankServiceProvider);
                        break;
                    case 6:
                        GetAccountDetails(bankServiceProvider);
                        break;
                    case 7:
                        ListAccounts(bankServiceProvider);
                        break;
                    case 8:
                        GetTransactions(bankServiceProvider);
                        break;
                    case 9:
                        Console.WriteLine("Exiting... Thank you for using the banking system.");
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
                WaitForUser();
            }
        }
        private static void WaitForUser()
        {
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }
        public static void CalculateInterest()
        {
            var conn = DBUtil.GetDBConn();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open(); // Only open the connection if it's closed
            }

            try
            {
                // Prepare and execute query
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM Accounts";
                cmd.Connection = conn;

                SqlDataReader sqlDataReader = cmd.ExecuteReader();

                // Display query result
                Console.WriteLine("Employee ID\tEmployee Name\tEmployee Address\tEmployee Salary\tEmployee Manager");
                while (sqlDataReader.Read())
                {
                    Console.WriteLine($"{sqlDataReader["account_id"]}\t\t{sqlDataReader[1]}\t\t{sqlDataReader[2]}\t\t{sqlDataReader[3]}");
                }

                // Close the reader after processing the result
                sqlDataReader.Close();
            }
            finally
            {
                // Ensure that the connection is properly closed after execution
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            Console.ReadKey();
        }

            private static void CreateAccount(IBankServiceProvider bankServiceProvider)
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter Email Address: ");
            string email = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();

            Console.Write("Enter Account Type (Savings, Current, ZeroBalance): ");
            string accType = Console.ReadLine();

            Console.Write("Enter Initial Balance: ");
            float balance = float.Parse(Console.ReadLine());

            Customer customer = new Customer(customerId, firstName, lastName, email, phoneNumber, address);

            try
            {
                //bankServiceProvider.CreateAccount(customer, accType, balance);
                bankServiceProvider.create_account(customer, 0, accType, balance); // Assuming 0 will be replaced with a generated account number
                Console.WriteLine("Account created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
            }
        }

        private static void Deposit(IBankServiceProvider bankServiceProvider)
        {
            Console.Clear();
            Console.WriteLine("DEPOSIT FUNDS");
            Console.Write("Enter Account Number: ");
            long accountNumber = long.Parse(Console.ReadLine());

            Console.Write("Enter Deposit Amount: ");
            float amount = float.Parse(Console.ReadLine());

            try
            {
                float newBalance = bankServiceProvider.deposit(accountNumber, amount);
                Console.WriteLine($"Deposit successful. New balance: {newBalance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during deposit: {ex.Message}");
            }
        }

        private static void Withdraw(IBankServiceProvider bankServiceProvider)
        {
            Console.Clear();
            Console.WriteLine("WITHDRAW FUNDS");
            Console.Write("Enter Account Number: ");
            long accountNumber = long.Parse(Console.ReadLine());

            Console.Write("Enter Withdrawal Amount: ");
            float amount = float.Parse(Console.ReadLine());

            try
            {
                float newBalance = bankServiceProvider.withdraw(accountNumber, amount);
                Console.WriteLine($"Withdrawal successful. New balance: {newBalance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during withdrawal: {ex.Message}");
            }
        }

        private static void GetBalance(IBankServiceProvider bankServiceProvider)
        {
            Console.Clear();
            Console.WriteLine("GET ACCOUNT BALANCE");
            Console.Write("Enter Account Number: ");
            long accountNumber = long.Parse(Console.ReadLine());

            try
            {
                float balance = bankServiceProvider.get_account_balance(accountNumber);
                Console.WriteLine($"Current balance: {balance}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving balance: {ex.Message}");
            }
        }

        private static void Transfer(IBankServiceProvider bankServiceProvider)
        {
            Console.Clear();
            Console.WriteLine("TRANSFER FUNDS");
            Console.Write("Enter Sender's Account Number: ");
            long fromAccountNumber = long.Parse(Console.ReadLine());

            Console.Write("Enter Receiver's Account Number: ");
            long toAccountNumber = long.Parse(Console.ReadLine());

            Console.Write("Enter Transfer Amount: ");
            float amount = float.Parse(Console.ReadLine());

            try
            {
                bankServiceProvider.transfer(fromAccountNumber, toAccountNumber, amount);
                Console.WriteLine("Transfer successful.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during transfer: {ex.Message}");
            }
        }
        private static void GetAccountDetails(IBankServiceProvider bankServiceProvider)
        {
            Console.Clear();
            Console.WriteLine("GET ACCOUNT DETAILS");
            Console.Write("Enter Account Number: ");
            if (!long.TryParse(Console.ReadLine(), out long accountNumber))
            {
                Console.WriteLine("Invalid account number format.");
                return;
            }

            try
            {
                Account accountDetails = bankServiceProvider.GetAccountDetails(accountNumber);
                Console.WriteLine(accountDetails); 
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }


        private static void ListAccounts(IBankServiceProvider bankServiceProvider)
        {
            Console.Clear();
            Console.WriteLine("LIST OF ALL ACCOUNTS");
            try
            {
                var accounts = bankServiceProvider.ListAccounts();
                foreach (var account in accounts)
                {
                    Console.WriteLine(account); // Implement toString in Account class to display account details
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving account list: {ex.Message}");
            }
        }

        private static void GetTransactions(IBankServiceProvider bankServiceProvider)
        {
            Console.Clear();
            Console.WriteLine("GET TRANSACTIONS");
            Console.Write("Enter Account Number: ");
            long accountNumber = long.Parse(Console.ReadLine());

            Console.Write("Enter From Date (yyyy-mm-dd): ");
            DateTime fromDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter To Date (yyyy-mm-dd): ");
            //DateTime toDate = DateTime.Parse(Console.ReadLine());
            string input = Console.ReadLine();
            DateTime toDate;

            if (DateTime.TryParse(input, out toDate))
            {
                Console.WriteLine($"You entered a valid date: {toDate}");
            }
            else
            {
                Console.WriteLine("Invalid date format. Please try again.");
            }

            try
            {
                var transactions = bankServiceProvider.getTransations(accountNumber, fromDate, toDate);
                Console.WriteLine("Transaction History:");
                foreach (var transaction in transactions)
                {
                    Console.WriteLine(transaction); // Implement toString in Transaction class to display transaction details
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving transactions: {ex.Message}");
            }


           

        }
}
}
