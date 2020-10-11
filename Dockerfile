FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY Simulation.Infrastructure/Simulation.Infrastructure.csproj Simulation.Infrastructure/
COPY Simulation.Models/Simulation.Models.csproj Simulation.Models/
COPY Simulation.Medium/Simulation.Medium.csproj Simulation.Medium/
COPY Simulation.DDA/Simulation.DDA.csproj Simulation.DDA/
COPY Simulation.DDA.Grpc/Simulation.DDA.Grpc.csproj Simulation.DDA.Grpc/
COPY Simulation.DDA.Console/Simulation.DDA.Console.csproj Simulation.DDA.Console/
COPY Simulation.FDTD/Simulation.FDTD.csproj Simulation.FDTD/
COPY Simulation.FDTD.Grpc/Simulation.FDTD.Grpc.csproj Simulation.FDTD.Grpc/
COPY Simulation.FDTD.Console/Simulation.FDTD.Console.csproj Simulation.FDTD.Console/
COPY Simulation.Tests.Common/Simulation.Tests.Common.csproj Simulation.Tests.Common/
COPY Tests/Simulation.DDA.Tests/Simulation.DDA.Tests.csproj Tests/Simulation.DDA.Tests/
COPY Tests/Simulation.FDTD.Tests/Simulation.FDTD.Tests.csproj Tests/Simulation.FDTD.Tests/
COPY Tests/Simulation.Medium.Tests/Simulation.Medium.Tests.csproj Tests/Simulation.Medium.Tests/
COPY Tests/Simulation.Models.Tests/Simulation.Models.Tests.csproj Tests/Simulation.Models.Tests/
COPY Simulation.Web/Simulation.Web.csproj Simulation.Web/
COPY Simulation.sln .
COPY nuget.config .

RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
COPY --from=build /app/https https/
RUN cp ./https/ca.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

FROM runtime as web
ENTRYPOINT ["dotnet", "Simulation.Web.dll"]

FROM runtime as dda-grpc
ENTRYPOINT ["dotnet", "Simulation.DDA.Grpc.dll"]

FROM runtime as fdtd-grpc
ENTRYPOINT ["dotnet", "Simulation.FDTD.Grpc.dll"]
