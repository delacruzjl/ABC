import React from "react"
import type { Behavior } from "../types/behavior"

interface BehaviorProps {
  behavior: Behavior
}

export const BehaviorCard: React.FC<BehaviorProps> = ({ behavior }) => (
  <div className="p-4 border rounded shadow bg-white">
    <h2 className="text-lg font-bold mb-2">{behavior.name}</h2>
    <p className="mb-2 text-gray-700">{behavior.description}</p>
    {behavior.observations && (
      <div>
        <h3 className="font-semibold">Observations</h3>
        <ul className="list-disc ml-5">
          {behavior.observations.map((obs, idx) => (
            <li key={idx}>Observation {idx + 1}</li>
          ))}
        </ul>
      </div>
    )}
  </div>
)
