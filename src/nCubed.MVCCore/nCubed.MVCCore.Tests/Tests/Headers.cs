using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using nCubed.MVCCore.Tests.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace nCubed.MVCCore.Tests.Tests
{
    [Collection(Collections.Api)]
    public class Headers
        : IClassFixture<WebApplicationFactory<Sample.Startup>>
    {
        private readonly WebApplicationFactory<Sample.Startup> factory;


        public Headers(WebApplicationFactory<Sample.Startup> factory)
        {
            this.factory = factory;
        }


        [Fact]
        [Trait("Pagination", "Header")]
        public async Task FirstTimeWithOnePage()
        {
            var client = this.factory.CreateClient();
            Pagination p = new Pagination();
            p.TotalCount = 2;
            p.PageSize = 10;
            p.CurrentPage = 1;
            p.TotalPage = 1;
            p.PreviousPageLink = "";
            p.NextPageLink = "";

            var response = await client.GetAsync("api/author/few");

            response.EnsureSuccessStatusCode();
            response.Headers.TryGetValues("X-Pagination", out var headers);
            response.Headers.First(x => x.Key == "X-Pagination").Value.Should().NotBeNull()
                .And.HaveCount(1);
            response.Headers.First(x => x.Key == "X-Pagination").Value.First().Should().Be(p.Serialize());
        }

        [Fact]
        [Trait("Pagination", "Header")]
        public async Task FirstTimeWithSeveralPages()
        {
            var client = this.factory.CreateClient();
            Pagination p = new Pagination();
            p.TotalCount = 99;
            p.PageSize = 10;
            p.CurrentPage = 1;
            p.TotalPage = 10;
            p.PreviousPageLink = "";
            p.NextPageLink = "/api/author/big?pageNumber=2";

            var response = await client.GetAsync("api/author/big");

            response.EnsureSuccessStatusCode();
            response.Headers.TryGetValues("X-Pagination", out var headers);
            response.Headers.First(x => x.Key == "X-Pagination").Value.Should().NotBeNull()
                .And.HaveCount(1);
            response.Headers.First(x => x.Key == "X-Pagination").Value.First().Should().Be(p.Serialize());
        }

        [Fact]
        [Trait("Pagination", "Header")]
        public async Task CurrentPageGreaterThanTotalPage()
        {
            var client = this.factory.CreateClient();
            Pagination p = new Pagination();
            p.TotalCount = 99;
            p.PageSize = 10;
            p.CurrentPage = 10;
            p.TotalPage = 10;
            p.PreviousPageLink = "/api/author/big?pageNumber=9";
            p.NextPageLink = string.Empty;

            var response = await client.GetAsync("api/author/big?PageNumber=10&PageSize=10");

            response.EnsureSuccessStatusCode();
            response.Headers.TryGetValues("X-Pagination", out var headers);
            response.Headers.First(x => x.Key == "X-Pagination").Value.Should().NotBeNull()
                .And.HaveCount(1);
            response.Headers.First(x => x.Key == "X-Pagination").Value.First().Should().Be(p.Serialize());
        }

        [Fact]
        [Trait("Pagination", "Header")]
        public async Task CurrentPageLowerThanZero()
        {
            var client = this.factory.CreateClient();
            Pagination p = new Pagination();
            p.TotalCount = 99;
            p.PageSize = 10;
            p.CurrentPage = -1;
            p.TotalPage = 10;
            p.PreviousPageLink = string.Empty;
            p.NextPageLink = "/api/author/big?pageNumber=2";

            var response = await client.GetAsync("api/author/big?PageNumber=-1&PageSize=10");

            response.EnsureSuccessStatusCode();
            response.Headers.TryGetValues("X-Pagination", out var headers);
            response.Headers.First(x => x.Key == "X-Pagination").Value.Should().NotBeNull()
                .And.HaveCount(1);
            response.Headers.First(x => x.Key == "X-Pagination").Value.First().Should().Be(p.Serialize());
        }

        [Fact]
        [Trait("Pagination", "Header")]
        public async Task PreviousAndNext()
        {
            var client = this.factory.CreateClient();
            Pagination p = new Pagination();
            p.TotalCount = 99;
            p.PageSize = 10;
            p.CurrentPage = 5;
            p.TotalPage = 10;
            p.PreviousPageLink = "/api/author/big?pageNumber=4";
            p.NextPageLink = "/api/author/big?pageNumber=6";

            var response = await client.GetAsync("api/author/big?PageNumber=5&PageSize=10");

            response.EnsureSuccessStatusCode();
            response.Headers.TryGetValues("X-Pagination", out var headers);
            response.Headers.First(x => x.Key == "X-Pagination").Value.Should().NotBeNull()
                .And.HaveCount(1);
            response.Headers.First(x => x.Key == "X-Pagination").Value.First().Should().Be(p.Serialize());
        }

        [Fact]
        [Trait("Pagination", "Header")]
        public async Task LastPage()
        {
            var client = this.factory.CreateClient();
            Pagination p = new Pagination();
            p.TotalCount = 99;
            p.PageSize = 10;
            p.CurrentPage = 10;
            p.TotalPage = 10;
            p.PreviousPageLink = "/api/author/big?pageNumber=9";
            p.NextPageLink = "";

            var response = await client.GetAsync("api/author/big?PageNumber=10&PageSize=10");

            response.EnsureSuccessStatusCode();
            response.Headers.TryGetValues("X-Pagination", out var headers);
            response.Headers.First(x => x.Key == "X-Pagination").Value.Should().NotBeNull()
                .And.HaveCount(1);
            response.Headers.First(x => x.Key == "X-Pagination").Value.First().Should().Be(p.Serialize());
        }

        [Fact]
        [Trait("Pagination", "Header")]
        public async Task EmptyResult()
        {
            var client = this.factory.CreateClient();
            Pagination p = new Pagination();
            p.TotalCount = 0;
            p.PageSize = 2;
            p.CurrentPage = 1;
            p.TotalPage = 0;
            p.PreviousPageLink = string.Empty;
            p.NextPageLink = string.Empty;

            var response = await client.GetAsync("api/author/empty?PageNumber=1&PageSize=2");

            response.EnsureSuccessStatusCode();
            response.Headers.TryGetValues("X-Pagination", out var headers);
            response.Headers.First(x => x.Key == "X-Pagination").Value.Should().NotBeNull()
                .And.HaveCount(1);
            response.Headers.First(x => x.Key == "X-Pagination").Value.First().Should().Be(p.Serialize());
        }

        public class Pagination
        {
            public int TotalCount { get; set; }
            public int PageSize { get; set; }
            public int CurrentPage { get; set; }
            public int TotalPage { get; set; }
            public string PreviousPageLink { get; set; }
            public string NextPageLink { get; set; }

            public string Serialize()
            {
                return JsonConvert.SerializeObject(this, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } });
            }
        }
    }
}
