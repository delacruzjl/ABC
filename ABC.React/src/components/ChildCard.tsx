import React, { useState } from "react"
import { useNavigate } from "react-router-dom"
import type { Child } from "../types/child"
import { ConfirmDialog } from "./ConfirmDialog"

interface Props {
  child: Child
  onEdit?: (id: string) => void
  onDelete: (id: string) => void
  isDefault?: boolean
  onSetDefault?: (id: string | null) => void | Promise<unknown>
}

export const ChildCard: React.FC<Props> = ({ child, onEdit, onDelete, isDefault, onSetDefault }) => {
  const [showConfirm, setShowConfirm] = useState(false)
  const navigate = useNavigate()

  return (
    <div className="bg-slate-700 rounded-lg p-4 border border-slate-600 flex items-center justify-between">
      <div className="flex-1">
        <div className="flex items-center gap-3 mb-1">
          <h3 className="text-lg font-semibold text-slate-100">
            {child.firstName} {child.lastName}
          </h3>
          <span className="text-xs bg-cyan-900 text-cyan-300 px-2 py-0.5 rounded">
            {child.observationCount} observation
            {child.observationCount !== 1 ? "s" : ""}
          </span>
          {isDefault && (
            <span className="text-xs bg-amber-900 text-amber-300 px-2 py-0.5 rounded">
              Default
            </span>
          )}
        </div>
        <p className="text-slate-400 text-sm">
          Birth Year: {child.birthYear}
        </p>
        {child.conditions.length > 0 && (
          <div className="flex flex-wrap gap-1 mt-2">
            {child.conditions.map((c) => (
              <span
                key={c.id}
                className="text-xs bg-purple-900 text-purple-300 px-2 py-0.5 rounded"
              >
                {c.name}
              </span>
            ))}
          </div>
        )}
      </div>
      <div className="flex items-center gap-3 ml-4">
        {onSetDefault && (
          <button
            onClick={() => onSetDefault(isDefault ? null : child.id)}
            className={`text-2xl leading-none transition ${
              isDefault
                ? "text-amber-400 hover:text-amber-300"
                : "text-slate-500 hover:text-amber-400"
            }`}
            title={isDefault ? "Remove as default" : "Set as default child"}
            aria-label={isDefault ? "Remove as default" : "Set as default child"}
          >
            {isDefault ? "★" : "☆"}
          </button>
        )}
        <button
          onClick={() => navigate(`/observation/${child.id}`)}
          className="text-green-400 hover:text-green-300 text-sm font-medium transition"
          title="Start observation"
        >
          Observe
        </button>
        {onEdit && (
          <button
            onClick={() => onEdit(child.id)}
            className="text-cyan-400 hover:text-cyan-300 text-sm font-medium transition"
            title="Edit child"
          >
            Edit
          </button>
        )}
        <button
          onClick={() => setShowConfirm(true)}
          className="text-red-400 hover:text-red-300 text-sm font-medium transition"
          title="Remove child"
        >
          Remove
        </button>
      </div>
      {showConfirm && (
        <ConfirmDialog
          message={`Are you sure you want to remove ${child.firstName} ${child.lastName}?`}
          onConfirm={() => {
            onDelete(child.id)
            setShowConfirm(false)
          }}
          onCancel={() => setShowConfirm(false)}
        />
      )}
    </div>
  )
}
