import React, { useEffect, useMemo, useRef, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useTranslation } from "react-i18next"
import { useChildren, useUsers, useChildConditions } from "../hooks/useChildren"
import { useAuth } from "../context/AuthContext"

export const ChildFormPage: React.FC = () => {
  const { t } = useTranslation()
  const defaultSuggestions = t("children.conditionSuggestions", { returnObjects: true }) as string[]
  const { id } = useParams<{ id: string }>()
  const isEditMode = Boolean(id)

  const [firstName, setFirstName] = useState("")
  const [lastName, setLastName] = useState("")
  const [birthYear, setBirthYear] = useState("")
  const [selectedUserId, setSelectedUserId] = useState("")
  const [selectedConditions, setSelectedConditions] = useState<string[]>([])
  const [conditionInput, setConditionInput] = useState("")
  const [showSuggestions, setShowSuggestions] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const { children, createChild, updateChild } = useChildren()
  const { isAdmin } = useAuth()
  const { users } = useUsers()
  const { conditions: availableConditions } = useChildConditions()
  const navigate = useNavigate()
  const inputRef = useRef<HTMLInputElement>(null)
  const suggestionsRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    if (isEditMode && children.length > 0) {
      const child = children.find((c) => c.id === id)
      if (child) {
        setFirstName(child.firstName)
        setLastName(child.lastName)
        setBirthYear(String(child.birthYear))
        setSelectedUserId(child.userId)
        setSelectedConditions(child.conditions.map((c) => c.name))
      }
    }
  }, [isEditMode, id, children])

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (
        suggestionsRef.current &&
        !suggestionsRef.current.contains(e.target as Node) &&
        inputRef.current &&
        !inputRef.current.contains(e.target as Node)
      ) {
        setShowSuggestions(false)
      }
    }

    document.addEventListener("mousedown", handleClickOutside)
    return () => document.removeEventListener("mousedown", handleClickOutside)
  }, [])

  const allSuggestions = useMemo(() => {
    const dbNames = availableConditions.map((c) => c.name)
    const merged = [...new Set([...defaultSuggestions, ...dbNames])]
    return merged.filter(
      (name) =>
        !selectedConditions.some(
          (s) => s.toLowerCase() === name.toLowerCase()
        )
    )
  }, [availableConditions, selectedConditions, defaultSuggestions])

  const filteredSuggestions = useMemo(() => {
    if (!conditionInput.trim()) return allSuggestions
    const lower = conditionInput.toLowerCase()
    return allSuggestions.filter((s) => s.toLowerCase().includes(lower))
  }, [conditionInput, allSuggestions])

  const addCondition = (name: string) => {
    const trimmed = name.trim()
    if (
      !trimmed ||
      selectedConditions.some((s) => s.toLowerCase() === trimmed.toLowerCase())
    ) {
      return
    }

    setSelectedConditions((prev) => [...prev, trimmed])
    setConditionInput("")
    setShowSuggestions(false)
    inputRef.current?.focus()
  }

  const removeCondition = (name: string) => {
    setSelectedConditions((prev) => prev.filter((c) => c !== name))
  }

  const handleConditionKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      e.preventDefault()
      if (conditionInput.trim()) {
        addCondition(conditionInput)
      }
    } else if (
      e.key === "Backspace" &&
      !conditionInput &&
      selectedConditions.length > 0
    ) {
      removeCondition(selectedConditions[selectedConditions.length - 1])
    }
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)

    const year = parseInt(birthYear, 10)
    if (isNaN(year) || year < 1900 || year > new Date().getFullYear()) {
      setError(t("children.invalidBirthYear"))
      return
    }

    if (isAdmin && !selectedUserId) {
      setError(t("children.selectParentUser"))
      return
    }

    try {
      if (isEditMode) {
        await updateChild(
          id!,
          firstName,
          lastName,
          year,
          isAdmin ? selectedUserId : undefined,
          selectedConditions
        )
      } else {
        await createChild(
          firstName,
          lastName,
          year,
          selectedConditions.length > 0 ? selectedConditions : undefined,
          isAdmin ? selectedUserId : undefined
        )
      }
      navigate("/children")
    } catch (err: any) {
      setError(err.message ?? t("children.failedSave"))
    }
  }

  return (
    <div>
      <div className="mb-6">
        <h1 className="text-2xl font-bold text-cyan-400">
          {isEditMode ? t("children.editChild") : t("children.addChild")}
        </h1>
        <p className="text-slate-400 text-sm mt-1">
          {isEditMode
            ? t("children.editDescription")
            : t("children.addDescription")}
        </p>
      </div>

      {error && (
        <div className="bg-red-900/50 border border-red-700 rounded-lg p-3 mb-4">
          <p className="text-red-300 text-sm">{error}</p>
        </div>
      )}

      <form onSubmit={handleSubmit} className="flex flex-col gap-4 max-w-md">
        <div>
          <label className="block text-sm text-slate-400 mb-1">
            {t("children.firstName")}
          </label>
          <input
            type="text"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            required
            className="w-full bg-slate-700 border border-slate-600 rounded-lg px-3 py-2 text-slate-100 focus:outline-none focus:border-cyan-400"
          />
        </div>
        <div>
          <label className="block text-sm text-slate-400 mb-1">
            {t("children.lastName")}
          </label>
          <input
            type="text"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            required
            className="w-full bg-slate-700 border border-slate-600 rounded-lg px-3 py-2 text-slate-100 focus:outline-none focus:border-cyan-400"
          />
        </div>
        <div>
          <label className="block text-sm text-slate-400 mb-1">
            {t("children.birthYear")}
          </label>
          <input
            type="number"
            value={birthYear}
            onChange={(e) => setBirthYear(e.target.value)}
            required
            min={1900}
            max={new Date().getFullYear()}
            className="w-full bg-slate-700 border border-slate-600 rounded-lg px-3 py-2 text-slate-100 focus:outline-none focus:border-cyan-400"
          />
        </div>

        {isAdmin && (
          <div>
            <label className="block text-sm text-slate-400 mb-1">
              {t("children.assignedUser")}
            </label>
            <select
              value={selectedUserId}
              onChange={(e) => setSelectedUserId(e.target.value)}
              required
              className="w-full bg-slate-700 border border-slate-600 rounded-lg px-3 py-2 text-slate-100 focus:outline-none focus:border-cyan-400"
            >
              <option value="">{t("children.selectUser")}</option>
              {users.map((u) => (
                <option key={u.id} value={u.id}>
                  {u.email}
                </option>
              ))}
            </select>
          </div>
        )}

        <div>
          <label className="block text-sm text-slate-400 mb-1">
            {t("children.conditions")}
          </label>
          <div className="w-full bg-slate-700 border border-slate-600 rounded-lg px-2 py-1.5 flex flex-wrap gap-1.5 items-center focus-within:border-cyan-400 min-h-[42px]">
            {selectedConditions.map((name) => (
              <span
                key={name}
                className="inline-flex items-center gap-1 bg-purple-700 text-purple-100 text-sm px-2.5 py-0.5 rounded-md"
              >
                {name}
                <button
                  type="button"
                  onClick={() => removeCondition(name)}
                  className="text-purple-300 hover:text-white font-bold leading-none"
                  aria-label={t("children.removeCondition", { name })}
                >
                  ×
                </button>
              </span>
            ))}
            <div className="relative flex-1 min-w-[120px]">
              <input
                ref={inputRef}
                type="text"
                value={conditionInput}
                onChange={(e) => {
                  setConditionInput(e.target.value)
                  setShowSuggestions(true)
                }}
                onFocus={() => setShowSuggestions(true)}
                onKeyDown={handleConditionKeyDown}
                placeholder={
                  selectedConditions.length === 0
                    ? t("children.conditionsPlaceholder")
                    : t("children.conditionsPlaceholderMore")
                }
                className="w-full bg-transparent text-slate-100 text-sm focus:outline-none py-1 px-1"
              />
              {showSuggestions && filteredSuggestions.length > 0 && (
                <div
                  ref={suggestionsRef}
                  className="absolute z-10 top-full left-0 mt-1 w-64 bg-slate-800 border border-slate-600 rounded-lg shadow-lg max-h-48 overflow-y-auto"
                >
                  {filteredSuggestions.map((name) => (
                    <button
                      key={name}
                      type="button"
                      onClick={() => addCondition(name)}
                      className="w-full text-left px-3 py-2 text-sm text-slate-200 hover:bg-slate-700 transition"
                    >
                      {name}
                    </button>
                  ))}
                </div>
              )}
            </div>
          </div>
          <p className="text-slate-500 text-xs mt-1">
            {t("children.conditionsHelp")}
          </p>
        </div>

        <div className="flex gap-3 mt-2">
          <button
            type="submit"
            className="bg-cyan-600 hover:bg-cyan-500 text-white font-medium px-6 py-2 rounded-lg transition"
          >
            {t("common.save")}
          </button>
          <button
            type="button"
            onClick={() => navigate("/children")}
            className="bg-slate-600 hover:bg-slate-500 text-white font-medium px-6 py-2 rounded-lg transition"
          >
            {t("common.cancel")}
          </button>
        </div>
      </form>
    </div>
  )
}
