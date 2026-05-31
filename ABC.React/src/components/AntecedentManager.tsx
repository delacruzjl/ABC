import React, { useState, useEffect } from "react"
import { useTranslation } from "react-i18next"
import type { Antecedent } from "../types/antecedent"

interface Props {
  antecedent: Antecedent | null | undefined
  onSave: (ant: Antecedent) => Promise<void> | void
  onCancel: () => void
  saving?: boolean
}

export const AntecedentManager: React.FC<Props> = ({
  antecedent,
  onSave,
  onCancel,
  saving = false,
}) => {
  const { t } = useTranslation()
  const [form, setForm] = useState<Partial<Antecedent>>({
    name: "",
    description: "",
  })
  const [error, setError] = useState<string | null>(null)

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

  const handleSubmit = async () => {
    if (!form.name || saving) return
    setError(null)
    const ant: Antecedent = {
      ...(antecedent || {}),
      id: antecedent?.id || "",
      name: form.name!,
      description: form.description || "",
      observations: antecedent?.observations || [],
    }
    try {
      await onSave(ant)
    } catch (err: any) {
      setError(err?.message ?? t("common.unexpectedError"))
    }
  }

  return (
    <div className="p-6 bg-slate-800 rounded-xl shadow-lg max-w-xl mx-auto border border-slate-800">
      <h2 className="text-xl font-bold mb-4 text-cyan-400">
        {antecedent ? t("antecedents.editItem") : t("antecedents.add")}
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
          placeholder={t("antecedents.name")}
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label={t("antecedents.name")}
        />
        <textarea
          name="description"
          value={form.description || ""}
          onChange={handleChange}
          placeholder={t("antecedents.description")}
          className="border border-cyan-400 focus:border-cyan-500 focus:ring-2 focus:ring-cyan-500 placeholder:text-slate-400 bg-slate-800 text-slate-100 p-3 rounded outline-none transition duration-150"
          aria-label={t("antecedents.description")}
        />
        <div className="flex gap-2">
          <button
            onClick={handleSubmit}
            disabled={saving}
            className="bg-cyan-400 hover:bg-cyan-500 text-slate-900 font-semibold px-4 py-2 rounded shadow disabled:opacity-50"
          >
            {saving ? t("common.saving") : t("antecedents.save")}
          </button>
          <button
            onClick={onCancel}
            className="bg-slate-800 hover:bg-slate-700 text-slate-100 px-4 py-2 rounded shadow"
          >
            {t("antecedents.cancel")}
          </button>
        </div>
      </div>
    </div>
  )
}
