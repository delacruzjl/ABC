import React, { useState, useEffect } from "react"
import type { Consequence } from "../types/consequence"

interface Props {
  consequence: Consequence | null | undefined
  onSave: (cons: Consequence) => Promise<void> | void
  onCancel: () => void
  saving?: boolean
}

export const ConsequenceManager: React.FC<Props> = ({
  consequence,
  onSave,
  onCancel,
  saving = false,
}) => {
  const [form, setForm] = useState<Partial<Consequence>>({
    name: "",
    description: "",
  })
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (consequence) {
      setForm({ name: consequence.name, description: consequence.description })
    } else {
      setForm({ name: "", description: "" })
    }
  }, [consequence])

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSubmit = async () => {
    if (!form.name || saving) return
    setError(null)
    const cons: Consequence = {
      ...(consequence || {}),
      id: consequence?.id || "",
      name: form.name!,
      description: form.description || "",
      observations: consequence?.observations || [],
    }
    try {
      await onSave(cons)
    } catch (err: any) {
      setError(err?.message ?? "An unexpected error occurred.")
    }
  }

  return (
    <div className="p-6 bg-slate-800 rounded-xl shadow-lg max-w-xl mx-auto border border-slate-800">
      <h2 className="text-xl font-bold mb-4 text-cyan-400">
        {consequence ? "Edit Consequence" : "Add Consequence"}
      </h2>
      {error && (
        <div className="bg-red-900/50 border border-red-700 rounded-lg p-3 mb-4">
          <p className="text-red-300 text-sm">{error}</p>
        </div>
      )}
      <div className="mb-4 flex flex-col gap-2">
        <input
          name="name"
          value={form.name || ""}
          onChange={handleChange}
          placeholder="Name"
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label="Consequence Name"
        />
        <textarea
          name="description"
          value={form.description || ""}
          onChange={handleChange}
          placeholder="Description"
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label="Consequence Description"
        />
        <div className="flex gap-2">
          <button
            onClick={handleSubmit}
            disabled={saving}
            className="bg-cyan-400 hover:bg-cyan-500 text-slate-900 font-semibold px-4 py-2 rounded shadow disabled:opacity-50"
          >
            {saving ? "Saving…" : consequence ? "Update" : "Add"}
          </button>
          <button
            onClick={onCancel}
            className="bg-slate-800 hover:bg-slate-700 text-slate-100 px-4 py-2 rounded shadow"
          >
            Cancel
          </button>
        </div>
      </div>
    </div>
  )
}
