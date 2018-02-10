

FROM dotnet

LABEL MAINTAINER = "lzqsky@163.com"

COPY ./ /app/

WORKDIR /app

EXPOSE 8002/tcp

ENTRYPOINT ["dotnet","/app/Dotnet_Service.dll"]