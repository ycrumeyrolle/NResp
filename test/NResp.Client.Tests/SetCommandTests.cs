namespace NRedis.Client.Tests
{
    using System;
    using NResp.Client;
    using Xunit;

    public class SetCommandTests
    {
        [Fact]
        public void Ctor_GuardClause()
        {
            Assert.Throws<ArgumentNullException>(() => new SetCommand(null, "value"));
            Assert.Throws<ArgumentNullException>(() => new SetCommand("key", null));
            Assert.Throws<ArgumentNullException>(() => new SetCommand(null, "value", TimeSpan.FromSeconds(1)));
            Assert.Throws<ArgumentNullException>(() => new SetCommand("key", null, TimeSpan.FromSeconds(1)));
            Assert.Throws<ArgumentException>(() => new SetCommand("key", "value", TimeSpan.FromSeconds(0)));
        }
    }
}
