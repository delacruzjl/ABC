import { useEffect, useState } from "react";
import "./App.css";
import { useQuery } from "@apollo/client"
import { GET_ANTECEDENTS_QUERY } from '../graphQL'

function App() {
    const {
        loading,
        error,
        data
    } = useQuery(
        GET_ANTECEDENTS_QUERY,
        {
            variables: {
                count: 10
            }
        }
        )
    const items = (data?.antecedents?.nodes || []).length !== 0
        ? data.antecedents.nodes
        : undefined

    if (loading) {
        return (<h2>Loading</h2>);
    }

    if (error) {
        return (<h2>Error</h2>);
    }

  return (
    <div className="App">
      <header className="App-header">
        <h1>React Weather</h1>
        <table>
          <thead>
            <tr>
              <th>Id</th>
              <th>Name</th>
              <th>Description</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {(
              items ?? [
                {
                  id: "N/A",
                  name: "",
                  description: ""
                },
              ]
            ).map((w) => {
              return (
                <tr key={w.id}>
                  <td>{w.id}</td>
                  <td>{w.name}</td>
                  <td>{w.description}</td>
                  <td></td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </header>
    </div>
  );
}

export default App;
