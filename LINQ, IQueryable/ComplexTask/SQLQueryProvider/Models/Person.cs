namespace SQLQueryProvider.Models
{
    public class Person : BaseSqlEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
    }
}
