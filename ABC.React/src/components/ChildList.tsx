import React from "react"
import { ChildCard } from "./ChildCard"
import type { Child } from "../types/child"

interface Props {
  children: Child[]
  onAdd: () => void
  onEdit?: (id: string) => void
  onDelete: (id: string) => void
}

export const ChildList: React.FC<Props> = ({
  children: childList,
  onAdd,
  onEdit,
  onDelete,
}) => {
  return (
    <div>
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-cyan-400">Children</h1>
          <p className="text-slate-400 text-sm mt-1">
            Manage children and view their observation counts
          </p>
        </div>
        <button
          onClick={onAdd}
          className="bg-cyan-600 hover:bg-cyan-500 text-white font-medium px-4 py-2 rounded-lg transition"
        >
          + Add Child
        </button>
      </div>

      {childList.length === 0 ? (
        <div className="text-center py-12">
          <p className="text-slate-400">
            No children yet. Add one to get started.
          </p>
        </div>
      ) : (
        <div className="flex flex-col gap-3">
          {childList.map((child) => (
            <ChildCard
              key={child.id}
              child={child}
              onEdit={onEdit}
              onDelete={onDelete}
            />
          ))}
        </div>
      )}
    </div>
  )
}
