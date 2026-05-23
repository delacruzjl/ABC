import React, { useState } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { useObservation } from "../hooks/useObservation"

interface SelectionSectionProps {
  title: string
  items: { id: string; name: string; description: string }[]
  selectedIds: string[]
  onToggle: (id: string) => void
  accentColor: string
}

const SelectionSection: React.FC<SelectionSectionProps> = ({
  title,
  items,
  selectedIds,
  onToggle,
  accentColor,
}) => (
  <div>
    <h3 className="text-lg font-semibold text-slate-200 mb-3">{title}</h3>
    {items.length === 0 ? (
      <p className="text-slate-500 text-sm italic">
        No {title.toLowerCase()} available. An admin needs to create them first.
      </p>
    ) : (
      <div className="flex flex-wrap gap-2">
        {items.map((item) => {
          const selected = selectedIds.includes(item.id)
          return (
            <button
              key={item.id}
              onClick={() => onToggle(item.id)}
              title={item.description}
              className={`px-3 py-1.5 rounded-lg text-sm font-medium border transition ${
                selected
                  ? `${accentColor} border-transparent`
                  : "bg-slate-700 text-slate-300 border-slate-600 hover:border-slate-400"
              }`}
            >
              {item.name}
            </button>
          )
        })}
      </div>
    )}
    <p className="text-xs text-slate-500 mt-1">
      {selectedIds.length} selected
    </p>
  </div>
)

export const ObservationPage: React.FC = () => {
  const { childId } = useParams<{ childId: string }>()
  const navigate = useNavigate()
  const {
    observation,
    antecedents,
    behaviors,
    consequences,
    selectedAntecedents,
    selectedBehaviors,
    selectedConsequences,
    notes,
    setNotes,
    toggleAntecedent,
    toggleBehavior,
    toggleConsequence,
    startObservation,
    saveAndEndObservation,
    canEnd,
    reset,
    listsLoading,
    starting,
    saving,
  } = useObservation()

  const [error, setError] = useState<string | null>(null)
  const [completed, setCompleted] = useState(false)

  const handleStart = async () => {
    if (!childId) return
    setError(null)
    try {
      await startObservation(childId)
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : "Failed to start observation")
    }
  }

  const handleEnd = async () => {
    setError(null)
    try {
      await saveAndEndObservation()
      setCompleted(true)
    } catch (e: unknown) {
      setError(
        e instanceof Error ? e.message : "Failed to end observation"
      )
    }
  }

  const handleNewObservation = () => {
    reset()
    setCompleted(false)
    setError(null)
  }

  if (listsLoading) {
    return (
      <div className="flex justify-center py-12">
        <span className="text-cyan-400 text-lg">
          Loading observation data…
        </span>
      </div>
    )
  }

  // Completed state
  if (completed && observation) {
    return (
      <div className="text-center py-12">
        <div className="text-green-400 text-5xl mb-4">✓</div>
        <h2 className="text-2xl font-bold text-slate-100 mb-2">
          Observation Complete
        </h2>
        <p className="text-slate-400 mb-6">
          Started:{" "}
          {observation.when.startedAt
            ? new Date(observation.when.startedAt).toLocaleString()
            : "—"}{" "}
          — Ended:{" "}
          {observation.when.endedAt
            ? new Date(observation.when.endedAt).toLocaleString()
            : "—"}
        </p>
        <div className="flex justify-center gap-4">
          <button
            onClick={handleNewObservation}
            className="bg-cyan-600 hover:bg-cyan-500 text-white font-medium px-4 py-2 rounded-lg transition"
          >
            New Observation
          </button>
          <button
            onClick={() => navigate("/children")}
            className="bg-slate-700 hover:bg-slate-600 text-slate-200 font-medium px-4 py-2 rounded-lg transition border border-slate-600"
          >
            Back to Children
          </button>
        </div>
      </div>
    )
  }

  // Pre-start state
  if (!observation) {
    return (
      <div className="text-center py-12">
        <h2 className="text-2xl font-bold text-cyan-400 mb-4">
          Start Observation
        </h2>
        <p className="text-slate-400 mb-6">
          Begin an ABC data collection session for this child. You'll select
          antecedents, behaviors, and consequences observed during the session.
        </p>
        {error && (
          <p className="text-red-400 text-sm mb-4">{error}</p>
        )}
        <div className="flex justify-center gap-4">
          <button
            onClick={handleStart}
            disabled={starting}
            className="bg-cyan-600 hover:bg-cyan-500 disabled:opacity-50 text-white font-medium px-6 py-2 rounded-lg transition"
          >
            {starting ? "Starting…" : "Start Observation"}
          </button>
          <button
            onClick={() => navigate("/children")}
            className="bg-slate-700 hover:bg-slate-600 text-slate-200 font-medium px-4 py-2 rounded-lg transition border border-slate-600"
          >
            Cancel
          </button>
        </div>
      </div>
    )
  }

  // Active observation
  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-cyan-400">
            Recording Observation
          </h1>
          {observation.child && (
            <p className="text-slate-400 text-sm mt-1">
              Child: {observation.child.firstName} {observation.child.lastName}
            </p>
          )}
          <p className="text-slate-500 text-xs mt-0.5">
            Started:{" "}
            {observation.when.startedAt
              ? new Date(observation.when.startedAt).toLocaleString()
              : "—"}
          </p>
        </div>
        <span className="text-xs bg-green-900 text-green-300 px-2 py-0.5 rounded uppercase tracking-wide">
          {observation.status}
        </span>
      </div>

      {error && (
        <div className="bg-red-900/30 border border-red-700 text-red-300 rounded-lg p-3 mb-4 text-sm">
          {error}
        </div>
      )}

      <div className="space-y-6 mb-8">
        <SelectionSection
          title="Antecedents"
          items={antecedents}
          selectedIds={selectedAntecedents}
          onToggle={toggleAntecedent}
          accentColor="bg-cyan-700 text-cyan-100"
        />
        <SelectionSection
          title="Behaviors"
          items={behaviors}
          selectedIds={selectedBehaviors}
          onToggle={toggleBehavior}
          accentColor="bg-purple-700 text-purple-100"
        />
        <SelectionSection
          title="Consequences"
          items={consequences}
          selectedIds={selectedConsequences}
          onToggle={toggleConsequence}
          accentColor="bg-amber-700 text-amber-100"
        />

        <div>
          <h3 className="text-lg font-semibold text-slate-200 mb-2">Notes</h3>
          <textarea
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            rows={4}
            placeholder="Add any additional context about the observation…"
            className="w-full bg-slate-700 text-slate-200 border border-slate-600 rounded-lg px-3 py-2 focus:outline-none focus:border-cyan-500 placeholder-slate-500 resize-y"
          />
        </div>
      </div>

      {!canEnd && (
        <p className="text-amber-400 text-sm mb-4">
          Select at least one antecedent, one behavior, and one consequence
          before ending the observation.
        </p>
      )}

      <div className="flex gap-3">
        <button
          onClick={handleEnd}
          disabled={!canEnd || saving}
          className="bg-green-700 hover:bg-green-600 disabled:opacity-50 disabled:cursor-not-allowed text-white font-medium px-5 py-2 rounded-lg transition"
        >
          {saving ? "Saving…" : "End Observation"}
        </button>
        <button
          onClick={() => navigate("/children")}
          className="bg-slate-700 hover:bg-slate-600 text-slate-200 font-medium px-4 py-2 rounded-lg transition border border-slate-600"
        >
          Cancel
        </button>
      </div>
    </div>
  )
}
