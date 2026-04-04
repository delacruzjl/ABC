import React, { useState, useEffect } from "react"
import type { Antecedent } from "../types/antecedent"

interface Props {
  antecedent: Antecedent | null | undefined
  onSave: (ant: Antecedent) => void
  onCancel: () => void
  saving?: boolean
}

export const AntecedentManager: React.FC<Props> = ({
  antecedent,
  onSave,
  onCancel,
  saving = false,
}) => {
  const [form, setForm] = useState<Partial<Antecedent>>({
    name: "",
    description: "",
  })

  useEffect(() => {
    if (antecedent) {
      setForm({ name: antecedent.name, description: antecedent.description })
    } else {
      setForm({ name: "", description: "" })
    }
  }, [antecedent])

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSubmit = () => {
    if (!form.name || saving) return
    const ant: Antecedent = {
      ...(antecedent || {}),
      id: antecedent?.id || "",
      name: form.name!,
      description: form.description || "",
      observations: antecedent?.observations || [],
    }
    onSave(ant)
  }

  return (
    <div className="p-6 bg-slate-800 rounded-xl shadow-lg max-w-xl mx-auto border border-slate-800">
      <h2 className="text-xl font-bold mb-4 text-cyan-400">
        {antecedent ? "Edit Antecedent" : "Add Antecedent"}
      </h2>
      <div className="mb-4 flex flex-col gap-2">
        <input
          name="name"
          value={form.name || ""}
          onChange={handleChange}
          placeholder="Name"
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label="Antecedent Name"
        />
        <textarea
          name="description"
          value={form.description || ""}
          onChange={handleChange}
          placeholder="Description"
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label="Antecedent Description"
        />
        <div className="flex gap-2">
          <button
            onClick={handleSubmit}
            disabled={saving}
            className="bg-cyan-400 hover:bg-cyan-500 text-slate-900 font-semibold px-4 py-2 rounded shadow disabled:opacity-50"
          >
            {saving ? "Saving…" : antecedent ? "Update" : "Add"}
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
