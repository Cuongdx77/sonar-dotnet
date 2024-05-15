FROM dxcuong206/dotnet-java:01 as build

WORKDIR /app

# Restore NuGet packages
COPY . .
RUN dotnet restore

# Copy the rest of the files over
# COPY . .

# Build and test the application
RUN dotnet publish --output /out/
RUN dotnet test \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=opencover \
  /p:CoverletOutput="/coverage"
