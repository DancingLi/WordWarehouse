# WordWarehouse

WordWarehouse is a small WPF desktop application for saving and reviewing words, phrases, and sentences across English, German, Japanese, and Korean.

## Stack

- .NET 6 WPF
- SQLite via `Microsoft.Data.Sqlite`
- MVVM-style view models with a small service/data layer

## Features in v1

- Home dashboard with counts and recent entries
- Quick Add screen for fast capture
- Library screen with search and filters
- Review screen with simple status-based progression
- Edit/delete support through a modal editor

## Local data

The app stores its SQLite database in:

`%LOCALAPPDATA%\WordWarehouse\wordwarehouse.db`

## Build

This workspace currently does not include a .NET SDK. To build locally, install a Windows Desktop SDK for .NET 6 or newer, then run:

```powershell
dotnet restore
dotnet build .\WordWarehouse\WordWarehouse.csproj
```
