namespace NResp.Client.Integration.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NResp.Client.Commands;
    using Xunit;

    public class RespConnectionTests : IDisposable
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
            await this.connection.SendAsync(new SetCommand("mykey", "Hello"), CancellationToken.None);
            RespCommand nonExistingCommand = new GetCommand("nonexisting");
            RespCommand command = new GetCommand("mykey");

            // Act
            RespResponse nonExistingResult = await this.connection.SendAsync(nonExistingCommand, CancellationToken.None);
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(nonExistingResult);
            Assert.True(nonExistingResult.Success);
            Assert.Equal(-1, nonExistingResult.Length);
            Assert.Null(nonExistingResult.Response);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(5, result.Length);
            Assert.Equal("Hello", result.Response);
        }

        [Fact]
        public async Task Lpop()
        {
            // Arrange
            await this.connection.SendAsync(new RPushCommand("mylist", "one"), CancellationToken.None);
            await this.connection.SendAsync(new RPushCommand("mylist", "two"), CancellationToken.None);
            await this.connection.SendAsync(new RPushCommand("mylist", "three"), CancellationToken.None);
            LPopCommand command = new LPopCommand("mylist");
            // TODO : LRANGE

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);
            RespResponse resultRange = await this.connection.SendAsync(new LRangeCommand("mylist", 0, -1), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("one", result.Response);
            Assert.NotNull(resultRange);
            Assert.True(resultRange.Success);
            Assert.Equal("two", resultRange.Responses[0]);
            Assert.Equal("three", resultRange.Responses[1]);
        }

        [Fact]
        public async Task Rpop()
        {
            // Arrange
            LPopCommand command = new LPopCommand("list1");

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
            await this.connection.SendAsync(new DelCommand("list1", "list2"), CancellationToken.None);
            await this.connection.SendAsync(new RPushCommand("list1", "a", "b", "c"), CancellationToken.None);
            BlpopCommand command = new BlpopCommand("list1", "list2");
            
            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal("list1", result.Responses[0]);
            Assert.Equal("a", result.Responses[1]);
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

        [Fact]
        public async Task GetRange()
        {
            // Arrange
            await this.connection.SendAsync(new SetCommand("mykey", "This is a string"), CancellationToken.None);
            var command1 = new GetRangeCommand("mykey", 0, 3);
            var command2 = new GetRangeCommand("mykey", -3, -1);
            var command3 = new GetRangeCommand("mykey", 0, -1);
            var command4 = new GetRangeCommand("mykey", 10, 100);

            // Act
            RespResponse result1 = await this.connection.SendAsync(command1, CancellationToken.None);
            RespResponse result2 = await this.connection.SendAsync(command2, CancellationToken.None);
            RespResponse result3 = await this.connection.SendAsync(command3, CancellationToken.None);
            RespResponse result4 = await this.connection.SendAsync(command4, CancellationToken.None);

            // Assert
            Assert.True(result1.Success);
            Assert.Equal("This", result1.Response);
            Assert.True(result2.Success);
            Assert.Equal("ing", result2.Response);
            Assert.True(result3.Success);
            Assert.Equal("This is a string", result3.Response);
            Assert.True(result4.Success);
            Assert.Equal("string", result4.Response);
        }

        [Fact]
        public async Task Append()
        {
            // Arrange
            await this.connection.SendAsync(new DelCommand("mykey"), CancellationToken.None);
            var command1 = new AppendCommand("mykey", "Hello");
            var command2 = new AppendCommand("mykey", " World");

            // Act
            RespResponse result1 = await this.connection.SendAsync(command1, CancellationToken.None);
            RespResponse result2 = await this.connection.SendAsync(command2, CancellationToken.None);

            RespResponse result = await this.connection.SendAsync(new GetCommand("mykey"), CancellationToken.None);

            // Assert
            Assert.Equal(5, result1.Length);
            Assert.Equal(11, result2.Length);
            Assert.True(result.Success);
            Assert.Equal("Hello World", result.Response);
        }

        [Fact]
        public async Task Exists()
        {
            // Arrange
            await this.connection.SendAsync(new SetCommand("key1", "Hello"), CancellationToken.None);
            var command1 = new ExistsCommand("key1");
            var command2 = new ExistsCommand("key2");

            // Act
            RespResponse result1 = await this.connection.SendAsync(command1, CancellationToken.None);
            RespResponse result2 = await this.connection.SendAsync(command2, CancellationToken.None);


            // Assert
            Assert.True(result1.Success);
            Assert.Equal(1, result1.Length);
            Assert.True(result2.Success);
            Assert.Equal(0, result2.Length);
        }

        [Fact]
        public async Task Del()
        {
            // Arrange
            await this.connection.SendAsync(new SetCommand("key1", "Hello"), CancellationToken.None);
            await this.connection.SendAsync(new SetCommand("key2", "World"), CancellationToken.None);
            var command = new DelCommand("key1", "key2", "key3");

            // Act
            RespResponse result = await this.connection.SendAsync(command, CancellationToken.None);


            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Length);
        }

        [Fact]
        public async Task Select()
        {
            // Arrange
            var command = new SelectCommand(0);

            // Act
            RespResponse result1 = await this.connection.SendAsync(command, CancellationToken.None);


            // Assert
            Assert.True(result1.Success);
            Assert.Equal("OK", result1.Response);
        }

        [Fact]
        public async Task FlushDb()
        {
            // Arrange
            var command = new FlushDbCommand();

            // Act
            RespResponse result1 = await this.connection.SendAsync(command, CancellationToken.None);
            
            // Assert
            Assert.True(result1.Success);
            Assert.Equal("OK", result1.Response);
        }

        private void Clean()
        {
            this.connection.SendAsync(new FlushDbCommand(), CancellationToken.None).Wait();
        }

        public void Dispose()
        {
            this.Clean();
        }
    }
}
