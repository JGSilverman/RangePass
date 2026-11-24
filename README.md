# RangePass

RangePass is a mobile-first range-visit journal built with .NET MAUI Blazor Hybrid.

## Current state

The first app scaffold includes:

- Android, iOS, Mac Catalyst, and Windows MAUI targets
- Microsoft Authentication Library (`Microsoft.Identity.Client`) sign-in/sign-out
- Microsoft identity callback declarations for Android and iOS
- A mobile visit-entry screen with range, date, rounds, and notes
- A setup state for an unconfigured identity app registration

The local visit entry is UI-only. It is not yet persisted or synchronized with an API.

## Microsoft identity setup

The app does not contain a real client ID or tenant secret. Create a public-client app registration in Microsoft Entra ID for the intended Microsoft identity tenant, then update:

- `src/RangePass.Mobile/Resources/Raw/microsoft-identity.json`

Set `ClientId`, verify `Authority`, and configure the matching native redirect URI (`rangepass://auth`) in the app registration.

Use a separate platform redirect URI if Microsoft Entra app registration or package signing requirements require it. No production deployment should use the placeholder `YOUR_CLIENT_ID` value.

## Run on Windows

```powershell
dotnet build src/RangePass.Mobile/RangePass.Mobile.csproj -f net10.0-windows10.0.19041.0
dotnet build src/RangePass.Mobile/RangePass.Mobile.csproj -t:Run -f net10.0-windows10.0.19041.0
```

## Run on Android

```powershell
dotnet build src/RangePass.Mobile/RangePass.Mobile.csproj -f net10.0-android
dotnet build src/RangePass.Mobile/RangePass.Mobile.csproj -t:Run -f net10.0-android
```

## Next phase

Phase 2 should add the ASP.NET Core API, persistence, ownership authorization, evidence uploads, and server-side validation before the local visit form is advertised as saving real records.

## Known limitation

iOS builds are not executable or verifiable from this Windows environment. Authentication and callback configuration must be verified on macOS and a registered Apple/iOS app registration before pilot use.
