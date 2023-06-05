# .NET 6 Mock publisher webhooks template
### How to run manually (.NET 6 required)
IV='[IV GOES HERE]' KEY='[KEY GOES HERE]' dotnet run

### How to build docker image
docker build -t appcharge/webhook-dotnet .

### How to run the container locally
docker run --rm -it -e KEY='[KEY GOES HERE]' -e IV='[IV GOES HERE]' -p8080:80 appcharge/webhook-dotnet

### How to test the container locally
curl --location 'http://localhost:8080/updateBalance' --header 'Content-Type: text/plain' --data '[encrypted data]'

### Where to get the key
Go to the admin panel, open the integration tab, if you are using signature authentication - copy the key from the primary key field, if you are using encryption, take the key from the primary key field and the IV from the secondary key field.
