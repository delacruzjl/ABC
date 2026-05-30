import React, { useState, useEffect } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { useObservation } from "../hooks/useObservation"
import type { DailyContext, Observation as ObservationType } from "../types/observation"

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

interface DailyContextSectionProps {
  dailyContext: DailyContext
  onChange: (ctx: DailyContext) => void
}

const DailyContextSection: React.FC<DailyContextSectionProps> = ({
  dailyContext,
  onChange,
}) => {
  const toggle = (field: keyof DailyContext) => {
    onChange({ ...dailyContext, [field]: !dailyContext[field] })
  }

  return (
    <div>
      <h3 className="text-lg font-semibold text-slate-200 mb-3">
        Daily Context
      </h3>
      <p className="text-slate-400 text-xs mb-3">
        Optional — record the child's meals and sleep before this session.
      </p>
      <div className="space-y-3">
        <div>
          <p className="text-sm text-slate-300 mb-2 font-medium">Meals</p>
          <div className="flex flex-wrap gap-4">
            {(["hadBreakfast", "hadLunch", "hadDinner", "hadSnack"] as const).map(
              (field) => (
                <label
                  key={field}
                  className="flex items-center gap-2 text-sm text-slate-300 cursor-pointer"
                >
                  <input
                    type="checkbox"
                    checked={dailyContext[field]}
                    onChange={() => toggle(field)}
                    className="w-4 h-4 rounded border-slate-500 bg-slate-700 text-cyan-500 focus:ring-cyan-500"
                  />
                  {field === "hadBreakfast" && "Breakfast"}
                  {field === "hadLunch" && "Lunch"}
                  {field === "hadDinner" && "Dinner"}
                  {field === "hadSnack" && "Snack"}
                </label>
              )
            )}
          </div>
        </div>
        <div>
          <p className="text-sm text-slate-300 mb-2 font-medium">Sleep</p>
          <div className="flex flex-wrap items-center gap-4">
            <label className="flex items-center gap-2 text-sm text-slate-300 cursor-pointer">
              <input
                type="checkbox"
                checked={dailyContext.sleptWell}
                onChange={() => toggle("sleptWell")}
                className="w-4 h-4 rounded border-slate-500 bg-slate-700 text-cyan-500 focus:ring-cyan-500"
              />
              Slept well
            </label>
            <label className="flex items-center gap-2 text-sm text-slate-300">
              <span>Hours of sleep:</span>
              <input
                type="number"
                min={0}
                max={24}
                value={dailyContext.hoursOfSleep ?? ""}
                onChange={(e) =>
                  onChange({
                    ...dailyContext,
                    hoursOfSleep: e.target.value ? parseInt(e.target.value, 10) : null,
                  })
                }
                placeholder="—"
                className="w-16 bg-slate-700 text-slate-200 border border-slate-600 rounded px-2 py-1 text-sm focus:outline-none focus:border-cyan-500"
              />
            </label>
          </div>
        </div>
      </div>
    </div>
  )
}

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
    dailyContext,
    setDailyContext,
    toggleAntecedent,
    toggleBehavior,
    toggleConsequence,
    startObservation,
    continueObservation,
    getOpenObservations,
    saveAndEndObservation,
    canEnd,
    reset,
    listsLoading,
    starting,
    saving,
    loadingOpen,
  } = useObservation()

  const [error, setError] = useState<string | null>(null)
  const [completed, setCompleted] = useState(false)
  const [openObservations, setOpenObservations] = useState<ObservationType[]>([])
  const [checkedOpen, setCheckedOpen] = useState(false)

  useEffect(() => {
    let cancelled = false
    if (childId && !observation && !checkedOpen) {
      setCheckedOpen(true)
      getOpenObservations(childId).then((obs) => {
        if (!cancelled) {
          setOpenObservations(obs)
        }
      })
    }
    return () => { cancelled = true }
  }, [childId, observation, checkedOpen, getOpenObservations])

  const handleContinue = (obs: ObservationType) => {
    setError(null)
    continueObservation(obs)
  }

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

        {/* Open observations to continue */}
        {(loadingOpen) && (
          <p className="text-slate-400 text-sm mb-4">Checking for open observations…</p>
        )}
        {openObservations.length > 0 && (
          <div className="max-w-md mx-auto mb-6 text-left bg-slate-800 rounded-lg p-4 border border-amber-700">
            <h3 className="text-lg font-semibold text-amber-400 mb-3">
              Open Observations
            </h3>
            <p className="text-slate-400 text-xs mb-3">
              You have open observations for this child. Continue where you left off or start a new one.
            </p>
            <div className="flex flex-col gap-2">
              {openObservations.map((obs) => (
                <button
                  key={obs.id}
                  onClick={() => handleContinue(obs)}
                  className="w-full text-left bg-slate-700 hover:bg-slate-600 border border-slate-600 hover:border-amber-400 rounded-lg p-3 transition"
                >
                  <div className="flex justify-between items-center">
                    <span className="text-sm text-slate-200">
                      Started:{" "}
                      {obs.when.startedAt
                        ? new Date(obs.when.startedAt).toLocaleString()
                        : "—"}
                    </span>
                    <span className="text-xs bg-yellow-900 text-yellow-300 px-2 py-0.5 rounded">
                      OPEN
                    </span>
                  </div>
                  {obs.notes && (
                    <p className="text-slate-400 text-xs mt-1 truncate">{obs.notes}</p>
                  )}
                  <div className="flex gap-1 mt-2">
                    {obs.antecedents.length > 0 && (
                      <span className="text-xs text-cyan-400">{obs.antecedents.length}A</span>
                    )}
                    {obs.behaviors.length > 0 && (
                      <span className="text-xs text-purple-400">{obs.behaviors.length}B</span>
                    )}
                    {obs.consequences.length > 0 && (
                      <span className="text-xs text-amber-400">{obs.consequences.length}C</span>
                    )}
                  </div>
                </button>
              ))}
            </div>
          </div>
        )}

        <div className="max-w-md mx-auto mb-6 text-left bg-slate-800 rounded-lg p-4 border border-slate-700">
          <DailyContextSection
            dailyContext={dailyContext}
            onChange={setDailyContext}
          />
        </div>

        {error && (
          <p className="text-red-400 text-sm mb-4">{error}</p>
        )}
        <div className="flex justify-center gap-4">
          <button
            onClick={handleStart}
            disabled={starting}
            className="bg-cyan-600 hover:bg-cyan-500 disabled:opacity-50 text-white font-medium px-6 py-2 rounded-lg transition"
          >
            {starting ? "Starting…" : "Start New Observation"}
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

        <DailyContextSection
          dailyContext={dailyContext}
          onChange={setDailyContext}
        />
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
