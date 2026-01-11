import React from "react"
import type { Antecedent } from "../types/antecedent"

interface Props {
  antecedents: Antecedent[]
  onAdd: () => void
  onEdit: (id: string) => void
  onDelete: (id: string) => void
}

export const AntecedentList: React.FC<Props> = ({
  antecedents,
  onAdd,
  onEdit,
  onDelete,
}) => (
  <div className="p-6 bg-slate-800 rounded-xl shadow-lg max-w-xl mx-auto border border-slate-800">
    <h2 className="text-xl font-bold mb-4 text-cyan-400">Antecedents</h2>
    <button
      onClick={onAdd}
      className="mb-4 bg-cyan-400 hover:bg-cyan-500 text-slate-900 font-semibold px-4 py-2 rounded shadow"
    >
      Add Antecedent
    </button>
    <ul className="divide-y divide-slate-800">
      {antecedents.map((a) => (
        <li key={a.id} className="py-2 flex justify-between items-center">
          <div>
            <span className="font-semibold text-cyan-400">{a.name}</span>
            <span className="text-slate-300 ml-2">{a.description}</span>
          </div>
          <div className="flex gap-2">
            <button
              onClick={() => onEdit(a.id)}
              className="bg-slate-800 text-cyan-400 hover:bg-slate-700 hover:text-cyan-300 px-3 py-1 rounded shadow"
            >
              Edit
            </button>
            <button
              onClick={() => onDelete(a.id)}
              className="bg-slate-800 text-red-400 hover:bg-slate-700 hover:text-red-300 px-3 py-1 rounded shadow"
            >
              Delete
            </button>
          </div>
        </li>
      ))}
    </ul>
  </div>
)
