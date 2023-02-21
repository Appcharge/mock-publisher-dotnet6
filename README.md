# .NET 6 Mock publisher webhooks template
### How to run
IV='IV GOES HERE' KEY='KEY GOES HERE' dotnet run

### How to build docker image
docker build -t appcharge/webhook-dotnet .

### How to run container locally
docker run --rm -it -e KEY='KEY GOES HERE' -e IV='IV GOES HERE' -p8080:80 appcharge/webhook-dotnet

### How to test container locally
    curl --location 'http://localhost:8080/updateBalance' --header 'Content-Type: text/plain' --data '<encrypted data>'