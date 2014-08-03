namespace NResp.Client.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class RespConnectionTests
    {
        private const string DefaultHost = "localhost";
        private const ushort DefaultPort = 6379;

        [Fact]
        public void Ctor()
        {
            // Act
            RespConnection connection = new RespConnection(DefaultHost, DefaultPort);

            // Assert
            Assert.Equal(DefaultHost, connection.Host);
            Assert.Equal(DefaultPort, connection.Port);
        }

        [Fact]
        public void Ctor_GuardClause()
        {
            // Act & assert
            Assert.Throws<ArgumentNullException>(() => new RespConnection(null, DefaultPort));
        }

        [Fact]
        public async Task SendAsync_Ping_Replies_Pong()
        {
            // Arrange
            RespConnection connection = new RespConnection(DefaultHost, DefaultPort);
            RespCommand command = new PingCommand();

            // Act
            RespResponse result = await connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("PONG", result.Response);
        }

        [Fact]
        public async Task SendInvalidCommand()
        {
            // Arrange
            RespConnection connection = new RespConnection(DefaultHost, DefaultPort);
            RespCommand command = new RespCommand("XXXX");


            // Act
            RespResponse result = await connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Set()
        {
            // Arrange
            RespConnection connection = new RespConnection(DefaultHost, DefaultPort);
            RespCommand command = new SetCommand("mykey", "myValue");

            // Act
            RespResponse result = await connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("OK", result.Response);
        }

        [Fact]
        public async Task Set_WithExpiration()
        {
            // Arrange
            RespConnection connection = new RespConnection(DefaultHost, DefaultPort);
            RespCommand command = new SetCommand("mykey", "myValue", TimeSpan.FromSeconds(1));

            // Act
            RespResponse result = await connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("OK", result.Response);
        }

        [Fact]
        public async Task Get_NonExisting()
        {
            // Arrange
            RespConnection connection = new RespConnection(DefaultHost, DefaultPort);
            RespCommand command = new GetCommand("X");

            // Act
            RespResponse result = await connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(-1, result.Length);
            Assert.Equal(null, result.Response);
        }

        [Fact]
        public async Task Get_Existing()
        {
            // Arrange
            RespConnection connection = new RespConnection(DefaultHost, DefaultPort);
            RespCommand command = new GetCommand("mykey");

            // Act
            RespResponse result = await connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(7, result.Length    );
            Assert.Equal("myValue", result.Response);
        }
    }
}
