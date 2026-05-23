import { PublicClientApplication, Configuration, LogLevel } from "@azure/msal-browser"

const msalConfig: Configuration = {
  auth: {
    clientId: process.env.AZURE_ENTRA_CLIENT_ID || "",
    authority: `https://login.microsoftonline.com/${process.env.AZURE_ENTRA_TENANT_ID || "common"}`,
    redirectUri: window.location.origin,
  },
  cache: {
    cacheLocation: "sessionStorage",
    storeAuthStateInCookie: false,
  },
  system: {
    loggerOptions: {
      logLevel: LogLevel.Warning,
      loggerCallback: (level, message) => {
        if (level === LogLevel.Error) console.error(message)
      },
    },
  },
}

export const msalInstance = new PublicClientApplication(msalConfig)

export const loginRequest = {
  scopes: ["openid", "profile", "email"],
}
