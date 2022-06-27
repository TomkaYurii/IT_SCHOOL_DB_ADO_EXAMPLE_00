using System;
using System.Data;
using System.Data.SqlClient;

namespace IT_SCHOOL_DB_ADO_EXAMPLE_00
{
    internal class Program
    {

        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=DESKTOP-TOMKA;Initial Catalog=IT_SCHOOL_DB_ADO_EXAMPLE_00;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            // Діалог із користувачем
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\ti - Insert");
            Console.WriteLine("\ts - Simple Read");
            Console.WriteLine("\tr - Dertail Read");
            Console.WriteLine("\te - StoredProcedure");
            Console.Write("Your option? ");

            //Вибір того що потрібно зробити
            switch (Console.ReadLine())
            {
                case "i":
                    InsertQuery(conn);
                    break;
                case "s":
                    ReadData(conn);
                    break;
                case "r":
                    ReadData2(conn);
                    break;
                case "e":
                    ExecStoredProcedure(conn);
                    break;
            }
            // Очікуємов поки користувач натисне довільну клавішу.
            Console.Write("Press any key to close console app...");
            Console.ReadKey();
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

                // Підготовка рядка підключення
                string insertString = "Insert into Authors (FirstName, LastName) values ('SUPERNAME', 'SUPERLASTNAME')";

                // 1. Реалізуємо команду для query та відповідного підключення
                SqlCommand cmd = new SqlCommand(insertString, conn);

                // 2. Вкиликаємо ExecuteNonQuery для відправки команди 
                cmd.ExecuteNonQuery();
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
        public static void ReadData(SqlConnection conn)
        {
            SqlDataReader rdr = null;

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("select * from Authors", conn);

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[1] + " " + rdr[2]);
                }
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
        public static void ReadData2(SqlConnection conn)
        {
            SqlDataReader rdr = null;

            try
            {
                // відкриваємо підключення
                conn.Open();

                SqlCommand cmd = new SqlCommand("select * from Authors", conn);

                rdr = cmd.ExecuteReader();
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
        public static void ExecStoredProcedure(SqlConnection conn)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("getBooksNumber", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@AuthorId", System.Data.SqlDbType.Int).Value = 1;

            SqlParameter outputParam = new SqlParameter("@BookCount", System.Data.SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            //outputParam.Value = 0;
            cmd.Parameters.Add(outputParam);

            cmd.ExecuteNonQuery();
            Console.WriteLine(cmd.Parameters["@BookCount"].Value.ToString());

        }
    }
}
