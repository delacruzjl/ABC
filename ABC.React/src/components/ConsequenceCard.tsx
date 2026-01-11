import React from "react"
import type { Consequence } from "../types/consequence"

interface ConsequenceProps {
  consequence: Consequence
}

export const ConsequenceCard: React.FC<ConsequenceProps> = ({
  consequence,
}) => (
  <div className="p-4 border rounded shadow bg-white">
    <h2 className="text-lg font-bold mb-2">{consequence.name}</h2>
    <p className="mb-2 text-gray-700">{consequence.description}</p>
    {consequence.observations && (
      <div>
        <h3 className="font-semibold">Observations</h3>
        <ul className="list-disc ml-5">
          {consequence.observations.map((obs, idx) => (
            <li key={idx}>Observation {idx + 1}</li>
          ))}
        </ul>
      </div>
    )}
  </div>
)
