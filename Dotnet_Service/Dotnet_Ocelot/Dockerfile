

FROM dotnet

LABEL MAINTAINER = "lzqsky@163.com"

COPY ./ /app/

WORKDIR /app

EXPOSE 8000/tcp

ENTRYPOINT ["dotnet","/app/Dotnet_Ocelot.dll"]