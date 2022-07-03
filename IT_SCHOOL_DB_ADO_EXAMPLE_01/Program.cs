using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace IT_SCHOOL_DB_ADO_EXAMPLE_00
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=DESKTOP-TOMKA;Initial Catalog=IT_SCHOOL_DB_ADO_EXAMPLE_00;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = Menu(conn);
            }
        }

        /// <summary>
        /// Меню
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        private static bool Menu(SqlConnection conn)
        {
            //Діалог із користувачем
            Console.Clear();
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\ti - Operation of Insert");
            Console.WriteLine("\ts - Operation of Simple Read");
            Console.WriteLine("\tr - Operation of Detail Read");
            Console.WriteLine("\tp - Operation with StoredProcedure");
            Console.WriteLine("\te - Exit");
            Console.Write("Select an option? ");

            //Вибір того що потрібно зробити
            switch (Console.ReadLine())
            {
                case "i":
                    InsertQuery(conn);
                    return true;
                case "s":
                    ReadData(conn);
                    return true;
                case "r":
                    ReadData2(conn);
                    return true;
                case "p":
                    ExecStoredProcedure(conn);
                    return true;
                case "e":
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// функція вставки
        /// </summary>
        /// <param name="conn"></param>
        public static void InsertQuery(SqlConnection conn)
            {
                try
                {
                    // Відкриваємо підключення
                    conn.Open();

                    // Підготовка запиту для вставки в таблицю авторів
                    string AUTHOR_insertString = "Insert into Authors (FirstName, LastName) values ('SUPERNAME', 'SUPERLASTNAME') SET @ID = SCOPE_IDENTITY();";
                    SqlCommand cmd_author = new SqlCommand(AUTHOR_insertString, conn);

                    SqlParameter authID_param = new SqlParameter("@ID", SqlDbType.Int, 4);
                    authID_param.Direction = ParameterDirection.Output;
                    cmd_author.Parameters.Add(authID_param);
                    // виликаємо ExecuteNonQuery для відправки команди (запис в таблицю авторів)
                    cmd_author.ExecuteNonQuery();
                    // отримуємо ID доданого атвора
                    var authorID = authID_param.Value;



                    // Підготовка запиту для вставки в таблицю книг  
                    string BOOK_insertString = "Insert into Books (AuthorId, Title, PRICE, PAGES) values (@authorID,'SUPERBOOK', '1000','500') SET @ID = SCOPE_IDENTITY();";
                    SqlCommand cmd_book = new SqlCommand(BOOK_insertString, conn);

                    SqlParameter authIDbook_param = new SqlParameter("@authorID", authorID);
                    cmd_book.Parameters.Add(authIDbook_param);

                    SqlParameter bookID_param = new SqlParameter("@ID", SqlDbType.Int, 4);
                    bookID_param.Direction = ParameterDirection.Output;
                    cmd_book.Parameters.Add(bookID_param);

                    // виликаємо ExecuteNonQuery для відправки команди (запис в таблицю авторів)
                    cmd_book.ExecuteNonQuery();

                    Console.WriteLine("============");
                    Console.WriteLine("Працюю із БД... Все працює як слід...");
                    Console.WriteLine("ID автора що ми додалу в таблицю авторів - {0}", authorID);
                    Console.WriteLine("ID книги що ми додалу в таблицю книг - {0}", bookID_param.Value);
                    Thread.Sleep(3000);

                    Console.WriteLine("Вертаюсь в головне меню...");
                    Thread.Sleep(2000);

                }
                finally
                {
                    // Закриваємо підключення
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

        /// <summary>
        /// функція читання
        /// </summary>
        /// <param name="conn"></param>
        private static void ReadData(SqlConnection conn)
        {
            SqlDataReader rdr = null;

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("select * from Authors", conn);

                rdr = cmd.ExecuteReader();

                Console.WriteLine("============");
                Console.WriteLine("Працюю із БД... Все працює як слід...");
                Thread.Sleep(3000);


                while (rdr.Read())
                {
                    Console.WriteLine(rdr[1] + " " + rdr[2]);
                }

                Console.WriteLine("============");
                Console.WriteLine("Вертаюсь в головне меню...");
                Thread.Sleep(2000);
            }
            finally
            {
                // закриваємо reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // закриваємо підключення
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// деталызована функція читання
        /// </summary>
        /// <param name="conn"></param>
        private static void ReadData2(SqlConnection conn)
        {
            SqlDataReader rdr = null;

            try
            {
                // відкриваємо підключення
                conn.Open();

                SqlCommand cmd = new SqlCommand("select * from Authors", conn);

                rdr = cmd.ExecuteReader();

                Console.WriteLine("============");
                Console.WriteLine("Працюю із БД... Все працює як слід...");
                Thread.Sleep(3000);

                int line = 0;

                // забираємо рядки
                do
                {
                    while (rdr.Read())
                    {
                        if (line == 0)  // формуємо шапку таблиці
                        {
                            // цикл по числу прочитаних полів
                            for (int i = 0; i < rdr.FieldCount; i++)
                            {
                                // виводмо у додаток імена полів
                                Console.Write(rdr.GetName(i).ToString() + "\t");
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        line++;
                        Console.WriteLine(rdr[0] + "\t" + rdr[1] + "\t" + rdr[2]);
                    }
                    Console.WriteLine("Total records processed: " + line.ToString());
                } while (rdr.NextResult());

                Console.WriteLine("============");
                Console.WriteLine("Вертаюсь в головне меню...");
                Thread.Sleep(3000);

            }
            finally
            {
                // закриваємо reader
                if (rdr != null)
                {
                    rdr.Close();
                }

                // закриваємо connection
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// виконання запиту на основі збережуваної процедури
        /// </summary>
        /// <param name="conn"></param>
        private static void ExecStoredProcedure(SqlConnection conn)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("getBooksNumber", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            Console.WriteLine("Введіть ID автора...");
            int authorID = Convert.ToInt32(Console.ReadLine());

            cmd.Parameters.Add("@AuthorId", System.Data.SqlDbType.Int).Value = authorID;

            SqlParameter outputParam = new SqlParameter("@BookCount", System.Data.SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            //outputParam.Value = 0;
            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();

            Console.WriteLine("============");
            Console.WriteLine("Працюю із БД... Все працює як слід...");
            Thread.Sleep(3000);

            Console.WriteLine("Кількість книг для автора із ID - {0}, складає - {1}", authorID, cmd.Parameters["@BookCount"].Value.ToString());

            Console.WriteLine("============");
            Console.WriteLine("Вертаюсь в головне меню...");
            Thread.Sleep(3000);

        }
    }
}
