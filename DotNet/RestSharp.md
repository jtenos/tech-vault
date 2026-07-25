```csharp
using RestSharp;
using System.Text.Json;

// JSONPlaceholder API - https://jsonplaceholder.typicode.com/
// A free fake REST API for testing and prototyping

var client = new RestClient("https://jsonplaceholder.typicode.com");

// ============================================================================
// GET - Retrieve a single resource
// ============================================================================
Console.WriteLine("=== GET Single Post ===");
var getRequest = new RestRequest("/posts/1", Method.Get);
var getResponse = await client.ExecuteAsync<Post>(getRequest);

if (getResponse.IsSuccessful && getResponse.Data != null)
{
    Console.WriteLine($"Title: {getResponse.Data.Title}");
    Console.WriteLine($"Body: {getResponse.Data.Body}");
    Console.WriteLine($"UserId: {getResponse.Data.UserId}");
}
else
{
    Console.WriteLine($"Error: {getResponse.ErrorMessage}");
}

// ============================================================================
// GET - Retrieve a collection of resources
// ============================================================================
Console.WriteLine("\n=== GET All Posts ===");
var getAllRequest = new RestRequest("/posts", Method.Get);
var getAllResponse = await client.ExecuteAsync<List<Post>>(getAllRequest);

if (getAllResponse.IsSuccessful && getAllResponse.Data != null)
{
    Console.WriteLine($"Total posts: {getAllResponse.Data.Count}");
    foreach (var post in getAllResponse.Data.Take(3))
    {
        Console.WriteLine($"- Post {post.Id}: {post.Title}");
    }
}

// ============================================================================
// GET - With query parameters
// ============================================================================
Console.WriteLine("\n=== GET Posts by UserId ===");
var getWithParamsRequest = new RestRequest("/posts", Method.Get);
getWithParamsRequest.AddQueryParameter("userId", "1");
var getWithParamsResponse = await client.ExecuteAsync<List<Post>>(getWithParamsRequest);

if (getWithParamsResponse.IsSuccessful && getWithParamsResponse.Data != null)
{
    Console.WriteLine($"Posts by user 1: {getWithParamsResponse.Data.Count}");
    foreach (var post in getWithParamsResponse.Data.Take(2))
    {
        Console.WriteLine($"- {post.Title}");
    }
}

// ============================================================================
// POST - Create a new resource
// ============================================================================
Console.WriteLine("\n=== POST Create New Post ===");
var postRequest = new RestRequest("/posts", Method.Post);
var newPost = new Post
{
    Title = "My New Post",
    Body = "This is the content of my new post created with RestSharp.",
    UserId = 1
};
postRequest.AddJsonBody(newPost);
var postResponse = await client.ExecuteAsync<Post>(postRequest);

if (postResponse.IsSuccessful && postResponse.Data != null)
{
    Console.WriteLine($"Created post with ID: {postResponse.Data.Id}");
    Console.WriteLine($"Title: {postResponse.Data.Title}");
}

// ============================================================================
// PUT - Update an existing resource (full update)
// ============================================================================
Console.WriteLine("\n=== PUT Update Post ===");
var putRequest = new RestRequest("/posts/1", Method.Put);
var updatedPost = new Post
{
    Id = 1,
    Title = "Updated Title",
    Body = "Updated content for the post.",
    UserId = 1
};
putRequest.AddJsonBody(updatedPost);
var putResponse = await client.ExecuteAsync<Post>(putRequest);

if (putResponse.IsSuccessful && putResponse.Data != null)
{
    Console.WriteLine($"Updated post {putResponse.Data.Id}");
    Console.WriteLine($"New title: {putResponse.Data.Title}");
}

// ============================================================================
// PATCH - Partial update of a resource
// ============================================================================
Console.WriteLine("\n=== PATCH Partial Update ===");
var patchRequest = new RestRequest("/posts/1", Method.Patch);
patchRequest.AddJsonBody(new { Title = "Patched Title Only" });
var patchResponse = await client.ExecuteAsync<Post>(patchRequest);

if (patchResponse.IsSuccessful && patchResponse.Data != null)
{
    Console.WriteLine($"Patched post {patchResponse.Data.Id}");
    Console.WriteLine($"New title: {patchResponse.Data.Title}");
}

// ============================================================================
// DELETE - Remove a resource
// ============================================================================
Console.WriteLine("\n=== DELETE Post ===");
var deleteRequest = new RestRequest("/posts/1", Method.Delete);
var deleteResponse = await client.ExecuteAsync(deleteRequest);

if (deleteResponse.IsSuccessful)
{
    Console.WriteLine($"Post deleted successfully. Status: {deleteResponse.StatusCode}");
}

// ============================================================================
// GET - With custom headers
// ============================================================================
Console.WriteLine("\n=== GET With Custom Headers ===");
var headersRequest = new RestRequest("/posts/1", Method.Get);
headersRequest.AddHeader("Accept", "application/json");
headersRequest.AddHeader("User-Agent", "RestSharpExample/1.0");
var headersResponse = await client.ExecuteAsync<Post>(headersRequest);

if (headersResponse.IsSuccessful && headersResponse.Data != null)
{
    Console.WriteLine($"Retrieved with headers: {headersResponse.Data.Title}");
}

// ============================================================================
// GET - Nested resources (comments for a post)
// ============================================================================
Console.WriteLine("\n=== GET Nested Resources (Comments) ===");
var commentsRequest = new RestRequest("/posts/1/comments", Method.Get);
var commentsResponse = await client.ExecuteAsync<List<Comment>>(commentsRequest);

if (commentsResponse.IsSuccessful && commentsResponse.Data != null)
{
    Console.WriteLine($"Total comments on post 1: {commentsResponse.Data.Count}");
    foreach (var comment in commentsResponse.Data.Take(2))
    {
        Console.WriteLine($"- {comment.Name} by {comment.Email}");
    }
}

// ============================================================================
// Error Handling - Handling non-existent resource
// ============================================================================
Console.WriteLine("\n=== Error Handling ===");
var errorRequest = new RestRequest("/posts/99999", Method.Get);
var errorResponse = await client.ExecuteAsync<Post>(errorRequest);

if (!errorResponse.IsSuccessful)
{
    Console.WriteLine($"Status Code: {errorResponse.StatusCode}");
    Console.WriteLine($"Error Message: {errorResponse.ErrorMessage ?? "Resource not found"}");
}

// ============================================================================
// Working with raw JSON response
// ============================================================================
Console.WriteLine("\n=== Raw JSON Response ===");
var rawRequest = new RestRequest("/users/1", Method.Get);
var rawResponse = await client.ExecuteAsync(rawRequest);

if (rawResponse.IsSuccessful && rawResponse.Content != null)
{
    Console.WriteLine("Raw JSON:");
    Console.WriteLine(rawResponse.Content);
    
    // Parse manually if needed
    var user = JsonSerializer.Deserialize<User>(rawResponse.Content, new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    });
    Console.WriteLine($"\nParsed User: {user?.Name} ({user?.Email})");
}

// ============================================================================
// Model Classes
// ============================================================================
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public int UserId { get; set; }
}

public class Comment
{
    public int PostId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Address? Address { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public Company? Company { get; set; }
}

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string Suite { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Zipcode { get; set; } = string.Empty;
    public Geo? Geo { get; set; }
}

public class Geo
{
    public string Lat { get; set; } = string.Empty;
    public string Lng { get; set; } = string.Empty;
}

public class Company
{
    public string Name { get; set; } = string.Empty;
    public string CatchPhrase { get; set; } = string.Empty;
    public string Bs { get; set; } = string.Empty;
}
```