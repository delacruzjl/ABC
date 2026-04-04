import React, { useState } from "react"
import type { Behavior } from "../types/behavior"
import { ConfirmDialog } from "./ConfirmDialog"

interface Props {
  behaviors: Behavior[]
  onAdd: () => void
  onEdit: (id: string) => void
  onDelete: (id: string) => void
}

export const BehaviorList: React.FC<Props> = ({
  behaviors,
  onAdd,
  onEdit,
  onDelete,
}) => {
  const [deleteId, setDeleteId] = useState<string | null>(null)

  return (
    <div className="p-6 bg-slate-800 rounded-xl shadow-lg max-w-xl mx-auto border border-slate-800">
      <h2 className="text-xl font-bold mb-4 text-cyan-400">Behaviors</h2>
      <button
        onClick={onAdd}
        className="mb-4 bg-cyan-400 hover:bg-cyan-500 text-slate-900 font-semibold px-4 py-2 rounded shadow"
      >
        Add Behavior
      </button>
      <ul className="divide-y divide-slate-800">
        {behaviors.map((b) => (
          <li key={b.id} className="py-2 flex justify-between items-center">
            <div>
              <span className="font-semibold text-cyan-400">{b.name}</span>
              <span className="text-slate-300 ml-2">{b.description}</span>
            </div>
            <div className="flex gap-2">
              <button
                onClick={() => onEdit(b.id)}
                className="bg-slate-800 text-cyan-400 hover:bg-slate-700 hover:text-cyan-300 px-3 py-1 rounded shadow"
              >
                Edit
              </button>
              <button
                onClick={() => setDeleteId(b.id)}
                className="bg-slate-800 text-red-400 hover:bg-slate-700 hover:text-red-300 px-3 py-1 rounded shadow"
              >
                Delete
              </button>
            </div>
          </li>
        ))}
      </ul>
      {deleteId && (
        <ConfirmDialog
          message="Are you sure you want to delete this behavior?"
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
