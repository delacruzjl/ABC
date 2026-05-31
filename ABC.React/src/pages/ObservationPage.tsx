import React, { useEffect, useState } from "react"
import { useParams, useNavigate } from "react-router-dom"
import { useTranslation } from "react-i18next"
import { useObservation } from "../hooks/useObservation"
import type { DailyContext, Observation as ObservationType } from "../types/observation"

interface SelectionSectionProps {
  title: string
  items: { id: string; name: string; description: string }[]
  selectedIds: string[]
  onToggle: (id: string) => void
  accentColor: string
  emptyMessageKey: string
}

const SelectionSection: React.FC<SelectionSectionProps> = ({
  title,
  items,
  selectedIds,
  onToggle,
  accentColor,
  emptyMessageKey,
}) => {
  const { t } = useTranslation()

  return (
    <div>
      <h3 className="text-lg font-semibold text-slate-200 mb-3">{title}</h3>
      {items.length === 0 ? (
        <p className="text-slate-500 text-sm italic">{t(emptyMessageKey)}</p>
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
        {t("observation.selectedCount", { count: selectedIds.length })}
      </p>
    </div>
  )
}

interface DailyContextSectionProps {
  dailyContext: DailyContext
  onChange: (ctx: DailyContext) => void
}

const DailyContextSection: React.FC<DailyContextSectionProps> = ({
  dailyContext,
  onChange,
}) => {
  const { t } = useTranslation()

  const toggle = (field: keyof DailyContext) => {
    onChange({ ...dailyContext, [field]: !dailyContext[field] })
  }

  return (
    <div>
      <h3 className="text-lg font-semibold text-slate-200 mb-3">
        {t("observation.dailyContext")}
      </h3>
      <p className="text-slate-400 text-xs mb-3">
        {t("observation.dailyContextDescription")}
      </p>
      <div className="space-y-3">
        <div>
          <p className="text-sm text-slate-300 mb-2 font-medium">
            {t("observation.meals")}
          </p>
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
                  {field === "hadBreakfast" && t("observation.breakfast")}
                  {field === "hadLunch" && t("observation.lunch")}
                  {field === "hadDinner" && t("observation.dinner")}
                  {field === "hadSnack" && t("observation.snack")}
                </label>
              )
            )}
          </div>
        </div>
        <div>
          <p className="text-sm text-slate-300 mb-2 font-medium">
            {t("observation.sleep")}
          </p>
          <div className="flex flex-wrap items-center gap-4">
            <label className="flex items-center gap-2 text-sm text-slate-300 cursor-pointer">
              <input
                type="checkbox"
                checked={dailyContext.sleptWell}
                onChange={() => toggle("sleptWell")}
                className="w-4 h-4 rounded border-slate-500 bg-slate-700 text-cyan-500 focus:ring-cyan-500"
              />
              {t("observation.sleptWell")}
            </label>
            <label className="flex items-center gap-2 text-sm text-slate-300">
              <span>{t("observation.hoursOfSleep")}</span>
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
  const { t } = useTranslation()
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
    return () => {
      cancelled = true
    }
  }, [childId, observation, checkedOpen, getOpenObservations])

  const getStatusLabel = (status: string) => {
    if (status === "OPEN") return t("observation.inProgress")
    if (status === "CLOSED") return t("observation.ended")
    return status
  }

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
      setError(e instanceof Error ? e.message : t("observation.failedStart"))
    }
  }

  const handleEnd = async () => {
    setError(null)
    try {
      await saveAndEndObservation()
      setCompleted(true)
    } catch (e: unknown) {
      setError(e instanceof Error ? e.message : t("observation.failedEnd"))
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
          {t("observation.loadingData")}
        </span>
      </div>
    )
  }

  if (completed && observation) {
    return (
      <div className="text-center py-12">
        <div className="text-green-400 text-5xl mb-4">✓</div>
        <h2 className="text-2xl font-bold text-slate-100 mb-2">
          {t("observation.completeTitle")}
        </h2>
        <p className="text-slate-400 mb-6">
          {t("common.started")}:{" "}
          {observation.when.startedAt
            ? new Date(observation.when.startedAt).toLocaleString()
            : "—"}{" "}
          — {t("common.ended")}:{" "}
          {observation.when.endedAt
            ? new Date(observation.when.endedAt).toLocaleString()
            : "—"}
        </p>
        <div className="flex justify-center gap-4">
          <button
            onClick={handleNewObservation}
            className="bg-cyan-600 hover:bg-cyan-500 text-white font-medium px-4 py-2 rounded-lg transition"
          >
            {t("observation.newObservation")}
          </button>
          <button
            onClick={() => navigate("/children")}
            className="bg-slate-700 hover:bg-slate-600 text-slate-200 font-medium px-4 py-2 rounded-lg transition border border-slate-600"
          >
            {t("observation.backToChildren")}
          </button>
        </div>
      </div>
    )
  }

  if (!observation) {
    return (
      <div className="text-center py-12">
        <h2 className="text-2xl font-bold text-cyan-400 mb-4">
          {t("observation.start")}
        </h2>
        <p className="text-slate-400 mb-6">{t("observation.intro")}</p>

        {loadingOpen && (
          <p className="text-slate-400 text-sm mb-4">
            {t("observation.checkingOpen")}
          </p>
        )}
        {openObservations.length > 0 && (
          <div className="max-w-md mx-auto mb-6 text-left bg-slate-800 rounded-lg p-4 border border-amber-700">
            <h3 className="text-lg font-semibold text-amber-400 mb-3">
              {t("observation.openObservations")}
            </h3>
            <p className="text-slate-400 text-xs mb-3">
              {t("observation.openObservationsDescription")}
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
                      {t("common.started")}:{" "}
                      {obs.when.startedAt
                        ? new Date(obs.when.startedAt).toLocaleString()
                        : "—"}
                    </span>
                    <span className="text-xs bg-yellow-900 text-yellow-300 px-2 py-0.5 rounded">
                      {t("observation.inProgress")}
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

        {error && <p className="text-red-400 text-sm mb-4">{error}</p>}
        <div className="flex justify-center gap-4">
          <button
            onClick={handleStart}
            disabled={starting}
            className="bg-cyan-600 hover:bg-cyan-500 disabled:opacity-50 text-white font-medium px-6 py-2 rounded-lg transition"
          >
            {starting ? t("common.starting") : t("observation.startNew")}
          </button>
          <button
            onClick={() => navigate("/children")}
            className="bg-slate-700 hover:bg-slate-600 text-slate-200 font-medium px-4 py-2 rounded-lg transition border border-slate-600"
          >
            {t("common.cancel")}
          </button>
        </div>
      </div>
    )
  }

  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-cyan-400">
            {t("observation.recordingTitle")}
          </h1>
          {observation.child && (
            <p className="text-slate-400 text-sm mt-1">
              {t("observation.childLabel")}: {observation.child.firstName}{" "}
              {observation.child.lastName}
            </p>
          )}
          <p className="text-slate-500 text-xs mt-0.5">
            {t("common.started")}:{" "}
            {observation.when.startedAt
              ? new Date(observation.when.startedAt).toLocaleString()
              : "—"}
          </p>
        </div>
        <span className="text-xs bg-green-900 text-green-300 px-2 py-0.5 rounded uppercase tracking-wide">
          {getStatusLabel(observation.status)}
        </span>
      </div>

      {error && (
        <div className="bg-red-900/30 border border-red-700 text-red-300 rounded-lg p-3 mb-4 text-sm">
          {error}
        </div>
      )}

      <div className="space-y-6 mb-8">
        <SelectionSection
          title={t("antecedents.title")}
          items={antecedents}
          selectedIds={selectedAntecedents}
          onToggle={toggleAntecedent}
          accentColor="bg-cyan-700 text-cyan-100"
          emptyMessageKey="observation.noAntecedentsAvailable"
        />
        <SelectionSection
          title={t("behaviors.title")}
          items={behaviors}
          selectedIds={selectedBehaviors}
          onToggle={toggleBehavior}
          accentColor="bg-purple-700 text-purple-100"
          emptyMessageKey="observation.noBehaviorsAvailable"
        />
        <SelectionSection
          title={t("consequences.title")}
          items={consequences}
          selectedIds={selectedConsequences}
          onToggle={toggleConsequence}
          accentColor="bg-amber-700 text-amber-100"
          emptyMessageKey="observation.noConsequencesAvailable"
        />

        <div>
          <h3 className="text-lg font-semibold text-slate-200 mb-2">
            {t("observation.notes")}
          </h3>
          <textarea
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            rows={4}
            placeholder={t("observation.notesPlaceholder")}
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
          {t("observation.endRequirements")}
        </p>
      )}

      <div className="flex gap-3">
        <button
          onClick={handleEnd}
          disabled={!canEnd || saving}
          className="bg-green-700 hover:bg-green-600 disabled:opacity-50 disabled:cursor-not-allowed text-white font-medium px-5 py-2 rounded-lg transition"
        >
          {saving ? t("common.saving") : t("observation.end")}
        </button>
        <button
          onClick={() => navigate("/children")}
          className="bg-slate-700 hover:bg-slate-600 text-slate-200 font-medium px-4 py-2 rounded-lg transition border border-slate-600"
        >
          {t("common.cancel")}
        </button>
      </div>
    </div>
  )
}
