import React, { useState, useEffect } from "react"
import type { Behavior } from "../types/behavior"

interface Props {
  behavior: Behavior | null | undefined
  onSave: (beh: Behavior) => void
  onCancel: () => void
  saving?: boolean
}

export const BehaviorManager: React.FC<Props> = ({
  behavior,
  onSave,
  onCancel,
  saving = false,
}) => {
  const [form, setForm] = useState<Partial<Behavior>>({
    name: "",
    description: "",
  })

  useEffect(() => {
    if (behavior) {
      setForm({ name: behavior.name, description: behavior.description })
    } else {
      setForm({ name: "", description: "" })
    }
  }, [behavior])

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSubmit = () => {
    if (!form.name || saving) return
    const beh: Behavior = {
      ...(behavior || {}),
      id: behavior?.id || "",
      name: form.name!,
      description: form.description || "",
      observations: behavior?.observations || [],
    }
    onSave(beh)
  }

  return (
    <div className="p-6 bg-slate-800 rounded-xl shadow-lg max-w-xl mx-auto border border-slate-800">
      <h2 className="text-xl font-bold mb-4 text-cyan-400">
        {behavior ? "Edit Behavior" : "Add Behavior"}
      </h2>
      <div className="mb-4 flex flex-col gap-2">
        <input
          name="name"
          value={form.name || ""}
          onChange={handleChange}
          placeholder="Name"
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label="Behavior Name"
        />
        <textarea
          name="description"
          value={form.description || ""}
          onChange={handleChange}
          placeholder="Description"
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label="Behavior Description"
        />
        <div className="flex gap-2">
          <button
            onClick={handleSubmit}
            disabled={saving}
            className="bg-cyan-400 hover:bg-cyan-500 text-slate-900 font-semibold px-4 py-2 rounded shadow disabled:opacity-50"
          >
            {saving ? "Saving…" : behavior ? "Update" : "Add"}
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
