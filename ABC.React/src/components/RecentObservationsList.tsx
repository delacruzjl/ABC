import React from "react"
import { useNavigate } from "react-router-dom"
import type { Observation } from "../types/observation"

interface Props {
  observations: Observation[]
}

function formatDate(dateStr: string | null): string {
  if (!dateStr) return "—"
  const d = new Date(dateStr)
  return d.toLocaleDateString("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  })
}

export const RecentObservationsList: React.FC<Props> = ({ observations }) => {
  const navigate = useNavigate()

  if (observations.length === 0) {
    return (
      <div className="bg-slate-700 rounded-lg p-4">
        <h3 className="text-lg font-semibold text-cyan-400 mb-3">
          Most Recent Observations
        </h3>
        <p className="text-slate-400 text-sm">No observations yet</p>
      </div>
    )
  }

  return (
    <div className="bg-slate-700 rounded-lg p-4">
      <h3 className="text-lg font-semibold text-cyan-400 mb-3">
        Most Recent Observations
      </h3>
      <div className="flex flex-col gap-3">
        {observations.map((obs) => (
          <div
            key={obs.id}
            className="bg-slate-800 rounded-lg p-4 border border-slate-600"
          >
            <div className="flex justify-between items-start mb-2">
              <span
                className={`text-xs font-semibold px-2 py-1 rounded ${
                  obs.status === "CLOSED"
                    ? "bg-green-900 text-green-300"
                    : "bg-yellow-900 text-yellow-300"
                }`}
              >
                {obs.status}
              </span>
              <div className="flex items-center gap-2">
                <span className="text-xs text-slate-400">
                  {formatDate(obs.when?.startedAt)}
                </span>
                {obs.status === "OPEN" && obs.child && (
                  <button
                    onClick={() => navigate(`/observation/${obs.child!.id}`)}
                    className="text-xs bg-amber-700 hover:bg-amber-600 text-amber-100 px-2 py-0.5 rounded transition font-medium"
                  >
                    Continue
                  </button>
                )}
              </div>
            </div>
            {obs.notes && (
              <p className="text-slate-300 text-sm mb-2">{obs.notes}</p>
            )}
            <div className="flex flex-wrap gap-1 mt-2">
              {obs.antecedents.map((a) => (
                <span
                  key={a.id}
                  className="text-xs bg-cyan-900 text-cyan-300 px-2 py-0.5 rounded"
                >
                  A: {a.name}
                </span>
              ))}
              {obs.behaviors.map((b) => (
                <span
                  key={b.id}
                  className="text-xs bg-purple-900 text-purple-300 px-2 py-0.5 rounded"
                >
                  B: {b.name}
                </span>
              ))}
              {obs.consequences.map((c) => (
                <span
                  key={c.id}
                  className="text-xs bg-amber-900 text-amber-300 px-2 py-0.5 rounded"
                >
                  C: {c.name}
                </span>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}
