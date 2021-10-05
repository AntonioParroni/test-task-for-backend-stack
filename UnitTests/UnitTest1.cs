using BLL.DapperRepo;
using NUnit.Framework;

namespace UnitTests
{
    public class Tests
    {
        private RegistrationsRepository repo;
        [SetUp]
        public void Setup()
        {
            repo = new RegistrationsRepository(
                "Server=localhost;Database=Contoso_Authentication_Logs;User Id=sa;Password=Password123;");
        }

        [Test]
        public void Test1()
        {
            Assert.IsNotEmpty(repo.GetRegistrationByCurrentMonth());

        }
    }
}