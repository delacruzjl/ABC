import React from "react"
import type { Antecedent } from "../types/antecedent"

interface AntecedentProps {
  antecedent: Antecedent
}

export const AntecedentCard: React.FC<AntecedentProps> = ({ antecedent }) => (
  <div className="p-4 border rounded shadow bg-white">
    <h2 className="text-lg font-bold mb-2">{antecedent.name}</h2>
    <p className="mb-2 text-gray-700">{antecedent.description}</p>
    {antecedent.observations && (
      <div>
        <h3 className="font-semibold">Observations</h3>
        <ul className="list-disc ml-5">
          {antecedent.observations.map((obs, idx) => (
            <li key={idx}>Observation {idx + 1}</li>
          ))}
        </ul>
      </div>
    )}
  </div>
)
