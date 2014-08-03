namespace NResp.Client.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Moq;
    using NResp.Client.Commands;
    using Xunit;

    public class RespClientTests
    {
        private readonly Mock<IRespConnection> connection = new Mock<IRespConnection>(MockBehavior.Strict);

        [Fact]
        public void Ctor()
        {
            Assert.Throws<ArgumentNullException>(() => new RespClient(null));
            Assert.DoesNotThrow(() => new RespClient(this.connection.Object));
        }

        [Fact]
        public async Task Set_WithoutExpiration()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = true, Response = "OK" });
            RespClient client = new RespClient(this.connection.Object);

            await client.SetAsync("key", "value", CancellationToken.None);

            this.connection.Verify(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Set_ThrowException()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = false });
            RespClient client = new RespClient(this.connection.Object);

            await Assert.ThrowsAsync<RespException>(() => client.SetAsync("key", "value", CancellationToken.None));
        }

        [Fact]
        public async Task Set_WithExpiration()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = true, Response = "OK" });
            RespClient client = new RespClient(this.connection.Object);

            await client.SetAsync("key", "value", TimeSpan.FromSeconds(10), CancellationToken.None);

            this.connection.Verify(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Get_ReturnsValue()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = true, Response = "myValue" });
            RespClient client = new RespClient(this.connection.Object);

            var result = await client.GetAsync("test", CancellationToken.None);

            Assert.Equal("myValue", result);
        }

        [Fact]
        public async Task Get_UnkownKey_ReturnsNull()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = true, Response = null });
            RespClient client = new RespClient(this.connection.Object);

            var result = await client.GetAsync("test", CancellationToken.None);

            Assert.Null(result);
        }

        [Fact]
        public async Task Get_ThrowException()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = false });
            RespClient client = new RespClient(this.connection.Object);

            await Assert.ThrowsAsync<RespException>(() => client.GetAsync("test", CancellationToken.None));
        }

        [Fact]
        public async Task Append()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = true, Response = "myValue" });
            RespClient client = new RespClient(this.connection.Object);

            await client.AppendAsync("list", "value", CancellationToken.None);

            this.connection.Verify(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Append_ThrowException()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = false });
            RespClient client = new RespClient(this.connection.Object);

            await Assert.ThrowsAsync<RespException>(() => client.AppendAsync("list", "value", CancellationToken.None));
        }


        [Fact]
        public async Task Blpop()
        {
            var response = new RespResponse { Success = true };
            response.Responses.Add("list1");
            response.Responses.Add("myValue");
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);
            RespClient client = new RespClient(this.connection.Object);

            var result = await client.BlockingLeftPopAsync(new[] { "list1", "list2" }, CancellationToken.None);

            this.connection.Verify(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.Equal("myValue", result);
        }

        [Fact]
        public async Task Blpop_ThrowException()
        {
            this.connection
                .Setup(c => c.SendAsync(It.IsAny<RespCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new RespResponse { Success = false });
            RespClient client = new RespClient(this.connection.Object);

            await Assert.ThrowsAsync<RespException>(() => client.BlockingLeftPopAsync(new[] { "list1", "list2" }, CancellationToken.None));
        }
    }
}
