namespace NResp.Client.Integration.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class RespConnectionTests
    {
        private readonly RespConnection connection;

        private const string DefaultHostName = "localhost";
        private const ushort DefaultPort = 6379;
        
        public RespConnectionTests()
        {
            this.connection = new RespConnection(DefaultHostName, DefaultPort);
        }

        [Fact]
        public async Task SendAsync_Ping_RepliesPong()
        {
            // Arrange
            RespCommand command = new PingCommand();

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("PONG", result.Response);
        }

        [Fact]
        public async Task SendInvalidCommand()
        {
            // Arrange
            RespCommand command = new RespCommand("XXXX");


            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Set()
        {
            // Arrange
            RespCommand command = new SetCommand("mykey", "myValue");

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("OK", result.Response);
        }

        [Fact]
        public async Task Set_WithExpiration()
        {
            // Arrange
            RespCommand command = new SetCommand("mykey", "myValue", TimeSpan.FromSeconds(1));

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("OK", result.Response);
        }

        [Fact]
        public async Task Get_NonExisting()
        {
            // Arrange
            RespCommand command = new GetCommand("X");

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

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
            RespCommand command = new GetCommand("mykey");

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(7, result.Length);
            Assert.Equal("myValue", result.Response);
        }

        [Fact]
        public async Task Lpop()
        {
            // Arrange
            LpopCommand command = new LpopCommand("list1");

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }


        [Fact]
        public async Task Blpop()
        {
            // Arrange
            BlpopCommand command = new BlpopCommand("list1", "list2");
            CancellationTokenSource tcs = new CancellationTokenSource(1000);
            
            // Act
            RespResponse result = await this.connection.SendAsync(command, tcs.Token);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task LPush()
        {
            // Arrange
            LPushCommand command = new LPushCommand("list1", "value1");

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
        }
    }
}
