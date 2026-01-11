import React from "react"
import ReactDOM from "react-dom/client"
import "./index.scss"
import App from "./App"
import reportWebVitals from "./reportWebVitals"

import { ApolloClient, HttpLink, InMemoryCache, gql } from "@apollo/client"
import { ApolloProvider } from "@apollo/client/react"

const client = new ApolloClient({
  link: new HttpLink({ uri: "/api/graphql" }),
  cache: new InMemoryCache(),
})

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement)
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
)

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals(console.log)
