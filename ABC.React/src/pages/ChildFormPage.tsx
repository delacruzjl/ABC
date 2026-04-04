import React, { useEffect, useState } from "react"
import { useNavigate, useParams } from "react-router-dom"
import { useChildren, useUsers, useChildConditions } from "../hooks/useChildren"
import { useAuth } from "../context/AuthContext"

export const ChildFormPage: React.FC = () => {
  const { id } = useParams<{ id: string }>()
  const isEditMode = Boolean(id)

  const [firstName, setFirstName] = useState("")
  const [lastName, setLastName] = useState("")
  const [birthYear, setBirthYear] = useState("")
  const [selectedUserId, setSelectedUserId] = useState("")
  const [selectedConditions, setSelectedConditions] = useState<string[]>([])
  const [error, setError] = useState<string | null>(null)
  const { children, createChild, updateChild } = useChildren()
  const { isAdmin } = useAuth()
  const { users } = useUsers()
  const { conditions: availableConditions } = useChildConditions()
  const navigate = useNavigate()

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

  const toggleCondition = (name: string) => {
    setSelectedConditions((prev) =>
      prev.includes(name) ? prev.filter((c) => c !== name) : [...prev, name]
    )
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setError(null)

    const year = parseInt(birthYear, 10)
    if (isNaN(year) || year < 1900 || year > new Date().getFullYear()) {
      setError("Please enter a valid birth year.")
      return
    }

    if (isAdmin && !selectedUserId) {
      setError("Please select a parent user.")
      return
    }

    try {
      if (isEditMode) {
        await updateChild(
          id!,
          firstName,
          lastName,
          year,
          selectedUserId,
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
      setError(err.message ?? "Failed to save child.")
    }
  }

  return (
    <div>
      <div className="mb-6">
        <h1 className="text-2xl font-bold text-cyan-400">
          {isEditMode ? "Edit Child" : "Add Child"}
        </h1>
        <p className="text-slate-400 text-sm mt-1">
          {isEditMode
            ? "Update child information"
            : "Register a new child for observations"}
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
            First Name
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
            Last Name
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
            Birth Year
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
              Assigned User
            </label>
            <select
              value={selectedUserId}
              onChange={(e) => setSelectedUserId(e.target.value)}
              required
              className="w-full bg-slate-700 border border-slate-600 rounded-lg px-3 py-2 text-slate-100 focus:outline-none focus:border-cyan-400"
            >
              <option value="">Select a user…</option>
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
            Conditions
          </label>
          {availableConditions.length === 0 ? (
            <p className="text-slate-500 text-sm italic">
              No conditions available. An admin can create them.
            </p>
          ) : (
            <div className="flex flex-wrap gap-2">
              {availableConditions.map((c) => {
                const isSelected = selectedConditions.includes(c.name)
                return (
                  <button
                    key={c.id}
                    type="button"
                    onClick={() => toggleCondition(c.name)}
                    className={`px-3 py-1.5 rounded-lg text-sm font-medium border transition ${
                      isSelected
                        ? "bg-purple-700 border-purple-500 text-purple-100"
                        : "bg-slate-700 border-slate-600 text-slate-300 hover:border-purple-500"
                    }`}
                  >
                    {c.name}
                  </button>
                )
              })}
            </div>
          )}
          {selectedConditions.length > 0 && (
            <p className="text-slate-500 text-xs mt-1">
              {selectedConditions.length} selected
            </p>
          )}
        </div>

        <div className="flex gap-3 mt-2">
          <button
            type="submit"
            className="bg-cyan-600 hover:bg-cyan-500 text-white font-medium px-6 py-2 rounded-lg transition"
          >
            {isEditMode ? "Update" : "Save"}
          </button>
          <button
            type="button"
            onClick={() => navigate("/children")}
            className="bg-slate-600 hover:bg-slate-500 text-white font-medium px-6 py-2 rounded-lg transition"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  )
}
