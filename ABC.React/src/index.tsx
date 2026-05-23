import React from "react"
import ReactDOM from "react-dom/client"
import "./index.scss"
import App from "./App"
import reportWebVitals from "./reportWebVitals"

import { ApolloClient, HttpLink, InMemoryCache, from } from "@apollo/client"
import { onError } from "@apollo/client/link/error"
import { setContext } from "@apollo/client/link/context"
import { ApolloProvider } from "@apollo/client/react"
import { AuthProvider } from "./context/AuthContext"
import { apiErrorVar } from "./state/apiError"
import { MsalProvider } from "@azure/msal-react"
import { msalInstance } from "./config/msalConfig"
import { GoogleOAuthProvider } from "@react-oauth/google"
import { googleConfig } from "./config/googleConfig"

const httpLink = new HttpLink({ uri: "/api/graphql" })

const authLink = setContext((_, { headers }) => {
  const token = localStorage.getItem("abc_token")
  return {
    headers: {
      ...headers,
      ...(token ? { authorization: `Bearer ${token}` } : {}),
    },
  }
})

const errorLink = onError(({ networkError, graphQLErrors }) => {
  if (networkError) {
    apiErrorVar(
      "Unable to connect to the server. Please check your connection and try again."
    )
    return
  }
  if (graphQLErrors?.some((e) => e.extensions?.code === "AUTH_NOT_AUTHENTICATED")) {
    return
  }
  if (graphQLErrors?.length) {
    apiErrorVar(graphQLErrors.map((e) => e.message).join("; "))
  }
})

const client = new ApolloClient({
  link: from([authLink, errorLink, httpLink]),
  cache: new InMemoryCache(),
})

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement)
root.render(
  <React.StrictMode>
    <MsalProvider instance={msalInstance}>
      <GoogleOAuthProvider clientId={googleConfig.clientId}>
        <ApolloProvider client={client}>
          <AuthProvider>
            <App />
          </AuthProvider>
        </ApolloProvider>
      </GoogleOAuthProvider>
    </MsalProvider>
  </React.StrictMode>
)

reportWebVitals(console.log)
