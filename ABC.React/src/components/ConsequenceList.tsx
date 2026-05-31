import React, { useState } from "react"
import { useTranslation } from "react-i18next"
import type { Consequence } from "../types/consequence"
import { ConfirmDialog } from "./ConfirmDialog"

interface Props {
  consequences: Consequence[]
  onAdd: () => void
  onEdit: (id: string) => void
  onDelete: (id: string) => void
}

export const ConsequenceList: React.FC<Props> = ({
  consequences,
  onAdd,
  onEdit,
  onDelete,
}) => {
  const { t } = useTranslation()
  const [deleteId, setDeleteId] = useState<string | null>(null)

  return (
    <div className="p-6 bg-slate-800 rounded-xl shadow-lg max-w-xl mx-auto border border-slate-800">
      <h2 className="text-xl font-bold mb-4 text-cyan-400">
        {t("consequences.title")}
      </h2>
      <button
        onClick={onAdd}
        className="mb-4 bg-cyan-400 hover:bg-cyan-500 text-slate-900 font-semibold px-4 py-2 rounded shadow"
      >
        {t("consequences.add")}
      </button>
      <ul className="divide-y divide-slate-800">
        {consequences.map((c) => (
          <li key={c.id} className="py-2 flex justify-between items-center">
            <div>
              <span className="font-semibold text-cyan-400">{c.name}</span>
              <span className="text-slate-300 ml-2">{c.description}</span>
            </div>
            <div className="flex gap-2">
              <button
                onClick={() => onEdit(c.id)}
                className="bg-slate-800 text-cyan-400 hover:bg-slate-700 hover:text-cyan-300 px-3 py-1 rounded shadow"
              >
                {t("common.edit")}
              </button>
              <button
                onClick={() => setDeleteId(c.id)}
                className="bg-slate-800 text-red-400 hover:bg-slate-700 hover:text-red-300 px-3 py-1 rounded shadow"
              >
                {t("common.delete")}
              </button>
            </div>
          </li>
        ))}
      </ul>
      {deleteId && (
        <ConfirmDialog
          message={t("consequences.confirmDelete")}
          onConfirm={() => {
            onDelete(deleteId)
            setDeleteId(null)
          }}
          onCancel={() => setDeleteId(null)}
        />
      )}
    </div>
  )
}
