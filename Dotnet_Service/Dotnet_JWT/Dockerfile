FROM dotnet

LABEL MAINTAINER = "lzqsky@163.com"

COPY ./ /app/

WORKDIR /app

EXPOSE 8001/tcp

ENTRYPOINT ["dotnet","/app/Dotnet_JWT.dll"]
