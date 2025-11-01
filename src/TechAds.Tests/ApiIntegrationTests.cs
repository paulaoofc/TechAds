using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using TechAds.Api.DTOs;
using TechAds.Domain.Entities;
using TechAds.Domain.ValueObjects;
using TechAds.Domain.Enums;
using TechAds.Infrastructure;
using TechAds.Application.DTOs;
using Xunit;
using FluentAssertions;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's DbContext registration
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TechAdsDbContext>));
                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }
                // Remove the app's DbContext registration by implementation
                var dbContextServiceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(TechAdsDbContext));
                if (dbContextServiceDescriptor != null)
                {
                    services.Remove(dbContextServiceDescriptor);
                }
                // Remove any existing database provider services
                var descriptorsToRemove = services.Where(d => d.ServiceType.Namespace?.StartsWith("Microsoft.EntityFrameworkCore") == true).ToList();
                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<TechAdsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Login_ValidCredentials_ShouldReturnToken()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TechAdsDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Create roles if they don't exist
        if (!await roleManager.RoleExistsAsync("Candidate"))
            await roleManager.CreateAsync(new IdentityRole("Candidate"));
        
        var user = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = "test@example.com", UserName = "test@example.com" };
        var result = await userManager.CreateAsync(user);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("password");
        await userManager.UpdateAsync(user);
        await userManager.AddToRoleAsync(user, "Candidate");
        await dbContext.SaveChangesAsync();

        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "password" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var resultContent = await response.Content.ReadFromJsonAsync<LoginResponse>();
        resultContent.Should().NotBeNull();
        resultContent.Token.Should().NotBeNullOrEmpty();
        resultContent.User.Should().NotBeNull();
        resultContent.User.Email.Should().Be("test@example.com");
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturnUnauthorized()
    {
        var loginRequest = new LoginRequest { Email = "invalid@example.com", Password = "wrong" };

        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateListing_AuthenticatedUser_ShouldCreateListing()
    {
        // Arrange - Login first
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TechAdsDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Create roles if they don't exist
        if (!await roleManager.RoleExistsAsync("Candidate"))
            await roleManager.CreateAsync(new IdentityRole("Candidate"));
        
        var user = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = "test@example.com", UserName = "test@example.com" };
        await userManager.CreateAsync(user);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("password");
        await userManager.UpdateAsync(user);
        await userManager.AddToRoleAsync(user, "Candidate");
        await dbContext.SaveChangesAsync();

        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "password" };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);

        var createRequest = new CreateListingRequest
        {
            Title = "Test Project",
            ShortDescription = "A test project",
            Requirements = "C# knowledge required",
            Tags = new List<string> { "csharp", "dotnet" }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/listings", createRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ListingDto>();
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Project");
        result.Tags.Should().Contain("csharp");
    }

    [Fact]
    public async Task GetListings_ShouldReturnListings()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TechAdsDbContext>();
        var user = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = "test@example.com", UserName = "test@example.com" };
        user.PasswordHash = new Microsoft.AspNetCore.Identity.PasswordHasher<IdentityUser>().HashPassword(user, "password");
        var listing = new ProjectListing(Guid.NewGuid(), "Test Listing", "Description", "Requirements", new List<Tag> { new Tag("csharp") }, Guid.Parse(user.Id), ListingStatus.Published, DateTime.UtcNow);
        dbContext.Users.Add(user);
        dbContext.ProjectListings.Add(listing);
        await dbContext.SaveChangesAsync();

        // Act
        var response = await _client.GetAsync("/api/listings");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<ListingDto>>();
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task SubmitApplication_AuthenticatedUser_ShouldCreateApplication()
    {
        // Arrange - Login and create listing
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TechAdsDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Create roles if they don't exist
        if (!await roleManager.RoleExistsAsync("Candidate"))
            await roleManager.CreateAsync(new IdentityRole("Candidate"));
        
        var user = new IdentityUser { Id = Guid.NewGuid().ToString(), Email = "test@example.com", UserName = "test@example.com" };
        await userManager.CreateAsync(user);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("password");
        await userManager.UpdateAsync(user);
        await userManager.AddToRoleAsync(user, "Candidate");
        var listing = new ProjectListing(Guid.NewGuid(), "Test Listing", "Description", "Requirements", new List<Tag> { new Tag("csharp") }, Guid.Parse(user.Id), ListingStatus.Published, DateTime.UtcNow);
        dbContext.ProjectListings.Add(listing);
        await dbContext.SaveChangesAsync();

        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "password" };
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.Token);

        var submitRequest = new SubmitApplicationRequest
        {
            Message = "I am interested in this project"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/listings/{listing.Id}/applications", submitRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        result.Should().ContainKey("id");
    }
}

public class LoginResponse
{
    public string Token { get; set; }
    public UserDto User { get; set; }
}