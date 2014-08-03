namespace NResp.Client.Tests
{
    using System;
    using NResp.Client.Commands;
    using Xunit;

    public class RespCommandTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            Assert.Throws<ArgumentNullException>(() => new RespCommand(null, new string[0]));
            Assert.Throws<ArgumentNullException>(() => new RespCommand("name", null));
            Assert.DoesNotThrow(() => new RespCommand("name", new string[0]));
        }
    }
}
