# StreamableNet

[![NuGet](https://img.shields.io/nuget/v/StreamableNet.svg)](https://www.nuget.org/packages/StreamableNet/)  
A **.NET 8.0 client library** for interacting with the [Streamable API](https://streamable.com/).  
It provides an easy-to-use wrapper around authentication, user management, video retrieval, and uploads.

---

## ✨ Features
- 🔑 **Authentication** with username & password via `BasicAuthProvider`
- 👤 **User operations** (authenticate and retrieve account info)
- 🎬 **Video operations** (fetch single videos or full collection)
- 📤 **Upload videos** with automatic S3 signing & transcoding
- ⚡ Built on `HttpClient` with async/await support
- 🚀 Compatible with **.NET 8.0**

---

## 📦 Installation

Install via **NuGet**:

```bash
dotnet add package StreamableNet
````

Or using the **NuGet Package Manager** in Visual Studio:

```powershell
Install-Package StreamableNet
```

---

## 🚀 Usage

### Authentication & Client Setup

```csharp
using StreamableNet;
using StreamableNet.Auth;

var authProvider = new BasicAuthProvider("your-username", "your-password");
using var client = new StreamableClient(authProvider);

// Authenticate user
var user = await client.User.AuthenticateAsync();
```

---

### Fetch Videos

```csharp
// Get all videos
var videos = await client.Video.GetVideosAsync();
Console.WriteLine($"You have {videos?.Total} videos");

// Get a specific video by ID
var video = await client.Video.GetVideoAsync("12345");
Console.WriteLine($"Video Title: {video?.Title}");
```

---

### Upload a Video

```csharp
string shortcode = await client.Upload.UploadFileAsync("video.mp4", "My Test Upload");
Console.WriteLine($"Uploaded with shortcode: {shortcode}");
```

---

## 📂 Project Structure

```
StreamableNet/
├── StreamableClient.cs         # Main entry point
├── Auth/                       # Authentication providers
├── Clients/                    # User, Video, and Upload clients
├── Models/                     # DTOs for API responses
├── Exceptions/                 # Custom exception handling
├── Utils/                      # AWS Signature V4 helpers
├── Consts/                     # API configuration options
└── StreamableNet.csproj
```

---

## 🛠 Requirements

* .NET 8.0 SDK

---

## 🤝 Contributing

Contributions are welcome! Please open an issue or submit a PR.

---

## 📜 License

This project is licensed under the terms of the [MIT License](./LICENSE.txt).

---

