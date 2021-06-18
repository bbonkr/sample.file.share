# Sample.File.Share

Sample project for file sharing.

* Share files with other users.
* Set expiration when file shared.
* Users who receive file sharing will have access until expires.

## Run

```bash
$ cd src/Sample.App
$ dotnet run
```

## Docker

### Build

```bash
$ docker build . -t your-name/app-name:tag
```

### Run

```bash
$ docker run -d \
  --name sample_file_share \
  -e TZ=Asia/Seoul \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:5000 \
  -e ConnectionStrings__Database=<database connection string> \
  -e ConnectionStrings__AzureStorageAccount=<Azure Storage account connection string> \
  -e App__Title="File Share App" \
  -e App__Description="Sample for File sharing" \
  your-name/app-name:tag
```