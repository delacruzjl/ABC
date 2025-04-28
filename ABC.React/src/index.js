import React from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./components/App";
import {
    ApolloClient,
    ApolloProvider,
    HttpLink,
    InMemoryCache,
} from "@apollo/client";

const container = document.getElementById("root");
const root = createRoot(container);
const client = new ApolloClient({
    cache: new InMemoryCache(),
    link: new HttpLink({
        uri: "/api/graphql",
    }),
    credentials: "same-origin",
});

root.render(
    <React.StrictMode>
        <ApolloProvider client={client}>
            <App />        
        </ApolloProvider>
    
  </React.StrictMode>
);
