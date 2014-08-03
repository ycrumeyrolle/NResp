namespace NResp.Client.Tests
{
    using System;
    using NResp.Client.Commands;
    using Xunit;

    public class BlpopCommandTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            Assert.Throws<ArgumentNullException>(() => new BlpopCommand((string)null));
            Assert.Throws<ArgumentException>(() => new BlpopCommand("list1", null));
        }
    }
}
