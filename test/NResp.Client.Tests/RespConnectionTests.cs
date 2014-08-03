namespace NResp.Client.Tests
{
    using System;
    using Xunit;

    public class RespConnectionTests
    {
        private const string DefaultHostName = "localhost";
        private const ushort DefaultPort = 6379;

        [Fact]
        public void Ctor()
        {
            // Act
            RespConnection connection = new RespConnection(DefaultHostName, DefaultPort);

            // Assert
            Assert.Equal(DefaultHostName, connection.HostName);
            Assert.Equal(DefaultPort, connection.Port);
        }

        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new RespConnection(null, DefaultPort));
        }
    }
}
