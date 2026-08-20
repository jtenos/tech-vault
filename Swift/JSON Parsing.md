```swift
// Swift json parsing

// https://www.avanderlee.com/swift/json-parsing-decoding/
/*
{
	"title": "Optionals in Swift explained: 5 things you should know",
	"url": "https://www.avanderlee.com/swift/optionals-in-swift-explained-5-things-you-should-know/",
	"category": "Swift",
	"views": 47093
}
*/

struct BlogPost: Decodable {
    enum Category: String, Decodable {
        case swift, combine, debugging, xcode
    }

    let title: String
    let url: URL
    let category: Category
    let views: Int
}

let JSON = """
{
    "title": "Optionals in Swift explained: 5 things you should know",
    "url": "https://www.avanderlee.com/swift/optionals-in-swift-explained-5-things-you-should-know/",
    "category": "swift",
    "views": 47093
}
"""

let jsonData = JSON.data(using: .utf8)!
let blogPost: BlogPost = try! JSONDecoder().decode(BlogPost.self, from: jsonData)

print(blogPost.title)


//Arrays:

[{
    "title": "Optionals in Swift explained: 5 things you should know",
    "url": "https://www.avanderlee.com/swift/optionals-in-swift-explained-5-things-you-should-know/"
},
{
    "title": "EXC_BAD_ACCESS crash error: Understanding and solving it",
    "url": "https://www.avanderlee.com/swift/exc-bad-access-crash/"
},
{
    "title": "Thread Sanitizer explained: Data Races in Swift",
    "url": "https://www.avanderlee.com/swift/thread-sanitizer-data-races/"
}]

struct BlogPost: Decodable {
    let title: String
    let url: URL
}

let blogPosts: [BlogPost] = try! JSONDecoder().decode([BlogPost].self, from: jsonData)
print(blogPosts.count)


//Custom property names:

struct BlogPost: Decodable {
    enum Category: String, Decodable {
        case swift, combine, debugging, xcode
    }

    enum CodingKeys: String, CodingKey {
        case title, category, views
        // Map the JSON key "url" to the Swift property name "htmlLink"
        case htmlLink = "url"
    }

    let title: String
    let htmlLink: URL
    let category: Category
    let views: Int
}


let blogPost: BlogPost = try! JSONDecoder().decode(BlogPost.self, from: jsonData)
print(blogPost.htmlLink)


//Camel from snake case:

{
    "title": "A weekly Swift Blog on Xcode and iOS Development - SwiftLee",
    "url": "https://www.avanderlee.com",
    "total_visitors": 378483,
    "number_of_posts": 47093
}

struct Blog: Decodable {
    let title: String
    let url: URL
    let totalVisitors: Int
    let numberOfPosts: Int
}

let decoder = JSONDecoder()
decoder.keyDecodingStrategy = .convertFromSnakeCase

let blog: Blog = try! decoder.decode(Blog.self, from: jsonData)
print(blog.numberOfPosts)


//Custom dates:

{
    "title": "Optionals in Swift explained: 5 things you should know",
    "date": "2019-10-21T09:15:00Z"
}

struct BlogPost: Decodable {
    let title: String
    let date: Date
}

let decoder = JSONDecoder()
let dateFormatter = DateFormatter()
dateFormatter.dateFormat = "yyyy-MM-dd'T'HH:mm:ssZ"
dateFormatter.locale = Locale(identifier: "en_US")
dateFormatter.timeZone = TimeZone(secondsFromGMT: 0)
decoder.dateDecodingStrategy = .formatted(dateFormatter)

let blogPost: BlogPost = try! decoder.decode(BlogPost.self, from: jsonData)
print(blogPost.date)
```